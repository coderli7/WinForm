using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace B1ImportTools
{
    public class ExcelHelper : IDisposable
    {
        public void Dispose()
        {
        }

        public ExcelHelper(string fileName)
        {
            this.fileName = fileName;
        }
        /// <summary>
        /// 记录文件名
        /// </summary>
        private string fileName;

        private IWorkbook workbook;

        public void transExcel()
        {

            FileStream fs = new FileStream(this.fileName, FileMode.Open, FileAccess.ReadWrite);

            if (true)
            {

            }
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            {
                workbook = new XSSFWorkbook(fs);
            }

            else if (fileName.IndexOf(".xls") > 0)
            {// 2003版本
                workbook = new HSSFWorkbook(fs);
            }


            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                
            }



        }

    }
}
