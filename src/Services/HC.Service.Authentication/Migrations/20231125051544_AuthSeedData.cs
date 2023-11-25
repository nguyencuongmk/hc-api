using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HC.Service.Authentication.Migrations
{
    /// <inheritdoc />
    public partial class AuthSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedOn", "Name", "Status", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, "ADM", "system", new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1066), "Admin", 1, null, new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1078) },
                    { 2, "CUS", "system", new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1080), "Customer", 1, null, new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1081) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedBy", "CreatedOn", "Email", "EmailConfirmed", "IsActive", "PasswordHash", "PhoneNumber", "Status", "UpdatedBy", "UpdatedOn", "UserName" },
                values: new object[] { 1, null, "system", new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1185), "administrator@localhost.com", true, true, "", null, 1, null, new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1186), "Administrator" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "CreatedBy", "CreatedOn", "Status", "UpdatedBy", "UpdatedOn" },
                values: new object[] { 1, 1, "system", new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1203), 1, null, new DateTime(2023, 11, 25, 12, 15, 43, 810, DateTimeKind.Local).AddTicks(1204) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
