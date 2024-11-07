using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lavadoras.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSuperAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CodeVerification", "Email", "FirstName", "Identification", "IsActive", "LastName", "Password", "RoleTypeId", "SecretKey", "Token", "UserName", "UserTypeId" },
                values: new object[] { 1, null, "andrestupar0@gmail.com", "Super", "1111111111", true, "Admin", null, 1, null, null, null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
