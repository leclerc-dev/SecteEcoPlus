using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SecteEcoPlus.Areas.Identity
{
    public class EmailService : IEmailSender
    {
        private static SendGridClient Client;

        public static void SetClientKey(string key) // seeeeecret
        {
            Client = new SendGridClient(key);
        }
        private static readonly EmailAddress NoReplyAddress = new EmailAddress("noreply@secteecoplus.ga", "Secte éco+");
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (Client is null) return;
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(NoReplyAddress, to, subject, htmlMessage, htmlMessage);
            await Client.SendEmailAsync(message);
        }
    }
}