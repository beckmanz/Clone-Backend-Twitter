using Clone_Backend_Twitter.Models.Dto;
using FluentValidation;

namespace Clone_Backend_Twitter.Validators;

public class TweetValidator : AbstractValidator<TweetDto>
{
    public TweetValidator()
    {
        RuleFor(Tweet => Tweet.Body)
            .NotEmpty().WithMessage("O corpo é obrigatório!");
    }
}