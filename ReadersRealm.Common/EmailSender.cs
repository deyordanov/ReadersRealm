namespace ReadersRealm.Common;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using static Common.Constants.Constants.SenderGridSettings;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        string apiKey = this._configuration[ApiKey]!;
        SendGridClient client = new SendGridClient(apiKey);
        EmailAddress from = new EmailAddress(FromEmail, FromUserName);
        EmailAddress to = new EmailAddress(email);
        SendGridMessage message = MailHelper
            .CreateSingleEmail(from, to, subject, string.Empty, htmlMessage);
        await client.SendEmailAsync(message);
    }
}