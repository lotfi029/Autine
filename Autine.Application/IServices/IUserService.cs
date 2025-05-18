using Autine.Application.Contracts.Chats;
using Autine.Application.Contracts.UserBots;
using Autine.Application.Contracts.Users;
using Microsoft.EntityFrameworkCore.Storage;

namespace Autine.Application.IServices;
public interface IUserService
{
    Task<bool> CheckUserExist(string userId, CancellationToken ct = default);
    Task<Result<DetailedUserResponse>> GetAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<UserResponse>> GetAllAsync(string userId, string? roleId, CancellationToken cancellationToken = default);
    Task<Result<string>> DeleteUserAsync(string userId, CancellationToken ct = default, IDbContextTransaction? existingTransaction = null);
    Task<IEnumerable<UserChatResponse>> GetAllUserChatAsync(string userId, CancellationToken ct = default);
    Task<Result<DetailedChatResponse>> GetChatByIdAsync(string userId, Guid Id, CancellationToken ct = default);
    Task<IEnumerable<ChatResponse>> GetAllChatsAsync(string userId, CancellationToken ct = default);
}
