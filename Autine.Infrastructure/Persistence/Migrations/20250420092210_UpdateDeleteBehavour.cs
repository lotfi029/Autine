using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_BotMessages_Messages_MessageId",
                table: "BotMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages",
                column: "BotPatientId",
                principalTable: "BotPatients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BotMessages_Messages_MessageId",
                table: "BotMessages",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_BotMessages_Messages_MessageId",
                table: "BotMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_BotMessages_BotPatients_BotPatientId",
                table: "BotMessages",
                column: "BotPatientId",
                principalTable: "BotPatients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BotMessages_Messages_MessageId",
                table: "BotMessages",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
