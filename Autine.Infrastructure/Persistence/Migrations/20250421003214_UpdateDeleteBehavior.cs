using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_Patients_PatientId",
                table: "ThreadMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMembers_Patients_PatientId",
                table: "ThreadMembers",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_Patients_PatientId",
                table: "ThreadMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMembers_Patients_PatientId",
                table: "ThreadMembers",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
