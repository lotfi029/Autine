using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToBotPatientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotPatients_AspNetUsers_ApplicationUserId",
                table: "BotPatients");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_BotId_PatientId",
                table: "BotPatients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BotPatients");

            migrationBuilder.RenameColumn(
                name: "IsDisabled",
                table: "BotPatients",
                newName: "IsUser");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "BotPatients",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BotPatients_ApplicationUserId",
                table: "BotPatients",
                newName: "IX_BotPatients_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "BotPatients",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_BotId_PatientId",
                table: "BotPatients",
                columns: new[] { "BotId", "PatientId" },
                unique: true,
                filter: "[PatientId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BotPatients_AspNetUsers_UserId",
                table: "BotPatients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BotPatients_AspNetUsers_UserId",
                table: "BotPatients");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_BotId_PatientId",
                table: "BotPatients");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "BotPatients",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "IsUser",
                table: "BotPatients",
                newName: "IsDisabled");

            migrationBuilder.RenameIndex(
                name: "IX_BotPatients_UserId",
                table: "BotPatients",
                newName: "IX_BotPatients_ApplicationUserId");

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
                name: "CreatedBy",
                table: "BotPatients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
        }
    }
}
