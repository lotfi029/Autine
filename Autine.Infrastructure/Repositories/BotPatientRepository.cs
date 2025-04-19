namespace Autine.Infrastructure.Repositories;
public class BotPatientRepository(ApplicationDbContext context) : Repository<BotPatient>(context), IBotPatientRepository
{    
    public async Task<IEnumerable<BotMessage>> GetMessagesAsync(Guid botPatientId, CancellationToken ct = default)
        => await _context.BotMessages
            .AsNoTracking()
            .Include(e => e.Message)
            .Where(e => e.BotPatientId == botPatientId)
            .ToListAsync(ct);
}
