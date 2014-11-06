using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace com.Sconit.Utility
{
    public static class SMTPHelper
    {
        public static bool SendSMTPEMail(string Subject, string Body, string MailFrom, string MailTo, string SmtpServer, string MailFromPasswd, string ReplyTo)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient(SmtpServer);
                foreach (string mailTo in MailTo.Split(';'))
                {
                    foreach (string mailto in mailTo.Split(','))
                    {
                        message.To.Add(new MailAddress(mailto));
                    }
                }
                message.Subject = Subject;
                message.Body = Body;
                message.From = new MailAddress(MailFrom);
                message.ReplyTo = new MailAddress(ReplyTo);

                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(MailFrom, MailFromPasswd);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                // System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment("D:\\logs\\" + filePath);
                // message.Attachments.Add(attachment);

                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                client.Send(message);
                // message.Dispose();
                // logger.Error(Subject);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
