namespace Autine.Infrastructure.Repositories;
public class PatientRepository(ApplicationDbContext context) : Repository<Patient>(context), IPatientRespository
{
    public async Task<IEnumerable<Patient>> GetAllThreads(string userId, CancellationToken ct = default)
    {
        var threads = await _context.ThreadMembers
            .Where(e => e.UserId == userId)
            .Join(_context.Patients,
                tm => tm.PatientId,
                p => p.Id,
                (tm, p) => new Patient() 
                {
                    Id = p.Id,
                    PatientId = p.PatientId,
                    IsSupervised = p.IsSupervised,
                    ThreadTitle = p.ThreadTitle,
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy,
                    Members = null!

                }
            ).ToListAsync(ct);


        return threads;
    }

}