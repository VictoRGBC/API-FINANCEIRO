using FinancialManager.Application.DTOs;
using FinancialManager.Application.Interfaces;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Exceptions;
using FinancialManager.Domain.Interfaces;
using FluentValidation;
using DomainValidationException = FinancialManager.Domain.Exceptions.ValidationException;

namespace FinancialManager.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IValidator<RegisterRequest> _registerValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IValidator<RegisterRequest> registerValidator,
        IValidator<LoginRequest> loginValidator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var validationResult = await _registerValidator.ValidateAsync(request);
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

        var existingUser = await _userRepository.ExistsByEmailAsync(request.Email);
        if (existingUser)
        {
            throw new BusinessRuleException("Email já está cadastrado");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User(request.Name, request.Email, passwordHash);

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return new AuthResponse(token, user.Email, user.Name, expiresAt);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);
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

        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new UnauthorizedException("Email ou senha inválidos");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException("Usuário inativo");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedException("Email ou senha inválidos");
        }

        user.UpdateLastLogin();
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _tokenService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return new AuthResponse(token, user.Email, user.Name, expiresAt);
    }
}
