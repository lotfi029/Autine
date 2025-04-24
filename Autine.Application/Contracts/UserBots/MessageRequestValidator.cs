namespace Autine.Application.Contracts.UserBots;
public class MessageRequestValidator : AbstractValidator<MessageRequest>
{
    public MessageRequestValidator()
    {
        RuleFor(e => e.Content)
            .NotEmpty();
    }
}
