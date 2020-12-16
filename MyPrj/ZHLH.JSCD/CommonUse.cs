
using mshtml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace BiHu.BaoXian.Artificial.ZHLH.JSCD
{
    public class CommonUse
    {

        /// <summary>
        /// 设置Input元素属性
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetInputAttribute(HtmlElement element, string value, string attribute = "")
        {
            if (element != null)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (string.IsNullOrEmpty(attribute))
                    {
                        element.SetAttribute("value", value);
                        element.SetAttribute("issValue", value);
                        element.SetAttribute("issText", value);
                        element.InvokeMember("onchange");
                    }
                    else
                    {
                        element.SetAttribute(attribute, value);
                    }
                }
            }
        }



        /// <summary>
        /// 通过Id触发点击事件
        /// </summary>
        /// <param name="web1"></param>
        /// <param name="elementId"></param>
        public static void ClickById(WebBrowser webbrowser, string elementId)
        {
            HtmlElement element = webbrowser.Document.GetElementById(elementId);
            if (element != null)
            {
                element.InvokeMember("click");
            }
        }

        /// <summary>
        /// 通过Id获取value值
        /// </summary>
        /// <param name="web1">当前页面</param>
        /// <param name="elementId">元素ID</param>
        /// <param name="attribute">指定获取那个属性的值</param>
        public static string GetValueById(WebBrowser webbrowser, string elementId, string attribute = "value")
        {
            string value = "";
            try
            {
                HtmlElement element = webbrowser.Document.GetElementById(elementId);
                if (element != null)
                {
                    value = element.GetAttribute(attribute);
                }
            }
            catch (Exception ex)
            {
            }
            return value;
        }
        /// <summary>
        /// 检查select的option是否还有指定选项
        /// </summary>
        /// <returns></returns>
        public static bool CheckSelectOption(HtmlElement eleSelect, string code)
        {
            bool result = false;
            try
            {
                if (eleSelect != null)
                {
                    HtmlElementCollection eleOptions = eleSelect.Children;
                    foreach (HtmlElement item in eleOptions)
                    {
                        if (item.GetAttribute("value") == code)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static void AddJsToDocument(WebBrowser webBrowser1)
        {
            List<string> changeElementIdList = new List<string>();
            changeElementIdList.Add("Applicant.CAppNme");
            changeElementIdList.Add("Applicant.CCertfCde");
            changeElementIdList.Add("Applicant.CMobile");
            changeElementIdList.Add("Applicant.CMobile.Hiden");
            changeElementIdList.Add("Applicant.CEmail");
            changeElementIdList.Add("Applicant.CTransactorName");
            changeElementIdList.Add("Applicant.CTransactorMobile");
            changeElementIdList.Add("Applicant.CSuffixAddr");
            changeElementIdList.Add("Insured.CInsuredNme");
            changeElementIdList.Add("Insured.CCertfCde");
            changeElementIdList.Add("Insured.CClntAddr");
            changeElementIdList.Add("Insured.CMobile.Hiden");
            changeElementIdList.Add("Insured.CMobile");
            changeElementIdList.Add("Insured.CEmail");
            changeElementIdList.Add("Cvrg.22.NRate");

            changeElementIdList.Add("Insured.CTransactorName");
            changeElementIdList.Add("Insured.CTransactorMobile");

            changeElementIdList.Add("Vhlowner.COwnerNme");
            changeElementIdList.Add("Vhlowner.CCertfCde");
            changeElementIdList.Add("Base.TAppTm");
            changeElementIdList.Add("VsTax.CTaxpayerId");
            changeElementIdList.Add("VsTax.CTaxReliefCertNo");
            changeElementIdList.Add("VsTax.CTaxAuthorities");
            changeElementIdList.Add("Vhl.CEngNo");
            changeElementIdList.Add("Vhl.CDisEngNo");
            changeElementIdList.Add("Vhl.CDisFrmNo");
            changeElementIdList.Add("Vhl.CDisVin");
            changeElementIdList.Add("Vhl.CVin");
            changeElementIdList.Add("Vhl.CFrmNo");
            changeElementIdList.Add("Vhl.NSeatNum");
            changeElementIdList.Add("Vhl.CPlateNo");
            changeElementIdList.Add("Base.CSlsTel");
            changeElementIdList.Add("Vhl.TStartDate");
            changeElementIdList.Add("Vhl.TTransferDate");
            changeElementIdList.Add("filter_policy.app_auditing_state_DW_CAppNo");
            changeElementIdList = changeElementIdList.Distinct().ToList();

            List<string> selectElementIdList = new List<string>();
            selectElementIdList.Add("Base.CNewChaType");
            selectElementIdList.Add("Base.CNewBsnsTyp");
            selectElementIdList.Add("Base.CDisptSttlCde");
            selectElementIdList.Add("Applicant.CApplicantTyp");
            selectElementIdList.Add("Applicant.CCertfCls");
            selectElementIdList.Add("Applicant.CCity");
            selectElementIdList.Add("Applicant.CContactWay");
            selectElementIdList.Add("Insured.CInsuredTyp");
            selectElementIdList.Add("Insured.CCertfCls");
            selectElementIdList.Add("Insured.CCounty");
            selectElementIdList.Add("Insured.CContactWay");
            selectElementIdList.Add("Vhlowner.CCertfCls");
            selectElementIdList.Add("Vhl.CLoanVehicleFlag");
            selectElementIdList.Add("JQ_Vhl.CUsageCde");
            selectElementIdList.Add("SY_Vhl.CUsageCde");
            selectElementIdList.Add("JQ_Vhl.CVhlTyp");
            selectElementIdList.Add("SY_Vhl.CVhlTyp");
            selectElementIdList.Add("Cvrg.2.CIndemLmtLvl");
            selectElementIdList.Add("Cvrg.13.CIndemLmtLvl");

            selectElementIdList.Add("Vhl.CCardDetail");
            selectElementIdList.Add("Vhl.CRegVhlTyp");
            selectElementIdList.Add("Vhl.CRegDriTyp");
            selectElementIdList.Add("Base.CClauseType");

            selectElementIdList = selectElementIdList.Distinct().ToList();


            //找到head元素
            HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
            //创建script标签
            HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
            //给script标签加js内容
            foreach (var item in changeElementIdList)
            {
                string funName = item.Replace(".", "_") + "MyChange";
                element.text += "function " + funName + "() { $(document.getElementById('" + item + "')).change() }";
            }
            foreach (var item in selectElementIdList)
            {
                string funName = item.Replace(".", "_") + "MySelect";
                element.text += "function " + funName + "() { $('#' + utils.tranStr('" + item + "')).trigger('change');utils.remove('QT_PO_SE');}";
            }
            //将script标签添加到head标签中
            head.AppendChild(scriptEl);
            //执行js代码
        }


        /// <summary>
        /// collection转换为list
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        public static List<HtmlElement> ConvertToListHtmlElement(HtmlElementCollection cols)
        {
            List<HtmlElement> list = new List<HtmlElement>();
            foreach (HtmlElement item in cols)
            {
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 获取当前元素的数值
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static double GetCurElementDoubleValue(HtmlElement element)
        {
            double elementVal = default(double);
            try
            {
                if (element != null)
                {
                    string val = element.GetAttribute("value");
                    double.TryParse(val, out elementVal);
                }
            }
            catch (Exception ex)
            {
            }
            return elementVal;
        }

        /// <summary>
        ///  获取IHTMLDocument2页面button按钮引用
        ///  （引申功能：获取 某个类》型某个属性》为某个值》的元素）
        /// </summary>
        /// <param name="curmshtml">IHTMLDocument2引用</param>
        /// <param name="curvalue">元素value</param>
        /// <param name="attributeName">是那个属性</param>
        /// <param name="typeName">元素类型</param>
        /// <returns></returns>
        public static IHTMLElement GetIHTMLDocument2ButtonByValue(IHTMLElementCollection elements, string curvalue, string attributeName = "value", string typeName = "button")
        {
            IHTMLElement curBtn = null;
            StringBuilder sb1 = new StringBuilder();
            try
            {
                foreach (IHTMLElement item in elements)
                {
                    var type = item.getAttribute("type");
                    var value = item.getAttribute(attributeName);
                    if (type != null && value != null)
                    {
                        sb1.Append(type + "=======" + value + "\r\n");
                        if (typeName.Equals(type) && curvalue.Contains(value))
                        {
                            curBtn = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return curBtn;
        }




        /// <summary>
        /// 获取某个元素样式（如背景色为红色等）
        /// </summary>
        /// <param name="web1"></param>
        /// <param name="elementid"></param>
        /// <returns></returns>
        public static string GetElementStyle(WebBrowser web1, string elementid)
        {
            string style = null;
            try
            {
                HtmlElement curElement = web1.Document.GetElementById(elementid);
                if (curElement != null)
                {
                    style = curElement.Style;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return style;
        }



        /// <summary>
        /// base64 字符串转换为bitmap
        /// </summary>
        /// <param name="strBase64">base64字符串</param>
        /// <returns></returns>
        public static Bitmap Base64ToImg(string strBase64)
        {
            byte[] bt = Convert.FromBase64String(strBase64);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(bt);
            Bitmap bitmap = new Bitmap(stream);
            return bitmap;
        }



        /// <summary>
        /// 获取xml的dataObjs串
        /// </summary>
        /// <returns></returns>
        public static string GetDataObjsStr(string source, string dwName = "")
        {
            string dataObjs = "";
            MatchCollection mc = Regex.Matches(source, @"<dataObjs[\S|\s]*?dataObjs>");
            if (mc.Count > 0)
            {
                foreach (Match item in mc)
                {
                    if (dwName.Length == 0)
                    {
                        dataObjs = item.Value;
                        break;
                    }
                    if (dwName.Length > 0 && item.Value.Contains("dwName=\"" + dwName))
                    {
                        dataObjs = item.Value;
                        break;
                    }
                }
            }
            return dataObjs;
        }
 

        /// <summary>
        /// 获取选择下拉框选中值
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <param name="elementId"></param>
        /// <returns></returns>
        public static String GetSelectedValueJSCD(WebBrowser webbrowser, String elementId)
        {
            String sltedVal = "";
            try
            {
                if (webbrowser != null && webbrowser.Document != null)
                {
                    HtmlElement element = webbrowser.Document.GetElementById(elementId);
                    if (element != null)
                    {
                        sltedVal = element.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return sltedVal;
        }

        /// <summary>
        /// 根据下拉框value,选中
        /// </summary>
        /// <param name="selectElement"></param>
        /// <param name="val"></param>
        public static void SetSelectByValJSCD(HtmlElement selectElement, String val)
        {
            try
            {
                if (selectElement != null && !String.IsNullOrEmpty(val))
                {
                    foreach (HtmlElement item in selectElement.Children)
                    {
                        if (item.GetAttribute("value") == val)
                        {
                            item.SetAttribute("selected", "selected");
                        }
                        else
                        {
                            item.SetAttribute("selected", "");
                        }
                    }
                    selectElement.RaiseEvent("onchange");
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 获取下拉框选中值
        /// </summary>
        /// <param name="selectElement"></param>
        /// <returns></returns>
        public static String GetSelectValJSCD(HtmlElement selectElement)
        {
            String sltedVal = "";
            try
            {
                if (selectElement != null)
                {
                    foreach (HtmlElement item in selectElement.Children)
                    {
                        if (item.GetAttribute("selected") == "selected")
                        {
                            sltedVal = item.GetAttribute("value");
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return sltedVal;
        }

        public static List<HtmlElement> ChgHtmlColToListJSCD(HtmlElementCollection htmlCollection)
        {
            List<HtmlElement> elementList = new List<HtmlElement>();
            try
            {
                if (htmlCollection != null)
                {
                    IEnumerator itrator = htmlCollection.GetEnumerator();
                    HtmlElement curElement = null;
                    while (itrator.MoveNext())
                    {
                        curElement = (HtmlElement)itrator.Current;
                        elementList.Add(curElement);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return elementList;
        }

        public static List<IHTMLElement> GetMShtmlByClassTagNameJSCD(WebBrowser webbrowser, String _className, String _tagName = "")
        {
            List<IHTMLElement> elementList = new List<IHTMLElement>();
            try
            {
                mshtml.IHTMLDocument2 mshtmlHtmlDocument = (IHTMLDocument2)webbrowser.Document.DomDocument;
                List<IHTMLElement> allMShtml = ChgHtmlColToMShtmlListJSCD(mshtmlHtmlDocument.all);

                foreach (IHTMLElement item in allMShtml)
                {
                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.tagName) && item.tagName.ToUpper().Equals(_tagName.ToUpper())
                            && !string.IsNullOrEmpty(item.className) && item.className.ToUpper().Equals(_className.ToUpper()))
                        {
                            elementList.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return elementList;
        }


        /// <summary>
        /// 根据className和TagName ,下级包含某个Id
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <param name="_className"></param>
        /// <param name="_tagName"></param>
        /// <param name="childId"></param>
        /// <returns></returns>
        public static IHTMLElement GetMShtmlByClassTagNameAndChildrenIdJSCD(WebBrowser webbrowser, String _className, String _tagName, String childId)
        {
            IHTMLElement element = null;
            try
            {
                mshtml.IHTMLDocument2 mshtmlHtmlDocument = (IHTMLDocument2)webbrowser.Document.DomDocument;
                List<IHTMLElement> allMShtml = ChgHtmlColToMShtmlListJSCD(mshtmlHtmlDocument.all);
                foreach (IHTMLElement item in allMShtml)
                {
                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.tagName) && item.tagName.ToUpper().Equals(_tagName.ToUpper())
                            && !string.IsNullOrEmpty(item.className) && item.className.ToUpper().Equals(_className.ToUpper()))
                        {
                            if (item.outerHTML.Contains(childId))
                            {
                                element = item;
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
            return element;
        }


        public static HtmlElement GethtmlByClassTagNameAndChildrenIdJSCD(WebBrowser webbrowser, String _className, String _tagName, String childId)
        {
            HtmlElement element = null;
            try
            {


                List<HtmlElement> allMShtml = ChgHtmlColToListJSCD(webbrowser.Document.All);
                foreach (HtmlElement item in allMShtml)
                {
                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.TagName) && item.TagName.ToUpper().Equals(_tagName.ToUpper()))
                        {
                            if (item.InnerHtml.Contains(_className) && item.InnerHtml.Contains(childId))
                            {
                                element = item;
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
            return element;
        }



        public static List<IHTMLElement> ChgHtmlColToMShtmlListJSCD(IHTMLElementCollection all)
        {
            List<IHTMLElement> list = new List<IHTMLElement>();
            foreach (var item in all)
            {
                IHTMLElement mshtmlItem = (IHTMLElement)item;
                list.Add(mshtmlItem);
            }
            return list;
        }

        /// <summary>
        /// 设置RadioButton值
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <param name="radioBtnName"></param>
        /// <param name="radioBtnVal"></param>
        public static void SetRadioValJSCD(WebBrowser webbrowser, String radioBtnName, String radioBtnVal)
        {
            try
            {
                List<HtmlElement> allInput = ChgHtmlColToListJSCD(webbrowser.Document.GetElementsByTagName("INPUT"));
                List<HtmlElement> newallInput = allInput.Where(c => c.GetAttribute("type").Contains("radio")).ToList();
                foreach (HtmlElement curInput in newallInput)
                {
                    String curInputName = curInput.Name;
                    if (curInputName.ToUpper().Equals(radioBtnName.ToUpper()))
                    {
                        String curInputVal = curInput.GetAttribute("value");
                        if (curInputVal.Equals(radioBtnVal))
                        {
                            curInput.SetAttribute("checked", "checked");
                        }
                        else
                        {
                            curInput.SetAttribute("checked", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取MS类型的元素
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IHTMLElement GetMShtmlByIdJSCD(WebBrowser webbrowser, String id)
        {
            mshtml.IHTMLDocument2 mshtmlHtmlDocument = (IHTMLDocument2)webbrowser.Document.DomDocument;
            List<IHTMLElement> allMShtml = ChgHtmlColToMShtmlListJSCD(mshtmlHtmlDocument.all);
            return allMShtml.Where(c => !String.IsNullOrEmpty(c.id) && c.id.Equals(id)).First();
        }

        /// <summary>
        /// 获取元素类型
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String GetElementTypeJSCD(WebBrowser webbrowser, String id)
        {
            String type = "";
            try
            {
                HtmlElement curEle = webbrowser.Document.GetElementById(id);
                type = (curEle == null ? "" : curEle.TagName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return type;
        }

        /// <summary>
        /// 获取元素double val
        /// </summary>
        /// <param name="webbrowser"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static double GetDblValByIdJSCD(WebBrowser webbrowser, String id)
        {
            double retVal = 0;
            HtmlElement curElement = webbrowser.Document.GetElementById(id);
            if (curElement != null)
            {
                String val = "";
                if ("INPUT".Equals(curElement.TagName))
                {
                    val = curElement.GetAttribute("value");
                }
                else if ("TD".Equals(curElement.TagName))
                {
                    val = curElement.InnerText;
                }
                double.TryParse(val, out retVal);
            }
            return retVal;
        }
    }
}
