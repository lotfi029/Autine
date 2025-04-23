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
}
