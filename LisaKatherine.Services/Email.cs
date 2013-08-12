namespace LisaKatherine.Services
{
    using System.Net.Mail;

    using LisaKatherine.Interface;

    public class Email
    {
        public void Send(IContact contact)
        {
            var mail = new MailMessage(contact.From, "lisa@lisakatherine.com", contact.Subject, contact.Message);
            var client = new SmtpClient("localhost") { UseDefaultCredentials = true };

            client.Send(mail);
        }
    }
}