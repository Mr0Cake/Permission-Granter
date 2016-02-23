using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PermissionGranter.ViewModel.Services
{
    public static class SendMail
    {

        public static void Mail(string address, string text)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("PermissionGranter123@hotmail.com");
            mail.To.Add(address);
            mail.Subject = "Your permissions have been updated";
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = text;
            mail.Body = htmlBody;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("PermissionGranter123@hotmail.com", "Azerty123");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
        




    }
}
