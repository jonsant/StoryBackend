namespace StoryBackend.Models;

public class UserPushNotificationToken

{
    public Guid UserPushNotificationTokenId { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }

    public static UserPushNotificationToken Instance(Guid userId, string token)
    {
        return new UserPushNotificationToken { UserId = userId, Token = token, Created = DateTimeOffset.UtcNow };
    }
}
