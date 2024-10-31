using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class REVIEW12345678 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01080656-575f-4500-ab76-6fdccdcaac44");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a5f751f-14ae-4781-9cda-425c160412c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70bdc119-20b1-4caa-b7c2-ca1594dd9abf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abdaf16c-79a2-416d-ae86-8a9acfbcb715");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "231d28b5-899b-4354-96f3-d518a5fa2125", null, "Mentor", "MENTOR" },
                    { "4aa5a92f-e6f1-4d5b-9c5f-57bf1d0da2fb", null, "Admin", "ADMIN" },
                    { "90566a1e-3d32-484a-a70b-3053d1683d12", null, "Developer", "DEVELOPER" },
                    { "c8f5d72b-9789-4028-af95-771292bd8e73", null, "HR", "HR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "231d28b5-899b-4354-96f3-d518a5fa2125");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4aa5a92f-e6f1-4d5b-9c5f-57bf1d0da2fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90566a1e-3d32-484a-a70b-3053d1683d12");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8f5d72b-9789-4028-af95-771292bd8e73");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01080656-575f-4500-ab76-6fdccdcaac44", null, "Mentor", "MENTOR" },
                    { "2a5f751f-14ae-4781-9cda-425c160412c0", null, "HR", "HR" },
                    { "70bdc119-20b1-4caa-b7c2-ca1594dd9abf", null, "Admin", "ADMIN" },
                    { "abdaf16c-79a2-416d-ae86-8a9acfbcb715", null, "Developer", "DEVELOPER" }
                });
        }
    }
}
