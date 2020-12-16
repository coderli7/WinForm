using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B1ImportTools
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //CommonUse.Login();
            ImportForm importForm = new ImportForm();
            importForm.Show();
            this.Hide();
            //Application.Run(importForm);
            //importForm.Show();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
