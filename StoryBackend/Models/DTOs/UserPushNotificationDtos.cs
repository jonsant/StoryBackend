namespace StoryBackend.Models.DTOs;

public class AddUserPushNotificationTokenDto
{
    public string Token { get; set; } = string.Empty;
}

public class DeleteUserPushNotificationTokenDto
{
    public string Token { get; set; } = string.Empty;

    public static DeleteUserPushNotificationTokenDto Instance(string token)
    {
        return new DeleteUserPushNotificationTokenDto { Token = token };
    }
}
