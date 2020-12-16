using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Monitor
{
    public class Config
    {
        /// <summary>
        /// 邮件地址
        /// </summary>
        public static string MailAdress = ConfigurationManager.AppSettings["MailAdress"];

        /// <summary>
        /// 邮箱密码
        /// </summary>
        public static string MailPassword = ConfigurationManager.AppSettings["MailPassword"];

        /// <summary>
        /// 选择保险公司
        /// </summary>
        public static string companyId = "3";

        /// <summary>
        /// 记录上次成功率
        /// 当低于预期成功率，且还一直在下降的，则持续发邮件，为了避免一直重复发邮件
        /// </summary>
        public static double LastSuccessed = 0;

        /// <summary>
        /// 收件人
        /// </summary>
        public static string MailTo = ConfigurationManager.AppSettings["MailTo"];

        /// <summary>
        /// 手动操作模式，不自动发送邮件,当手工点击发送邮件时，则直接发送
        /// </summary>
        public static string NotAutoSendEmail = "1";
    }
}
