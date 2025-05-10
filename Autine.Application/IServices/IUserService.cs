using Autine.Application.Contracts.Users;
using Microsoft.EntityFrameworkCore.Storage;

namespace Autine.Application.IServices;
public interface IUserService
{
    Task<bool> CheckUserExist(string userId, CancellationToken ct = default);
    Task<Result<DetailedUserResponse>> GetAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<UserResponse>> GetAllAsync(string userId, string? roleId, CancellationToken cancellationToken = default);
    Task<Result<string>> DeleteUserAsync(string userId, CancellationToken ct = default, IDbContextTransaction? existingTransaction = null);
}
