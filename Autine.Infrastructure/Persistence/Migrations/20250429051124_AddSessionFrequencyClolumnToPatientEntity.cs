using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionFrequencyClolumnToPatientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NestSession",
                table: "Patients",
                newName: "NextSession");

            migrationBuilder.AddColumn<string>(
                name: "SessionFrequency",
                table: "Patients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionFrequency",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "NextSession",
                table: "Patients",
                newName: "NestSession");
        }
    }
}
