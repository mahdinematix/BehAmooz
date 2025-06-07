using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace _01_Framework.Application.Email
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string title, string messageBody, string destination)
        {
            var message = new MimeMessage();

            var from = new MailboxAddress("RozAbiAccessory", "m6hdi16@gmail.com");
            message.From.Add(from);

            var to = new MailboxAddress("User", destination);
            message.To.Add(to);

            message.Subject = title;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>{messageBody}</h1>",
            };

            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            client.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            client.Authenticate("rowan.dach84@ethereal.email", "AmgFp1GktT9B33CMGv");
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}