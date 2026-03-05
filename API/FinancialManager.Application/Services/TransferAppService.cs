public class TransferAppService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly TransferService _transferDomainService;

    public TransferAppService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        TransferService transferDomainService)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _transferDomainService = transferDomainService;
    }

    public async Task ExecuteAsync(TransferRequest request)
    {
        // 1. Busca as duas contas envolvidas
        var source = await _accountRepository.GetByIdAsync(request.SourceAccountId);
        var destination = await _accountRepository.GetByIdAsync(request.DestinationAccountId);

        if (source == null || destination == null)
            throw new Exception("Uma ou ambas as contas não foram encontradas.");

        // 2. Chama o Serviço de Domínio para gerar o par de transações (Débito e Crédito)
        var (debit, credit) = _transferDomainService.CreateTransfer(
            source,
            destination,
            request.Amount,
            request.Description,
            request.CategoryId);

        // 3. Persiste ambas no repositório
        await _transactionRepository.AddRangeAsync(new[] { debit, credit });
    }
}