using Autine.Application.Contracts.Users;
using Microsoft.EntityFrameworkCore.Storage;
using static Autine.Infrastructure.Identity.Consts.DefaultRoles;
using static Autine.Infrastructure.Persistence.DBCommands.StoredProcedures;
namespace Autine.Infrastructure.Services;
public class UserService(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    IFileService fileService,
    IUrlGenratorService urlGenratorService) : IUserService
{
    public async Task<bool> CheckUserExist(string userId, CancellationToken ct = default)
        => await context.Users.AnyAsync(e => e.Id == userId, ct);

    
    public async Task<Result<string>> DeleteUserAsync(string userId, CancellationToken ct = default, IDbContextTransaction? existingTransaction = null)
    {
        if (await context.Users.FindAsync([userId], ct) is not { } user)
            return UserErrors.UserNotFound;

        var userRole = await userManager.GetRolesAsync(user);
        
        var image = await context.Users
            .Where(e => e.Id == userId)
            .Select(e => e.ProfilePicture)
            .SingleOrDefaultAsync(ct);

        
        var useLocalTransaction = existingTransaction == null;
        var transaction = existingTransaction ?? await context.Database.BeginTransactionAsync(ct);

        try
        {
            //if (userRole.Contains(Admin.Name, StringComparer.OrdinalIgnoreCase))
            //{
            //    await context.Database.ExecuteSqlRawAsync(
            //        AdminSPs.DeleteAdminWithRelationCall,
            //        [AdminSPs.DeleteAdminWithRelationParamter(userId)],
            //        ct
            //    );

            //    await fileService.DeleteImageAsync(image!, false);
            //    if (useLocalTransaction)
            //        await transaction.CommitAsync(ct);

            //    return Admin.Name;
            //}
            //if (userRole.Contains(DefaultRoles.Patient.Name, StringComparer.OrdinalIgnoreCase))
            //{
            //    await context.Database.ExecuteSqlRawAsync(
            //        PatientSPs.DeletePatientWithRelationCall,
            //        [PatientSPs.DeletePatientWithRelationParamter(userId)],
            //        ct
            //    );

            //    await fileService.DeleteImageAsync(image!, false);
            //    if (useLocalTransaction)
            //        await transaction.CommitAsync(ct);

            //    return DefaultRoles.User.Name;
            //}

            //var role = string.Empty;
            //if (userRole.Contains("supervisor", StringComparer.OrdinalIgnoreCase))
            //{
            //    await context.Database.ExecuteSqlRawAsync(
            //        SupervisorSPs.DeleteSupervisorRelationsCall,
            //        [SupervisorSPs.DeleteSupervisorRelationsParamter(userId)],
            //        ct
            //    );
            //    role = "supervisor";
            //}

            //await context.Database.ExecuteSqlRawAsync(
            //    UserSPs.DeleteUserWithRelationCall,
            //    [UserSPs.DeleteUserWithRelationParamter(userId)],
            //    ct
            //);
            //role = role == string.Empty ? "user" : role;


            await context.Database.ExecuteSqlRawAsync(
                DeleteUserSPs.DeleteUserWithAllRelationsCall,
                DeleteUserSPs.DeleteUserWithAllRelationsParamter(userId),
                ct
                );

            await fileService.DeleteImageAsync(image!, false);

            if (useLocalTransaction)
                await transaction.CommitAsync(ct);


            var role = userRole.Contains(Admin.Name, StringComparer.OrdinalIgnoreCase) ? Admin.Name :
                userRole.Contains(Doctor.Name, StringComparer.OrdinalIgnoreCase) ? Doctor.Name :
                userRole.Contains(Parent.Name, StringComparer.OrdinalIgnoreCase) ? Parent.Name :
                userRole.Contains(DefaultRoles.Patient.Name, StringComparer.OrdinalIgnoreCase) ? DefaultRoles.Patient.Name :
                DefaultRoles.User.Name;

            return Result.Success(role);
        }
        catch
        {
            // TODO: log error
            if (useLocalTransaction)
                await transaction.RollbackAsync(ct);

            return Error.BadRequest("Error.DeleteUser", "error occure while deleting user.");
        }
    }

    public async Task<Result<DetailedUserResponse>> GetAsync(string id, CancellationToken ct = default)
    {
        var response = await (
            from u in context.Users
            join ur in context.UserRoles
            on u.Id equals ur.UserId
            join r in context.Roles
            on ur.RoleId equals r.Id into rls
            where u.Id == id && u.Id != "019409bf-3ae7-7cdf-995b-db4620f2ff5f"
            select new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.UserName,
                u.Bio,
                u.Gender,
                u.ProfilePicture,
                roles = rls.Select(e => e.Name)
            })
            .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.UserName, u.Bio, u.Gender, u.ProfilePicture })
            .Select(x => new DetailedUserResponse
            (
                x.Key.Id,
                x.Key.FirstName,
                x.Key.LastName,
                x.Key.UserName,
                x.Key.Bio,
                x.Key.Gender,
                urlGenratorService.GetImageUrl(x.Key.ProfilePicture, false)!,
                x.SelectMany(e => e.roles).ToList()
            ))
            .SingleOrDefaultAsync(ct);


        if (response == null)
            return UserErrors.UserNotFound;

        return Result.Success(response);
    }
    public async Task<IEnumerable<UserResponse>> GetAllAsync(string userId, string? roleId, CancellationToken cancellationToken = default)
    {
        var response = await (
        from u in context.Users
        join ur in context.UserRoles
        on u.Id equals ur.UserId
        where u.Id != userId && u.Id != "019409bf-3ae7-7cdf-995b-db4620f2ff5f"
        where ur.RoleId == roleId || roleId == null
        select new
        {
            u.Id,
            u.FirstName,
            u.LastName,
            u.ProfilePicture
        })
        .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.ProfilePicture })
        .Select(x => new UserResponse
        (
            x.Key.Id,
            x.Key.FirstName,
            x.Key.LastName,
            x.Key.ProfilePicture
        ))
        .ToListAsync(cancellationToken);


        return response;
    }
}

