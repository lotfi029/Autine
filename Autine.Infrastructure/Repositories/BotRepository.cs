namespace Autine.Infrastructure.Repositories;
public class BotRepository(ApplicationDbContext context) : Repository<Bot>(context), IBotRepository
{
    public async new Task<Guid> AddAsync(Bot entity, CancellationToken ct = default)
    {
        await _context.Bots.AddAsync(entity, ct);

        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task DeleteBotAsync(Bot bot, CancellationToken ct = default)
    {
        var botPatients = await _context.BotPatients
            .Where(e => e.BotId == bot.Id)
            .Include(e => e.BotMessages)
            .ThenInclude(e => e.Message)
            .ToListAsync(ct);

        foreach (var botPatient in botPatients)
        {
            if (botPatient.BotMessages != null)
            {
                _context.BotMessages.RemoveRange(botPatient.BotMessages);
                _context.Messages.RemoveRange(botPatient.BotMessages.Select(e => e.Message));
            }
        }

        _context.BotPatients.RemoveRange(botPatients);
        _context.Bots.Remove(bot);

        await _context.SaveChangesAsync(ct);
    }
}
