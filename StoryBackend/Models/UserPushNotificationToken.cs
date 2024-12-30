namespace StoryBackend.Models;

public class UserPushNotificationToken

{
    public Guid UserPushNotificationTokenId { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    public bool Enabled { get; set; }

    public static UserPushNotificationToken Instance(Guid userId, string token, bool enabled = true)
    {
        return new UserPushNotificationToken { UserId = userId, Token = token, Timestamp = DateTimeOffset.UtcNow, Enabled = enabled };
    }
}
