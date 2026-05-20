using FinancialManager.Application.DTOs;
using FinancialManager.Domain.Exceptions;
using FinancialManager.Domain.Interfaces;
using FluentValidation;
using DomainValidationException = FinancialManager.Domain.Exceptions.ValidationException;

namespace FinancialManager.Application.Services;

public class UserProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateProfileRequest> _updateProfileValidator;
    private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;

    public UserProfileService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateProfileRequest> updateProfileValidator,
        IValidator<ChangePasswordRequest> changePasswordValidator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _updateProfileValidator = updateProfileValidator;
        _changePasswordValidator = changePasswordValidator;
    }

    public async Task<UserProfileResponse> GetProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        return new UserProfileResponse(
            user.Id,
            user.Name,
            user.Email,
            user.CreatedAt,
            user.LastLoginAt,
            user.IsActive,
            user.Accounts.Count
        );
    }

    public async Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var validationResult = await _updateProfileValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            throw new DomainValidationException(errors);
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        // Verificar se o novo email já está em uso por outro usuário
        if (user.Email != request.Email.ToLowerInvariant())
        {
            var emailExists = await _userRepository.ExistsByEmailAsync(request.Email);
            if (emailExists)
            {
                throw new BusinessRuleException("Email já está em uso por outro usuário");
            }
        }

        user.UpdateProfile(request.Name, request.Email);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserProfileResponse(
            user.Id,
            user.Name,
            user.Email,
            user.CreatedAt,
            user.LastLoginAt,
            user.IsActive,
            user.Accounts.Count
        );
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
            throw new DomainValidationException(errors);
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        // Verificar se a senha atual está correta
        var isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash);
        if (!isCurrentPasswordValid)
        {
            throw new UnauthorizedException("Senha atual incorreta");
        }

        // Atualizar para a nova senha
        var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatePassword(newPasswordHash);
        
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeactivateAccountAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado");
        }

        user.Deactivate();
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
