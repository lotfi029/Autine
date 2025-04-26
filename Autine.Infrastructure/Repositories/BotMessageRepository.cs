using Microsoft.Data.SqlClient;
using static Autine.Infrastructure.Persistence.DBCommands.StoredProcedures;

namespace Autine.Infrastructure.Repositories;

public class BotMessageRepository(ApplicationDbContext context) : Repository<BotMessage>(context), IBotMessageRepository
{
    public async new Task<Guid> AddAsync(BotMessage entity, CancellationToken ct = default)
    {
        await _context.BotMessages.AddAsync(entity, ct);

        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<Result> DeleteBotMessageWithRelationAsync(Guid botPatientId, CancellationToken ct = default)
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync(
                $"EXEC {BotMessageSPs.DeleteBotMessagesWithRelations}",
                [new SqlParameter(BotMessageSPs.DeleteBotMessageParameter, botPatientId)],
                ct);

            return Result.Success();
        }
        catch
        {
            // TODO: log error
            return Error.InternalServerError("Error.DeleteBotMessage", "Error while delete the chat.");
        }
    }
}