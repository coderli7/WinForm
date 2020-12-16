using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tessnet2;

namespace ImageUtils
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                Random random = new Random();

                WebRequest request = WebRequest.Create("https://car.tpi.cntaiping.com:8001/casserver/getRandCode?" + random.NextDouble().ToString());
                WebResponse response = request.GetResponse();
                Stream st = response.GetResponseStream();
                Bitmap bitmap = (Bitmap)Bitmap.FromStream(st);


                bitmap.Save("C:\\a.jpg");
                pictureBox1.Image = bitmap;


                //UnCodebase ud = new UnCodebase(bitmap);
                //bitmap = ud.GrayByPixels();
                //ud.ClearNoise(128, 2);

                //pictureBox1.Image = bitmap;

                //tessnet2.Tesseract ocr = new tessnet2.Tesseract();//声明一个OCR类
                //ocr.SetVariable("tessedit_char_whitelist", "0123456789"); //设置识别变量，当前只能识别数字。

                ////ocr.Init(Application.StartupPath + @"\\tmpe", "eng", true); //应用当前语言包。注，Tessnet2是支持多国语的。语言包下载链接：http://code.google.com/p/tesseract-ocr/downloads/list
                //List<tessnet2.Word> result = ocr.DoOCR(bitmap, Rectangle.Empty);//执行识别操作
                //string code = result[0].Text;
                //this.richTextBox1.Text = code;

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
