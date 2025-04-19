using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ManagementUser.API.Migrations
{
    /// <inheritdoc />
    public partial class AddaSuperAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedDate", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "UserTitleId" },
                values: new object[] { "47d9b5c2-9aff-484d-87b9-e8ebe16a5868", 0, "0d0f2ba6-96e4-4ed6-a87d-5988084dbcac", new DateTime(2025, 4, 13, 23, 47, 7, 147, DateTimeKind.Local).AddTicks(1383), "SuperAdmin@ManagementUser.com", false, "Super", "Admin", false, null, "SUPERADMIN@MANAGEMENTUSER.COM", "SUPERADMIN@MANAGEMENTUSER.COM", "AQAAAAIAAYagAAAAEHCevet44SZKhYhNSru5vneoLjCOQ7D81zK95ZR/K3vSVTyxolp1LDrcUmEu7Z7rMg==", null, false, "ce05acce-ac18-4551-9007-c8b5cd9e5d2a", false, "SuperAdmin@ManagementUser.com", new Guid("2b28190b-de83-4263-8ad4-a69407f72113") });

            migrationBuilder.InsertData(
                table: "ApplicationUserPermissions",
                columns: new[] { "Id", "PermissionId", "UserId" },
                values: new object[] { new Guid("d6931b44-e2fd-4f7b-9503-ff2f77976737"), new Guid("1bbc8dd6-43ac-48bd-8c1a-72fe753cdb61"), "47d9b5c2-9aff-484d-87b9-e8ebe16a5868" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "2f3acff7-944b-4c05-83dc-a4175d93a018", "47d9b5c2-9aff-484d-87b9-e8ebe16a5868" },
                    { "9ab55ee9-52a9-45c9-ae26-1013c376b383", "47d9b5c2-9aff-484d-87b9-e8ebe16a5868" },
                    { "c6b9517f-4a39-4154-abb1-9811d825059b", "47d9b5c2-9aff-484d-87b9-e8ebe16a5868" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUserPermissions",
                keyColumn: "Id",
                keyValue: new Guid("d6931b44-e2fd-4f7b-9503-ff2f77976737"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2f3acff7-944b-4c05-83dc-a4175d93a018", "47d9b5c2-9aff-484d-87b9-e8ebe16a5868" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9ab55ee9-52a9-45c9-ae26-1013c376b383", "47d9b5c2-9aff-484d-87b9-e8ebe16a5868" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c6b9517f-4a39-4154-abb1-9811d825059b", "47d9b5c2-9aff-484d-87b9-e8ebe16a5868" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "47d9b5c2-9aff-484d-87b9-e8ebe16a5868");
        }
    }
}
