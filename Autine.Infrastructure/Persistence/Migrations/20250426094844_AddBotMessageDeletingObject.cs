using Microsoft.EntityFrameworkCore.Migrations;
using static Autine.Infrastructure.Persistence.DBCommands.StoredProcedures;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBotMessageDeletingObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(BotMessageSPs.DeleteBotMessagesWithRelationsProcedure);
            migrationBuilder.Sql(BotMessageSPs.BotMessageDeleteTrigger);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS TR_BotMessage_Delete;");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.DeleteBotMessagesWithRelations;");
        }
    }
}
