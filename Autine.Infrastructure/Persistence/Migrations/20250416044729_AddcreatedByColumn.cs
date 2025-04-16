using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddcreatedByColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bots_AspNetUsers_CreatorId",
                table: "Bots");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_SupervisorId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMember_Patients_ThreadId",
                table: "ThreadMember");

            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "ThreadMember",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMember_ThreadId",
                table: "ThreadMember",
                newName: "IX_ThreadMember_PatientId");

            migrationBuilder.RenameColumn(
                name: "SupervisorId",
                table: "Patients",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_SupervisorId",
                table: "Patients",
                newName: "IX_Patients_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Bots",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Bots_CreatorId",
                table: "Bots",
                newName: "IX_Bots_CreatedBy");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ThreadMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Bots_AspNetUsers_CreatedBy",
                table: "Bots",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedBy",
                table: "Patients",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMember_Patients_PatientId",
                table: "ThreadMember",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bots_AspNetUsers_CreatedBy",
                table: "Bots");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_CreatedBy",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMember_Patients_PatientId",
                table: "ThreadMember");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ThreadMember");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "ThreadMember",
                newName: "ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMember_PatientId",
                table: "ThreadMember",
                newName: "IX_ThreadMember_ThreadId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Patients",
                newName: "SupervisorId");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_CreatedBy",
                table: "Patients",
                newName: "IX_Patients_SupervisorId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Bots",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Bots_CreatedBy",
                table: "Bots",
                newName: "IX_Bots_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bots_AspNetUsers_CreatorId",
                table: "Bots",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_SupervisorId",
                table: "Patients",
                column: "SupervisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMember_Patients_ThreadId",
                table: "ThreadMember",
                column: "ThreadId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
