namespace ReadersRealm.Common;

using Contracts;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using static Common.Constants.Constants.SendGridSettingsConstants;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ISendGridSettings _sendGridSettings;

    public EmailSender(
        IConfiguration configuration, 
        ISendGridSettings sendGridSettings)
    {
        this._configuration = configuration;
        this._sendGridSettings = sendGridSettings;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        SendGridClient client = new SendGridClient(this._sendGridSettings.SecretKey);
        EmailAddress from = new EmailAddress(FromEmail, FromUserName);
        EmailAddress to = new EmailAddress(email);
        SendGridMessage message = MailHelper
            .CreateSingleEmail(from, to, subject, string.Empty, htmlMessage);
        await client.SendEmailAsync(message);
    }
}