namespace StoryBackend.Models.DTOs;

public class CreateEntryDto
{
    public Guid StoryId { get; set; }
    public string First { get; set; } = string.Empty;
    public string Second { get; set; } = string.Empty;
    public bool EndStory { get; set; }

}

public class FinalStoryEntryDto
{
    public string Username { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    public static FinalStoryEntryDto Instance(string username, string text) => new()
    {
        Username = username,
        Text = text
    };
}
