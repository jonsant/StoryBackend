using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LobbyMessages_Stories_StoryId",
                table: "LobbyMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_LobbyMessages_Users_UserId",
                table: "LobbyMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Stories_StoryId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_Users_UserId",
                table: "Stories");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryEntries_Stories_StoryId",
                table: "StoryEntries");

            migrationBuilder.DropIndex(
                name: "IX_StoryEntries_StoryId",
                table: "StoryEntries");

            migrationBuilder.DropIndex(
                name: "IX_Stories_UserId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Participants_StoryId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_UserId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_LobbyMessages_StoryId",
                table: "LobbyMessages");

            migrationBuilder.DropIndex(
                name: "IX_LobbyMessages_UserId",
                table: "LobbyMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Stories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoryEntries_StoryId",
                table: "StoryEntries",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UserId",
                table: "Stories",
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
                name: "IX_LobbyMessages_StoryId",
                table: "LobbyMessages",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LobbyMessages_UserId",
                table: "LobbyMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LobbyMessages_Stories_StoryId",
                table: "LobbyMessages",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LobbyMessages_Users_UserId",
                table: "LobbyMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Stories_StoryId",
                table: "Participants",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Users_UserId",
                table: "Participants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_Users_UserId",
                table: "Stories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoryEntries_Stories_StoryId",
                table: "StoryEntries",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
