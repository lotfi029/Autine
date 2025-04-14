namespace Autine.Application.Contracts.Patient;

public class PatientRequestValidator : AbstractValidator<PatientRequest>
{
    public PatientRequestValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .Length(ValidationConstants.MinLength, ValidationConstants.MaxLength).WithMessage(ValidationConstants.LengthErrorMesssage);

        RuleFor(e => e.LastName)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .Length(ValidationConstants.MinLength, ValidationConstants.MaxLength).WithMessage(ValidationConstants.LengthErrorMesssage);

        RuleFor(e => e.City)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .Length(ValidationConstants.MinLength, ValidationConstants.MaxLength).WithMessage(ValidationConstants.LengthErrorMesssage);

        RuleFor(e => e.Country)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .Length(ValidationConstants.MinLength, ValidationConstants.MaxLength).WithMessage(ValidationConstants.LengthErrorMesssage);

        RuleFor(e => e.Gender)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .Must(g =>
            {
                if (g == null) return false;
                var gender = g.ToLower();

                return gender == "male" || g == "female";
            })
            .WithMessage("{PropertyName} must be Male, Female.");

        RuleFor(e => e.DateOfBirth)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .LessThan(DateTime.Today).WithMessage("{PropertyName} must be a past date.");

        RuleFor(e => e.Email)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .EmailAddress().WithMessage("Invalid {PropertyName} format.");

        RuleFor(e => e.UserName)
            .NotEmpty().WithMessage(ValidationConstants.RequiredErrorMessage)
            .MinimumLength(ValidationConstants.MinUsernameLength).WithMessage("{PropertyName} must be at least {MinLength} characters long.")
            .Matches(ValidationConstants.UsernamePattern).WithMessage("{PropertyName} contains invalid characters. Allowed characters: a-z, A-Z, 0-9, ., _, !, @, #, $. Given: {PropertyValue}");

        RuleFor(e => e.Bio)
            .NotEmpty().WithMessage("Bio is required.")
            .Length(1, 2500).WithMessage(ValidationConstants.LengthErrorMesssage);


        RuleFor(e => e.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
    }
}