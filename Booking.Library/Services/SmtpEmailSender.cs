using Bookuj.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Booking.Infrastructure.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public SmtpEmailSender(SmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailAddress mailAddressFrom = new MailAddress(_smtpSettings.Login);
            MailAddress mailAddresTo = new MailAddress(email);
            MailMessage mailMessage = new MailMessage(mailAddressFrom, mailAddresTo);
            mailMessage.Subject = subject;
            mailMessage.Body = htmlMessage;

            SmtpClient client = new SmtpClient
            {
                Port = _smtpSettings.Port,
                Host = _smtpSettings.Host,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Login, _smtpSettings.Password),        
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}
