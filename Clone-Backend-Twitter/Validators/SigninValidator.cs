using Clone_Backend_Twitter.Models.Dto;
using FluentValidation;

namespace Clone_Backend_Twitter.Validators;

public class SigninValidator : AbstractValidator<SigninDto>
{
    public SigninValidator()
    {
        RuleFor(signup => signup.Email)
            .NotEmpty().WithMessage("Email é obrigatório!")
            .EmailAddress().WithMessage("Email inválido!");
        
        RuleFor(signup => signup.Password)
            .NotEmpty().WithMessage("Senha é obrigatória!")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres!");
    }
}