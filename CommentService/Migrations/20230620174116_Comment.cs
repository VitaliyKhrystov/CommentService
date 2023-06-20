using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CommentService.Migrations
{
    /// <inheritdoc />
    public partial class Comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "f8a696b0-e0ad-4b6c-b939-521e961d770d");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "372adb8a-6c71-4310-a4e6-0afd3e702a2c");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8f074fad-eaec-4a38-b1a6-fc55a0d63990");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3996a1f1-bea0-41fd-8153-8ad42a2d918a");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "58eebf46-dacd-48cc-9922-1e927c2b63b2");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TopicURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParrentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "DisLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisLike_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Like",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Like", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Like_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { "2dca8c83-d82b-4db1-934f-28f564b68985", 1 },
                    { "72dc298d-fd3e-4435-907b-24cb9a4f97fd", 2 },
                    { "fc1f56da-96e3-4067-9dc3-d4939c7db164", 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthYear", "Email", "NickName", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId" },
                values: new object[,]
                {
                    { "581f334b-a57f-4758-8841-c280d9f3ad70", 2000, "moderator@ukr.net", "Moderator", "bW9kZXJhdG9yMjAyMw==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2dca8c83-d82b-4db1-934f-28f564b68985" },
                    { "cbaafc18-e3b8-48b9-9eef-b6e1c92d6ed5", 1990, "admin@ukr.net", "Admin", "YWRtaW4yMDIz", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fc1f56da-96e3-4067-9dc3-d4939c7db164" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisLike_CommentId",
                table: "DisLike",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Like_CommentId",
                table: "Like",
                column: "CommentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisLike");

            migrationBuilder.DropTable(
                name: "Like");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "72dc298d-fd3e-4435-907b-24cb9a4f97fd");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "581f334b-a57f-4758-8841-c280d9f3ad70");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "cbaafc18-e3b8-48b9-9eef-b6e1c92d6ed5");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2dca8c83-d82b-4db1-934f-28f564b68985");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "fc1f56da-96e3-4067-9dc3-d4939c7db164");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { "3996a1f1-bea0-41fd-8153-8ad42a2d918a", 1 },
                    { "58eebf46-dacd-48cc-9922-1e927c2b63b2", 0 },
                    { "f8a696b0-e0ad-4b6c-b939-521e961d770d", 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthYear", "Email", "NickName", "Password", "RefreshToken", "RefreshTokenExpiryTime", "RoleId" },
                values: new object[,]
                {
                    { "372adb8a-6c71-4310-a4e6-0afd3e702a2c", 2000, "moderator@ukr.net", "Moderator", "bW9kZXJhdG9yMjAyMw==", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3996a1f1-bea0-41fd-8153-8ad42a2d918a" },
                    { "8f074fad-eaec-4a38-b1a6-fc55a0d63990", 1990, "admin@ukr.net", "Admin", "YWRtaW4yMDIz", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "58eebf46-dacd-48cc-9922-1e927c2b63b2" }
                });
        }
    }
}
