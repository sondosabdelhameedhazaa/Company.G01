using Company.G01.PL.Helpers;
using Company.G01.PL.Settings;
using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Company.G01.PL.Helpers
{
    public class MailService : IMailService
    {
        private readonly IOptions<MailSettings> _options;

        public MailService(IOptions<MailSettings> options)
        {
            _options = options;
        }
        public void SendEmail(Email email)
        {
            // Build Message "Email"

            var mail = new MimeMessage();
            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Establish Connection 

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email, _options.Value.password);

            // Send Message
            smtp.Send(mail);
        }
    }
}