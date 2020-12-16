using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace FeiCheDS
{
    public delegate void SetTextCallBack(string text);

    public partial class UploadFileForm : Form
    {
        #region 一些常量


        /// <summary>
        /// 单号
        /// </summary>
        public static String docNo { get; set; }

        #endregion

        #region 初始化

        public UploadFileForm()
        {
            InitializeComponent();
        }

        private void UploadFileForm_Load(object sender, EventArgs e)
        {
            TokenService.GetToken();
            ShowLog(String.Format("登陆成功({0})!", TokenService.Token));
        }

        #endregion

        #region 按钮点击事件
        private void OpenDataViewFormBtn_Click(object sender, EventArgs e)
        {
            new DataViewForm().Show();
        }
        private void fileSltedBtn_Click(object sender, EventArgs e)
        {
            if (!ParamCheck())
            { return; }
            //定义一个文件打开控件
            OpenFileDialog ofd = new OpenFileDialog();
            //设置打开对话框的初始目录，默认目录为exe运行文件所在的路径
            //ofd.InitialDirectory = Application.StartupPath;
            //设置打开对话框的标题
            ofd.Title = "请选择要打开的文件";
            //设置打开对话框可以多选
            ofd.Multiselect = true;
            //设置对话框打开的文件类型
            //文本文件|*.txt|音频文件|*.wav|图片文件|*.jpg|所有文件|
            ofd.Filter = "文本文件|*.txt|音频文件|*.wav|图片文件|*.jpg|所有文件|";
            //设置文件对话框当前选定的筛选器的索引
            ofd.FilterIndex = 5;
            //设置对话框是否记忆之前打开的目录
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //获取用户选择的文件完整路径
                string filePath = ofd.FileName;
                //获取对话框中所选文件的文件名和扩展名，文件名不包括路径
                string fileName = ofd.SafeFileName;
                StringBuilder allFilePathSb = new StringBuilder();
                foreach (var item in ofd.FileNames)
                {
                    allFilePathSb.AppendLine(item);
                }
                this.fileRichTxt.Text = allFilePathSb.ToString();
                //using (FileStream fsRead = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                //{
                //    //定义二进制数组
                //    byte[] buffer = new byte[1024 * 1024 * 5];
                //    //从流中读取字节
                //    int r = fsRead.Read(buffer, 0, buffer.Length);
                //    OutLog(Encoding.Default.GetString(buffer, 0, r));
                //}
            }

        }
        private void uptBtn_Click(object sender, EventArgs e)
        {
            UploadFileForm.docNo = this.docNoTxt.Text;
            if (!ParamCheck())
            { return; }

            /*
             * 1.将选中图片，转移到安装目录下，生成新的目录和文件名
             * 目录规则为：报案号+下划线+原名称+后缀。例如（111_abc.jpg）
             * 2.将文件名信息，传递给安诚
             */

            #region 1.选中
            String docNo = docNoTxt.Text;
            //String basePath = String.Format("{0}\\{1}", System.Environment.CurrentDirectory, "image");
            String basePath = Config.ImgLocation;
            if (!Directory.Exists(basePath))
            { Directory.CreateDirectory(basePath); }
            String docPath = String.Format("{0}\\{1}", basePath, docNo);
            if (!Directory.Exists(docPath))
            { Directory.CreateDirectory(docPath); }
            foreach (var item in fileRichTxt.Lines)
            {
                if (item.LastIndexOf(@"\") > 0)
                {
                    string preFileName = item.Substring(item.LastIndexOf(@"\") + 1);
                    string newFileName = String.Format("{0}_{1}", docNo, preFileName);
                    String newFilePath = String.Format(@"{0}\{1}", docPath, newFileName);
                    File.Copy(item, newFilePath, true);
                }
            }
            #endregion

            #region 2.上传
            FileUploadService.UploadFile();
            #endregion

            this.fileRichTxt.Text = "";
            MessageBox.Show("上传成功！");
        }

        #endregion

        #region 参数校验

        public bool ParamCheck()
        {
            bool ctnFlag = true;
            if (String.IsNullOrEmpty(this.docNoTxt.Text))
            {
                MessageBox.Show("请填写报案号!");
                ctnFlag = false;
            }
            return ctnFlag;
        }


        private void ShowLog(string text)
        {
            if (this.logRichTxt.InvokeRequired)
            {
                SetTextCallBack stcb = new SetTextCallBack(ShowLog);
                this.Invoke(stcb, new object[] { text });
            }
            else
            {
                this.logRichTxt.Text = string.Format("{0}\r\n{1}", this.logRichTxt.Text, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + text);
            }
        }

        #endregion

    }
}
