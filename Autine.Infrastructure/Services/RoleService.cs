using Microsoft.Extensions.ObjectPool;

namespace Autine.Infrastructure.Services;
public class RoleService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : IRoleService
{
    public async Task<Result> CheckUserInRoleAsync(string userId, string roleName)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return UserErrors.UserNotFound;

        if (await roleManager.RoleExistsAsync(roleName))
            return RoleErrors.RoleNotFound;

        if (await userManager.IsInRoleAsync(user, roleName))
            return Result.Success();

        return RoleErrors.UserNotFound;
    }

    public async Task<Result> UserIsAdminAsync(string userId)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return UserErrors.UserNotFound;
        if (await userManager.IsInRoleAsync(user, DefaultRoles.Admin.Name))
            return Result.Success();
        return RoleErrors.UserNotFound;
    }

    public async Task<bool> UserIsSupervisorAsync(string userId)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return false;
        if (await userManager.IsInRoleAsync(user, DefaultRoles.Parent.Name)
            || await userManager.IsInRoleAsync(user, DefaultRoles.Doctor.Name))
            return true;
        return false;
    }

    public async Task<Result<string>> GetUserRoleAsync(string userId)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return UserErrors.UserNotFound;

        var result = await userManager.GetRolesAsync(user);

        if (result.Contains(DefaultRoles.Admin.Name, StringComparer.OrdinalIgnoreCase))
            return DefaultRoles.Admin.Name.ToLower();

        if (result.Contains(DefaultRoles.Parent.Name, StringComparer.OrdinalIgnoreCase))
            return "supervisor";
        
        if (result.Contains(DefaultRoles.Doctor.Name, StringComparer.OrdinalIgnoreCase))
            return "supervisor";

        
        return DefaultRoles.User.Name.ToLower();
    }
}
