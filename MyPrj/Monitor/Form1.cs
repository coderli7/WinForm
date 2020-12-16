using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Reflection;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;

namespace Monitor
{
    public partial class Form1 : Form
    {

        public System.Timers.Timer moniterTimer = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
            Login();
        }

        void moniterTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            moniterTimer.Stop();
            try
            {
                if (DateTime.Now.Hour >= 9 && DateTime.Now.Hour < 23 && Config.NotAutoSendEmail != "1")
                {
                    DoMonitor();
                }
                else
                {
                    Config.LastSuccessed = 0;
                    this.dateTimePicker1.Value = System.DateTime.Now;
                    this.dateTimePicker2.Value = System.DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                this.richTextBox1.AppendText(string.Format("执行moniterTimer_Elapsed出现异常{0}", ex.Message));
            }
            finally
            {
                moniterTimer.Start();
            }
        }

        private void DoMonitor()
        {
            string startDate = GetDateStr(this.dateTimePicker1, "0");
            string endDate = GetDateStr(this.dateTimePicker2, "1");
            double percent = Convert.ToDouble(GetSucessedPercent(startDate, endDate)["SuccessPercent"]);
            double Amount = Convert.ToDouble(GetSucessedPercent(startDate, endDate)["Amount"]);
            double prePercent = 80;
            double txt2Percent;
            if (double.TryParse(this.textBox2.Text, out txt2Percent))
            {
                prePercent = txt2Percent;
            }
            if ((percent < prePercent && Amount >= 100) || Config.NotAutoSendEmail == "1")
            {
                #region 内容初始化
                string subject = "请注意国寿财报价成功率！！！";
                string body = string.Format("{2}—{3} \r\n\r\n当前成功率为：{0}，低于预期成功率{1}！\r\n", percent, prePercent, startDate, endDate);
                string errorInfo = GetErrorinfo(startDate, endDate);
                if (!string.IsNullOrEmpty(errorInfo))
                {
                    body += "\r\n\r\n====================错误码分布====================\r\n\r\n" + errorInfo;
                }
                string[] adresses = this.textBox3.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (!string.IsNullOrEmpty(Config.MailTo))
                {
                    adresses = Config.MailTo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                }
                MailSender mailSender = new MailSender(subject, body, adresses);
                string[] errorReasons = new string[] { GetEncodeStr("报价超时"), GetEncodeStr("登录超时"), GetEncodeStr("报价其他异常") };
                #endregion

                #region 发送邮件

                if (Config.LastSuccessed == 0)
                {
                    mailSender.Body = mailSender.Body + "\r\n\r\n====================请着重关注以下渠道====================\r\n\r\n" + GetReason(startDate, endDate, errorReasons);
                    mailSender.SendMail();
                    Config.LastSuccessed = percent;
                }
                else
                {
                    if ((Config.LastSuccessed - percent) >= 2)
                    {
                        mailSender.Subject = mailSender.Subject + string.Format("成功率又下降了{0}个百分点", Config.LastSuccessed - percent);
                        mailSender.Body = mailSender.Body + "\r\n\r\n====================请着重关注以下渠道====================\r\n\r\n" + GetReason(startDate, endDate, errorReasons);
                        mailSender.SendMail();
                        Config.LastSuccessed = percent;
                    }
                }
                #endregion
            }
        }

        private string GetReason(string startDate, string endDate, string[] errorReasons)
        {
            string reason = "";
            try
            {
                List<QuotebasicDateItem> list = GetErrorInfoList(errorReasons, startDate, endDate);
                var groupResult = list.GroupBy(c => c.TopAgentName + string.Format("({0})", c.TopAgent) + "\t\t\t\t" + c.quote_result + string.Format("({0})", c.err_code)).OrderByDescending(d => d.Count());
                StringBuilder sb = new StringBuilder();
                foreach (var item in groupResult)
                {
                    sb.AppendLine(item.Key + string.Format("总计{0}个", item.Count()));
                    sb.AppendLine("");
                }
                reason = sb.ToString();
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = ex.Message;
            }
            return reason;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BeginMointor();
        }

        private void BeginMointor()
        {
            if (moniterTimer.Enabled == false)
            {
                moniterTimer.Interval = Convert.ToDouble(this.textBox1.Text) * 60000;
                moniterTimer.Elapsed += moniterTimer_Elapsed;
                moniterTimer.Start();
            }
        }

        /// <summary>
        /// 获取某个城市某个时间区间的成功率
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="citycode">成功率</param>
        private static Dictionary<string, string> GetSucessedPercent(string startDate, String EndDate, string citycode = "-1")
        {

            Dictionary<string, string> retDic = new Dictionary<string, string>();
            retDic.Add("Amount", "");
            retDic.Add("SuccessPercent", "");
            /**
             * 1.先登录cqa。
             * 2.查询成功率。
             * 3.低于设置百分比后，发送邮件警示。
             */
            double sucedPercent = 0;
            try
            {
                string param1 = HttpUtility.UrlEncode(startDate, Encoding.GetEncoding("utf-8"));
                string param2 = HttpUtility.UrlEncode(EndDate, Encoding.GetEncoding("utf-8"));
                string param3 = HttpUtility.UrlEncode("2016-10-01 00:00:00", Encoding.GetEncoding("utf-8"));
                string getUrl1 = string.Format("http://cqa.91bihu.com/chart/Getcount?cityid={3}&sourec={4}&topagentid=&ensure=1&cunt=0&fasttips=0&firststa={0}&firstend={1}&secondsta={2}&secondend={2}&thirdsta={2}&thirdend={2}", param1, param2, param3, citycode, Config.companyId);
                string getUrl1Result = HttpHelper.SimpleGetOrPostUrlData(getUrl1, "", "GET");
                Root rootobj = JavaScriptConvert.DeserializeObject<Root>(getUrl1Result);
                if (rootobj == null)
                {
                    return retDic;
                }
                double Amount;
                foreach (var item in rootobj.quantity)
                {
                    double successQty, failQty;
                    double.TryParse(item.succeedQuantity.ToString(), out successQty);
                    double.TryParse(item.failureQuantity.ToString(), out failQty);
                    Amount = successQty + failQty;
                    double percent = 0;
                    if (successQty != 0 && failQty != 0)
                    {
                        percent = Math.Round((successQty * 1.0 / (successQty + failQty) * 1.0) * 100, 2);
                    }
                    else if (successQty != 0 && failQty == 0)
                    {
                        percent = 100.00;
                    }
                    else
                    {
                        percent = 0.0;
                    }
                    sucedPercent = percent;

                    retDic["Amount"] = Amount.ToString();
                    retDic["SuccessPercent"] = sucedPercent.ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDic;
        }

        private static void Login()
        {
            string result1 = HttpHelper.SimpleGetOrPostUrlData("http://cqa.91bihu.com/", "", "GET");
            string result2 = HttpHelper.SimpleGetOrPostUrlData("http://cqa.91bihu.com/login/CreateCaptcha", "", "GET");
            string password = HttpUtility.UrlEncode("8ik,$RFV", Encoding.GetEncoding("utf-8"));
            string loginPostData = string.Format("txtLoginId=cqas&txtLoginPwd={0}&Verificationcode=&hiddenUrl=", password);
            string result = HttpHelper.SimpleGetOrPostUrlData("http://cqa.91bihu.com/Login/UserLogin", loginPostData, "POST");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoMonitor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.moniterTimer.Enabled = false;
            BeginMointor();
        }

        /// <summary>
        /// 获取字符串时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="beginorend">0，代表开始时间，1 代表结束时间</param>
        /// <returns></returns>
        public string GetDateStr(DateTimePicker dt, string beginorend)
        {
            string retDateVal = "";
            try
            {
                if (beginorend == "1")
                {
                    retDateVal = dt.Value.Date.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    retDateVal = dt.Value.Date.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retDateVal;
        }

        /// <summary>
        /// 成功率统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn1_Click(object sender, EventArgs e)
        {
            string startDate = GetDateStr(this.dateTimePicker1, "0");
            string endDate = GetDateStr(this.dateTimePicker2, "1");
            Dictionary<int, string> cityDic = EnumToDictionary<CityEnum>();
            List<Dictionary<string, string>> cityDataList = new List<Dictionary<string, string>>();
            foreach (var item in cityDic)
            {
                Dictionary<string, string> curCityDataDic = new Dictionary<string, string>();
                curCityDataDic.Add("CityId", item.Key.ToString());
                curCityDataDic.Add("CityName", item.Value);
                Dictionary<string, string> retDic = GetSucessedPercent(startDate, endDate, item.Key.ToString());
                #region 获取失败原因分布
                string curCityErrorInfo = GetErrorinfo(startDate, endDate, item.Key.ToString());
                curCityDataDic.Add("ErrorInfo", curCityErrorInfo);
                #endregion
                string curSuccessedPercent = retDic["SuccessPercent"];
                string Amount = retDic["Amount"];
                curCityDataDic.Add("CityPercent", curSuccessedPercent + "%" + string.Format("({0})", Amount));
                cityDataList.Add(curCityDataDic);
            }
            ExcelHandler.GenScussedExcel(cityDataList);
        }

        /// <summary>
        /// 耗时统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string startDate = HttpUtility.UrlEncode(GetDateStr(this.dateTimePicker1, "0"), Encoding.GetEncoding("utf-8"));
            string endDate = HttpUtility.UrlEncode(GetDateStr(this.dateTimePicker2, "1"), Encoding.GetEncoding("utf-8"));
            Dictionary<int, string> cityDic = EnumToDictionary<CityEnum>();
            List<Dictionary<string, string>> cityDataList = new List<Dictionary<string, string>>();

            foreach (var item in cityDic)
            {
                Dictionary<string, string> curCityDataDic = new Dictionary<string, string>();
                curCityDataDic.Add("CityId", item.Key.ToString());
                curCityDataDic.Add("CityName", item.Value);
                curCityDataDic.Add("ZeroToTwenty", "");
                curCityDataDic.Add("TwentyToFourty", "");
                curCityDataDic.Add("FourtyToSixty", "");
                curCityDataDic.Add("SixtyToBigger", "");

                #region 获取耗时Json
                string url1 = string.Format("http://cqa.91bihu.com/consumingcount/Getcount?cityid={2}&sourec={3}&topagentid=&sta_data={0}&end_data={1}", startDate, endDate, item.Key.ToString(), Config.companyId);
                string result = HttpHelper.SimpleGetOrPostUrlData(url1, "", "GET");
                string resultData = "{\"Result\":" + result.Trim().Trim(new char[] { '{', '}' }) + "}";
                GetPriceTimeRoot getPriceTimeRoot = JavaScriptConvert.DeserializeObject<GetPriceTimeRoot>(resultData);
                #endregion

                double amount = getPriceTimeRoot.Result.Sum(c => Convert.ToDouble(c.value));
                double ZeroToTwenty = Math.Round(getPriceTimeRoot.Result.Where(c => c.name.Contains("0—20")).Sum(d => Convert.ToDouble(d.value)) / amount, 4) * 100;
                ZeroToTwenty = ZeroToTwenty.ToString() == "非数字" ? 0 : ZeroToTwenty;
                double TwentyToFourty = Math.Round(getPriceTimeRoot.Result.Where(c => c.name.Contains("20—40")).Sum(d => Convert.ToDouble(d.value)) / amount, 4) * 100;
                TwentyToFourty = TwentyToFourty.ToString() == "非数字" ? 0 : TwentyToFourty;
                double FourtyToSixty = Math.Round(getPriceTimeRoot.Result.Where(c => c.name.Contains("40—60")).Sum(d => Convert.ToDouble(d.value)) / amount, 4) * 100;
                FourtyToSixty = FourtyToSixty.ToString() == "非数字" ? 0 : FourtyToSixty;

                double SixtyToBigger = Math.Round(100 - (ZeroToTwenty + TwentyToFourty + FourtyToSixty), 2);
                curCityDataDic["ZeroToTwenty"] = ZeroToTwenty.ToString() + "%";
                curCityDataDic["TwentyToFourty"] = TwentyToFourty.ToString() + "%";
                curCityDataDic["FourtyToSixty"] = FourtyToSixty.ToString() + "%";
                curCityDataDic["SixtyToBigger"] = SixtyToBigger.ToString() + "%";
                cityDataList.Add(curCityDataDic);
            }
            ExcelHandler.GenTimeExcel(cityDataList);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                this.richTextBox1.Text = "";
                //1.查看成功率
                //2.查看错误分布百分比
                //3.打印到richtext上
                string startDate = GetDateStr(this.dateTimePicker1, "0");
                string endDate = GetDateStr(this.dateTimePicker2, "1");
                string percent = GetSucessedPercent(startDate, endDate)["SuccessPercent"];
                string Amount = GetSucessedPercent(startDate, endDate)["Amount"];
                string errRetVal = GetErrorinfo(startDate, endDate);
                this.richTextBox1.AppendText(string.Format("{0} - {1}成功率为：{2}%(报价总量{3})\r\n===============================================\r\n", startDate, endDate, percent, Amount));
                this.richTextBox1.AppendText(string.Format("错误原因分布：\r\n{0}", errRetVal));
            }
            catch (Exception ex)
            {
                this.richTextBox1.AppendText(string.Format("\r\n执行函数button4_Click发生异常：{0}", ex.Message));
            }
        }

        private string GetErrorinfo(string startDate, String endDate, string cityid = "-1")
        {
            string errorInfo = "";
            try
            {
                startDate = HttpUtility.UrlEncode(startDate, Encoding.GetEncoding("utf-8"));
                endDate = HttpUtility.UrlEncode(endDate, Encoding.GetEncoding("utf-8"));
                string getErrorUrl = string.Format("http://cqa.91bihu.com/ErrorType/Index?cityid={2}&sourec={3}&topagentid=&sta_data={0}&end_data={1}", startDate, endDate, cityid, Config.companyId);
                string errorHtml = HttpHelper.SimpleGetOrPostUrlData(getErrorUrl, "", "GET");
                HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                hd.LoadHtml(errorHtml);
                HtmlNode node = hd.GetElementbyId("data");
                if (node != null)
                {
                    string jsonResult = node.InnerHtml.Trim().Replace("&quot;", "\"");
                    jsonResult = "{\"Result\":" + jsonResult.Trim().Trim(new char[] { '{', '}' }) + "}";
                    GetPriceTimeRoot jsonResultRoot = JavaScriptConvert.DeserializeObject<GetPriceTimeRoot>(jsonResult);
                    double Amount = jsonResultRoot.Result.Sum(c => Convert.ToDouble(c.value));
                    StringBuilder errorSb = new StringBuilder();
                    foreach (var item in jsonResultRoot.Result.OrderByDescending(c => Convert.ToDouble(c.value)))
                    {
                        double curCoun = item.value;
                        string percent = Math.Round((curCoun / Amount) * 100, 2) + "%";
                        string curErrorInfo = string.Format("{0}\t\t{1}个，占比 ({2})", item.name, item.value, percent);
                        errorSb.AppendLine(curErrorInfo);
                    }
                    errorInfo = errorSb.ToString();
                }
            }
            catch (Exception ex)
            {
                this.richTextBox1.AppendText(string.Format("执行函数GetErrorinfo发生异常：{0}", ex.Message));
            }
            return errorInfo;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 根据车牌生成请求串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {

            try
            {

                /**
                 * 1.查询报价历史列表，获取最新一条记录获取到buid。
                 * 2.根据uid查询到结果。
                 */

                string postData = string.Format("CarNo={0}&strdate=&enddate=&buid=&CarVIN=&btn={1}", HttpUtility.UrlEncode(this.textBox4.Text, Encoding.GetEncoding("utf-8")), HttpUtility.UrlEncode("查询", Encoding.GetEncoding("utf-8")));
                string listhtml = HttpHelper.SimpleGetOrPostUrlData("http://cqa.91bihu.com/CarNo", postData, "POST");

                HtmlAgilityPack.HtmlDocument hd = new HtmlAgilityPack.HtmlDocument();
                hd.LoadHtml(listhtml);

                HtmlAgilityPack.HtmlNode buidNode = hd.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[3]/div/div/div/div/div/table/tr[1]/td[6]");
                if (buidNode != null)
                {
                    string buid = buidNode.InnerText;
                    string getUidUrl = string.Format("http://cqa.91bihu.com/Caching/Getvalue?Source={0}&buid={1}&type=1", Config.companyId, buid);
                    string getQuDaoInfoUrl = string.Format("http://cqa.91bihu.com/Caching/Getinfo?buid={1}&source={0}", Config.companyId, buid);
                    string Result = HttpHelper.SimpleGetOrPostUrlData(getUidUrl, "", "GET");
                    string QuDaoInfoResult = HttpHelper.SimpleGetOrPostUrlData(getQuDaoInfoUrl, "", "GET");
                    Result = ConvertJsonString(Result);
                    this.richTextBox1.Text = Result;
                    this.textBox6.Text = QuDaoInfoResult;
                }
                else
                {
                    this.richTextBox1.Text = "未查询到请求串";
                }
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = string.Format("生成请串失败:{0}", ex.Message);
            }

        }

        /// <summary>
        /// 格式化Json字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);

            JsonReader jtr = new JsonReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonWriter jsonWriter = new JsonWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 查看错误原因分布，找出不正常的渠道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string errorCode = this.textBox5.Text;
                List<string> errorCodeList = new List<string>();
                if (string.IsNullOrEmpty(errorCode))
                {
                    return;
                }
                else
                {
                    this.dataGridView1.Columns.Clear();
                    foreach (var item in errorCode.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        errorCodeList.Add(GetEncodeStr(item));
                    }
                }
                string startDate = GetDateStr(this.dateTimePicker1, "0");
                string endDate = GetDateStr(this.dateTimePicker2, "1");
                startDate = GetEncodeStr(startDate);
                endDate = GetEncodeStr(endDate);
                List<QuotebasicDateItem> quoteBasicDateItem = GetErrorInfoList(errorCodeList.ToArray(), startDate, endDate);
                if (quoteBasicDateItem.Count > 0)
                {
                    /*
                     * 1.先给数据源添加列。
                     * 2.遍历列赋值。
                     */
                    Type type = typeof(QuotebasicDateItem);
                    PropertyInfo[] p = type.GetProperties();
                    foreach (var item in p)
                    {
                        string name = item.Name;
                        this.dataGridView1.Columns.Add(name, name);
                    }
                    int colCount = this.dataGridView1.Columns.Count;
                    int addRowCount = quoteBasicDateItem.Count;
                    this.dataGridView1.Rows.Add(addRowCount);

                    for (int row = 0; row < addRowCount; row++)
                    {
                        for (int col = 0; col < colCount; col++)
                        {
                            string colName = dataGridView1.Columns[col].Name;
                            QuotebasicDateItem quote = quoteBasicDateItem[row];
                            if (quote.GetType().GetProperty(colName) != null && quote.GetType().GetProperty(colName).GetValue(quote) != null)
                            {
                                var lastVal = quote.GetType().GetProperty(colName).GetValue(quote);
                                this.dataGridView1.Rows[row].Cells[col].Value = lastVal;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.richTextBox1.Text = string.Format("执行button6_Click发生异常：{0}", ex.StackTrace);
            }
        }

        /// <summary>
        /// 根据当前错误码查询列表
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        private List<QuotebasicDateItem> GetErrorInfoList(string[] errorCodes, String startDate, string endDate, string cityId = "-1")
        {
            /*
             * 1.根据当前错误码查询列表（默认查询前三页）
             * 2.查询的结果放在一个datatable里面
             */

            List<QuotebasicDateItem> quoteBasicDateItem = new List<QuotebasicDateItem>();
            try
            {
                foreach (var errorCode in errorCodes)
                {
                    string postUrl = "http://cqa.91bihu.com/ErrorType/getErrorcode";
                    for (int pageIndex = 1; pageIndex <= 10; pageIndex++)
                    {
                        string postData = string.Format("source={3}&name={0}&sta_data={1}&end_data={2}&topagentid=&CityCode={4}&pageIndexs={5}", errorCode, startDate, endDate, Config.companyId, cityId, pageIndex);
                        string resultHtml = HttpHelper.SimpleGetOrPostUrlData(postUrl, postData, "POST");
                        string patern = @"\""QuotebasicDate\""[\S|\s]*?\]";
                        if (Regex.IsMatch(resultHtml, patern))
                        {
                            string jsonResult = "{" + Regex.Match(resultHtml, patern).Value + "}";
                            ErrorInfoRoot errorRoot = JavaScriptConvert.DeserializeObject<ErrorInfoRoot>(jsonResult);
                            if (errorRoot.QuotebasicDate.Count > 0)
                            {
                                quoteBasicDateItem.AddRange(errorRoot.QuotebasicDate);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return quoteBasicDateItem;
        }

        private string GetEncodeStr(string str, string encode = "utf-8")
        {
            return HttpUtility.UrlEncode(str, Encoding.GetEncoding(encode));
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = System.DateTime.Now.AddDays(-1);
            this.dateTimePicker2.Value = System.DateTime.Now.AddDays(-1);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            int dayCount = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
            if (dayCount == 1)
            {
                this.dateTimePicker1.Value = System.DateTime.Now;
                this.dateTimePicker2.Value = System.DateTime.Now.AddDays(6);
            }
            else if (dayCount == 0)
            {
                this.dateTimePicker1.Value = System.DateTime.Now.AddDays(-6);
                this.dateTimePicker2.Value = System.DateTime.Now;
            }
            else if (dayCount > 1 && dayCount < 7)
            {
                this.dateTimePicker1.Value = System.DateTime.Now.AddDays(-(dayCount - 1));
                this.dateTimePicker2.Value = System.DateTime.Now.AddDays(7 - dayCount); ;
            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.dateTimePicker1.Value = System.DateTime.Now;
            this.dateTimePicker2.Value = System.DateTime.Now;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Config.NotAutoSendEmail = "1";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Config.NotAutoSendEmail = "";
        }

        #region 选择保险公司
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            //国寿财
            Config.companyId = "3";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            //中华联合
            Config.companyId = "4";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            //人保
            Config.companyId = "2";
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            //太平洋
            Config.companyId = "1";
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            //平安
            Config.companyId = "0";
        }

        #endregion

        #region 枚举解析

        /// <summary>
        /// 扩展方法,获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute,是否使用枚举名代替,默认是使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription<T>(Object value, String otherDesc = "", Boolean nameInstead = false)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("该对象不是一个枚举类型！");
            }
            string name = Enum.GetName(type, Convert.ToInt32(value));
            if (name == null)
            {
                return otherDesc;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute == null ? otherDesc : attribute.Description;
        }

        /// <summary>
        /// 把枚举转换为键值对集合
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="getText">获得值得文本</param>
        /// <returns>以枚举值为key，枚举文本为value的键值对集合</returns>
        public static Dictionary<Int32, String> EnumToDictionary<T>()
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }
            Dictionary<Int32, String> enumDic = new Dictionary<int, string>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum enumValue in enumValues)
            {
                Int32 key = Convert.ToInt32(enumValue);
                String value = GetDescription<T>(key);
                enumDic.Add(key, value);
            }
            return enumDic;
        }
        #endregion

        private void button7_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { btn1_Click(sender, e); });
            Task.Factory.StartNew(() => { button3_Click(sender, e); });
        }

    }

    #region 成功率解析Model

    public class QuantityItemOld
    {


        public string Datas
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string succeedquantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Failurequantity { get; set; }
    }

    public class RootOld
    {
        /// <summary>
        /// 
        /// </summary>
        public List<QuantityItem> quantity { get; set; }
    }


    public class QuantityItem
    {
        /// <summary>
        /// {"indexdata":[{"value":246,"name":"失败量"},{"value":598,"name":"成功量"} ]}
        /// </summary>
        public string Datas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int succeedQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int failureQuantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sourecId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string agentName { get; set; }
        /// <summary>
        /// 壁虎车险——国寿财
        /// </summary>
        public string graphicsTitle { get; set; }
        /// <summary>
        /// 国寿财
        /// </summary>
        public string corporationText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dateOrder { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<QuantityItem> quantity { get; set; }
    }


    #endregion

    #region 耗时解析Model
    public class ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
    }

    public class GetPriceTimeRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem> Result { get; set; }
    }

    #endregion

    #region 错误码解析Model
    public class QuotebasicDateItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int buid { get; set; }
        /// <summary>
        /// 鲁A653QG
        /// </summary>
        public string license_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string update_time { get; set; }
        /// <summary>
        /// 无法连接
        /// </summary>
        public string quote_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Agent { get; set; }
        /// <summary>
        /// 苏世航
        /// </summary>
        public string AgentName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TopAgent { get; set; }
        /// <summary>
        /// 国寿财济南市中心支公司
        /// </summary>
        public string TopAgentName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string err_code { get; set; }
    }

    public class ErrorInfoRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public List<QuotebasicDateItem> QuotebasicDate { get; set; }
    }

    #endregion

}
