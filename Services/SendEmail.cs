using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
   public class SendEmail
    {
        public static bool Send(string username, string password,string email)
        {
            try
            {
                var fromAddress = "inforesumeportal@gmail.com";
                var toAddress = email;
                const string fromPassword = "resumeportal1234";
                string subject = "Username and Password Information";
                string body = "Hello!" + "\n";
                body += "Mr./Mrs. " + username + "\n";
                body += "Your Username: " + username + "\n";
                body += "Your Password: " + password + "\n";
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                smtp.Send(fromAddress, toAddress, subject, body);
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }
    }
}
