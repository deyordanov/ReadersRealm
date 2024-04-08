namespace ReadersRealm.Common;

using Contracts;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using static Common.Constants.Constants.SendGridSettingsConstants;

public class EmailSender(
    IConfiguration configuration,
    ISendGridSettings sendGridSettings)
    : IEmailSender
{
    private readonly IConfiguration _configuration = configuration;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        SendGridClient client = new SendGridClient(sendGridSettings.SecretKey);
        EmailAddress from = new EmailAddress(FromEmail, FromUserName);
        EmailAddress to = new EmailAddress(email);
        SendGridMessage message = MailHelper
            .CreateSingleEmail(from, to, subject, string.Empty, htmlMessage);
        await client.SendEmailAsync(message);
    }
}