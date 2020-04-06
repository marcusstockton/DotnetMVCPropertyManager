using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Website.Services
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage mail = new MailMessage();
            //set the addresses
            mail.From = new MailAddress("admin@softcorp.com");
            mail.To.Add(email);

            //set the content
            mail.Subject = subject;
            mail.Body = htmlMessage;

            //send the message
            SmtpClient smtp = new SmtpClient("localhost");
            smtp.UseDefaultCredentials = true;
            await smtp.SendMailAsync(mail);
        }
    }
}