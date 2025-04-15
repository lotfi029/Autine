namespace Autine.Application.Interfaces;
public interface IRoleService
{
    Task<Result> CheckUserInRoleAsync(string userId, string roleName);
    Task<Result> UserIsAdminAsync(string userId);
    Task<Result> UserIsSupervisorAsync(string userId);
}
