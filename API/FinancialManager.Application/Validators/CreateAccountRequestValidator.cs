using FinancialManager.Application.DTOs;
using FluentValidation;

namespace FinancialManager.Application.Validators;

public class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
{
    public CreateAccountRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da conta é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("O tipo da conta é obrigatório.")
            .MaximumLength(50).WithMessage("O tipo não pode ter mais de 50 caracteres.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
    }
}
