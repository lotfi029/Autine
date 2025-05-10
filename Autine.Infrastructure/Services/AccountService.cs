using Autine.Application.Contracts.Auth;
using Autine.Application.Contracts.Profiles;
using Autine.Application.ExternalContracts.Auth;
using Autine.Application.IServices;
using Microsoft.AspNetCore.Http;

namespace Autine.Infrastructure.Services;

public class AccountService(
    ApplicationDbContext context, 
    UserManager<ApplicationUser> userManager,
    IFileService fileService,
    IUrlGenratorService urlGenratorService) : IAccountService
{
    //get
    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken ct = default)
    {
        var userProfile = await context.Users
            .Where(e => e.Id == userId)
            .Select(x => new UserProfileResponse (
                x.Id, 
                x.FirstName, 
                x.LastName, 
                x.Bio, 
                x.Gender, 
                x.Country, 
                x.City, 
                urlGenratorService.GetImageUrl(x.ProfilePicture, false),
                x.DateOfBirth
            )).SingleOrDefaultAsync(ct);

        if (userProfile is null)
            return UserErrors.UserNotFound;

        return userProfile;
    }
    // put
    public async Task<Result<AIRegisterRequest>> UpdateProfileAsync(string userId, UpdateUserProfileRequest request, CancellationToken ct = default)
    {
        var user = await context.Users
            .Where(e => e.Id == userId)
            .Select(x => new AIRegisterRequest(
                x.Email!,
                x.Id,
                x.PasswordHash!,
                request.FirstName,
                request.LastName,
                x.DateOfBirth,
                x.Gender
                )).SingleOrDefaultAsync(ct);

        if (user is null)
            return UserErrors.UserNotFound;

        await context.Users
            .Where(e => e.Id == userId)
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.FirstName, request.FirstName)
                .SetProperty(e => e.LastName, request.LastName)
                .SetProperty(e => e.Bio, request.Bio)
                .SetProperty(e => e.City, request.City)
                .SetProperty(e => e.Country, request.Country)
                .SetProperty(e => e.Gender, request.Gender)
                .SetProperty(e => e.DateOfBirth, request.DateOfBirth),
                ct
            );

        return Result.Success(user);
    }
    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken ct = default)
    {
        if (await context.Users.FindAsync([userId], ct) is not { } user)
            return UserErrors.UserNotFound;

        var changeResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!changeResult.Succeeded)
        {
            var error = changeResult.Errors.FirstOrDefault()!;
            return Error.BadRequest(error.Code, error.Description);
        }

        return Result.Success();
    }

    public async Task<Result> ChangeProfilePictureAsync(string userId, IFormFile image, CancellationToken ct = default)
    {
        if(await context.Users.FindAsync([userId], ct)is not { } user)
            return UserErrors.UserNotFound;

        var imageUrl = await fileService.UpdateImageAsync(user.ProfilePicture, image, false, ct);

        if (imageUrl.IsFailure)
            return imageUrl.Error;

        await context.Users
            .Where(e => e.Id == userId)
            .ExecuteUpdateAsync(setters =>
            setters.SetProperty(e => e.ProfilePicture, imageUrl.Value),
            ct
            );

        return Result.Success();
    }
}