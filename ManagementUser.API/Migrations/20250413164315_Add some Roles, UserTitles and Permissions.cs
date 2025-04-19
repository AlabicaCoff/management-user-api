using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ManagementUser.API.Migrations
{
    /// <inheritdoc />
    public partial class AddsomeRolesUserTitlesandPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f3acff7-944b-4c05-83dc-a4175d93a018", "2f3acff7-944b-4c05-83dc-a4175d93a018", "isWritable", "ISWRITABLE" },
                    { "9ab55ee9-52a9-45c9-ae26-1013c376b383", "9ab55ee9-52a9-45c9-ae26-1013c376b383", "isReadable", "ISREADABLE" },
                    { "c6b9517f-4a39-4154-abb1-9811d825059b", "c6b9517f-4a39-4154-abb1-9811d825059b", "isDeletable", "ISDELETABLE" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "IsDeleatable", "IsReadable", "IsWritable", "Name" },
                values: new object[,]
                {
                    { new Guid("1bbc8dd6-43ac-48bd-8c1a-72fe753cdb61"), true, true, true, "Super Admin" },
                    { new Guid("3d408d29-dd7d-4c17-b670-ba1ae8f6b30e"), false, true, true, "Admin" },
                    { new Guid("4181a1f0-5e9b-47b2-bed8-0fc232089ebe"), true, true, true, "HR Admin" },
                    { new Guid("8ec3db5a-0eed-4e47-a9fb-dab325d35690"), false, false, false, "Intern" },
                    { new Guid("94f06510-d4ac-4594-9462-79553d7a686d"), false, true, false, "Employee" },
                    { new Guid("fc07f013-1939-4618-9617-0098aba1e71a"), false, true, false, "HR" }
                });

            migrationBuilder.InsertData(
                table: "UserTitles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("18cb7a6e-e953-45ab-9ccd-a04743dfa9fa"), "Accountant" },
                    { new Guid("2b28190b-de83-4263-8ad4-a69407f72113"), "Super Admin" },
                    { new Guid("5d0b040d-36bc-48e8-960e-0e5564fcd3ee"), "CTO" },
                    { new Guid("d11f7b70-e56d-47f3-abec-065233b16739"), "Sales" },
                    { new Guid("d41a4de8-69f0-46c2-bfea-c1037f09d5c3"), "HR Manager" },
                    { new Guid("fe041e1f-6a8c-431f-8f71-ae0ec3cdcf17"), "CEO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f3acff7-944b-4c05-83dc-a4175d93a018");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ab55ee9-52a9-45c9-ae26-1013c376b383");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6b9517f-4a39-4154-abb1-9811d825059b");

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("1bbc8dd6-43ac-48bd-8c1a-72fe753cdb61"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("3d408d29-dd7d-4c17-b670-ba1ae8f6b30e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("4181a1f0-5e9b-47b2-bed8-0fc232089ebe"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("8ec3db5a-0eed-4e47-a9fb-dab325d35690"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("94f06510-d4ac-4594-9462-79553d7a686d"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("fc07f013-1939-4618-9617-0098aba1e71a"));

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: new Guid("18cb7a6e-e953-45ab-9ccd-a04743dfa9fa"));

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: new Guid("2b28190b-de83-4263-8ad4-a69407f72113"));

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: new Guid("5d0b040d-36bc-48e8-960e-0e5564fcd3ee"));

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: new Guid("d11f7b70-e56d-47f3-abec-065233b16739"));

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: new Guid("d41a4de8-69f0-46c2-bfea-c1037f09d5c3"));

            migrationBuilder.DeleteData(
                table: "UserTitles",
                keyColumn: "Id",
                keyValue: new Guid("fe041e1f-6a8c-431f-8f71-ae0ec3cdcf17"));
        }
    }
}
