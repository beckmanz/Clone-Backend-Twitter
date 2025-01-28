using Clone_Backend_Twitter.Models.Dto;
using FluentValidation;

namespace Clone_Backend_Twitter.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Name)
            .MinimumLength(2).WithMessage("O Nome deve ter pelo menos 2 caracteres!")
            .When(u => !string.IsNullOrWhiteSpace(u.Name));
        RuleFor(u => u.Link)
            .Must(IsValidUrl).WithMessage("O Link deve ter uma URL!")
            .When(u => !string.IsNullOrWhiteSpace(u.Link));
    }
    
    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}