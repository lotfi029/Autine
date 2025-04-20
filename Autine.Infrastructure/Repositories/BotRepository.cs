namespace Autine.Infrastructure.Repositories;
public class BotRepository(ApplicationDbContext context) : Repository<Bot>(context), IBotRepository
{
    public async new Task<Guid> AddAsync(Bot entity, CancellationToken ct = default)
    {
        await _context.Bots.AddAsync(entity, ct);

        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }

    
}
