using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedAppApi.Migrations
{
    public partial class devicedelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "device",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "device");
        }
    }
}
