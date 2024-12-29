namespace StoryBackend.Models;

public class PushNotification
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public static PushNotification Instance(string title, string message, Guid userId)
    {
        return new PushNotification { Title = $"GroupWriter: {title}", Message = message, UserId = userId };
    }
}
