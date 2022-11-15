using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedAppApi.Migrations
{
    public partial class adddevice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    hashedConnectionKey = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PollId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_device_poll_PollId",
                        column: x => x.PollId,
                        principalTable: "poll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_device_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "device_vote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    PollId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device_vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_device_vote_device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "device",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_device_vote_poll_PollId",
                        column: x => x.PollId,
                        principalTable: "poll",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_Id",
                table: "device",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_PollId",
                table: "device",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_device_UserId",
                table: "device",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_device_vote_DeviceId",
                table: "device_vote",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_device_vote_PollId",
                table: "device_vote",
                column: "PollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device_vote");

            migrationBuilder.DropTable(
                name: "device");
        }
    }
}
