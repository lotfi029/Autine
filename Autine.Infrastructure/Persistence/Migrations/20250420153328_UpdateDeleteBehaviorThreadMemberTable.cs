using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviorThreadMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMessages_Messages_MessageId",
                table: "ThreadMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMessages_ThreadMembers_ThreadMemberId",
                table: "ThreadMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMessages_Messages_MessageId",
                table: "ThreadMessages",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMessages_ThreadMembers_ThreadMemberId",
                table: "ThreadMessages",
                column: "ThreadMemberId",
                principalTable: "ThreadMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMessages_Messages_MessageId",
                table: "ThreadMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadMessages_ThreadMembers_ThreadMemberId",
                table: "ThreadMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMessages_Messages_MessageId",
                table: "ThreadMessages",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadMessages_ThreadMembers_ThreadMemberId",
                table: "ThreadMessages",
                column: "ThreadMemberId",
                principalTable: "ThreadMembers",
                principalColumn: "Id");
        }
    }
}
