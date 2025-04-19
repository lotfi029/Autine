using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToSomeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_AspNetUsers_UserId",
                table: "ThreadMembers");

            migrationBuilder.DropIndex(
                name: "IX_ThreadMessages_ThreadMemberId",
                table: "ThreadMessages");

            migrationBuilder.DropIndex(
                name: "IX_ThreadMembers_PatientId",
                table: "ThreadMembers");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_BotId",
                table: "BotPatients");

            migrationBuilder.DropIndex(
                name: "IX_BotMessages_BotPatientId",
                table: "BotMessages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ThreadMembers",
                newName: "MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMembers_UserId",
                table: "ThreadMembers",
                newName: "IX_ThreadMembers_MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMessages_ThreadMemberId_MessageId",
                table: "ThreadMessages",
                columns: new[] { "ThreadMemberId", "MessageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMembers_PatientId_MemberId",
                table: "ThreadMembers",
                columns: new[] { "PatientId", "MemberId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientId_CreatedBy",
                table: "Patients",
                columns: new[] { "PatientId", "CreatedBy" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_BotId_PatientId",
                table: "BotPatients",
                columns: new[] { "BotId", "PatientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BotMessages_BotPatientId_MessageId",
                table: "BotMessages",
                columns: new[] { "BotPatientId", "MessageId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMembers_AspNetUsers_MemberId",
                table: "ThreadMembers",
                column: "MemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_AspNetUsers_MemberId",
                table: "ThreadMembers");

            migrationBuilder.DropIndex(
                name: "IX_ThreadMessages_ThreadMemberId_MessageId",
                table: "ThreadMessages");

            migrationBuilder.DropIndex(
                name: "IX_ThreadMembers_PatientId_MemberId",
                table: "ThreadMembers");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientId_CreatedBy",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_BotId_PatientId",
                table: "BotPatients");

            migrationBuilder.DropIndex(
                name: "IX_BotMessages_BotPatientId_MessageId",
                table: "BotMessages");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "ThreadMembers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMembers_MemberId",
                table: "ThreadMembers",
                newName: "IX_ThreadMembers_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMessages_ThreadMemberId",
                table: "ThreadMessages",
                column: "ThreadMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMembers_PatientId",
                table: "ThreadMembers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientId",
                table: "Patients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_BotId",
                table: "BotPatients",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_BotMessages_BotPatientId",
                table: "BotMessages",
                column: "BotPatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMembers_AspNetUsers_UserId",
                table: "ThreadMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
