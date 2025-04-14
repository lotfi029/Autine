using Autine.Application.Contracts.Auth;
using Autine.Application.Interfaces;
using Autine.Infrastructure.Identity.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;


namespace Autine.Infrastructure.Services;
public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext context,
    IJwtProvider jwtProvider, 
    IFileService _fileService) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ApplicationDbContext _context = context;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<AuthResponse>> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredinitails);


        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);

        if (!result.Succeeded)
        {
            var error =
                result.IsNotAllowed
                ? UserErrors.EmailNotConfirmed
                : result.IsLockedOut
                ? UserErrors.LockedUser
                : UserErrors.InvalidCredinitails;

            return Result.Failure<AuthResponse>(error);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var (token, expireIn) = _jwtProvider.GenerateToken(user, roles);

        var response = new AuthResponse(token, expireIn);

        return Result.Success(response);
    }
    public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _context.Users.AnyAsync(e => e.Email == request.Email || e.UserName == request.UserName, cancellationToken))
            return Result.Failure<RegisterResponse>(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();
        user.Bio ??= string.Empty;
        if (request.ProfilePic is not null)
        {
            var imagePath = await _fileService.UploadImageAsync(request.ProfilePic!, cancellationToken);

            if (imagePath.IsFailure)
                return Result.Failure<RegisterResponse>(imagePath.Error);

            user.ProfilePicture = imagePath.Value;
        }
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return Result.Failure<RegisterResponse>(Error.BadRequest(error.Code, error.Description));
        }
        

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var response = new RegisterResponse(code, user.Id);

        return Result.Success(response);
    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return UserErrors.InvalidCode;

        var code = request.Code;
        IdentityResult result;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            result = await _userManager.ConfirmEmailAsync(user, code);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }


        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Error.BadRequest(error.Code, error.Description);
        }

        if (!(await _userManager.IsInRoleAsync(user, DefaultRoles.Parent.Name) || await _userManager.IsInRoleAsync(user, DefaultRoles.Doctor.Name)))
            await _userManager.AddToRoleAsync(user, DefaultRoles.User.Name);

        return Result.Success();
    }
    public async Task<Result<RegisterResponse>> ReConfirmEmailAsync(ResendConfirmEmailRequest request)
    {

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<RegisterResponse>(UserErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure<RegisterResponse>(UserErrors.EmailConfirmed);


        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var response = new RegisterResponse(code, user.Id);

        return Result.Success(response);
    }
    public async Task<Result<RegisterResponse>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return UserErrors.UserNotFound;

        if (!user.EmailConfirmed)
            return UserErrors.EmailNotConfirmed;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var response = new RegisterResponse(token, user.Id);

        return response;
    }
    
    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        var code = request.Code;
        IdentityResult result;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            if (await _userManager.CheckPasswordAsync(user, request.Password))
                return Error.Conflict("User.InvalidPassword", "this password is used before Select another one.");
            result = await _userManager.ResetPasswordAsync(user, code, request.Password);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (!result.Succeeded)
        {
            var error = result.Errors.FirstOrDefault();
            return Error.BadRequest(error!.Code, error.Description);
        }

        return Result.Success();
    }
}
