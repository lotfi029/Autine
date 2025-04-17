namespace Autine.Infrastructure.Repositories;
public class PatientRepository(ApplicationDbContext context) : Repository<Patient>(context), IPatientRespository
{
    public async Task<IEnumerable<Patient>> GetAllThreads(string userId, CancellationToken ct = default)
        => await GetThreadsAsync(userId, Guid.Empty, ct);
    public async Task<Patient?> GetThreadByIdAsync(string userId, Guid id, CancellationToken ct = default)
    {
        var thread = await _context.Patients
            .Include(e => e.Members)
            .SingleOrDefaultAsync(e => e.Id == id, ct) ?? null!;

        return thread;
    }
    private async Task<IEnumerable<Patient>> GetThreadsAsync(string userId, Guid id, CancellationToken ct = default)
    {
        var threads = await _context.ThreadMembers
            .Where(e => e.UserId == userId)
            .Join(_context.Patients.Where(e => id == Guid.Empty || e.Id == id),
                tm => tm.PatientId,
                p => p.Id,
                (tm, p) => new Patient()
                {
                    Id = p.Id,
                    PatientId = p.PatientId,
                    IsSupervised = p.IsSupervised,
                    ThreadTitle = p.ThreadTitle,
                    CreatedAt = p.CreatedAt,
                    CreatedBy = p.CreatedBy
                }
            ).ToListAsync(ct);


        return threads;
    }
}