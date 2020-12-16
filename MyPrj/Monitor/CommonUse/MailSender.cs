using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class MailSender
    {

        /// <summary>
        /// 初始化邮件发送
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="mainAdress">收件箱</param>
        public MailSender(string subject, string body, string[] mainAdress)
        {
            this.subject = subject;
            this.body = body;
            this.mainAdress = mainAdress;
        }

        private string subject;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string body;

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        private string[] mainAdress;

        public string[] MainAdress
        {
            get { return mainAdress; }
            set { mainAdress = value; }
        }

        public void SendMail()
        {
            try
            {
                //实例化一个发送邮件类。
                MailMessage mailMessage = new MailMessage();
                //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                mailMessage.From = new MailAddress(Config.MailAdress);
                //收件人邮箱地址。
                foreach (var item in mainAdress)
                {
                    mailMessage.To.Add(new MailAddress(item));
                }
                //邮件标题。
                mailMessage.Subject = subject;
                //邮件内容。
                mailMessage.Body = body;

                //实例化一个SmtpClient类。
                SmtpClient client = new SmtpClient();
                //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
                client.Host = "smtp.exmail.qq.com";
                //使用安全加密连接。
                //client.EnableSsl = true;
                //不和请求一块发送。
                client.UseDefaultCredentials = false;
                //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                client.Credentials = new NetworkCredential(Config.MailAdress, Config.MailPassword);
                //发送
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
