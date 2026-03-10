using FinancialManager.Application.DTOs;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialManager.Application.Services;

public class AccountAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountAppService> _logger;

    public AccountAppService(IUnitOfWork unitOfWork, ILogger<AccountAppService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AccountResponse> CreateAsync(CreateAccountRequest request)
    {
        _logger.LogInformation("Criando conta {Name} para usuário {UserId}", request.Name, request.UserId);

        var account = new Account(request.Name, request.Type, request.UserId);

        await _unitOfWork.AccountRepository.AddAsync(account);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Conta criada com sucesso. ID: {AccountId}", account.Id);

        return new AccountResponse(
            account.Id,
            account.Name,
            account.Type,
            account.UserId,
            account.GetBalance(),
            account.CreatedAt
        );
    }

    public async Task<AccountResponse?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando conta {AccountId}", id);

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
        if (account == null)
        {
            _logger.LogWarning("Conta {AccountId} não encontrada", id);
            return null;
        }

        return new AccountResponse(
            account.Id,
            account.Name,
            account.Type,
            account.UserId,
            account.GetBalance(),
            account.CreatedAt
        );
    }

    public async Task<IEnumerable<AccountResponse>> GetByUserIdAsync(Guid userId)
    {
        _logger.LogInformation("Buscando contas do usuário {UserId}", userId);

        var accounts = await _unitOfWork.AccountRepository.GetByUserIdAsync(userId);

        return accounts.Select(a => new AccountResponse(
            a.Id,
            a.Name,
            a.Type,
            a.UserId,
            a.GetBalance(),
            a.CreatedAt
        ));
    }
}
