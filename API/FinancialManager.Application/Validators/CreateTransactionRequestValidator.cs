using FinancialManager.Application.DTOs;
using FinancialManager.Domain.Enums;
using FluentValidation;

namespace FinancialManager.Application.Validators;

public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
{
    public CreateTransactionRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição não pode ter mais de 200 caracteres.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("A data é obrigatória.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("A data não pode ser futura.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de transação inválido. Use 1 para Receita ou 2 para Despesa.");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("O ID da conta é obrigatório.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("O ID da categoria é obrigatório.");
    }
}
