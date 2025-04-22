using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages",
                column: "BotPatientId",
                principalTable: "BotPatients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages",
                column: "BotPatientId",
                principalTable: "BotPatients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
