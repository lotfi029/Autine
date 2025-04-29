using Autine.Application.Contracts.Auth;
using Autine.Application.Contracts.Auths;
using Autine.Infrastructure.Identity.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
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
    private readonly int _refreshTokenExpiryDays = 14;
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

        var response = await GenerateTokenAsync(user);

        return response.IsFailure
            ? response.Error
            : response.Value;
    }
    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId == null || await _userManager.FindByIdAsync(userId) is not { } user)
            return UserErrors.InvalidToken;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken == null)
            return UserErrors.InvalidToken;

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var response = await GenerateTokenAsync(user);

        return response.IsFailure
            ? response.Error
            : response.Value;
    }
    
    public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = await RegisterValidationAsync(request, ct: cancellationToken);

        if (user.IsFailure)
            return user.Error;

        var code = await GenerateEmailConfirmationCodeAync(user.Value);
        var response = new RegisterResponse(code, user.Value.Id);

        return Result.Success(response);
    }
    public async Task<Result<RegisterResponse>> RegisterSupervisorAsync(CreateSupervisorRequest request, CancellationToken cancellationToken = default)
    {

        var registerRequest = request.Adapt<RegisterRequest>();

        var user = await RegisterValidationAsync(registerRequest,ct: cancellationToken);

        if (user.IsFailure)
            return user.Error;

        var addToRoleResult = await _userManager.AddToRoleAsync(user.Value!, request.SuperviorRole);
        
        if (!addToRoleResult.Succeeded)
        {
            var error = addToRoleResult.Errors.First();
            return Error.BadRequest(error.Code, error.Description);
        }

        var code = await GenerateEmailConfirmationCodeAync(user.Value);

        var response = new RegisterResponse(code, user.Value.Id);

        return Result.Success(response);
    }
    public async Task<Result<string>> RegisterPatient(RegisterRequest request, CancellationToken ct = default)
    {
        var result = await RegisterValidationAsync(request,true, ct: ct);

        if (result.IsFailure)
            return result.Error;

        var addToRoleResult = await _userManager.AddToRolesAsync(result.Value, [DefaultRoles.Patient.Name, DefaultRoles.User.Name]);

        if (!addToRoleResult.Succeeded)
        {
            var error = addToRoleResult.Errors.First();
            return Error.BadRequest(error.Code, error.Description);
        }

        return Result.Success(result.Value.Id);
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
                return UserErrors.InvalidPassword;
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
    private async Task<string> GenerateEmailConfirmationCodeAync(ApplicationUser user)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        return code;
    }
    private async Task<Result<ApplicationUser>> RegisterValidationAsync(RegisterRequest request, bool IsConfirmed = false, CancellationToken ct = default)
    {
        if (await _context.Users.AnyAsync(e => e.Email == request.Email || e.UserName == request.UserName, ct))
            return UserErrors.DuplicatedEmail;

        var user = request.Adapt<ApplicationUser>();
        user.EmailConfirmed = IsConfirmed;
        user.Bio ??= string.Empty;
        if (request.ProfilePic is not null)
        {
            var imagePath = await _fileService.UploadImageAsync(request.ProfilePic!, false, ct);

            if (imagePath.IsFailure)
                return imagePath.Error;

            user.ProfilePicture = imagePath.Value;
        }
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();

            return Error.BadRequest(error.Code, error.Description);
        }

        return Result.Success(user);
    }
    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private async Task<Result<AuthResponse>> GenerateTokenAsync(ApplicationUser user)
    {
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpriation = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new()
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpriation
        });

        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        var (newToken, expireIn) = _jwtProvider.GenerateToken(user, roles);

        var response = new AuthResponse(AccessToken: newToken, ExpiresIn: expireIn, RefreshToken: refreshToken, RefreshTokenExpiration: refreshTokenExpriation);

        return response;
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId == null || await _userManager.FindByIdAsync(userId) is not { } user)
            return UserErrors.InvalidToken;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken == null)
            return UserErrors.InvalidToken;

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
}