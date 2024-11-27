namespace StoryBackend.Abstract;

public interface IEmailSend
{
    public Task SendEmailAsync(string toEmail, string subject, string message);
    public Task Execute(string apiKey, string subject, string message, string toEmail);
}
