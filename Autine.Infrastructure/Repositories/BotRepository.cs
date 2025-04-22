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
        var botPatients = _context.BotPatients
            .Where(e => e.BotId == bot.Id)
            .Join(_context.BotMessages.Include(e => e.Message),
            bp => bp.Id,
            bm => bm.BotPatientId,
            (bp, bm) => new BotPatient
            {
                Id = bp.Id,
                BotId = bp.BotId,
                PatientId = bp.PatientId,
                BotMessages = new List<BotMessage> 
                {
                    bm
                },
                CreatedBy = bp.CreatedBy,
                CreatedAt = bp.CreatedAt,
                IsDisabled = bp.IsDisabled
            });



        var botMessage = _context.BotMessages
            .Where(e => e.BotPatient.BotId == bot.Id)
            .Include(e => e.Message);

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
