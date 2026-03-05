public record CreateTransactionRequest(
    string Description,
    decimal Amount,
    DateTime Date,
    int Type, // 1 para Revenue, 2 para Expense
    Guid AccountId,
    Guid CategoryId
);