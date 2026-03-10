using FinancialManager.Application.DTOs;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Enums;
using FinancialManager.Domain.Exceptions;
using FinancialManager.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FinancialManager.Application.Services;

public class TransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        IUnitOfWork unitOfWork,
        ILogger<TransactionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task ExecuteAsync(CreateTransactionRequest request)
    {
        _logger.LogInformation("Iniciando criação de transação para conta {AccountId}", request.AccountId);

        var account = await _unitOfWork.AccountRepository.GetByIdAsync(request.AccountId);
        if (account == null)
        {
            _logger.LogWarning("Conta {AccountId} não encontrada", request.AccountId);
            throw new NotFoundException("Conta", request.AccountId);
        }

        var transaction = new Transaction(
            request.Description,
            request.Amount,
            request.Date,
            (TransactionType)request.Type,
            request.AccountId,
            request.CategoryId
        );

        await _unitOfWork.TransactionRepository.AddRangeAsync(new[] { transaction });
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Transação criada com sucesso. ID: {TransactionId}", transaction.Id);
    }
}