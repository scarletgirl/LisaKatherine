using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace LisaKatherine.Models
{
    public class Email
    {
                public void Send(Contact contact)
        {
            MailMessage mail = new MailMessage(
                contact.From,
                "lisa@lisakatherine.com",
                contact.Subject,
                contact.Message);
            SmtpClient client = new SmtpClient("localhost");
            client.UseDefaultCredentials = true;
    
            client.Send(mail);
        }
    }
}
