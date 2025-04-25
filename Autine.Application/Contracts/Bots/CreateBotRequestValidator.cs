namespace Autine.Application.Contracts.Bots;

public class CreateBotRequestValidator : AbstractValidator<CreateBotRequest>
{
    public CreateBotRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("{ProperityName} is required.")
            .MaximumLength(100)
            .WithMessage("{ProperityName} must be at most {MaxLength} characters long.");
        RuleFor(x => x.Context)
            .NotEmpty();

        RuleFor(x => x.Bio)
            .NotEmpty();

        RuleFor(e => e.PatientIds)
            .Must(e =>
            {
                if (e is null)
                    return true;

                foreach (var id in e)
                {
                    if (string.IsNullOrEmpty(id))
                        return false;
                }
                return true;
            }).WithMessage("{PropertyName} cannot contain empty Ids or duplicated patient.");
    }
}
