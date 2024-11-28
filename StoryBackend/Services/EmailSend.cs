using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using StoryBackend.Abstract;
using StoryBackend.Configurations;

namespace StoryBackend.Services;

public class EmailSend : IEmailSend
{
    private readonly SendingEmail _sendingEmail;

    public EmailSend(IOptionsMonitor<SendingEmail> emailOptionsMonitor)
    {
        _sendingEmail = emailOptionsMonitor.CurrentValue;
    }
    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        SendGridClient client = new SendGridClient(apiKey);
        SendGridMessage msg = new SendGridMessage()
        {
            From = new EmailAddress(_sendingEmail.FromAddress, "GroupWriter: Reset Password"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        Response? response = await client.SendEmailAsync(msg);
        //_logger.LogInformation(response.IsSuccessStatusCode
        //                       ? $"Email to {toEmail} queued successfully!"
        //                       : $"Failure Email to {toEmail}");

    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(_sendingEmail.SendGridApiKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(_sendingEmail.SendGridApiKey, subject, message, toEmail);
    }
}
