using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Auction.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            const string credentialUserName = "rentservicegg@gmail.com";
            const string sentFrom = "noreply@effective-rent.com";
            const string pwd = "bdfbdr34523gsdfh6DD";

            // Configure the client:
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com");

            client.Port = 587;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            // Create the credentials:
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(credentialUserName, pwd);
            client.EnableSsl = true;
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination)
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            };
            return client.SendMailAsync(mail);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.FromResult(0);
        }
    }
}
