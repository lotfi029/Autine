namespace Autine.Infrastructure.Repositories;
public class ThreadMemberRepository(ApplicationDbContext context) : Repository<ThreadMember>(context), IThreadMemberRepository
{
    public new async Task<Guid> AddAsync(ThreadMember entity, CancellationToken ct = default)
    {
        await _context.ThreadMembers.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
        return entity.Id;
    }

}
