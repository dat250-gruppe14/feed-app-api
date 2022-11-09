using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedAppApi.Migrations
{
    public partial class UserRefreshTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpires",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "user");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpires",
                table: "user");
        }
    }
}
