using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryCheck
{

    delegate void setValDelegate(String str1);

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartCheckACStatus();
        }


        private void StartCheckACStatus()
        {
            try
            {
                System.Timers.Timer bateryTimerCheck = new System.Timers.Timer();
                bateryTimerCheck.Interval = 10000;
                bateryTimerCheck.Elapsed += BateryTimerCheck_Elapsed;
                bateryTimerCheck.Start();
            }
            catch (Exception ex)
            {
            }
        }

        private void BateryTimerCheck_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool acStatus = CheckIfACOn();
            String acStatusTips = "";
            if (acStatus)
            {
                acStatusTips = "电源连上了";
            }
            else
            {
                ShutDownPC();
                acStatusTips = "电源断开,准备关机";
            }
            this.richTextBox1.BeginInvoke(new setValDelegate((c) =>
            {
                String rchStr = this.richTextBox1.Text;
                if (!String.IsNullOrEmpty(rchStr) && rchStr.Length > 1000)
                {
                    this.richTextBox1.Text = "";
                }
                this.richTextBox1.Text += (DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss   ") + c + "\r\n");
            }), acStatusTips);

        }

        public void ShutDownPC()
        {
            try
            {

                Process.Start("shutdown", "-s -t 0");


            }
            catch (Exception)
            {
            }
        }



        public bool CheckIfACOn()
        {
            SystemPowerStatus sysPowerStatus = new SystemPowerStatus();
            GetSystemPowerStatus(out sysPowerStatus);
            return ACPowerState.Online == sysPowerStatus.LineStatus;
        }


        [DllImport("kernel32.dll")]
        protected static extern Boolean GetSystemPowerStatus(out SystemPowerStatus sps);

        protected enum ACPowerState : byte
        {
            Offline = 0,
            Online = 1,
            Unknown = 255
        }

        protected enum BatteryFlag : byte
        {
            High = 1,
            Low = 2,
            Critical = 4,
            Charging = 8,
            NoSystemBattery = 128,
            Unknown = 255
        }

        protected struct SystemPowerStatus
        {
            public ACPowerState LineStatus;
            public BatteryFlag _BatteryFlag;
            public Byte _BatteryLifePercent;
            public Byte _Reserved1;
            public Int32 _BatteryLifeTime;
            public Int32 _BatteryFullLifeTime;
        }


    }
}
