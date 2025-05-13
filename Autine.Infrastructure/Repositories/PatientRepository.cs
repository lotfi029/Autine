namespace Autine.Infrastructure.Repositories;
public class PatientRepository(ApplicationDbContext context) : Repository<Patient>(context), IPatientRespository
{
    public async Task<IEnumerable<Patient>> ArePatientsAsync(IList<string> ids, CancellationToken ct = default)
    {
        var patients = await _context.Patients
            .Where(e => ids.Contains(e.PatientId))
            .ToListAsync(ct);

        return ids.Count == patients.Count ? patients : Enumerable.Empty<Patient>();
    }
    public async Task<IEnumerable<Patient>> GetAllThreads(string userId, CancellationToken ct = default)
        => await GetThreadsAsync(userId, Guid.Empty, ct);
    public async Task<Patient?> GetThreadByIdAsync(string userId, string id, CancellationToken ct = default)
    {
        var thread = await _context.Patients
            .Include(e => e.Members)
            .SingleOrDefaultAsync(e => e.PatientId == id, ct) ?? null!;

        return thread;
    }
    public Task<Result> DeletePatientAsync(string id, CancellationToken ct = default)
    {

        throw new NotImplementedException();
    }
    private async Task<IEnumerable<Patient>> GetThreadsAsync(string userId, Guid id, CancellationToken ct = default)
    {
        var threads = await _context.ThreadMembers
            .Where(e => e.MemberId == userId)
            .Join(_context.Patients.Where(e => id == Guid.Empty || e.Id == id),
                tm => tm.ThreadId,
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