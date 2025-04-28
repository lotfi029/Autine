using Autine.Application.Contracts.Users;
using static Autine.Infrastructure.Persistence.DBCommands.StoredProcedures;

namespace Autine.Infrastructure.Services;
public class UserService(
    ApplicationDbContext context,
    IRoleService roleService,
    IFileService fileService,
    IUrlGenratorService urlGenratorService) : IUserService
{
    public async Task<bool> CheckUserExist(string userId, CancellationToken ct = default)
        => await context.Users.AnyAsync(e => e.Id == userId, ct);


    public async Task<Result<string>> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        var userRole = await roleService.GetUserRoleAsync(userId);

        if (userRole.IsFailure)
            return UserErrors.UserNotFound;

        var image = await context.Users
            .Where(e => e.Id == userId)
            .Select(e => e.ProfilePicture)
            .SingleOrDefaultAsync(ct);


        try 
        {
            if (userRole.Value.Equals(DefaultRoles.Admin.Name))
            {
                await context.Database.ExecuteSqlRawAsync(
                    AdminSPs.DeleteAdminWithRelationCall,
                    [AdminSPs.DeleteAdminWithRelationParamter(userId)],
                    ct
                    );

                return Result.Success(userRole.Value);
            }
            if (userRole.Value.Equals("supervisor"))
                await context.Database.ExecuteSqlRawAsync(
                    SupervisorSPs.DeleteSupervisorRelationsCall,
                    [SupervisorSPs.DeleteSupervisorRelationsParamter(userId)],
                    ct
                    );

            else if (userRole.Value.Equals(DefaultRoles.Patient.Name))
                await context.Database.ExecuteSqlRawAsync(
                    AdminSPs.DeleteAdminWithRelationCall,
                    [AdminSPs.DeleteAdminWithRelationParamter(userId)],
                    ct
                    );

            await context.Database.ExecuteSqlRawAsync(
                    UserSPs.DeleteUserWithRelationCall,
                    [UserSPs.DeleteUserWithRelationParamter(userId)],
                    ct
                    );

            await fileService.DeleteImageAsync(image!, ct);

            return Result.Success(userRole.Value);
        }
        catch
        {
            // TODO: log error
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
            where u.Id == id
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
                urlGenratorService.GetImageUrl(x.Key.ProfilePicture)!,
                x.SelectMany(e => e.roles).ToList()
            ))
            .SingleOrDefaultAsync(ct);


        if (response == null)
            return UserErrors.UserNotFound;

        return Result.Success(response);
    }
    public async Task<IEnumerable<UserResponse>> GetAllAsync(string roleId, CancellationToken cancellationToken = default)
    {
        var response = await (
        from u in context.Users
        join ur in context.UserRoles
        on u.Id equals ur.UserId
        where ur.RoleId == roleId
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

