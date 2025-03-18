using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystemLoginHr.Migrations
{
    /// <inheritdoc />
    public partial class addAuditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRoles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e3f5264-16a9-4de7-8db5-c23cde6c3061", "AQAAAAIAAYagAAAAEOcuNP4C3o5e/6wS3/mAhGcGDwlStVO4m8m200IE7KVxhBHv2xDhgMP0TZS5AP7eGw==", "04fdeeba-3aef-4e60-b171-b9028fa9379c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3f6a972f-bba6-496c-a85f-e5549fbbfb7e", "AQAAAAIAAYagAAAAEJ6UDIGdbTqWTK1Ms0UWO+8FQJuC6WVnty/O3f2dkvx+nBDpnR1589oQ43lmpMXQtg==", "c810ed37-7817-465a-b0b1-a0311706fea7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cf38ea05-5e48-4547-94fd-38b51f6ac62d", "AQAAAAIAAYagAAAAEOD65Lmrq3PY9WBLkiDClyUAzKYl03FJIM3Sq1dW3vJQJVo5V3Ry7TkWWQqvyzDP4A==", "e487e3ac-122e-444a-8852-4996d902ddeb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "7bdb9275-8cd4-4d86-bea6-bbdb5125e28b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a1d05072-65ae-44ca-86c7-6dffa140ee37", "AQAAAAIAAYagAAAAEDFnT9h9b4X65lX/3QmhTp9RXopbDFEKlG+obeBbsYvpAz9Q+bMwcyUzD08pG0/NDQ==", "ebdb02da-d9fd-4ad1-98e3-5d6660be517f" });
        }
    }
}
