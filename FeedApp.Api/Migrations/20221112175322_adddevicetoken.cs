using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedAppApi.Migrations
{
    public partial class adddevicetoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_device_user_UserId",
                table: "device");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "device",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "connectionToken",
                table: "device",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_device_user_UserId",
                table: "device",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_device_user_UserId",
                table: "device");

            migrationBuilder.DropColumn(
                name: "connectionToken",
                table: "device");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "device",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_device_user_UserId",
                table: "device",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
