using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolShuttleBus.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeNumberToStaffProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                table: "StaffProfiles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE StaffProfiles
                SET EmployeeNumber =
                    CASE
                        WHEN UserId = '51000000-0000-0000-0000-000000000001' THEN 'E0001'
                        WHEN UserId = '52000000-0000-0000-0000-000000000001' THEN 'T0001'
                        WHEN UserId = '52000000-0000-0000-0000-000000000002' THEN 'T0002'
                        WHEN UserId = '22222222-2222-2222-2222-222222222222' THEN 'T0999'
                        ELSE CONCAT('EMP-', REPLACE(CONVERT(nvarchar(36), UserId), '-', ''))
                    END
                WHERE EmployeeNumber IS NULL OR EmployeeNumber = '';
                """);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeNumber",
                table: "StaffProfiles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_EmployeeNumber",
                table: "StaffProfiles",
                column: "EmployeeNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StaffProfiles_EmployeeNumber",
                table: "StaffProfiles");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "StaffProfiles");
        }
    }
}
