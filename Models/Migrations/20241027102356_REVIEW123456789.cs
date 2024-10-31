using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Models.Migrations
{
    /// <inheritdoc />
    public partial class REVIEW123456789 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "16e77a15-7a22-40dd-9184-a3c19a913432", null, "Admin", "ADMIN" },
                    { "267d3884-d7d9-4aa3-b6f1-9cda61b3a291", null, "HR", "HR" },
                    { "6beebd9f-41e6-4868-a590-19645dbabe3d", null, "Mentor", "MENTOR" },
                    { "8549a4f4-ecc4-4dd2-a9c8-52efb395ec3d", null, "Developer", "DEVELOPER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16e77a15-7a22-40dd-9184-a3c19a913432");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "267d3884-d7d9-4aa3-b6f1-9cda61b3a291");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6beebd9f-41e6-4868-a590-19645dbabe3d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8549a4f4-ecc4-4dd2-a9c8-52efb395ec3d");

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
    }
}
