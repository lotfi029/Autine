//namespace Autine.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore.Infrastructure;

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

    public async Task<Result> UserIsSupervisorAsync(string userId)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return UserErrors.UserNotFound;
        if (await userManager.IsInRoleAsync(user, DefaultRoles.Parent.Name) 
            || await userManager.IsInRoleAsync(user, DefaultRoles.Doctor.Name))
            return Result.Success();
        return RoleErrors.UserNotFound;
    }
}
