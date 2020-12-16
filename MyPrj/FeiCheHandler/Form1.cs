using System;
using System.Collections.Generic;
using System.Windows.Automation;
using System.Windows.Forms;

namespace FeiCheHandler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            AutomationElement calWindow = null;



            //2362396
            int parent = 787508;
            int id = WindowFormAPI.GetDlgCtrlID(new IntPtr(parent));

            var res = WindowFormAPI.GetChildWindows(parent);

            List<int> childs = WindowFormAPI.GetChildWindows(id);


        }
    }
}
