using System.Net.Mail;
using System.Net;

namespace CourseManagerApp.Services
{
    public class Email : IEmail
    {
        public void SendEmail(string subject, string body, string toAddress)
        {
            string fromAddress = "purav111.patel@gmail.com";
            const string appPassword = "ozyu icbz ssuk icfa";

            // NOTE: to make this work "2FA" must be turned on for gmail
            // and then you add an app and they generate a password for use with that app
            // see here: https://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromAddress, appPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toAddress);

            smtpClient.Send(mailMessage);
        }
    }
}
