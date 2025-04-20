using System.Net.Mail;
using System.Net;
namespace VMS.Helper
{
    public interface ISenderEmail
    {
        Task SendEmailAsync(string ToEmail, string Subject, string Body, Stream attachmentStream, string attachmentName, string senderUserEmail, string senderUserName, bool IsBodyHtml = true);
        bool sendEmail(string ToEmail, string Subject, string Body, Stream attachmentStream, string attachmentName, string senderUserEmail, string senderUserName, bool IsBodyHtml = false);
    }

    public class EmailService : ISenderEmail
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string ToEmail, string Subject, string Body, Stream attachmentStream, string attachmentName, string senderUserEmail, string senderUserName, bool IsBodyHtml = false)
        {
            string MailServer = _configuration["EmailSettings:MailServer"] ?? string.Empty;
            string FromEmail = _configuration["EmailSettings:FromEmail"] ?? string.Empty;
            string Password = _configuration["EmailSettings:Password"] ?? string.Empty;
            int Port = int.Parse(_configuration["EmailSettings:MailPort"]);
            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                EnableSsl = true,
                UseDefaultCredentials = true
            };

            MailMessage mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Body)
            {
                IsBodyHtml = IsBodyHtml,
            };

            mailMessage.CC.Add(new MailAddress(senderUserEmail, senderUserName));
            mailMessage.IsBodyHtml = true;

            if (attachmentStream != null)
            {
                mailMessage.Attachments.Add(new Attachment(attachmentStream, attachmentName));
            }
            
            return client.SendMailAsync(mailMessage);
        }

        public bool sendEmail(string ToEmail, string Subject,
                              string Body, Stream attachmentStream, 
                              string attachmentName, string senderUserEmail, 
                              string senderUserName, bool IsBodyHtml = true)
        {
            bool isMailSent = false;

            string host = _configuration["EmailSettings:MailServer"] ?? string.Empty;
            int port = int.Parse(_configuration["EmailSettings:MailPort"]);
            bool enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);
            bool defaultCredentials = bool.Parse(_configuration["EmailSettings:DefaultCredentials"]);
            string FromEmail = _configuration["EmailSettings:FromEmail"] ?? string.Empty;
            string Password = _configuration["EmailSettings:Password"] ?? string.Empty;

            using (MailMessage mm = new MailMessage(FromEmail, ToEmail, Subject, Body))
            {
                if (attachmentStream != null)
                {
                    mm.Attachments.Add(new Attachment(attachmentStream, attachmentName));
                }

                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = host;
                    smtp.EnableSsl = enableSsl;
                    NetworkCredential networkCred = new NetworkCredential(FromEmail, Password);
                    smtp.UseDefaultCredentials = defaultCredentials;
                    smtp.Credentials = networkCred;
                    smtp.Port = port;
                    smtp.Send(mm);
                    isMailSent = true;
                }
            }
            return isMailSent;
        }
    }
}
