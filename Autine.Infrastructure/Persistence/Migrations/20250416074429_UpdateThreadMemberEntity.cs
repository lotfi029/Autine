using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateThreadMemberEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMember_AspNetUsers_UserId",
                table: "ThreadMember");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMember_Patients_PatientId",
                table: "ThreadMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThreadMember",
                table: "ThreadMember");

            migrationBuilder.RenameTable(
                name: "ThreadMember",
                newName: "ThreadMembers");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMember_UserId",
                table: "ThreadMembers",
                newName: "IX_ThreadMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMember_PatientId",
                table: "ThreadMembers",
                newName: "IX_ThreadMembers_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThreadMembers",
                table: "ThreadMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMembers_AspNetUsers_UserId",
                table: "ThreadMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_AspNetUsers_UserId",
                table: "ThreadMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMembers_Patients_PatientId",
                table: "ThreadMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThreadMembers",
                table: "ThreadMembers");

            migrationBuilder.RenameTable(
                name: "ThreadMembers",
                newName: "ThreadMember");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMembers_UserId",
                table: "ThreadMember",
                newName: "IX_ThreadMember_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ThreadMembers_PatientId",
                table: "ThreadMember",
                newName: "IX_ThreadMember_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThreadMember",
                table: "ThreadMember",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMember_AspNetUsers_UserId",
                table: "ThreadMember",
                column: "UserId",
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
    }
}
