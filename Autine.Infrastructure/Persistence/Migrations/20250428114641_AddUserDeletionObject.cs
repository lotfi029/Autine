using Microsoft.EntityFrameworkCore.Migrations;
using static Autine.Infrastructure.Persistence.DBCommands.StoredProcedures;

#nullable disable

namespace Autine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDeletionObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(AdminSPs.DeleteAdminWithRelationProdcedure);
            migrationBuilder.Sql(SupervisorSPs.DeleteSuperisorRelationsProcedure);
            migrationBuilder.Sql(PatientSPs.DeletePatientWithRelationProdcedure);
            migrationBuilder.Sql(UserSPs.DeleteUserWithRelationProdcedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DROP TRIGGER IF EXISTS {AdminSPs.DeleteAdminWithRelation}");
            migrationBuilder.Sql($"DROP TRIGGER IF EXISTS {SupervisorSPs.DeleteSupervisorRelations}");
            migrationBuilder.Sql($"DROP TRIGGER IF EXISTS {PatientSPs.DeletePatientWithRelation}");
            migrationBuilder.Sql($"DROP TRIGGER IF EXISTS {UserSPs.DeleteUserWithRelation}");

        }
    }
}
