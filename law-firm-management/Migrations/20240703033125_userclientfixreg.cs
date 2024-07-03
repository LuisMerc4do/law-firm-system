using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace law_firm_management.Migrations
{
    /// <inheritdoc />
    public partial class userclientfixreg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ab9d24d-8ccd-4175-8190-24be67c76134");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf59d122-7b42-4175-9b2d-bf4adce28dc8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff66e94a-105a-4cd2-805f-328b2da159ec");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21655b77-9980-4de3-b014-53276c739fb5", null, "User", "USER" },
                    { "4e7fd451-9074-4000-addf-a8df95b2c4cd", null, "Lawyer", "LAWYER" },
                    { "baef29f6-3eeb-46ac-b197-5e4d26b203a4", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21655b77-9980-4de3-b014-53276c739fb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e7fd451-9074-4000-addf-a8df95b2c4cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "baef29f6-3eeb-46ac-b197-5e4d26b203a4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7ab9d24d-8ccd-4175-8190-24be67c76134", null, "Lawyer", "LAWYER" },
                    { "cf59d122-7b42-4175-9b2d-bf4adce28dc8", null, "Admin", "ADMIN" },
                    { "ff66e94a-105a-4cd2-805f-328b2da159ec", null, "Client", "CLIENT" }
                });
        }
    }
}
