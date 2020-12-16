using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Windows.Forms;

namespace Monitor
{
    public class ExcelHandler
    {

        public static string Exportpath = Environment.CurrentDirectory + "\\ExcelData";
        /// <summary>
        /// 导出报价成功率Excel
        /// </summary>
        /// <param name="dataList"></param>
        public static void GenScussedExcel(List<Dictionary<string, string>> dataList)
        {
            HSSFWorkbook hssfworkbook;
            hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            IRow row = sheet1.CreateRow(0);
            for (int i = 0; i < dataList.Count; i++)
            {
                ICell curCell = row.CreateCell(i, CellType.String);
                curCell.SetCellValue(dataList[i]["CityName"]);
            }

            row = sheet1.CreateRow(1);
            for (int i = 0; i < dataList.Count; i++)
            {
                ICell curCell = row.CreateCell(i, CellType.String);
                curCell.SetCellValue(dataList[i]["CityPercent"]);
                HSSFPatriarch patr1 = sheet1.CreateDrawingPatriarch() as HSSFPatriarch;
                HSSFComment comment1 = patr1.CreateCellComment(new HSSFClientAnchor(0, 0, 0, 0, 1, 3, 3, 7)) as HSSFComment;
                comment1.String = new HSSFRichTextString(dataList[i]["ErrorInfo"]);
                curCell.CellComment = comment1;
            }

            if (!Directory.Exists(Exportpath))
            {
                Directory.CreateDirectory(Exportpath);
            }

            using (FileStream fs = File.OpenWrite(string.Format(@"{0}\{1}报价_成功率.xls", Exportpath, DateTime.Now.ToString("yyyy-MM-dd HH-mm"))))
            {
                hssfworkbook.Write(fs);
            }

            //MessageBox.Show("导出完成！");

            System.Diagnostics.Process.Start("explorer.exe", Exportpath);
        }

        /// <summary>
        /// 导出报价耗时excel
        /// </summary>
        /// <param name="dataList"></param>
        public static void GenTimeExcel(List<Dictionary<string, string>> dataList)
        {
            HSSFWorkbook hssfworkbook;
            hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            IRow row = sheet1.CreateRow(0);
            for (int i = 0; i < dataList.Count; i++)
            {
                ICell curCell = row.CreateCell(i, CellType.String);
                curCell.SetCellValue(dataList[i]["CityName"]);
            }

            row = sheet1.CreateRow(1);
            for (int i = 0; i < dataList.Count; i++)
            {
                ICell curCell = row.CreateCell(i, CellType.String);
                curCell.SetCellValue(dataList[i]["ZeroToTwenty"]);
            }

            row = sheet1.CreateRow(2);
            for (int i = 0; i < dataList.Count; i++)
            {
                ICell curCell = row.CreateCell(i, CellType.String);
                curCell.SetCellValue(dataList[i]["TwentyToFourty"]);
            }

            row = sheet1.CreateRow(3);
            for (int i = 0; i < dataList.Count; i++)
            {
                ICell curCell = row.CreateCell(i, CellType.String);
                curCell.SetCellValue(dataList[i]["FourtyToSixty"]);
            }

            row = sheet1.CreateRow(4);
            for (int i = 0; i < dataList.Count; i++)
            {
                ICell curCell = row.CreateCell(i, CellType.String);
                curCell.SetCellValue(dataList[i]["SixtyToBigger"]);
            }

            if (!Directory.Exists(Exportpath))
            {
                Directory.CreateDirectory(Exportpath);
            }

            using (FileStream fs = File.OpenWrite(string.Format(@"{0}\{1}报价_耗时.xls", Exportpath, DateTime.Now.ToString("yyyy-MM-dd HH-mm"))))
            {
                hssfworkbook.Write(fs);
            }
            //MessageBox.Show("导出完成！");
            System.Diagnostics.Process.Start("explorer.exe", Exportpath);
        }
    }
}
