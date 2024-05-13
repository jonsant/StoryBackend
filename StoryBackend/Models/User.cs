namespace StoryBackend.Models;

public class User
{
    public Guid UserId { get; set; }

    public Guid GlobalUserId { get; set; }

    public string UserName { get; set; } = string.Empty;
}
