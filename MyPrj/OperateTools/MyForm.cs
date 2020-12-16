using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperateTools
{
    public partial class MyForm : Form
    {

        private System.Timers.Timer timer1;


        public MyForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initTimer1();

        }

        private void initTimer1()
        {
            timer1 = new System.Timers.Timer();
            timer1.Interval = 300000;//
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Start();
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                BdTask bdTask = new BdTask();
                bdTask.excute();
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
