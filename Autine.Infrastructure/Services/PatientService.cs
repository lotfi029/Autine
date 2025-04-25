using Autine.Application.Contracts.Bots;
using Autine.Application.Contracts.Patients;

namespace Autine.Infrastructure.Services;

public class PatientService(ApplicationDbContext context) : IPatientService
{

    public async Task<IEnumerable<PatientResponse>> GetPatientsAsync(string userId, bool isFollowing = false, CancellationToken ct = default)
    {

        var query = await (
            from tm in context.ThreadMembers
            join t in context.Patients
            on tm.ThreadId equals t.Id
            join u in context.Users
            on t.PatientId equals u.Id
            where
            (isFollowing && tm.MemberId == userId && t.CreatedBy != userId)
            ||
            (!isFollowing && tm.MemberId == userId && tm.CreatedBy == userId)
            select new PatientResponse(
            u.Id,
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
    public async Task<PatientResponse?> GetPatientByIdAsync(string userId, string id, CancellationToken ct = default)
        => await (
            from t in context.Patients
            join u in context.Users
            on t.PatientId equals u.Id
            join tm in context.ThreadMembers
            on t.Id equals tm.ThreadId
            where t.PatientId == id && (tm.MemberId == userId)
            select new PatientResponse(
                    u.Id,
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
            from p in context.Patients
            join u in context.Users
            on p.PatientId equals u.Id
            join bp in context.BotPatients
            on p.PatientId equals bp.UserId
            where bp.BotId == botId
            select new BotPatientResponse(
                bp.Id,
                $"{u.FirstName} {u.LastName}",
                bp.CreatedAt,
                u.ProfilePicture
                )
            ).ToListAsync(ct);

}