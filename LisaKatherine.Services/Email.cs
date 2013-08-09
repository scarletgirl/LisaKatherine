namespace LisaKatherine.Services
{
    using System.Net.Mail;

    public class Email
    {
        public void Send(Contact contact)
        {
            var mail = new MailMessage(contact.From, "lisa@lisakatherine.com", contact.Subject, contact.Message);
            var client = new SmtpClient("localhost") { UseDefaultCredentials = true };

            client.Send(mail);
        }
    }
}