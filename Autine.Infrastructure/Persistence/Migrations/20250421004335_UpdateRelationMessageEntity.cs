using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationMessageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThreadMessages_MessageId",
                table: "ThreadMessages");

            migrationBuilder.DropIndex(
                name: "IX_BotMessages_MessageId",
                table: "BotMessages");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMessages_MessageId",
                table: "ThreadMessages",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BotMessages_MessageId",
                table: "BotMessages",
                column: "MessageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ThreadMessages_MessageId",
                table: "ThreadMessages");

            migrationBuilder.DropIndex(
                name: "IX_BotMessages_MessageId",
                table: "BotMessages");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMessages_MessageId",
                table: "ThreadMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_BotMessages_MessageId",
                table: "BotMessages",
                column: "MessageId");
        }
    }
}
