using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
        var botMessages = _context.BotMessages
            .Where(e => e.BotPatientId == bot.Id)
            .Include(e => e.Message);
        
        _context.RemoveRange(botMessages);
        _context.RemoveRange(botMessages.Select(e => e.Message));
        _context.Remove(bot);


        return Result.Success();
    }
}
class CreateSTP (ApplicationDbContext _dbContext) 
{
    public async Task CreateBotDeletionDbObjectsAsync()
    {
        // Check if the stored procedure already exists
        var spExists = await CheckIfStoredProcedureExistsAsync("DeleteBotWithRelations");
        var triggerExists = await CheckIfTriggerExistsAsync("tr_BotMessage_Delete");

        // Create the stored procedure if it doesn't exist
        if (!spExists)
        {
            var createSpSql = @"
                CREATE PROCEDURE dbo.DeleteBotWithRelations
                    @BotId uniqueidentifier
                AS
                BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRANSACTION;
    
                    BEGIN TRY
                        -- Identify related Messages
                        DECLARE @MessageIds TABLE (Id uniqueidentifier)
                        INSERT INTO @MessageIds
                        SELECT MessageId FROM BotMessages
                        WHERE BotPatientId IN (SELECT Id FROM BotPatients WHERE BotId = @BotId)
        
                        -- Delete BotMessages
                        DELETE FROM BotMessages 
                        WHERE BotPatientId IN (SELECT Id FROM BotPatients WHERE BotId = @BotId)
        
                        -- Delete related Messages
                        DELETE FROM Messages 
                        WHERE Id IN (SELECT Id FROM @MessageIds)
        
                        -- Delete BotPatients
                        DELETE FROM BotPatients 
                        WHERE BotId = @BotId
        
                        -- Delete Bot
                        DELETE FROM Bots 
                        WHERE Id = @BotId
        
                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END";

            await _dbContext.Database.ExecuteSqlRawAsync(createSpSql);
        }

        // Create the trigger if it doesn't exist
        if (!triggerExists)
        {
            var createTriggerSql = @"
                CREATE TRIGGER tr_BotMessage_Delete
                ON BotMessages
                AFTER DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;
    
                    -- Delete associated Messages that are no longer referenced
                    DELETE FROM Messages
                    WHERE Id IN (
                        SELECT MessageId FROM deleted
                    ) AND NOT EXISTS (
                        -- Keep messages that are still referenced elsewhere
                        SELECT 1 FROM BotMessages WHERE MessageId = Messages.Id
                    );
                END";

            await _dbContext.Database.ExecuteSqlRawAsync(createTriggerSql);
        }
    }

    // Helper method to check if a stored procedure exists
    private async Task<bool> CheckIfStoredProcedureExistsAsync(string procedureName)
    {
        var query = @"
                SELECT COUNT(1) 
                FROM sys.procedures 
                WHERE name = @name";

        var result = await _dbContext.Database.SqlQueryRaw<int>(
            query,
            new SqlParameter("@name", procedureName))
            .FirstOrDefaultAsync();

        return result > 0;
    }

    // Helper method to check if a trigger exists
    private async Task<bool> CheckIfTriggerExistsAsync(string triggerName)
    {
        var query = @"
            SELECT COUNT(1) 
            FROM sys.triggers 
            WHERE name = @name";

        var result = await _dbContext.Database.SqlQueryRaw<int>(
            query,
            new SqlParameter("@name", triggerName))
            .FirstOrDefaultAsync();

        return result > 0;
    }
}