using System;
using System.Data;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;

namespace Library.Excel
{
    public class NpoiHelper
    {
        public static HSSFWorkbook BuildWorkbook(DataTable dt)
        {
            var book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            //Data Rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow drow = sheet.CreateRow(i);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = drow.CreateCell(j, CellType.String);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            //自动列宽
            for (int i = 0; i <= dt.Columns.Count; i++)
                sheet.AutoSizeColumn(i, true);

            return book;
        }

        public static XSSFWorkbook BuildWorkbook07(DataTable dt)
        {
            var book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet("Sheet1");
            //Data Rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow drow = sheet.CreateRow(i);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = drow.CreateCell(j, CellType.String);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            //自动列宽
            for (int i = 0; i <= dt.Columns.Count; i++)
                sheet.AutoSizeColumn(i, true);

            return book;
        }

        public static void ExportExcel(DataTable dt, string fileName = "")
        {
            //生成Excel
            IWorkbook book = BuildWorkbook(dt);

            //web 下载
            if (fileName == "")
                fileName = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now);
            fileName = fileName.Trim();
            string ext = Path.GetExtension(fileName);

            if (ext.ToLower() == ".xls" || ext.ToLower() == ".xlsx")
                fileName = fileName.Replace(ext, string.Empty);
            HttpResponse httpResponse = HttpContext.Current.Response;
            httpResponse.Clear();
            httpResponse.Buffer = true;
            httpResponse.Charset = Encoding.UTF8.BodyName;
            httpResponse.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");
            httpResponse.ContentEncoding = Encoding.UTF8;
            httpResponse.ContentType = "application/vnd.ms-excel; charset=UTF-8";
            book.Write(httpResponse.OutputStream);
            httpResponse.End();
        }

        /// <summary>
        /// 可以导出
        /// var ms = NpoiHelper.RenderToExcel(dt);
        ///return File(ms, "application/vnd.ms-excel", "用户信息" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(DataTable table)
        {
            MemoryStream ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                // handling header.
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        public static MemoryStream RenderToExcel(IDataReader reader)
        {
            MemoryStream ms = new MemoryStream();

            using (reader)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                int cellCount = reader.FieldCount;

                // handling header.
                for (int i = 0; i < cellCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(reader.GetName(i));
                }

                // handling value.
                int rowIndex = 1;
                while (reader.Read())
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    for (int i = 0; i < cellCount; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(reader[i].ToString());
                    }
                    rowIndex++;
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strFilePath">路径</param>
        /// <param name="strTableName">datatable的tableName</param>
        /// <param name="iSheetIndex">sheet表</param>
        /// <returns></returns>
        public static DataTable XlSToDataTable(string strFilePath, string strTableName, int iSheetIndex)
        {
            string strExtName = Path.GetExtension(strFilePath);

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }
            if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    var workbook = new HSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(iSheetIndex);

                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (HSSFRow)rows.Current;
                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;

                                case CellType.Error:
                                    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                    break;

                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;

                                        case CellType.Error:
                                            dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                            break;

                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = item.NumericCellValue;
                                            }
                                            break;

                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;

                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;

                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;

                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;

                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
    }
}