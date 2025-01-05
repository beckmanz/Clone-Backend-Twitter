using Clone_Backend_Twitter.Models.Dto;
using FluentValidation;

namespace Clone_Backend_Twitter.Validators;

public class SignupValidator : AbstractValidator<SignupDto>
{
    public SignupValidator()
    {
        RuleFor(signup => signup.Name)
            .NotEmpty().WithMessage("Nome é obrigatório!")
            .MinimumLength(2).WithMessage("O nome deve ter pelo menos 2 caracteres!");

        RuleFor(signup => signup.Email)
            .NotEmpty().WithMessage("Email é obrigatório!")
            .EmailAddress().WithMessage("Email inválido!");
        
        RuleFor(signup => signup.Password)
            .NotEmpty().WithMessage("Senha é obrigatória!")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres!");
    }
}