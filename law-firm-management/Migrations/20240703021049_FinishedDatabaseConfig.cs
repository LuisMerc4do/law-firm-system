using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace law_firm_management.Migrations
{
    /// <inheritdoc />
    public partial class FinishedDatabaseConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0eb97e32-5206-4bd1-8e82-69149d23b8b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2503480-1b74-4864-8fc1-d0354d56dfb5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5e0abc8-515b-4dba-8606-14744eeeb0fd");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "0eb97e32-5206-4bd1-8e82-69149d23b8b9", null, "Admin", "ADMIN" },
                    { "e2503480-1b74-4864-8fc1-d0354d56dfb5", null, "Client", "CLIENT" },
                    { "f5e0abc8-515b-4dba-8606-14744eeeb0fd", null, "Lawyer", "LAWYER" }
                });
        }
    }
}
