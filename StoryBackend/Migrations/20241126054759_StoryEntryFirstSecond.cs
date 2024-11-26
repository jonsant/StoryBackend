using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryBackend.Migrations
{
    /// <inheritdoc />
    public partial class StoryEntryFirstSecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "StoryEntries",
                newName: "Second");

            migrationBuilder.AddColumn<string>(
                name: "First",
                table: "StoryEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "First",
                table: "StoryEntries");

            migrationBuilder.RenameColumn(
                name: "Second",
                table: "StoryEntries",
                newName: "Content");
        }
    }
}
