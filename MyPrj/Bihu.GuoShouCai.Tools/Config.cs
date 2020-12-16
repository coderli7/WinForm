using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Bihu.GuoShouCai.Tools
{
    public class Config
    {

        public static string UserCode = ConfigurationManager.AppSettings["UserCode"];

        public static string Password = ConfigurationManager.AppSettings["Password"];

        public static string ComCode = ConfigurationManager.AppSettings["ComCode"];

        

    }
}
