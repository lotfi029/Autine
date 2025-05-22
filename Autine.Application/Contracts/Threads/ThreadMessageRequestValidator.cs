namespace Autine.Application.Contracts.Threads;

public class ThreadMessageRequestValidator : AbstractValidator<ThreadMessageRequest>
{
    public ThreadMessageRequestValidator()
    {
        RuleFor(e => e.ThreadId)
            .NotEmpty();

        RuleFor(e => e.Content)
            .NotEmpty()
            .WithMessage("Content is required.");
    }
}