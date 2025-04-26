using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019409bf-3ae7-7cdf-995b-db4620f2ff5f",
                columns: new[] { "Bio", "Email", "FirstName", "LastName", "NormalizedEmail" },
                values: new object[] { "Admin", "admin@autine.com", "Autine", "Admin", "ADMIN@AUTINE.COM" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019409bf-3ae7-7cdf-995b-db4620f2ff5f",
                columns: new[] { "Bio", "Email", "FirstName", "LastName", "NormalizedEmail" },
                values: new object[] { "grad", "admin@graduation.edu", "Admin", "grad", "ADMIN@GRADUATION.EDU" });
        }
    }
}
