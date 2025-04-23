namespace Autine.Application.Interfaces;
public interface IRoleService
{
    Task<Result> CheckUserInRoleAsync(string userId, string roleName);
    Task<Result> UserIsAdminAsync(string userId);
    Task<bool> UserIsSupervisorAsync(string userId);
    Task<Result<string>> GetUserRoleAsync(string userId);
}
