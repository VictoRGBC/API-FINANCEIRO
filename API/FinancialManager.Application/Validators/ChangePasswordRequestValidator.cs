using FinancialManager.Application.DTOs;
using FluentValidation;

namespace FinancialManager.Application.Validators;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Senha atual é obrigatória");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Nova senha é obrigatória")
            .MinimumLength(6).WithMessage("Nova senha deve ter no mínimo 6 caracteres")
            .MaximumLength(100).WithMessage("Nova senha deve ter no máximo 100 caracteres");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Confirmação de senha é obrigatória")
            .Equal(x => x.NewPassword).WithMessage("As senhas não conferem");
    }
}
