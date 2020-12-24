using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            int memSize = GetPhisicalMemory();

        }



        /// <summary>
        /// 获取系统内存大小
        /// 以G为单位
        /// </summary>
        /// <returns>内存大小（单位M）</returns>
        private int GetPhisicalMemory()
        {
            int phyMemSize = 0;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher();   //用于查询一些如系统信息的管理对象 
                searcher.Query = new SelectQuery("Win32_PhysicalMemory ", "", new string[] { "Capacity" });//设置查询条件 
                ManagementObjectCollection collection = searcher.Get();   //获取内存容量 
                ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();
                long capacity = 0;
                while (em.MoveNext())
                {
                    ManagementBaseObject baseObj = em.Current;
                    if (baseObj.Properties["Capacity"].Value != null)
                    {
                        try
                        {
                            capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                }
                phyMemSize = (int)(capacity / 1024 / 1024 / 1024);
            }
            catch (Exception)
            {
            }
            return phyMemSize;
        }
    }
}
