using Autine.Domain.Entities;

namespace Autine.Infrastructure.Repositories;
public class BotPatientRepository(ApplicationDbContext context) : Repository<BotPatient>(context), IBotPatientRepository
{    
    public async Task<IEnumerable<BotMessage>> GetMessagesAsync(Guid botPatientId, CancellationToken ct = default)
        => await _context.BotMessages
            .AsNoTracking()
            .Include(e => e.Message)
            .Where(e => e.BotPatientId == botPatientId)
            .ToListAsync(ct);


    public async Task<Result> DeleteBotPatientAsync(BotPatient bot, CancellationToken ct = default)
    {
        var botMessages = await _context.BotMessages
            .Where(e => e.BotPatientId == bot.Id)
            .Include(e => e.Message)
            .ToListAsync(ct);
        
        _context.RemoveRange(botMessages);
        _context.RemoveRange(botMessages.Select(e => e.Message));
        _context.Remove(bot);

        //await _context.SaveChangesAsync(ct);

        return Result.Success();
    }

    //public async Task<Result> DeleteAllBotPatientAsync(Bot bot, CancellationToken ct = default)
    //{

    //}
}
