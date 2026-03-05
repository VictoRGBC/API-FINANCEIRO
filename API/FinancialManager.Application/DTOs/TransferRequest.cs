public record TransferRequest(
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string Description,
    Guid CategoryId // Categoria de sistema: "Transferência"
);