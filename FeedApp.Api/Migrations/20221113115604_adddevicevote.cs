using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedAppApi.Migrations
{
    public partial class adddevicevote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Option1",
                table: "device_vote",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Option2",
                table: "device_vote",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Option1",
                table: "device_vote");

            migrationBuilder.DropColumn(
                name: "Option2",
                table: "device_vote");
        }
    }
}
