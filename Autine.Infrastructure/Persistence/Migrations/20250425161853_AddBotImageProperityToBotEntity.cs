using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBotImageProperityToBotEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Bots");

            migrationBuilder.AddColumn<string>(
                name: "BotImage",
                table: "Bots",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BotImage",
                table: "Bots");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Bots",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
