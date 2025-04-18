namespace Autine.Application.Contracts.Users;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(e => e.City)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(e => e.Country)
            .NotEmpty()
            .Length(1, 100);

        RuleFor(e => e.Bio)
            .NotEmpty()
            .Length(1, 2500);
    }
}