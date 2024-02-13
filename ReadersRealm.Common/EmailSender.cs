namespace ReadersRealm.Common;

using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //Logic to send email....
        return Task.CompletedTask;
    }
}