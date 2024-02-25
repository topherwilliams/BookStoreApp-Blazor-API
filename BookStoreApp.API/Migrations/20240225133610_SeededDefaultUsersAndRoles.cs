using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "89e8711d-c4b6-47fc-82d0-597a89f6dfd4", null, "Administrator", "ADMINISTRATOR" },
                    { "9573a312-7d3e-4072-b007-b867d4c45d4c", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "aaa4cd17-f532-4511-b637-7991e66ee51e", 0, "0cb2a7da-2cf4-465c-bbdb-133bd420d7f2", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAECHlyvdhfhFmC3GN6GV7WKORY7kRP03V+pNCBPcIpztCCWdMWKwOTlrF6yvq9EQKSA==", null, false, "c6ff07e1-898e-4d1e-90d9-8ed7dae61544", false, "admin@bookstore.com" },
                    { "fb4fad45-1fd3-43d5-81e1-03a8e044f6bc", 0, "1ccb3209-15ce-4b39-9b9a-439dd062332d", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEGh+2K2N1LTkiq1TqqxJB8bPq0MTzu6MVLEBhYKc0bJCwgoqaEv0wCUEgLOS8q8vfA==", null, false, "c3f9da33-a35b-4ea5-98a2-daf4f6db89c5", false, "user@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "89e8711d-c4b6-47fc-82d0-597a89f6dfd4", "aaa4cd17-f532-4511-b637-7991e66ee51e" },
                    { "9573a312-7d3e-4072-b007-b867d4c45d4c", "fb4fad45-1fd3-43d5-81e1-03a8e044f6bc" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "89e8711d-c4b6-47fc-82d0-597a89f6dfd4", "aaa4cd17-f532-4511-b637-7991e66ee51e" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9573a312-7d3e-4072-b007-b867d4c45d4c", "fb4fad45-1fd3-43d5-81e1-03a8e044f6bc" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89e8711d-c4b6-47fc-82d0-597a89f6dfd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9573a312-7d3e-4072-b007-b867d4c45d4c");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaa4cd17-f532-4511-b637-7991e66ee51e");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fb4fad45-1fd3-43d5-81e1-03a8e044f6bc");
        }
    }
}
