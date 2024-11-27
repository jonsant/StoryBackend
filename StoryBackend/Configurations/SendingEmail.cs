namespace StoryBackend.Configurations;

public class SendingEmail
{
    public string SendGridApiKey { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
}
