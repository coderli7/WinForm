using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public static class MyCommonUtil
{
    static Regex regContainChinese = new Regex("[\u4e00-\u9fa5]");

    public static bool IsChineseChan(string s)
    {
        return regContainChinese.IsMatch(s);
    }


    /// <summary>
    /// 获取本机MAC地址
    /// </summary>
    public static string GetMacAddress()
    {
        string macAddress = string.Empty;
        using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
        {
            foreach (ManagementObject mo in mc.GetInstances())
            {
                using (mo)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        macAddress = mo["MacAddress"].ToString();
                        break;
                    }
                }
            }
        }
        return macAddress;
    }

    public static string GetDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes =
              (DescriptionAttribute[])fi.GetCustomAttributes(
              typeof(DescriptionAttribute), false);

        return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
    }

    /// <summary>
    /// 处理平安特殊字符
    /// </summary>
    /// <param name="Msg"></param>
    /// <returns></returns>
    public static string RemoveUnUsefulWord(string Msg)
    {
        if (Msg != null && Msg != "")
        {
            //去除后面的字母   申请报价出错: 投保查询出错 ,请录入行驶证车主名称 <T=K0MVEexHPPWXNAjL>
            var match = Regex.Match(Msg, @"<{0,1}T=([a-zA-Z]|[0-9])+>{0,1}");
            if (match.Success && match.Groups[0].Value != null && match.Groups[0].Value != "")
            {
                Msg = Msg.Replace(match.Groups[0].Value, "");
            }
        }
        return Msg;
    }

    public static string LicenseNoAdd_(string licenseNo)
    {
        if (licenseNo.IsNotNullOrEmpty() && !licenseNo.Contains('-'))
        {
           return  licenseNo.Insert(2, "-");
        }
        return licenseNo;
    }

    public static string LicenseNoRemove_(string licenseNo)
    {
        if (licenseNo.IsNotNullOrEmpty() && licenseNo.Contains('-'))
        {
           return licenseNo.Replace("-",string.Empty);
        }
        return licenseNo;
    }

    /// <summary>
    /// cookie 不再生效
    /// </summary>
    /// <param name="?"></param>
    /// <returns></returns>
    public static bool IsCookieNoEffect(string s)
    {

        /*   <html>
<head>
   <meta http-equiv="cache-control" content="no-cache">
   <script type="text/javascript">		window.onload = function () { document.getElementById('cas').submit(); }	</script>
</head>
<body>
   <form style='display:none;' id='cas' action='https://pacas-login.pingan.com.cn/cas/PA003/ICORE_PNBS_OUTER/login' method='post'>			<input type='text' name='appId' value='9e90f8465fc55bc20160004303fd0018' />			<input type='text' name='message' value='' />			<input type='text' name='param' value='' />		</form>
</body>
</html>
         * 
         */

        if (s.IsNullOrEmpty()) return false;

        return s.Contains("https://pacas-login.pingan.com.cn/cas/PA003/ICORE_PNBS_OUTER/login");
    }

    public static int ProcessId
    {
        get
        {
            if (_processId == 0)
            {
                _processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            }
            return _processId;
        }
    }
    static int _processId = 0;


    public static readonly Random Random = new Random();

    /// <summary>
    /// 生成N位的随机数 全字母
    /// </summary>
    /// <param name="lenght"></param>
    /// <returns></returns>
    public static string GetRandowAllLetter(int lenght)
    {
        var seed = "abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ";
        string k = "";
        Random rand = new Random(GetRandomSeed());
        for (int i = 0; i < lenght; i++)
        {
            k += seed[rand.Next(0, 51)].ToString();
        }
        return k;
    }

    /// <summary>
    /// 取随机数种子
    /// </summary>
    /// <returns></returns>
    public static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
}

public class ReflectUtil
{
    public static object InvokeMethod(object o, string method, object[] arg)
    {
        Type type = o.GetType();

        var methodInfo = type.GetMethod(method);

        return methodInfo.Invoke(o, arg);
    }
}

public static class ObjectExtensions
{
    public static bool IsNull(this object o)
    {
        return o == null;
    }

    public static bool IsNotNull(this object o)
    {
        return o != null;
    }


    public static bool IsNull(this JToken o)
    {
        return o == null;
    }

    public static bool IsNotNull(this JToken o)
    {
        return o != null;
    }

    public static bool IsNotNullOrEmpty(this JToken o)
    {
        if (o.IsNull())
            return false;

        return o.ToString().IsNotNullOrEmpty();
    }


    #region 序列化

    //系统自带的，这个一般不用
    //static JavaScriptSerializer a = new JavaScriptSerializer();

    public static string SerializeObject(this object o)
    {
        return JsonConvert.SerializeObject(o);
    }

    public static object DeserializeObject(this string o)
    {
        return JsonConvert.DeserializeObject(o);
    }

    public static T DeserializeObject<T>(this string o)
    {
        return JsonConvert.DeserializeObject<T>(o);
    }

    /// <summary>
    /// 格式化JSon串
    /// </summary>
    public static string ConvertJsonString(this string str)
    {
        //格式化json字符串
        JsonSerializer serializer = new JsonSerializer();
        using (TextReader tr = new StringReader(str))
        using (JsonTextReader jtr = new JsonTextReader(tr))
        {
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                using (StringWriter textWriter = new StringWriter())
                using (JsonTextWriter jsonWriter = new JsonTextWriter(textWriter))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.Indentation = 4;
                    jsonWriter.IndentChar = ' ';
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
            }
            else
            {
                return str;
            }
        }
    }


    public static T DeepCopy<T>(this T t) where T : class
    {
        return t.SerializeObject().DeserializeObject<T>();
    }

    #endregion

    //public static string SerializeObject(this object o)
    //{
    //    // JsonConvert.DeserializeObject<WaQuoteRequest>(paramString);

    //    if (o.IsNull())
    //        return null;

    //    return JsonConvert.SerializeObject(o);
    //}

    //public static T DeepCopy<T>(this T t)
    //{
    //    if (t == null)
    //        return default(T);

    //    return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(t));

    //    //object o = new object();

    //    //object o1 = o.
    //}
}

public static class StringExtensions
{
    #region null 相关处理
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrWhiteSpace(s);
    }

    public static bool IsNotNullOrEmpty(this string s)
    {
        return !string.IsNullOrWhiteSpace(s);
    }

    public static string NullTrim(this string s)
    {
        if (s == null) return s;
        return s.Trim();
    }

    public static string NullTrim(this string s,char a)
    {
        if (s == null) return s;
        return s.Trim(a);
    }

    public static bool NullStartsWith(this string s, string value)
    {
        if (s == null) return false;

        return s.StartsWith(value);
    }

    /// <summary>
    /// 有 否 运算的的地方，不能直接使用
    /// </summary>
    public static bool NullContains(this string s, string value)
    {
        if (s == null) return false;

        return s.Contains(value);
    }

    /// <summary>
    /// 有 否 运算的的地方，不能直接使用
    /// </summary>
    public static bool NullNotContains(this string s, string value)
    {
        if (s == null) return true;

        return !s.Contains(value);
    }

    #endregion

    #region 截取，去空格
    //public static string SubstringMost(this string str, int i)
    //{
    //    if (str.IsNullOrEmpty())
    //        return str;

    //    str = str.Replace("https://icorepnbs.pingan.com.cn/icore_pnbs/", string.Empty);
    //    if (str.Length < i)
    //    {
    //        return str;
    //    }

    //    return $"{str.Substring(0, i)}...";
    //}


    //public static string TrimAllSpace(this string t)
    //{
    //    return t?.Replace(" ", string.Empty);
    //}

    #endregion

    #region 编码,没有使用

    /*
    /// <summary>
    /// 编码结果如果出现三个字母，最后一个字母小写（平安特殊规则）
    /// </summary>
    public static string PAUrlEncode(this string strSource, Encoding encoding)
    {
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < strSource.Length; i++)
        {
            string charT = strSource[i].ToString();
            string k = HttpUtility.UrlEncode(charT, encoding);

            if (charT == k)
            {
                stringBuilder.Append(charT);
            }
            else
            {
                if (k == "%0A" || k == "%0a")
                {
                    k = "%0d%0a";
                }

                //%abh
                if (Regex.Match(k, @"%[a-zA-Z0-9]{3}").Success)
                {
                    stringBuilder.Append(k.Substring(0, 2).ToUpper());
                    stringBuilder.Append(k[2].ToString().ToLower());
                }
                else
                {
                    stringBuilder.Append(k.ToUpper());
                }
            }
        }

        return stringBuilder.ToString().Replace("+", "%2B");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Url_GBK_Decode(this string s)
    {
        return HttpUtility.UrlDecode(s, Encoding.GetEncoding("GBK"));
    }

    /// <summary>
    /// 使用gbk编码的URlencode
    /// </summary>
    /// <param name="?"></param>
    /// <returns></returns>
    public static string DictionaryToUrlString(this Dictionary<string, string> o)
    {
        var gbk = Encoding.GetEncoding("GBK");
        StringBuilder sb = new StringBuilder();

        foreach (var kv in o)
        {
            sb.Append(string.Format("{0}={1}&", kv.Key, kv.Value.PAUrlEncode(gbk)));
        }

        var result = sb.ToString();
        result = result.Substring(0, result.Length - 1);
        return result;
    }

    */
    #endregion

    #region 加解密，哈希
    public static string StringToMD5Hash(this string str)
    {
        try
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] md5Bytes = md5.ComputeHash(bytes);

                foreach (byte item in md5Bytes)
                {
                    stringBuilder.Append(item.ToString("x2"));
                }
            }
            return stringBuilder.ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    #endregion

    //这四个方法还是有点问题，应该是isnotnullorempty

    //效率更高，故意冗余
    public static string ValueChain(params string[] list)
    {
        if (list.IsNotNull())
        {
            foreach (object o in list)
            {
                if (o.IsNotNull())
                {
                    if (o is string)
                        return o as string;

                    return o.ToString();
                }
            }
        }

        return string.Empty;
    }
    public static string ValueChain(params object[] list)
    {
        if (list.IsNotNull())
        {
            foreach (object o in list)
            {
                if (o.IsNotNull())
                {
                    if (o is string)
                        return o as string;

                    return o.ToString();
                }
            }
        }

        return string.Empty;
    }

    //故意冗余
    public static string ValueChain(this string o1, string o2)
    {
        if (o1.IsNotNull())
        {
            if (o1 is string)
                return o1 as string;

            return o1.ToString();
        }

        if (o2.IsNotNull())
        {
            if (o2 is string)
                return o2 as string;

            return o2.ToString();
        }

        return string.Empty;
    }
    public static string ValueChain(this object o1, object o2)
    {
        if (o1.IsNotNull())
        {
            if (o1 is string)
                return o1 as string;

            return o1.ToString();
        }

        if (o2.IsNotNull())
        {
            if (o2 is string)
                return o2 as string;

            return o2.ToString();
        }

        return string.Empty;
    }


    //这份注释掉的，或许更好点，易孟飞
    /*
    //效率更高，故意冗余
    public static string ValueChain(params string[] list)
    {
        if (list.IsNotNull())
        {
            foreach (string o in list)
            {
                if (o.IsNotNullOrEmpty())
                {
                    return o;
                }
            }
        }

        return string.Empty;
    }

    public static string ValueChain(params object[] list)
    {
        if (list.IsNotNull())
        {
            foreach (object o in list)
            {
                if (o.IsNotNull())
                {
                    if (o is string)
                    {
                        var res = o as string;

                        if (res.IsNotNullOrEmpty())
                            return res;

                        //继续
                        continue;
                    }
                    else
                    {
                        return o.ToString();
                    }
                }
            }
        }

        return string.Empty;
    }

    //故意冗余
    public static string ValueChain(this string o1, string o2)
    {
        if (o1.IsNotNullOrEmpty())
        {
            return o1;
        }

        if (o2.IsNotNullOrEmpty())
        {
            return o2;
        }

        return string.Empty;
    }
    public static string ValueChain(this object o1, object o2)
    {
        if (o1.IsNotNull())
        {
            if (o1 is string)
            {
                var res = o1 as string;

                if (res.IsNotNullOrEmpty())
                    return res;
            }
            else
            {
                return o1.ToString();
            }
        }

        o1 = o2;
        if (o1.IsNotNull())
        {
            if (o1 is string)
            {
                var res = o1 as string;

                if (res.IsNotNullOrEmpty())
                    return res;
            }
            else
            {
                return o1.ToString();
            }
        }

        return string.Empty;
    }
*/

}
