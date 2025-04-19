using Autine.Application.Contracts.Bots;
using Autine.Application.Contracts.Patients;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Autine.Infrastructure.Services;
public class UserService(ApplicationDbContext context) : IUserService
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
            (isFollowing && tm.UserId == userId && t.CreatedBy != userId)
            ||
            (!isFollowing && tm.UserId == userId && tm.CreatedBy == userId)
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
            where t.Id == id && (tm.UserId == userId)
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

    public async Task<IEnumerable<BotPatientResponse>> GetBotPatientAsync(Guid[] ids, CancellationToken ct = default)
        => await (
            from p in context.Patients
            join u in context.Users
            on p.PatientId equals u.Id
            where ids.Contains(p.Id)
            select new BotPatientResponse(
                p.Id,
                $"{u.FirstName} {u.LastName}",
                p.CreatedAt,
                u.ProfilePicture
                )
            ).ToListAsync(ct);


}
