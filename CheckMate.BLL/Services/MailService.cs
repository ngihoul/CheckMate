using CheckMate.Domain.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace CheckMate.BLL.Services
{
    public class MailService
    {
        private readonly string _noReplyName;
        private readonly string _noReplyEmail;
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        public MailService(IConfiguration configuration)
        {
            _noReplyName = configuration["Smtp:NoReply:Name"]!;
            _noReplyEmail = configuration["Smtp:NoReply:Email"]!;
            _smtpHost = configuration["Smtp:Host"]!;
            _smtpPort = Convert.ToInt32(configuration["Smtp:Port"]);
        }

        private SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Connect(_smtpHost, _smtpPort);

            return client;
        }

        public void SendMail(User user, string subject, string body)
        {
            string username = user.Username!;

            // Création du mail
            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress(_noReplyName, _noReplyEmail));
            email.To.Add(new MailboxAddress(username, user.Email));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = body
            };

            using var client = GetSmtpClient();
            client.Send(email);
            client.Disconnect(true);
        }
    }
}
