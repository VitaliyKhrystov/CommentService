using System.Net;
using System.Net.Mail;

namespace CommentService.Services
{
    public class EmailSender
    {
        public void Sender(string textSubject, string textBody, string emailTo ) {

            MailAddress fromContact = new MailAddress(EmailClient.Address);
            MailAddress toContact = new MailAddress(emailTo);

            using MailMessage mailMessage = new MailMessage(fromContact, toContact);
            using SmtpClient smtpClient = new SmtpClient();

            mailMessage.Subject = textSubject;
            mailMessage.Body = textBody;

            smtpClient.Host = EmailClient.Host;
            smtpClient.Port = EmailClient.Port;
            smtpClient.EnableSsl = EmailClient.EnableSsl;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromContact.Address, EmailClient.Password);

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
