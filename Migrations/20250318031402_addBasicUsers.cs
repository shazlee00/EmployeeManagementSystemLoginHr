using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeManagementSystemLoginHr.Migrations
{
    /// <inheritdoc />
    public partial class addBasicUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a", null, "Admin", "ADMIN" },
                    { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a", 0, "cf38ea05-5e48-4547-94fd-38b51f6ac62d", "admin@gmail.com", true, "Admin", "Admin", false, null, "ADMIN@gmail.com", "ADMIN", "AQAAAAIAAYagAAAAEOD65Lmrq3PY9WBLkiDClyUAzKYl03FJIM3Sq1dW3vJQJVo5V3Ry7TkWWQqvyzDP4A==", null, false, "e487e3ac-122e-444a-8852-4996d902ddeb", false, "admin" },
                    { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b", 0, "a1d05072-65ae-44ca-86c7-6dffa140ee37", "user@gmail.com", true, "userfirstname", "ulastname", false, null, "USER@GMAIL.COM", "USERNORMAL", "AQAAAAIAAYagAAAAEDFnT9h9b4X65lX/3QmhTp9RXopbDFEKlG+obeBbsYvpAz9Q+bMwcyUzD08pG0/NDQ==", null, false, "ebdb02da-d9fd-4ad1-98e3-5d6660be517f", false, "userNormal" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a", "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a" },
                    { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b", "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a", "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b", "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b");
        }
    }
}
