using Autine.Application.Contracts.Chats;
using Autine.Application.Contracts.UserBots;
using Autine.Application.Contracts.Users;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
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
            await context.Database.ExecuteSqlRawAsync(
                DeleteUserSPs.DeleteUserWithAllRelationsCall,
                DeleteUserSPs.DeleteUserWithAllRelationsParamter(userId),
                ct
                );

            await fileService.DeleteImageAsync(image!, false);

            if (useLocalTransaction)
                await transaction.CommitAsync(ct);


            var role = userRole.Contains(Admin.Name, StringComparer.OrdinalIgnoreCase) ? Admin.Name :
                userRole.Contains(Doctor.Name, StringComparer.OrdinalIgnoreCase) ? "supervisor" :
                userRole.Contains(Parent.Name, StringComparer.OrdinalIgnoreCase) ? "supervisor" :
                userRole.Contains(DefaultRoles.Patient.Name, StringComparer.OrdinalIgnoreCase) ? DefaultRoles.User.Name :
                DefaultRoles.User.Name;

            return Result.Success(role.ToLower());
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

    public async Task<IEnumerable<UserChatResponse>> GetAllUserChatAsync(string userId, CancellationToken ct = default)
    {
        var q1 = await (
            from u in context.Users
            join c in context.Chats
            on u.Id equals c.UserId
            where c.CreatedBy == userId || c.UserId == userId
            select new UserChatResponse(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Bio,
                urlGenratorService.GetImageUrl(u.ProfilePicture, false)!
                )
            ).ToListAsync(ct);
        
        var q2 = await (
            from u in context.Users
            join c in context.Chats
            on u.Id equals c.CreatedBy
            where c.UserId == userId || c.CreatedBy == userId
            select new UserChatResponse(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Bio,
                urlGenratorService.GetImageUrl(u.ProfilePicture, false)!
                )
            ).ToListAsync(ct);


        var response = q1.Union(q2);

        if (response == null || !response.Any())
        {
            var userChat = await context.Users
                .Where(e => e.Id == userId)
                .Select(e => new UserChatResponse(
                    e.Id,
                    e.FirstName,
                    e.LastName,
                    e.Bio,
                    urlGenratorService.GetImageUrl(e.ProfilePicture, false)!
                    ))
                .ToListAsync(ct);

            return userChat;
        }

        return response;
    }
    public async Task<Result<DetailedChatResponse>> GetChatByIdAsync(string userId, Guid Id, CancellationToken ct = default)
    {
        var chat = await context.Chats
            .Include(e => e.Messages)
            .Where(e => e.Id == Id)
            .SingleOrDefaultAsync(ct);

        if (chat is null)
            return ChatErrors.ChatNotFound;

        var memberId = userId.Equals(chat.UserId, StringComparison.OrdinalIgnoreCase) ? chat.CreatedBy : chat.UserId;

        if (await context.Users.FindAsync([userId], ct) is not { } user)
            return ChatErrors.UserNotExist;

        var response = new DetailedChatResponse(
            chat.Id,
            $"{user.FirstName} {user.LastName}",
            memberId,
            urlGenratorService.GetImageUrl(user.ProfilePicture, false)!,
            chat.CreatedAt,
            chat.Messages.Select(m => new MessageResponse(
                m.Id,
                m.Content,
                m.CreatedDate,
                m.Status,
                userId == m.SenderId
                )).ToList() ?? []
            );

        return Result.Success(response);
    }

    public async Task<IEnumerable<ChatResponse>> GetAllChatsAsync(string userId, CancellationToken ct = default)
    {
        var chats = await context.Chats
            .Where(e => e.UserId == userId || e.CreatedBy == userId)
            .Select(e => new ChatResponse(
                e.Id,
                "",
                e.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase) ? e.CreatedBy : e.UserId,
                "",
                e.CreatedAt
                ))
            .ToListAsync(ct);

        if (chats == null || chats.Count == 0)
            return [];

        var users = await context.Users
            .Where(e => chats.Select(c => c.UserId).Contains(e.Id))
            .Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.ProfilePicture
            }).ToListAsync(ct);


        var response = chats
            .Join(users,
                c => c.UserId,
                u => u.Id,
                (c, u) => new ChatResponse(
                    c.Id,
                    $"{u.FirstName} {u.LastName}",
                    u.Id,
                    urlGenratorService.GetImageUrl(u.ProfilePicture, false)!,
                    c.CreatedAt
                    )
                );

        return response;
    }
}

