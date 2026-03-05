public class TransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    // Injeção de Dependência via Construtor
    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task ExecuteAsync(CreateTransactionRequest request)
    {
        // 1. Validar se a conta existe
        var account = await _accountRepository.GetByIdAsync(request.AccountId);
        if (account == null) throw new Exception("Conta não encontrada.");

        // 2. Criar a entidade de domínio
        var transaction = new Transaction(
            request.Description,
            request.Amount,
            request.Date,
            (TransactionType)request.Type,
            request.AccountId,
            request.CategoryId
        );

        // 3. Persistir
        await _transactionRepository.AddRangeAsync(new[] { transaction });
    }
}