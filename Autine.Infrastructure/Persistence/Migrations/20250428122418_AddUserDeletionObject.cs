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
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS {AdminSPs.DeleteAdminWithRelation}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS {SupervisorSPs.DeleteSupervisorRelations}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS {PatientSPs.DeletePatientWithRelation}");
            migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS {UserSPs.DeleteUserWithRelation}");

        }
    }
}
