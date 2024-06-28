using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveParticipantPlayerInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerInOrder",
                table: "Participants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerInOrder",
                table: "Participants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
