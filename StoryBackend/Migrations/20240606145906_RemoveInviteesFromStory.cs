using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInviteesFromStory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitees_Stories_StoryId",
                table: "Invitees");

            migrationBuilder.DropIndex(
                name: "IX_Invitees_StoryId",
                table: "Invitees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Invitees_StoryId",
                table: "Invitees",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitees_Stories_StoryId",
                table: "Invitees",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
