using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserFromInvitee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitees_Users_UserId",
                table: "Invitees");

            migrationBuilder.DropIndex(
                name: "IX_Invitees_UserId",
                table: "Invitees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Invitees_UserId",
                table: "Invitees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitees_Users_UserId",
                table: "Invitees",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
