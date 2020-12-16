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
using System.Reflection;
using System.Collections;

namespace FeiCheDS
{
    public partial class DataViewForm : Form
    {

        #region 假数据

        List<String> docList = new List<string>();
        Dictionary<String, List<String>> docImageDic = new Dictionary<string, List<String>>();

        ImageInfoModel tmpImgInfo = new ImageInfoModel();

        private void InitBaseData()
        {









            //1.案件列表
            docList = new List<string>();
            docList.Add("111");
            docList.Add("222");
            docList.Add("333");
            docList.Add("444");

            //2.案件对应图片
            docImageDic = new Dictionary<string, List<String>>();
            List<String> imgList1 = new List<string>();
            imgList1.Add("A1.jpg");
            imgList1.Add("A2.jpg");
            imgList1.Add("A3.jpg");
            imgList1.Add("A4.jpg");
            imgList1.Add("A5.jpg");
            docImageDic.Add("111", imgList1);

            List<String> imgList2 = new List<string>();
            imgList2.Add("B1.jpg");
            imgList2.Add("B2.jpg");
            imgList2.Add("B3.jpg");
            imgList2.Add("B4.jpg");
            imgList2.Add("B5.jpg");
            docImageDic.Add("222", imgList2);

            List<String> imgList3 = new List<string>();

            imgList3.Add("C1.jpg");
            imgList3.Add("C2.jpg");
            imgList3.Add("C3.jpg");
            imgList3.Add("C4.jpg");
            imgList3.Add("C5.jpg");
            docImageDic.Add("333", imgList3);

            List<String> imgList4 = new List<string>();

            imgList4.Add("D1.jpg");
            imgList4.Add("D2.jpg");
            imgList4.Add("D3.jpg");
            imgList4.Add("D4.jpg");
            docImageDic.Add("444", imgList4);

            //3.造假照片


            tmpImgInfo.titleDic = new Dictionary<string, string>();
            tmpImgInfo.titleDic.Add("单据号", "DSAOINDSI123456789");
            tmpImgInfo.titleDic.Add("单据类型", "药物清单");
            tmpImgInfo.titleDic.Add("单据金额", "1000");
            tmpImgInfo.titleDic.Add("日期", "2019-10-11");


            tmpImgInfo.lineList = new List<List<string>>();

            List<String> line1 = new List<string>();
            line1.Add("阿莫西林");
            line1.Add("3");
            line1.Add("12.34元");
            tmpImgInfo.lineList.Add(line1);

            List<String> line2 = new List<string>();
            line2.Add("感康");
            line2.Add("4");
            line2.Add("132.34元");
            tmpImgInfo.lineList.Add(line2);

            List<String> line3 = new List<string>();
            line3.Add("复方氨酚烷胺片");
            line3.Add("31");
            line3.Add("14元");
            tmpImgInfo.lineList.Add(line3);

            List<String> line4 = new List<string>();
            line4.Add("诺氟沙星");
            line4.Add("5");
            line4.Add("12元");
            tmpImgInfo.lineList.Add(line4);


            List<String> line5 = new List<string>();
            line5.Add("999感冒灵颗粒");
            line5.Add("3");
            line5.Add("12.34元");
            tmpImgInfo.lineList.Add(line5);


            List<String> line6 = new List<string>();
            line6.Add("皮康王");
            line6.Add("30");
            line6.Add("13元");
            tmpImgInfo.lineList.Add(line6);
        }

        #endregion

        public DataViewForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitBaseData();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //fun1();
            //this.dataGridView1.Rows.Clear();
            //this.dataGridView1.Rows.Add(docList.Count);
            //for (int i = 0; i < docList.Count; i++)
            //{
            //    this.dataGridView1.Rows[i].Cells[0].Value = docList[i];
            //}
        }

        #region 表格选中事件


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var no = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            var ImgList = docImageDic.Where(c => c.Key.Equals(no)).First().Value;
            ClearDV2();
            ClearDV3();
            this.dataGridView2.Rows.Add(ImgList.Count);
            for (int i = 0; i < ImgList.Count; i++)
            {
                this.dataGridView2.Rows[i].Cells[0].Value = no;
                this.dataGridView2.Rows[i].Cells[1].Value = ImgList[i];
            }


        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var no = this.dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            var imgNo = this.dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            InitBaseData();
            tmpImgInfo.titleDic.Add("报案号:", no);
            tmpImgInfo.titleDic.Add("图片名:", imgNo);
            ShowImgData(tmpImgInfo);
            ShowImg(tmpImgInfo);
        }


        #endregion

        #region 清空数据


        /// <summary>
        ///  清空第二页签数据
        /// </summary>
        public void ClearDV2()
        {
            this.dataGridView2.Rows.Clear();
        }

        /// <summary>
        /// 清空第三页签数据
        /// </summary>
        public void ClearDV3()
        {
            this.dataGridView3.Rows.Clear();
            this.dataGridView3.Columns.Clear();
            this.richTextBox1.Text = "";
        }

        #endregion

        #region 展示数据

        public void ShowImgData(ImageInfoModel ImgInfo)
        {
            try
            {
                ClearDV3();

                //1.抬头

                StringBuilder titleSb = new StringBuilder();
                foreach (var item in ImgInfo.titleDic)
                {
                    titleSb.AppendLine(String.Format("{0}:{1}", item.Key, item.Value));
                }

                this.richTextBox1.Text = titleSb.ToString();

                //2.行明细

                if (ImgInfo.lineList != null && ImgInfo.lineList.Count > 0)
                {
                    List<String> firLineList = ImgInfo.lineList.First();
                    for (int i = 0; i < firLineList.Count; i++)
                    {
                        this.dataGridView3.Columns.Add(string.Format("{0}{1}", "Columns", i), string.Format("{0}{1}", "列", i));
                    }
                    this.dataGridView3.Rows.Add(ImgInfo.lineList.Count);

                    for (int lineIdx = 0; lineIdx < ImgInfo.lineList.Count; lineIdx++)
                    {
                        for (int colIdx = 0; colIdx < firLineList.Count; colIdx++)
                        {
                            this.dataGridView3.Rows[lineIdx].Cells[colIdx].Value = ImgInfo.lineList[lineIdx][colIdx];
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public void ShowImg(ImageInfoModel ImgInfo)
        {
            this.pictureBox1.Image = Image.FromFile("D:\\07007246.jpg");
        }
        #endregion

        #region tmp


        public StringBuilder sb = new StringBuilder();

        private void fun1()
        {
            String data = File.ReadAllText(@"C:\Users\Administrator\Desktop\非车-医审\票据-1.txt");
            Root_PiaoJu tmp = JsonConvert.DeserializeObject<Root_PiaoJu>(data);
            fun2(tmp);
            this.richTextBox1.Text = this.sb.ToString();
        }


        private void fun2<T>(T data)
        {
            Type type = data.GetType();
            var pros = type.GetProperties();
            foreach (PropertyInfo item in pros)
            {
                var name = item.Name;
                var value = item.GetValue(data, null);
                DescriptionAttribute atriDes = ((DescriptionAttribute)Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)));
                var des = (atriDes == null) ? "" : atriDes.Description;
                if (value != null && !String.IsNullOrEmpty(des))
                {
                    String curType = value.GetType().Name;
                    if ("String".Equals(curType))
                    {
                        //直接追加即可
                        String curVal = String.Format("{0}({1}):{2}", des, name, value);
                        sb.AppendLine(curVal);
                    }
                    else
                    {
                        if (curType.Contains("List"))
                        {
                            IEnumerable ir = (IEnumerable)value;
                            foreach (var curItem in ir)
                            {
                                sb.AppendLine();
                                sb.AppendLine(String.Format("****************{0}({1})****************", des, name));
                                fun2(curItem);
                            }
                        }
                        else
                        {
                            sb.AppendLine();
                            sb.AppendLine(String.Format("****************{0}({1})****************", des, name));
                            fun2(value);
                        }

                    }
                }
            }
        }
        #endregion

    }
}
