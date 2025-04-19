namespace Autine.Infrastructure.Repositories;

public class BotMessageRepository(ApplicationDbContext context) : Repository<BotMessage>(context), IBotMessageRepository
{
    public async new Task<Guid> AddAsync(BotMessage entity, CancellationToken ct = default)
    {
        await _context.BotMessages.AddAsync(entity, ct);

        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }
}