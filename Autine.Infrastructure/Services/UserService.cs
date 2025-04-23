using Autine.Application.Contracts.Bots;
using Autine.Application.Contracts.Patients;
using Autine.Application.Contracts.Profiles;

namespace Autine.Infrastructure.Services;
public class UserService(
    ApplicationDbContext context) : IUserService
{
    public async Task<bool> CheckUserExist(string userId, CancellationToken ct = default)
        => await context.Users.AnyAsync(e => e.Id == userId, ct);


    public async Task<IEnumerable<PatientResponse>> GetPatientsAsync(string userId, bool isFollowing = false, CancellationToken ct = default)
    {

        var query = await (
            from tm in context.ThreadMembers
            join t in context.Patients
            on tm.PatientId equals t.Id
            join u in context.Users
            on t.PatientId equals u.Id
            where
            (isFollowing && tm.MemberId == userId && t.CreatedBy != userId)
            ||
            (!isFollowing && tm.MemberId == userId && tm.CreatedBy == userId)
            select new PatientResponse(
            t.Id,
            u.FirstName,
            u.LastName,
            u.Email!,
            u.UserName!,
            u.DateOfBirth,
            u.Gender,
            u.Country!,
            u.City!
            )).ToListAsync(cancellationToken: ct);

        if (query is null)
            return [];

        return query;
    }
    public async Task<PatientResponse?> GetPatientByIdAsync(string userId, Guid id, CancellationToken ct = default)
        => await (
            from tm in context.ThreadMembers
            join t in context.Patients
            on tm.PatientId equals t.Id
            join u in context.Users
            on t.PatientId equals u.Id
            where t.Id == id && (tm.MemberId == userId)
            select new PatientResponse(
                    t.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email!,
                    u.UserName!,
                    u.DateOfBirth,
                    u.Gender,
                    u.Country!,
                    u.City!
            ))
            .SingleOrDefaultAsync(ct);

    public async Task<IEnumerable<BotPatientResponse>> GetBotPatientAsync(Guid botId, CancellationToken ct = default)
        => await (
            from p in context.Patients.Where(e => !e.IsDisabled)
            join u in context.Users.Where(e => !e.IsDisabled)
            on p.PatientId equals u.Id
            join bp in context.BotPatients.Where(e => !e.IsDisabled)
            on p.Id equals bp.PatientId
            where bp.BotId == botId
            select new BotPatientResponse(
                bp.Id,
                $"{u.FirstName} {u.LastName}",
                bp.CreatedAt,
                u.ProfilePicture
                )
            ).ToListAsync(ct);

    public async Task<Result> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        await context.Users
            .Where(e => e.Id == userId)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.IsDisabled, true), ct);

        return Result.Success();
    }
    //get
    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken ct = default)
    {
        var userProfile = await context.Users
            .Where(e => e.Id == userId)
            .ProjectToType<UserProfileResponse>()
            .SingleOrDefaultAsync(ct);

        if (userProfile is null)
            return UserErrors.UserNotFound;

        return userProfile;
    }
    // put
    public async Task<Result> UpdateProfileAsync(string userId, UpdateUserProfileRequest request, CancellationToken ct = default)
    {
        if (!await context.Users.AnyAsync(e => e.Id == userId, ct))
            return UserErrors.UserNotFound;

        await context.Users
            .ExecuteUpdateAsync(setters =>
                setters
                .SetProperty(e => e.FirstName, request.FirstName)
                .SetProperty(e => e.LastName, request.LastName)
                .SetProperty(e => e.Bio, request.Bio)
                .SetProperty(e => e.City, request.City)
                .SetProperty(e => e.Country, request.Country),
                ct
            );

        return Result.Success();
    }
}
