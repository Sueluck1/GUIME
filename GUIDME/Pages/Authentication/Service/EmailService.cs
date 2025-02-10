using MimeKit;

namespace GUIDME.Pages.Authenthication.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("TinTruong", "kienkoi48@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);// Thay đổi thông tin máy chủ SMTP
                client.Authenticate("kienkoi48@gmail.com", "xxns frip zcpa odvl"); // Thay đổi thông tin xác thực
                await client.SendAsync(emailMessage);
                client.Disconnect(true);
            }
        }
    }

}
