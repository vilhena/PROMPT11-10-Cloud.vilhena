using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Business;
using System.Net.Mail;
using System.Net;

namespace Ana.Business.Managers
{
    public class GoogleEmailProvider:IEmailProvider
    {
        private const string fromPassword = "prompt2012";
        private MailAddress fromAddress = new MailAddress("ana.cloudapp@gmail.com", "ANA");

        public void SendEmail(string to, string subject, string body)
        {
            var toAddress = new MailAddress(to);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
