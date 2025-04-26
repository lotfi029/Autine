using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBotPatientDeletionObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE dbo.DeleteBotPatientWithRelations
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
    
                    -- Delete BotMessages first (maintain referential integrity)
                    DELETE FROM BotMessages
                    WHERE BotPatientId = @BotPatientId;
    
                    -- Delete Messages
                    DELETE FROM Messages
                    WHERE Id IN (SELECT MessageId FROM @MessageIds);
    
                    -- Delete BotPatient
                    DELETE FROM BotPatients
                    WHERE Id = @BotPatientId;
    
                    RETURN 0;
                END;");

            migrationBuilder.Sql(@"
                CREATE OR ALTER TRIGGER TR_BotPatient_Delete
                ON BotPatients
                INSTEAD OF DELETE
                AS
                BEGIN
                    SET NOCOUNT ON;
    
                    DECLARE @BotPatientIds TABLE (Id UNIQUEIDENTIFIER);
                    DECLARE @MessageIds TABLE (MessageId UNIQUEIDENTIFIER);
    
                    -- Capture BotPatient IDs being deleted
                    INSERT INTO @BotPatientIds (Id)
                    SELECT Id FROM deleted;
    
                    -- Get all Message IDs linked to the BotPatients
                    INSERT INTO @MessageIds (MessageId)
                    SELECT BM.MessageId
                    FROM BotMessages BM
                    WHERE BM.BotPatientId IN (SELECT Id FROM @BotPatientIds);
    
                    -- Delete BotMessages first
                    DELETE FROM BotMessages
                    WHERE BotPatientId IN (SELECT Id FROM @BotPatientIds);
    
                    -- Delete Messages
                    DELETE FROM Messages
                    WHERE Id IN (SELECT MessageId FROM @MessageIds);
    
                    -- Finally delete BotPatient records
                    DELETE FROM BotPatients
                    WHERE Id IN (SELECT Id FROM @BotPatientIds);
                END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TR_BotPatient_Delete;");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.DeleteBotPatientWithRelations;");
        }
    }
}
 