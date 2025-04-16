
using Autine.Application.Contracts.Patient;
using Microsoft.EntityFrameworkCore;

namespace Autine.Infrastructure.Services;
public class UserService(ApplicationDbContext context) : IUserService
{
    public async Task<bool> CheckUserExist(string userId, CancellationToken ct = default)
        => await context.Users.AnyAsync(e => e.Id == userId, ct);

    public async Task<IEnumerable<PatientResponse>> GetPatientsAsync(string userId, CancellationToken ct = default)
        => await context.Patients
            .Where(e =>
                e.CreatedBy == userId
            )
            .Join(
                context.Users,
                p => p.PatientId,
                u => u.Id,
                (p, u) => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.UserName,
                    u.DateOfBirth,
                    u.Gender,
                    u.Country,
                    u.City,
                    p.IsSupervised
                }
            )
            .Select(e => new PatientResponse
            (
                e.Id,
                e.FirstName,
                e.LastName,
                e.Email!,
                e.UserName!,
                e.DateOfBirth,
                e.Gender,
                e.Country!,
                e.City!,
                e.IsSupervised
            ))
            .ToListAsync(ct);

}
