using BiHu.BaoXian.Artificial.ZHLH.JSCD;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZHLH.JSCD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(this.textBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            this.webBrowser1.Navigate(this.textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            WebBrowser webbrowser = this.webBrowser1;
            webbrowser.Document.GetElementById("username").SetAttribute("value", "qiaoyabing001");
            webbrowser.Document.GetElementById("password").SetAttribute("value", "Xcic2018");
            //webbrowser.Document.InvokeScript("beforesubmitcheck");

        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.webBrowser1.Navigate("http://jscd.cic.cn:8897/ipartner/insure/initInsureDirectly");
        }



        public String JSFireEvent(IHTMLDocument2 doc, string tag, string key, string value, string eventStr)
        {
            string proc = "(function () { var aElements = document.getElementsByTagName('" + tag + "'); for (var i = 0; i < aElements.length; i++) { if (aElements[i].getAttribute('" + key + "') == '" + value + "') { if ('createEvent' in document) { var evt = document.createEvent('HTMLEvents'); evt.initEvent('" + eventStr + "', false, true); aElements[i].dispatchEvent(evt); } else { aElements[i].fireEvent('on" + eventStr + "'); } break; } } })();";
            return proc;
        }




        private void button4_Click(object sender, EventArgs e)
        {
            WebBrowser webbrowser = this.webBrowser1;
            //找到head元素
            HtmlElement head = webbrowser.Document.GetElementsByTagName("head")[0];
            //创建script标签
            HtmlElement scriptEl = webbrowser.Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;

            //initChannel({'deptCode':$("#channelInfo\\.attributionOrg").val(), 'deptName':$("#channelInfo\\.attributionOrg").find("option:selected").text()});
            //给script标签加js内容
            element.text = @"function sayHello() { $('#channelInfo\\.attributionOrg').val('65970600');      $('#channelInfo\\.attributionOrg').change(); }";
            //initChannel({'deptCode':$('#channelInfo\\.attributionOrg').val(), 'deptName':$('#channelInfo\\.attributionOrg').find('option:selected').text()});
            //将script标签添加到head标签中
            head.AppendChild(scriptEl);
            ////执行js代码

            //IHTMLDocument2 doc = (IHTMLDocument2)webbrowser.Document.DomDocument;
            //string jsStr = JSFireEvent(doc, "select", "", "", "change");

            //doc.parentWindow.execScript(jsStr, "javascript");



            //IHTMLElement chanelEle = CommonUse.GetMShtmlByClassTagNameAndChildrenIdJSCD(webbrowser, "select2-selection select2-selection--single", "SPAN", "select2-channelInfoattributionOrg-container");

            //((IHTMLElement3)chanelEle).FireEvent("onblur");
            //((IHTMLElement3)chanelEle).FireEvent("onfocus");
            //((IHTMLElement3)chanelEle).FireEvent("onkeydown");
            //((IHTMLElement3)chanelEle).FireEvent("onmousedown");

            // ((IHTMLElement3)chanelEle).FireEvent("onclick");



            //HtmlElement chanelEle1 = CommonUse.GethtmlByClassTagNameAndChildrenIdJSCD(webbrowser, "select2-selection select2-selection--single", "SPAN", "select2-channelInfoattributionOrg-container");
            //chanelEle1.InvokeMember("click");
            //chanelEle1.InvokeMember("focus");
            //chanelEle1.InvokeMember("keydown");
            //chanelEle1.InvokeMember("mousedown");
            //webbrowser.Document.InvokeScript("sayHello");
            //HtmlElement htm1 = webbrowser.Document.GetElementById("channelInfo.attributionOrg");
            //htm1.InvokeMember("change");

            uint uiPid = (uint)Process.GetCurrentProcess().Id;
            WindowHanler.WindowFormAPI.ClickByLocation(300, 290);
            WindowHanler.WindowFormAPI.ClickByLocation(300, 390);
            //HtmlElement chanelEle1 = CommonUse.GethtmlByClassTagNameAndChildrenIdJSCD(webbrowser, "select2-selection select2-selection--single", "SPAN", "select2-channelInfoattributionOrg-container");
            //chanelEle.click();

            //chanelEle1.InvokeMember("click");
            //chanelEle1.InvokeMember("keydown");
            //chanelEle1.InvokeMember("keypress");
            //chanelEle1.InvokeMember("focus");
            //chanelEle1.InvokeMember("onblur");
            //chanelEle1.InvokeMember("mousedown");


            //((IHTMLElement3)chanelEle).FireEvent("onclick");
            //((IHTMLElement3)chanelEle).FireEvent("onclick");
            //((IHTMLElement3)chanelEle).FireEvent("onfocus");
            //((IHTMLElement3)chanelEle).FireEvent("onblur");
            //((IHTMLElement3)chanelEle).FireEvent("keypress");


            //CommonUse.SetSelectByValJSCD(webBrowser1.Document.GetElementById("channelInfo.attributionOrg"), "65970600");

            //IHTMLElement hidenEle = CommonUse.GetMShtmlByIdJSCD(webBrowser1, "channelInfo.attributionOrg");
            //hidenEle.setAttribute("selectedIndex", "1");
            //hidenEle = CommonUse.GetMShtmlByIdJSCD(webBrowser1, "channelInfo.attributionOrg");

            //((IHTMLElement3)hidenEle).FireEvent("onchange");
            //selectedIndex


            //IHTMLElement chanelEle2 = chanelEle.parentElement;

            //chanelEle2.click();


            //IHTMLElement chanelEle3 = chanelEle2.parentElement;
            //chanelEle3.click();

            //((IHTMLElement3)chanelEle3).FireEvent("onfocus");

            //webBrowser1.Document.GetElementById("btnVehRecognition").InvokeMember("click");


            //List<IHTMLElement> all = CommonUse.ChgHtmlColToMShtmlListJSCD(chanelEle.all);
            //all[2].click();


            //foreach (var item in all)
            //{
            //    ((IHTMLElement3)all[2]).FireEvent("onblur");
            //}
            //((IHTMLElement3)all[2]).FireEvent("onblur");

            //webBrowser1.Document.GetElementById("channelInfo.saleMan").InvokeMember("focus");

            //webBrowser1.Document.GetElementById("channelInfo.saleMan").InvokeMember("click");

            //webBrowser1.Document.GetElementById("channelInfo.saleMan").InvokeMember("");



            //webBrowser1.Document.GetElementById("searchBtnCha").InvokeMember("click");
            //document.fireevent(element, eventname)


            //IHTMLElement table = CommonUse.GetMShtmlByClassTagNameAndChildrenIdJSCD(webbrowser, "tableview", "DIV", "select2-channelInfoattributionOrg-container");
            //table.outerHTML = tmpVal;

            //((IHTMLElement3)chanelEle).FireEvent("_handleBlur");

            //webBrowser1.Document.InvokeScript("orgChange");

            //webbrowser.Document.InvokeScript("orgChange");
            //webbrowser.Document.InvokeScript("sayHello");
            //webbrowser.Document.GetElementById("org").InvokeMember("select2:select");

            //checkOrg({'deptCode': orgCode, 'deptAbbr': orgName});

            //webbrowser.Document.InvokeScript("checkOrg", new object[] { "{'deptCode': 65970600, 'deptAbbr': aaaa}" });

            //webbrowser.Document.InvokeScript("addOrgInfoTmp");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.GetElementById("select2-org-container").InvokeMember("click");
        }






        private String tmpVal = @"<div class='tableview'>
   	<table width='100%' class='table2' cellpadding='0' cellspacing='0' border='0'>
   		<input type='hidden' id='channelInfo.operDept' name='channelInfo.operDept' value='659706'>
   		<input type='hidden' id='empDuty' name='empDuty' value=''>
		<tbody><tr>
			<th>承保机构：</th>
			<td>
				<select id='channelInfo.attributionOrg' name='channelInfo.attributionOrg' class='text select2-hidden-accessible' style='width: 500px' oldval='' data-select2-id='channelInfo.attributionOrg' tabindex='-1' aria-hidden='true'><option title='请选择' value='' data-index='0' data-select2-id='4'>请选择</option><option title='65970600-经纪渠道一部' value='65970600' data-index='1' data-select2-id='8'>65970600-经纪渠道一部</option></select><span class='select2 select2-container select2-container--default select2-container--below' dir='ltr' data-select2-id='3' style='width: 500px;'><span class='selection'><span class='select2-selection select2-selection--single' role='combobox' aria-haspopup='true' aria-expanded='false' tabindex='0' aria-labelledby='select2-channelInfoattributionOrg-container'><span class='select2-selection__rendered' id='select2-channelInfoattributionOrg-container' role='textbox' aria-readonly='true' title='65970600-经纪渠道一部'>65970600-经纪渠道一部</span><span class='select2-selection__arrow' role='presentation'><b role='presentation'></b></span></span></span><span class='dropdown-wrapper' aria-hidden='true'></span></span>
				
					<input type='button' id='searchBtnCha' class='smallbut' style='width:70px' value='销管查询'>
				
			</td>
			<th>业务员：</th>
			<td>
				<input type='text' class='text' id='codeEmp' autocomplete='off' value='65970336' style='width: 90px;' readonly=''> 
				
				<select data-rule-required='true' data-msg-required='业务员不能为空！' id='channelInfo.saleMan' name='channelInfo.saleMan' defval='' class='text' style='width: 300px' aria-required='true' oldval='65970336'><option title='尤洪明-最会保保险经纪有限公司-刘心荣' value='65970336' data-index='0'>尤洪明-最会保保险经纪有限公司-刘心荣</option><option title='鞠凌逸-合翔保险经纪有限公司-合翔' value='65970333' data-index='1'>鞠凌逸-合翔保险经纪有限公司-合翔</option><option title='乔亚冰-刘瑞萍-刘瑞萍' value='65970281' data-index='2'>乔亚冰-刘瑞萍-刘瑞萍</option><option title='乔亚冰-李佳雯-李佳雯' value='65970281' data-index='3'>乔亚冰-李佳雯-李佳雯</option><option title='乔亚冰-王科-王科' value='65970281' data-index='4'>乔亚冰-王科-王科</option><option title='廖倩文-刘建忠-刘建忠' value='65210105' data-index='5'>廖倩文-刘建忠-刘建忠</option><option title='鞠凌逸-苗帅-苗帅' value='65970333' data-index='6'>鞠凌逸-苗帅-苗帅</option><option title='鞠凌逸-向欢欢-向欢欢' value='65970333' data-index='7'>鞠凌逸-向欢欢-向欢欢</option><option title='廖倩文-蒋勇飞-蒋勇飞' value='65210105' data-index='8'>廖倩文-蒋勇飞-蒋勇飞</option><option title='廖倩文-齐新军-齐新军' value='65210105' data-index='9'>廖倩文-齐新军-齐新军</option></select>
				<input type='hidden' id='channelInfo.saleManName' name='channelInfo.saleManName' value='尤洪明' oldval='尤洪明'>
				<input type='hidden' id='channelInfo.subConferCode' name='channelInfo.subConferCode' value=''>
				<input type='hidden' id='channelInfo.coopBranchName' name='channelInfo.coopBranchName' value=''>
				<input type='hidden' id='channelInfo.teamCode' name='channelInfo.teamCode' value='65970601' oldval='65970601'>
				<input type='hidden' id='channelInfo.teamName' name='channelInfo.teamName' value='新疆分公司营业部经纪业务一部' oldval='新疆分公司营业部经纪业务一部'>
				<input type='hidden' id='channelInfo.uuid' name='channelInfo.uuid' value='832a2099e07a48efa32d0cd86440caae' oldval='832a2099e07a48efa32d0cd86440caae'>
			</td>
		</tr>
		<tr id='agentOrgsAndAgencyAgreementsInfo'>
			<th>代理机构：</th>
			<td>
				<select id='channelInfo.agentOrg' name='channelInfo.agentOrg' class='text' defval='' style='width:500px' disabled='disabled' oldval='2017O01538'><option title='2017O01538-最会保保险经纪有限公司' value='2017O01538' data-index='0'>2017O01538-最会保保险经纪有限公司</option><option title='2018O02738-合翔保险经纪有限公司' value='2018O02738' data-index='1'>2018O02738-合翔保险经纪有限公司</option><option title='2018A03329-刘瑞萍' value='2018A03329' data-index='2'>2018A03329-刘瑞萍</option><option title='2018A03263-李佳雯' value='2018A03263' data-index='3'>2018A03263-李佳雯</option><option title='2018A03243-王科' value='2018A03243' data-index='4'>2018A03243-王科</option><option title='2018A03241-刘建忠' value='2018A03241' data-index='5'>2018A03241-刘建忠</option><option title='2018A06154-苗帅' value='2018A06154' data-index='6'>2018A06154-苗帅</option><option title='2018A10066-向欢欢' value='2018A10066' data-index='7'>2018A10066-向欢欢</option><option title='2018A03242-蒋勇飞' value='2018A03242' data-index='8'>2018A03242-蒋勇飞</option><option title='2017A09335-齐新军' value='2017A09335' data-index='9'>2017A09335-齐新军</option></select>
				<input type='hidden' id='channelInfo.agentOrgName' name='channelInfo.agentOrgName' value='最会保保险经纪有限公司' oldval='最会保保险经纪有限公司'>
			</td>
			<th>中介协议：</th>
			<td>
				<select id='channelInfo.agencyTocol' name='channelInfo.agencyTocol' defval='' class='text' style='width:400px' disabled='disabled' oldval='B201765001396'><option title='B201765001396' value='B201765001396' data-index='0'>B201765001396</option><option title='B201865002018' value='B201865002018' data-index='1'>B201865002018</option><option title='B201865000709' value='B201865000709' data-index='2'>B201865000709</option><option title='B201865000676' value='B201865000676' data-index='3'>B201865000676</option><option title='B201865000675' value='B201865000675' data-index='4'>B201865000675</option><option title='B201865000699' value='B201865000699' data-index='5'>B201865000699</option><option title='B201865001345' value='B201865001345' data-index='6'>B201865001345</option><option title='B201865001944' value='B201865001944' data-index='7'>B201865001944</option><option title='B201865000700' value='B201865000700' data-index='8'>B201865000700</option><option title='B201765001410' value='B201765001410' data-index='9'>B201765001410</option></select>
			</td>
		</tr>
		<tr>
			<th>销售渠道：</th>
            <td>
				<select id='channelInfo.salesChannel' name='channelInfo.salesChannel' class='text' defval='' style='width:500px' disabled='disabled' oldval='E01'><option title='E01-经纪（专属）' value='E01' data-index='0'>E01-经纪（专属）</option><option title='E01-经纪（专属）' value='E01' data-index='1'>E01-经纪（专属）</option><option title='E03-经纪重客（个人）' value='E03' data-index='2'>E03-经纪重客（个人）</option><option title='E03-经纪重客（个人）' value='E03' data-index='3'>E03-经纪重客（个人）</option><option title='E03-经纪重客（个人）' value='E03' data-index='4'>E03-经纪重客（个人）</option><option title='E03-经纪重客（个人）' value='E03' data-index='5'>E03-经纪重客（个人）</option><option title='E03-经纪重客（个人）' value='E03' data-index='6'>E03-经纪重客（个人）</option><option title='E03-经纪重客（个人）' value='E03' data-index='7'>E03-经纪重客（个人）</option><option title='E03-经纪重客（个人）' value='E03' data-index='8'>E03-经纪重客（个人）</option><option title='E03-经纪重客（个人）' value='E03' data-index='9'>E03-经纪重客（个人）</option></select>
			</td>
			
			<th>业务来源：</th>
			<td>
				<select id='channelInfo.bizSource' name='channelInfo.bizSource' defval='' class='text' style='width:400px' disabled='disabled' oldval='E0101'><option title='E0101-经纪业务' value='E0101' data-index='0'>E0101-经纪业务</option><option title='E0101-经纪业务' value='E0101' data-index='1'>E0101-经纪业务</option><option title='E0302-个人代理' value='E0302' data-index='2'>E0302-个人代理</option><option title='E0302-个人代理' value='E0302' data-index='3'>E0302-个人代理</option><option title='E0302-个人代理' value='E0302' data-index='4'>E0302-个人代理</option><option title='E0302-个人代理' value='E0302' data-index='5'>E0302-个人代理</option><option title='E0302-个人代理' value='E0302' data-index='6'>E0302-个人代理</option><option title='E0302-个人代理' value='E0302' data-index='7'>E0302-个人代理</option><option title='E0302-个人代理' value='E0302' data-index='8'>E0302-个人代理</option><option title='E0302-个人代理' value='E0302' data-index='9'>E0302-个人代理</option></select>
			</td>
        </tr>
        <tr>
            <th>服务代码：</th>
            <td>
				<select id='channelInfo.coopBranch' name='channelInfo.coopBranch' defval='' class='text' style='width:500px' disabled='disabled' oldval='6597G1000337'><option title='6597G1000337-刘心荣' value='6597G1000337' data-index='0'>6597G1000337-刘心荣</option><option title='6597J4056000-合翔' value='6597J4056000' data-index='1'>6597J4056000-合翔</option><option title='6597G1000557-刘瑞萍' value='6597G1000557' data-index='2'>6597G1000557-刘瑞萍</option><option title='6597G1000558-李佳雯' value='6597G1000558' data-index='3'>6597G1000558-李佳雯</option><option title='6597G1000559-王科' value='6597G1000559' data-index='4'>6597G1000559-王科</option><option title='6597G1000561-刘建忠' value='6597G1000561' data-index='5'>6597G1000561-刘建忠</option><option title='6597G1000581-苗帅' value='6597G1000581' data-index='6'>6597G1000581-苗帅</option><option title='6597G1000611-向欢欢' value='6597G1000611' data-index='7'>6597G1000611-向欢欢</option><option title='6597G1000560-蒋勇飞' value='6597G1000560' data-index='8'>6597G1000560-蒋勇飞</option><option title='6597G1000503-齐新军' value='6597G1000503' data-index='9'>6597G1000503-齐新军</option></select>
			</td>
			<th>签单日期：</th>
            <td>
				<input type='text' class='text' style='width:395px' id='vehDTO.issueDate' name='vehDTO.issueDate' value='2018-12-29' readonly='readonly'>
				<input type='hidden' class='text' style='width:295px' id='appDate' name='appDate' value='2018-12-29' readonly='readonly'>
				
			</td>
        </tr>
        
        
       	<tr id='sellerTr' style='display: none'>
       		<th>销售员：</th>
       		<td colspan='3'>
       			<input data-rule-nowhitespace='true' data-msg-required='请填写销售员！' style='width:494px' type='text' id='channelInfo.seller' name='channelInfo.seller' value='' maxlength='24'>
       		</td>
       	</tr>
       	
       	
		<tr>
           	<th>合同争议解决方式：</th>
            <td>
            	<select id='vehDTO.disputeResolution' name='vehDTO.disputeResolution' class='text' style='width: 500px'>
            		
	            		
	            		
	            			
	            				
	            			
	            			
							<option title='诉讼' value='007001' selected=''>诉讼</option><option title='仲裁' value='007002'>仲裁</option>
						
					
				</select>
			</td>
            <th id='vehDTO.arbitrationInstitutionElementsTxt' style='display:none'><span class='red'>*</span>仲裁机构：</th>
            <td id='vehDTO.arbitrationInstitutionElementsVal' style='display:none'>
				<input type='text' class='text' style='width:330px' id='vehDTO.arbitrationInstitution' name='vehDTO.arbitrationInstitution' value='' maxlength='25' data-msg-required='仲裁机构不能为空！'>仲裁委员会
			</td>
        </tr>
        <tr>
       		<th>投保组合：</th>
       		<td colspan='3'>
       			<input type='checkbox' id='isAppCommercial_' name='commCombination' value='1' checked='checked'><label for='isAppCommercial_'>商业险 </label>
			    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			    <input type='checkbox' id='isAppCompulsory_' value='1' checked='checked' name='isAppCompulsory'><label for='isAppCompulsory_'>交强险 0332</label>
				 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
			    <input type='checkbox' id='isAppCompulsory_0334' value='1'><label for='isAppCompulsory_0334'>交强险 0334</label>
				
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<input type='checkbox' id='isAppAccident_' name='isAppAccident' value='1'><label for='isAppAccident_'>意外险</label>
				
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				
					
						<input type='checkbox' name='insuredManyYearsBtn' id='insuredManyYearsBtn' style='display: none;'>
						<span style='display: none;' name='insuredManyYearsBtn'>车贷投保多年</span>
						<input type='hidden' name='vehDTO.insuredManyYears' id='vehDTO.insuredManyYears' value=''>
					
				
       		</td>
       	</tr>
        
	        <tr id='commAppNo_rows' style='display: none;'>
				<th>
					商业险投保单号：
				</th>
				<td>
					<input type='text' readonly='readonly' id='commPolDTO.policyAppNo' name='commPolDTO.policyAppNo' value='' style='width:300px;border: none;background-color:#f8f8f8;'>
				</td>
				<!-- <th>
					商业险报价单号：
				</th> -->
					<input type='hidden' readonly='readonly' id='commPolDTO.deliveryCode' name='commPolDTO.deliveryCode' value='' style='width:300px;border: none;background-color:#f8f8f8;'>
					<input type='hidden' readonly='readonly' id='commPolDTO.trafficCheckCode' name='commPolDTO.trafficCheckCode'><!-- 交管校验码 -->
				<th>商业险投保查询码：</th>
				<td>
					<span>		
						<label id='commPolDTO.querySequenceNo_' class='text'></label>
						<input type='hidden' class='text' id='commPolDTO.querySequenceNo' name='commPolDTO.querySequenceNo' value='' readonly='readonly'>
						<input type='hidden' class='text' id='commPolDTO.querySequenceStart' name='commPolDTO.querySequenceStart' value=''>
						<input type='hidden' class='text' id='commPolDTO.querySequenceEnd' name='commPolDTO.querySequenceEnd' value=''>
						<input type='hidden' class='text' id='imageSerialNumber' name='imageSerialNumber' value=''>
					</span>
				</td>
			</tr>
			<tr id='commAppNo_rows2' style='display: none;'>
				<th>
					商业险上年保单号：
				</th>
				<td>
					<input type='text' readonly='readonly' id='ciRenewalNo' name='ciRenewalNo' value='' style='width:300px;border: none;background-color:#f8f8f8;'>
				</td>
			</tr>
		
		
			<tr id='compAppNo_rows' style='display: none;'>
				<th>
					交强险投保单号：
				</th>
				<td>
					<input type='text' readonly='readonly' id='compPolDTO.policyAppNo' name='compPolDTO.policyAppNo' value='' style='width:300px;border: none;background-color:#f8f8f8;'>
				</td>
				<!-- <th> 
					交强险报价单号：
				</th> -->
					<input type='hidden' readonly='readonly' id='compPolDTO.deliveryCode' name='compPolDTO.deliveryCode' value='' style='width:300px;border: none;background-color:#f8f8f8;'>
					<input type='hidden' readonly='readonly' id='compPolDTO.trafficCheckCode' name='compPolDTO.trafficCheckCode'><!-- 交管校验码 -->
				<th>交强险投保查询码：</th>
				<td>
					<span>
						<label id='compPolDTO.querySequenceNo_' class='text'></label>
						<input type='hidden' class='text' id='compPolDTO.querySequenceNo' name='compPolDTO.querySequenceNo' value='' readonly='readonly'>
						<input type='hidden' class='text' id='compPolDTO.querySequenceStart' name='compPolDTO.querySequenceStart' value=''>
						<input type='hidden' class='text' id='compPolDTO.querySequenceEnd' name='compPolDTO.querySequenceEnd' value=''>
					</span>
				</td>
			</tr>
			<tr id='compAppNo_rows2' style='display: none;'>
				<th>
					交强险上年保单号：
				</th>
				<td>
					<input type='text' readonly='readonly' id='sailRenewalNo' name='sailRenewalNo' value='' style='width:300px;border: none;background-color:#f8f8f8;'>
				</td>
			</tr>
		
		
		<input type='hidden' readonly='text' id='unionSJMrkComm' name='unionSJMrkComm'>
		<input type='hidden' readonly='text' id='unionSJMrkComp' name='unionSJMrkComp'>
		<input type='hidden' readonly='text' id='unionAppSerialNo' name='unionAppSerialNo' value=''>
		<input type='hidden' readonly='text' id='unionAppNoComm' name='unionAppNoComm' value=''>
		<input type='hidden' readonly='text' id='unionAppNoComp' name='unionAppNoComp' value=''>
        
        
	       	<tr id='commDateContainer'>
				<th>商业险保险期间：</th>
				<td colspan='3'>
					<span class='alignleft'>
						<input type='text' class='text hasDatepicker' id='commPolDTO.startDate' name='commPolDTO.startDate' data-rule-commstartdate='true' value='2018-12-30 00:00:00'><img class='ui-datepicker-trigger' src='/ipartner/common/images/calendar.gif' alt='...' title='...'>
						至
						<input type='text' class='text' id='commPolDTO.endDate' name='commPolDTO.endDate' readonly='readonly' value='2019-12-29 23:59:59'>
						共
						<input type='text' class='num' id='commPolDTO.insuredDays' name='commPolDTO.insuredDays' data-rule-comminsureddays='true' value='365' maxlength='5' onkeyup='this.value=this.value.replace(/[^+0-9]/g,'')'>
						天&nbsp;&nbsp;
						<input type='checkbox' id='commPolDTO.isNowFlag' name='commPolDTO.isNowFlag' title='即时生效' class='checkbox' value='1'>
						<label for='commPolDTO.isNowFlag'><span id='commPolDTO.isNowFlagText'>即时生效<span></span>
					</span>
				</label></span></td>
			</tr>
		
		
		
		
			<tr id='compDateContainer'>
				<th>交强险保险期间：</th>
				<td colspan='3'>
					<span class='alignleft'>
						<input type='text' class='text hasDatepicker' id='compPolDTO.startDate' name='compPolDTO.startDate' data-rule-compstartdate='true' value='2018-12-30 00:00:00'><img class='ui-datepicker-trigger' src='/ipartner/common/images/calendar.gif' alt='...' title='...'>
						至
						<input type='text' class='text' id='compPolDTO.endDate' name='compPolDTO.endDate' readonly='readonly' value='2019-12-29 23:59:59'>
						共
						<input type='text' class='num' id='compPolDTO.insuredDays' name='compPolDTO.insuredDays' data-rule-compinsureddays='true' value='365' maxlength='5' onkeyup='this.value=this.value.replace(/[^+0-9]/g,'')'>
						天&nbsp;&nbsp;
						<input type='checkbox' id='compPolDTO.isNowFlag' name='compPolDTO.isNowFlag' title='即时生效' class='checkbox' value='1'>
						<label for='compPolDTO.isNowFlag'>
							<span id='compPolDTO.isNowFlagText'>即时生效</span>
						</label>
						&nbsp;&nbsp;
						<span id='compPolDTO.recordDiv' style='display: none'>
							<input type='checkbox' id='compPolDTO.record' name='compPolDTO.record' title='补录' class='checkbox' value='1'>
							<span>补录</span>
							<span id='compPolDTO.handWorkDiv' style='display: none;'>
								&nbsp;&nbsp;
								<span><span class='red'>*</span>手工单单证号：<span>
								<input value='' type='text' class='text' id='compPolDTO.handWorkNo' onkeyup='this.value=this.value.replace(/[^\d]/g,'')' name='compPolDTO.handWorkNo' data-msg-required='请填写手工单单证号' maxlength='10'>
							</span>
						</span>
					</span>	
				</span></span></td>
			</tr>
		
		
	    <tr>
       		<th>签报流水号：</th>
			<td>
				<input type='text' class='text' style='width:330px' id='signTheWatermark' name='signTheWatermark' value='' maxlength='20'>
			</td>
			<th>车队号：</th>
			<td>
				<input type='text' class='text' style='width:330px' id='motorcadeNumber' name='motorcadeNumber' value='' maxlength='20'>
			</td>
		</tr>
		<tr>
		
       		<th>保单类型：</th>
			<td>
				<label>
					<input type='radio' class='input25' id='appPolicyType' name='appPolicyType' value='1'>监制保单
				</label>
				<label>&nbsp;&nbsp;&nbsp;&nbsp;
					<input type='radio' class='input25' id='appPolicyType' name='appPolicyType' value='0' checked='checked'>电子保单
				</label>
			</td>
		
		
		</tr>
        <tr style='display: none;'>
           	<th><span class='red'>*</span>反洗钱可疑交易特征：</th>
           	<td colspan='3'>
               <span>
					
					<select type='hidden' id='antiMoney' class='text' style='width: 88%'>
						
					</select>
					
					<input type='hidden' id='vehDTO.antiMoney' name='vehDTO.antiMoney' value=''>
				</span>
          </td>
      </tr>
    </tbody></table>
</div>";

    }
}
