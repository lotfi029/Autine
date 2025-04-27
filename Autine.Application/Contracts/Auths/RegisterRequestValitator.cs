namespace Autine.Application.Contracts.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
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
            .Must(e =>
            {
                if (e == null) 
                    return true;

                if (1 <= e.Length && e.Length <= 2500)
                    return true;

                return false;
            })
            .WithMessage(ValidationConstants.LengthErrorMesssage);

        RuleFor(x => x.ProfilePic)
            .Must(ValidImage)
            .WithMessage("{PropertyName} allowed image .jpg, .jpeg, .png, .gif");


        RuleFor(e => e.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
    }
    private bool ValidImage(IFormFile? image)
    {
        if (image == null)
            return true;

        const long maxSize = 5 * 1024 * 1024;
        if (image.Length > maxSize)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
            return false;

        var allowedContentTypes = new[]
        {
            "image/jpeg",
            "image/png",
            "image/gif"
        };
        return allowedContentTypes.Contains(image.ContentType.ToLowerInvariant());
    }
}