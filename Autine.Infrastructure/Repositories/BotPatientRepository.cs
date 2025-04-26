using Autine.Infrastructure.Persistence.DBCommands;
using Microsoft.Data.SqlClient;

namespace Autine.Infrastructure.Repositories;
public class BotPatientRepository(ApplicationDbContext context) : Repository<BotPatient>(context), IBotPatientRepository
{
    public async Task<IEnumerable<BotMessage>> GetMessagesAsync(Guid botPatientId, CancellationToken ct = default)
        => await _context.BotMessages
            .AsNoTracking()
            .Include(e => e.Message)
            .Where(e => e.BotPatientId == botPatientId)
            .ToListAsync(ct);
    

    public async Task<Result> DeleteBotPatientAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC {StoredProcedures.BotPatientSPs.DeleteBotPatientWithRelations}",
                [new SqlParameter("@BotPatientId", id)],
                ct);
            return Result.Success();
        }
        catch
        {
            // TODO:log error
            return Error.BadRequest("Exception", "error occure while delete bot");
        }
    }
}