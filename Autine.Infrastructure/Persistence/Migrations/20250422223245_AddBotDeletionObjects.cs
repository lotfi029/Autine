using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBotDeletionObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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
                END");

                // Create the trigger 
                migrationBuilder.Sql(@"
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
                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_BotMessage_Delete");

            // Drop the stored procedure
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.DeleteBotWithRelations");
        }
    }
}
