using Autine.Application.Contracts.Profiles;
using Autine.Application.ExternalContracts.Auth;

namespace Autine.Application.Interfaces;
public interface IUserService
{
    Task<bool> CheckUserExist(string userId, CancellationToken ct = default);
    

    Task<Result> DeleteUserAsync(string userId, CancellationToken ct = default);

    // get
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken ct = default);
    // put
    Task<Result<AIRegisterRequest>> UpdateProfileAsync(string userId, UpdateUserProfileRequest request, CancellationToken ct = default);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken ct = default);
}
