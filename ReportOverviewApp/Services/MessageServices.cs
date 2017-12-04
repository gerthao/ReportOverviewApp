using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Net.Pop3;

namespace ReportOverviewApp.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    
    //uses Mailkit's SmtpClient class, not the version from System.Net.Mail

    public class MessageServices : IEmailService, ISmsSender
    {
        private readonly IEmailConfiguration _emailConfiguration;
        public MessageServices(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
        public async Task<List<EmailMessage>> ReceiveEmailAsync(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                await emailClient.ConnectAsync(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await emailClient.AuthenticateAsync(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);
                List<EmailMessage> emails = new List<EmailMessage>();
                for(int i = 0; i < emailClient.Count && i < maxCount; i++){
                    var message = await emailClient.GetMessageAsync(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(internetAddress => internetAddress as MailboxAddress).Select(mailboxAddress => new EmailAddress { Address = mailboxAddress.Address, Name = mailboxAddress.Name }));
                    emailMessage.FromAddresses.AddRange(message.To.Select(internetAddress => internetAddress as MailboxAddress).Select(mailboxAddress => new EmailAddress { Address = mailboxAddress.Address, Name = mailboxAddress.Name }));
                }
                return emails;
            }
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(e => new MailboxAddress(e.Name, e.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(e => new MailboxAddress(e.Name, e.Address)));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };
            using(var emailClient = new SmtpClient())
            {
                await emailClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await emailClient.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
                await emailClient.SendAsync(message);
                await emailClient.DisconnectAsync(true);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
