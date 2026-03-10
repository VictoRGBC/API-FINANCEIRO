using FinancialManager.Application.DTOs;
using FinancialManager.Domain.Entities;
using FinancialManager.Domain.Exceptions;
using FinancialManager.Domain.Interfaces;
using FinancialManager.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FinancialManager.Application.Services;

public class TransferAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TransferService _transferDomainService;
    private readonly ILogger<TransferAppService> _logger;

    public TransferAppService(
        IUnitOfWork unitOfWork,
        TransferService transferDomainService,
        ILogger<TransferAppService> logger)
    {
        _unitOfWork = unitOfWork;
        _transferDomainService = transferDomainService;
        _logger = logger;
    }

    public async Task ExecuteAsync(TransferRequest request)
    {
        _logger.LogInformation(
            "Iniciando transferência de {Amount:C} da conta {SourceId} para {DestinationId}",
            request.Amount, request.SourceAccountId, request.DestinationAccountId);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var source = await _unitOfWork.AccountRepository.GetByIdAsync(request.SourceAccountId);
            if (source == null)
            {
                _logger.LogWarning("Conta de origem {SourceId} não encontrada", request.SourceAccountId);
                throw new NotFoundException("Conta de origem", request.SourceAccountId);
            }

            var destination = await _unitOfWork.AccountRepository.GetByIdAsync(request.DestinationAccountId);
            if (destination == null)
            {
                _logger.LogWarning("Conta de destino {DestinationId} não encontrada", request.DestinationAccountId);
                throw new NotFoundException("Conta de destino", request.DestinationAccountId);
            }

            var currentBalance = source.GetBalance();
            if (currentBalance < request.Amount)
            {
                _logger.LogWarning(
                    "Saldo insuficiente na conta {SourceId}. Saldo: {Balance:C}, Necessário: {Amount:C}",
                    request.SourceAccountId, currentBalance, request.Amount);
                throw new InsufficientBalanceException(currentBalance, request.Amount);
            }

            var (debit, credit) = _transferDomainService.CreateTransfer(
                source,
                destination,
                request.Amount,
                request.Description,
                request.CategoryId);

            await _unitOfWork.TransactionRepository.AddRangeAsync(new[] { debit, credit });
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation(
                "Transferência concluída com sucesso. Débito: {DebitId}, Crédito: {CreditId}",
                debit.Id, credit.Id);
        }
        catch
        {
            _logger.LogError("Erro durante transferência. Realizando rollback.");
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}