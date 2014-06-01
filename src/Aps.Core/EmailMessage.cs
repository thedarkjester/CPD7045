using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Seterlund.CodeGuard;

namespace Aps.Core
{
    class EmailMessage
    {

        public EmailMessage()
        {

        }

        public bool SendEmail(String CustomerEmail, String emailSubject, String emailBody)
        {

            Guard.That(CustomerEmail).IsNotNullOrEmpty();
            Guard.That(emailSubject).IsNotNullOrEmpty();
            Guard.That(emailBody).IsNotNullOrEmpty();
            
            // Inject this portion??
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("mail.APS.co.za");

            mail.From = new MailAddress("info@APS.co.za");
            mail.To.Add(CustomerEmail);
            mail.Subject = emailSubject;
            mail.Body = emailBody;

            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("username", "password");
            smtp.EnableSsl = true;

            smtp.Send(mail);
            return true;  
        }

    }
}
