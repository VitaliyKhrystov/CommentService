using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CommentService.Migrations
{
    /// <inheritdoc />
    public partial class AddNickNameToCommentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3286c4d3-57d6-472c-a97f-cc9ff9dff5da");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8a192f1d-e700-4330-8eed-dd7da29a9ecd");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "d099104a-79a4-406a-aa04-ba9155056edf");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e551ea5e-700e-468a-86ce-a82c78dc40b2");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f6666ab0-945c-4d6e-a7f2-7ceabf167acf");

            migrationBuilder.AddColumn<string>(
                name: "UserNickName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { "0c4690b5-5a01-4364-a818-963f2d9f004c", 2 },
                    { "ac92c48d-cbf2-4ec3-998b-b4b5c5c936b5", 0 },
                    { "e0bf70c0-9425-458f-92a3-0d938317a471", 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthYear", "Email", "NickName", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId" },
                values: new object[,]
                {
                    { "4b2939ac-e7a9-4eb5-b2b8-b784f9036393", 1990, "admin@ukr.net", "Admin", "YWRtaW4yMDIz", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ac92c48d-cbf2-4ec3-998b-b4b5c5c936b5" },
                    { "7bdac8b1-3f85-480c-a619-0c24b03671a3", 2000, "moderator@ukr.net", "Moderator", "bW9kZXJhdG9yMjAyMw==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "e0bf70c0-9425-458f-92a3-0d938317a471" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0c4690b5-5a01-4364-a818-963f2d9f004c");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "4b2939ac-e7a9-4eb5-b2b8-b784f9036393");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "7bdac8b1-3f85-480c-a619-0c24b03671a3");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "ac92c48d-cbf2-4ec3-998b-b4b5c5c936b5");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e0bf70c0-9425-458f-92a3-0d938317a471");

            migrationBuilder.DropColumn(
                name: "UserNickName",
                table: "Comments");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { "3286c4d3-57d6-472c-a97f-cc9ff9dff5da", 2 },
                    { "e551ea5e-700e-468a-86ce-a82c78dc40b2", 0 },
                    { "f6666ab0-945c-4d6e-a7f2-7ceabf167acf", 1 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthYear", "Email", "NickName", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId" },
                values: new object[,]
                {
                    { "8a192f1d-e700-4330-8eed-dd7da29a9ecd", 1990, "admin@ukr.net", "Admin", "YWRtaW4yMDIz", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "e551ea5e-700e-468a-86ce-a82c78dc40b2" },
                    { "d099104a-79a4-406a-aa04-ba9155056edf", 2000, "moderator@ukr.net", "Moderator", "bW9kZXJhdG9yMjAyMw==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "f6666ab0-945c-4d6e-a7f2-7ceabf167acf" }
                });
        }
    }
}
