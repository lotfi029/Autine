using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnAndUpdateBotPatientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotPatients_AspNetUsers_ApplicationUserId",
                table: "BotPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_BotPatients_Patients_PatientId",
                table: "BotPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_Patients_PatientId",
                table: "ThreadMembers");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_ApplicationUserId",
                table: "BotPatients");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_BotId_PatientId",
                table: "BotPatients");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "BotPatients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BotPatients");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "ThreadMembers",
                newName: "ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMembers_PatientId_MemberId",
                table: "ThreadMembers",
                newName: "IX_ThreadMembers_ThreadId_MemberId");

            migrationBuilder.RenameColumn(
                name: "IsDisabled",
                table: "BotPatients",
                newName: "IsUser");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "BotPatients",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BotPatients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_BotId_UserId",
                table: "BotPatients",
                columns: new[] { "BotId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_UserId",
                table: "BotPatients",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BotPatients_AspNetUsers_UserId",
                table: "BotPatients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BotPatients_Patients_PatientId",
                table: "BotPatients",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMembers_Patients_ThreadId",
                table: "ThreadMembers",
                column: "ThreadId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotPatients_AspNetUsers_UserId",
                table: "BotPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_BotPatients_Patients_PatientId",
                table: "BotPatients");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_Patients_ThreadId",
                table: "ThreadMembers");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_BotId_UserId",
                table: "BotPatients");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_UserId",
                table: "BotPatients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BotPatients");

            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "ThreadMembers",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMembers_ThreadId_MemberId",
                table: "ThreadMembers",
                newName: "IX_ThreadMembers_PatientId_MemberId");

            migrationBuilder.RenameColumn(
                name: "IsUser",
                table: "BotPatients",
                newName: "IsDisabled");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "BotPatients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "BotPatients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BotPatients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_ApplicationUserId",
                table: "BotPatients",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_BotId_PatientId",
                table: "BotPatients",
                columns: new[] { "BotId", "PatientId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BotPatients_AspNetUsers_ApplicationUserId",
                table: "BotPatients",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BotPatients_Patients_PatientId",
                table: "BotPatients",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMembers_Patients_PatientId",
                table: "ThreadMembers",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
