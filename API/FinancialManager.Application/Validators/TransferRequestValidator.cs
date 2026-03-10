using FinancialManager.Application.DTOs;
using FluentValidation;

namespace FinancialManager.Application.Validators;

public class TransferRequestValidator : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.SourceAccountId)
            .NotEmpty().WithMessage("A conta de origem é obrigatória.");

        RuleFor(x => x.DestinationAccountId)
            .NotEmpty().WithMessage("A conta de destino é obrigatória.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor da transferência deve ser maior que zero.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição não pode ter mais de 200 caracteres.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("O ID da categoria é obrigatório.");

        RuleFor(x => x)
            .Must(x => x.SourceAccountId != x.DestinationAccountId)
            .WithMessage("As contas de origem e destino devem ser diferentes.");
    }
}
