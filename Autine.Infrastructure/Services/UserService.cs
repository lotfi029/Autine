using Autine.Application.Contracts.Auths;
using Autine.Application.Contracts.Profiles;
using Autine.Application.ExternalContracts.Auth;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Autine.Infrastructure.Services;
public class UserService(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<bool> CheckUserExist(string userId, CancellationToken ct = default)
        => await context.Users.AnyAsync(e => e.Id == userId, ct);


    public async Task<Result> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        await context.Users
            .Where(e => e.Id == userId)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsDisabled, true), ct);

        return Result.Success();
    }
    //get
    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken ct = default)
    {
        var userProfile = await context.Users
            .Where(e => e.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .SingleOrDefaultAsync(ct);

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
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.FirstName, request.FirstName)
                .SetProperty(e => e.LastName, request.LastName)
                .SetProperty(e => e.Bio, request.Bio)
                .SetProperty(e => e.City, request.City)
                .SetProperty(e => e.Country, request.Country),
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
            var errors = changeResult.Errors.FirstOrDefault()!;
            return Error.BadRequest(errors.Code, errors.Description);
        }

        return Result.Success();
    }
}
