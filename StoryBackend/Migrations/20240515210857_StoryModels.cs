using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class StoryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    StoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Finished = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentPlayerInOrder = table.Column<int>(type: "INTEGER", nullable: true),
                    FinalStory = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.StoryId);
                    table.ForeignKey(
                        name: "FK_Stories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Invitees",
                columns: table => new
                {
                    InviteeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitees", x => x.InviteeId);
                    table.ForeignKey(
                        name: "FK_Invitees_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "StoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LobbyMessages",
                columns: table => new
                {
                    LobbyMessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobbyMessages", x => x.LobbyMessageId);
                    table.ForeignKey(
                        name: "FK_LobbyMessages_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "StoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LobbyMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerInOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.ParticipantId);
                    table.ForeignKey(
                        name: "FK_Participants_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "StoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryEntries",
                columns: table => new
                {
                    StoryEntryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryEntries", x => x.StoryEntryId);
                    table.ForeignKey(
                        name: "FK_StoryEntries_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "StoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitees_StoryId",
                table: "Invitees",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitees_UserId",
                table: "Invitees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyMessages_StoryId",
                table: "LobbyMessages",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyMessages_UserId",
                table: "LobbyMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_StoryId",
                table: "Participants",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UserId",
                table: "Stories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryEntries_StoryId",
                table: "StoryEntries",
                column: "StoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitees");

            migrationBuilder.DropTable(
                name: "LobbyMessages");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "StoryEntries");

            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Users");
        }
    }
}
