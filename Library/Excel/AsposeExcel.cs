using System;
using System.Data;
using System.IO;
using Aspose.Cells;

namespace Library.Excel
{
    public class AsposeExcel
    {
        public static void ToExcel(DataTable dt, string fullFileName)
        {
            Workbook workbook = new Workbook();
            Worksheet cellSheet = workbook.Worksheets[0];
            cellSheet.Name = dt.TableName;
            int rowIndex = 0;
            int colIndex = 0;
            int colCount = dt.Columns.Count;
            int rowCount = dt.Rows.Count;
            //列名的处理
            for (int i = 0; i < colCount; i++)
            {
                cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                //cellSheet.Cells[rowIndex, colIndex].SetStyle.Font.IsBold = true;
                //cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";
                colIndex++;
            }
            // Style style = workbook.Styles[workbook.Styles.Add()];
            Style style = workbook.CreateStyle();
            style.Font.Name = "Arial";
            style.Font.Size = 10;
            StyleFlag styleFlag = new StyleFlag();
            cellSheet.Cells.ApplyStyle(style, styleFlag);
            rowIndex++;
            for (int i = 0; i < rowCount; i++)
            {
                colIndex = 0;
                for (int j = 0; j < colCount; j++)
                {
                    cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Rows[i][j].ToString());
                    colIndex++;
                }
                rowIndex++;
            }
            cellSheet.AutoFitColumns();
            fullFileName = Path.GetFullPath(fullFileName);
            workbook.Save(fullFileName);
        }

        public static void ToExcel(DataTable dt)
        {
            if (dt.Rows.Count <= 0)
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('没有检测到需要导出数据!');</script>");
                return;
            }
            Workbook workbook = new Workbook();
            Worksheet cellSheet = workbook.Worksheets[0];
            cellSheet.Name = dt.TableName;
            int rowIndex = 0;
            int colIndex = 0;
            int colCount = dt.Columns.Count;
            int rowCount = dt.Rows.Count;
            //列名的处理
            for (int i = 0; i < colCount; i++)
            {
                cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                //cellSheet.Cells[rowIndex, colIndex].SetStyle.Font.IsBold = true;
                //cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";
                colIndex++;
            }
            // Style style = workbook.Styles[workbook.Styles.Add()];
            Style style = workbook.CreateStyle();
            style.HorizontalAlignment = TextAlignmentType.Center; //设置居中
            style.Font.Name = "Arial";
            style.Font.Size = 12;
            StyleFlag styleFlag = new StyleFlag();
            cellSheet.Cells.ApplyStyle(style, styleFlag);
            rowIndex++;
            for (int i = 0; i < rowCount; i++)
            {
                colIndex = 0;
                for (int j = 0; j < colCount; j++)
                {
                    cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Rows[i][j].ToString());
                    colIndex++;
                }
                rowIndex++;
            }
            cellSheet.AutoFitColumns();
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Save(ms, new OoxmlSaveOptions(SaveFormat.Xlsx));//默认支持xls版，需要修改指定版本
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                workbook = null;
                System.Web.HttpContext.Current.Response.End();
            }
        }

        public static void FromExcel(string path)
        {
        }
    }
}