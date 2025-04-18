using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMessagingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BotPatients",
                table: "BotPatients");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BotPatients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BotPatients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "BotPatients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BotPatients",
                table: "BotPatients",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BotMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BotPatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BotMessages_BotPatients_BotPatientId",
                        column: x => x.BotPatientId,
                        principalTable: "BotPatients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BotMessages_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThreadMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThreadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThreadMessages_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThreadMessages_Patients_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotPatients_BotId",
                table: "BotPatients",
                column: "BotId");

            migrationBuilder.CreateIndex(
                name: "IX_BotMessages_BotPatientId",
                table: "BotMessages",
                column: "BotPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_BotMessages_MessageId",
                table: "BotMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMessages_MessageId",
                table: "ThreadMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadMessages_ThreadId",
                table: "ThreadMessages",
                column: "ThreadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotMessages");

            migrationBuilder.DropTable(
                name: "ThreadMessages");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BotPatients",
                table: "BotPatients");

            migrationBuilder.DropIndex(
                name: "IX_BotPatients_BotId",
                table: "BotPatients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BotPatients");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BotPatients");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BotPatients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BotPatients",
                table: "BotPatients",
                columns: new[] { "BotId", "PatientId" });
        }
    }
}
