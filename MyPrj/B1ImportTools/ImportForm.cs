using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAPbobsCOM;
using NPOI;

namespace B1ImportTools
{
    public partial class ImportForm : Form
    {

        /// <summary>
        /// 记录选择的文件路径
        /// </summary>
        public static string importPath;
        public ImportForm()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void ImportForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.richTextBox1.Text = "";
                importPath = this.openFileDialog1.FileName;
                importPath = importPath.Substring(0,importPath.LastIndexOf('\\'));
                foreach (var item in openFileDialog1.SafeFileNames)
                {
                    this.richTextBox1.Text = this.richTextBox1.Text + item + ";\r\n";
                }

                //if (CommonUse.curCompany.Connected)
                //{
                //    this.richTextBox2.Text = "连接上了";
                //}
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //01、面板类提料单.xls
            ExcelHelper excelhelper = new ExcelHelper("D:\\01、面板类提料单.xls");
            excelhelper.transExcel();
            
            
        }

        private void ImportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
