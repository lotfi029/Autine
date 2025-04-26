namespace Autine.Infrastructure.Persistence.DBCommands;
public class StoredProcedures
{
    public static class BotSPs
    {
        public const string DeleteBotWithRelations = $"dbo.{nameof(DeleteBotWithRelations)}";
    }
    public static class BotPatientSPs
    {
        public const string DeleteBotPatientWithRelations = $"dbo.{nameof(DeleteBotPatientWithRelations)} @BotPatientId";

    }
    public static class BotMessageSPs
    {
        public const string DeleteBotMessagesWithRelations = $"dbo.{nameof(DeleteBotMessagesWithRelations)} @BotPatientId";
        public const string DeleteBotMessageParameter = "@BotPatientId";
        public static string DeleteBotMessagesWithRelationsProcedure => @"
            CREATE OR ALTER PROCEDURE dbo.DeleteBotMessagesWithRelations
                @BotPatientId UNIQUEIDENTIFIER
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @MessageIds TABLE (MessageId UNIQUEIDENTIFIER);

                -- Get all Message IDs linked to the BotPatient
                INSERT INTO @MessageIds (MessageId)
                SELECT BM.MessageId
                FROM BotMessages BM
                WHERE BM.BotPatientId = @BotPatientId;

                -- Delete BotMessages FIRST (this is crucial)
                -- BotMessages has the foreign key to Messages, so it must be deleted first
                DELETE FROM BotMessages
                WHERE BotPatientId = @BotPatientId;

                -- Now it's safe to delete Messages since no BotMessages references them anymore
                DELETE FROM Messages
                WHERE Id IN (SELECT MessageId FROM @MessageIds);

                RETURN 0;
            END;";

        public static string BotMessageDeleteTrigger => @"
            CREATE OR ALTER TRIGGER TR_BotMessage_Delete
            ON BotMessages
            INSTEAD OF DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @MessageIds TABLE (MessageId UNIQUEIDENTIFIER);

                -- Capture Message IDs from BotMessages being deleted
                INSERT INTO @MessageIds (MessageId)
                SELECT MessageId FROM deleted;

                -- First, delete the BotMessages
                DELETE FROM BotMessages
                WHERE MessageId IN (SELECT MessageId FROM @MessageIds);

                -- Then, delete the Messages (after the foreign key reference is gone)
                DELETE FROM Messages
                WHERE Id IN (SELECT MessageId FROM @MessageIds);
            END;";

    }
}