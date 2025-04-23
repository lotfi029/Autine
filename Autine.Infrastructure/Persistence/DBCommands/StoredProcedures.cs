namespace Autine.Infrastructure.Persistence.DBCommands;
public class StoredProcedures
{
    public static class BotSPs
    {
        public const string DeleteBotWithRelations = $"dbo.{nameof(DeleteBotWithRelations)}";
        public const string RemovePatientBotData = $"dbo.{nameof(RemovePatientBotData)}";
            
    }
}
