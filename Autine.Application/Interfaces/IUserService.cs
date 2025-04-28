using Autine.Application.Contracts.Users;

namespace Autine.Application.Interfaces;
public interface IUserService
{
    Task<bool> CheckUserExist(string userId, CancellationToken ct = default);
    Task<Result<DetailedUserResponse>> GetAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<UserResponse>> GetAllAsync(string roleId, CancellationToken cancellationToken = default);
    Task<Result<string>> DeleteUserAsync(string userId, CancellationToken ct = default);
}
