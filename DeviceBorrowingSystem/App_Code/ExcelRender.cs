using System.Data;
using System.IO;
using System.Text;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using System;


public class ExcelRenderNode : IDisposable
{
    public MemoryStream ms = new MemoryStream();
    public string errMsg = "";

    public void Dispose()
    {
        this.ms.Dispose();
    }
}
/// <summary>
/// 使用NPOI操作Excel，无需Office COM组件
/// Created By 囧月 http://lwme.cnblogs.com
/// 部分代码取自http://msdn.microsoft.com/zh-tw/ee818993.aspx
/// NPOI是POI的.NET移植版本，目前稳定版本Support xls, xlsx, docx文件格式的读写
/// NPOI官方网站http://npoi.codeplex.com/
/// </summary>
public class ExcelRender
{
    public HSSFWorkbook _MyWorkbook = null;
    public ISheet _MyWorkSheet = null;
    public ICell _MyCell = null;
    public IRow _MyRow = null;
    public ICellStyle _MyCellStyle = null;
    /// <summary>
    /// 新建Excel
    /// </summary>
    public void NewExcel()
    {
        _MyWorkbook = new HSSFWorkbook();
    }
    public void OpenExcel(string fileName)
    {
        FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
        _MyWorkbook = new HSSFWorkbook(file);
    }
    /// <summary>
    /// 新建Sheet
    /// </summary>
    /// <param name="sheetName"></param>
    public void CreateSheet(string sheetName)
    {
        //HSSFWorkbook book = new HSSFWorkbook();
        if (_MyWorkbook == null)
            NewExcel();
        _MyWorkSheet = _MyWorkbook.CreateSheet(sheetName);
        //return _MyWorkSheet;
    }
    /// <summary>
    /// 向单元格写入数据
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public void SetCellValue(int row, int col, object value, System.TypeCode type = TypeCode.String)
    {
        GetCell(row, col);

        switch (type)
        {
            case System.TypeCode.String:
                _MyCell.SetCellValue(value.ToString());
                break;
            case System.TypeCode.Double:
                double doubV = 0;
                double.TryParse(String.Format("{0:0.0}", value), out doubV);
                _MyCell.SetCellValue(doubV);
                break;
            case TypeCode.DateTime:
                if (value == null || value.ToString() == String.Empty)
                {
                    _MyCell.SetCellValue("");
                }
                else
                {
                    string date = Convert.ToDateTime(value).ToString("yyyy/MM/dd HH:mm");
                    _MyCell.SetCellValue(date);
                }
                break;
            default:
                _MyCell.SetCellValue("");
                break;
        }
    }
    /// <summary>
    /// 设置单元格值
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public string GetCellValue(int row, int col)
    {
        GetCell(row, col);
        if (_MyCell == null)
            return String.Empty;

        switch (_MyCell.CellType)
        {
            case CellType.Blank:
                return string.Empty;
            case CellType.Boolean:
                return _MyCell.BooleanCellValue.ToString();
            case CellType.Error:
                return _MyCell.ErrorCellValue.ToString();
            case CellType.Numeric:
            case CellType.Unknown:
            default:
                return _MyCell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
            case CellType.String:
                return _MyCell.StringCellValue;
            case CellType.Formula:
                try
                {
                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(_MyCell.Sheet.Workbook);
                    e.EvaluateInCell(_MyCell);
                    return _MyCell.ToString();
                }
                catch
                {
                    return _MyCell.NumericCellValue.ToString();
                }
        }
    }
    /// <summary>
    /// 定位单元格
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void GetCell(int row, int col)
    {
        IRow cellrow = null;
        cellrow = _MyWorkSheet.GetRow(row);
        if (cellrow == null)
            cellrow = _MyWorkSheet.CreateRow(row);
        ICell cell = null;
        cell = cellrow.GetCell(col);
        if (cell == null)
            cell = cellrow.CreateCell(col);

        _MyCell = cell;
        //_MyCell.SetAsActiveCell();
    }
    /// <summary>
    /// 定位某行
    /// </summary>
    /// <param name="row"></param>
    private void GetRow(int row)
    {
        IRow cellrow = null;
        cellrow = _MyWorkSheet.GetRow(row);
        if (cellrow == null)
            cellrow = _MyWorkSheet.CreateRow(row);

        _MyRow = cellrow;
    }
    /// <summary>
    /// 设置列宽
    /// </summary>
    /// <param name="col"></param>
    /// <param name="width"></param>
    public void SetCellWidth(int col, int width)
    {
        _MyWorkSheet.SetColumnWidth(col, width * 256);

        //GetCell(row, col);
        //_MyCell.SetCellFormula(
    }
    /// <summary>
    /// 设置行高
    /// </summary>
    /// <param name="row"></param>
    /// <param name="height"></param>
    public void SetRowHeight(int row, int height)
    {
        GetRow(row);
        _MyRow.HeightInPoints = (short)height;
    }

    /// <summary>
    /// 根据Excel列类型获取列的值
    /// </summary>
    /// <param name="cell">Excel列</param>
    /// <returns></returns>
    public static string GetCellValue(ICell cell)
    {
        if (cell == null)
            return string.Empty;
        switch (cell.CellType)
        {
            case CellType.Blank:
                return string.Empty;
            case CellType.Boolean:
                return cell.BooleanCellValue.ToString();
            case CellType.Error:
                return cell.ErrorCellValue.ToString();
            case CellType.Numeric:
            case CellType.Unknown:
            default:
                return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
            case CellType.String:
                return cell.StringCellValue;
            case CellType.Formula:
                try
                {
                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                    e.EvaluateInCell(cell);
                    return cell.ToString();
                }
                catch
                {
                    return cell.NumericCellValue.ToString();
                }
        }
    }

    /// <summary>
    /// 自动设置Excel列宽
    /// </summary>
    /// <param name="sheet">Excel表</param>
    private static void AutoSizeColumns(ISheet sheet)
    {
        if (sheet.PhysicalNumberOfRows > 0)
        {
            IRow headerRow = sheet.GetRow(0);

            for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }
    }
    /// <summary>
    /// 设置边框类型
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="borderStyle"></param>
    private static void SetCellBorder(ICell cell, BorderStyle borderStyle)
    {
        cell.CellStyle.BorderTop = borderStyle;
        cell.CellStyle.BorderLeft = borderStyle;
        cell.CellStyle.BorderRight = borderStyle;
        cell.CellStyle.BorderBottom = borderStyle;
    }

    public void SetCellStyle(int row, int col, short fillBackgroundColor, HorizontalAlignment ha, bool border)
    {

        _MyCellStyle = _MyWorkbook.CreateCellStyle();
        if (fillBackgroundColor != -1)
        {
            _MyCellStyle.FillForegroundColor = fillBackgroundColor;
            _MyCellStyle.FillPattern = FillPattern.SolidForeground;
        }
        if (border)
        {
            _MyCellStyle.BorderLeft = BorderStyle.Thin;
            _MyCellStyle.BorderRight = BorderStyle.Thin;
            _MyCellStyle.BorderTop = BorderStyle.Thin;
            _MyCellStyle.BorderBottom = BorderStyle.Thin;
        }
        _MyCellStyle.Alignment = ha;
        GetCell(row, col);
        _MyCell.CellStyle = _MyCellStyle;
    }
    public void SetOrientation(int row, int col, short angle)
    {

        _MyCellStyle = _MyWorkbook.CreateCellStyle();
        _MyCellStyle.Rotation = angle;

        GetCell(row, col);
        _MyCell.CellStyle = _MyCellStyle;
    }
    public void AutoFit(int col)
    {
        _MyWorkSheet.AutoSizeColumn(col);
    }

    public void AutoFitAll()
    {
        if (_MyWorkSheet.PhysicalNumberOfRows > 0)
        {
            IRow headerRow = _MyWorkSheet.GetRow(0);

            for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
            {
                _MyWorkSheet.AutoSizeColumn(i);
            }
        }
    }

    public void AddPicture(string picPath, int row, int col)
    {
        //add picture data to this workbook.
        byte[] bytes = System.IO.File.ReadAllBytes(picPath);
        int pictureIdx = _MyWorkbook.AddPicture(bytes, PictureType.PNG);

        // Create the drawing patriarch.  This is the top level container for all shapes. 
        IDrawing patriarch = _MyWorkSheet.CreateDrawingPatriarch();

        //add a picture
        HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 255, 255, col, row, col + 2, row + 2);
        IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);
        pict.Resize();
    }

    public void AddPicture_Ex(string picPath, int row, int col)
    {
        //add picture data to this workbook.
        byte[] bytes = System.IO.File.ReadAllBytes(picPath);
        int pictureIdx = _MyWorkbook.AddPicture(bytes, PictureType.PNG);

        // Create the drawing patriarch.  This is the top level container for all shapes. 
        IDrawing patriarch = _MyWorkSheet.CreateDrawingPatriarch();

        //add a picture
        int x1 = 0;
        int y1 = 0;
        int x2 = 10;
        int y2 = 10;
        HSSFClientAnchor anchor = new HSSFClientAnchor(x1, y1, x2, y2, col, row, col + 1, row + 1);
        IPicture pict = patriarch.CreatePicture(anchor, pictureIdx);

        //pict.Resize();
    }
    /// <summary>
    /// 设置字体
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="size"></param>
    public void SetCellFont(int row, int col, int size = -1, bool bold = false, short color = -1)
    {
        GetCell(row, col);

        HSSFFont font1 = (HSSFFont)_MyWorkbook.CreateFont();
        //字體顏色
        if (color != -1)
            font1.Color = color;
        //字體粗體
        if (bold)
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
        //字體尺寸
        font1.FontHeightInPoints = (short)size;

        _MyCell.CellStyle.SetFont(font1);
    }
    /// <summary>
    /// 获取单元格样式
    /// </summary>
    /// <param name="hssfworkbook">Excel操作类</param>
    /// <param name="font">单元格字体</param>
    /// <param name="fillForegroundColor">图案的颜色</param>
    /// <param name="fillPattern">图案样式</param>
    /// <param name="fillBackgroundColor">单元格背景</param>
    /// <param name="ha">垂直对齐方式</param>
    /// <param name="va">垂直对齐方式</param>
    /// <returns></returns>
    public static ICellStyle GetCellStyle(HSSFWorkbook hssfworkbook, IFont font, HSSFColor fillForegroundColor, FillPattern fillPattern, HSSFColor fillBackgroundColor, HorizontalAlignment ha, VerticalAlignment va)
    {
        ICellStyle cellstyle = hssfworkbook.CreateCellStyle();
        cellstyle.FillPattern = fillPattern;
        cellstyle.Alignment = ha;
        cellstyle.VerticalAlignment = va;
        if (fillForegroundColor != null)
        {
            cellstyle.FillForegroundColor = fillForegroundColor.Indexed;
        }
        if (fillBackgroundColor != null)
        {
            cellstyle.FillBackgroundColor = fillBackgroundColor.Indexed;
        }
        if (font != null)
        {
            cellstyle.SetFont(font);
        }
        //有边框
        cellstyle.BorderBottom = BorderStyle.Thin;
        cellstyle.BorderLeft = BorderStyle.Thin;
        cellstyle.BorderRight = BorderStyle.Thin;
        cellstyle.BorderTop = BorderStyle.Thin;
        return cellstyle;
    }
    /// <summary>
    /// 合并单元格
    /// </summary>
    /// <param name="sheet">要合并单元格所在的sheet</param>
    /// <param name="rowstart">开始行的索引</param>
    /// <param name="rowend">结束行的索引</param>
    /// <param name="colstart">开始列的索引</param>
    /// <param name="colend">结束列的索引</param>
    public void MergeCells(int rowstart, int rowend, int colstart, int colend)
    {
        CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
        _MyWorkSheet.AddMergedRegion(cellRangeAddress);
    }

    /// <summary>
    /// 保存Excel文档流到文件
    /// </summary>
    /// <param name="ms">Excel文档流</param>
    /// <param name="fileName">文件名</param>
    public static void SaveToFile(MemoryStream ms, string fileName)
    {
        using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
        {
            byte[] data = ms.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();

            data = null;
        }
    }

    /// <summary>
    /// 输出文件到浏览器
    /// </summary>
    /// <param name="ms">Excel文档流</param>
    /// <param name="context">HTTP上下文</param>
    /// <param name="fileName">文件名</param>
    public static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
    {
        if (context.Request.Browser.Browser == "IE")
            fileName = HttpUtility.UrlEncode(fileName);
        context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
        context.Response.BinaryWrite(ms.ToArray());
        context.Response.End();
    }

    /// <summary>
    /// DataReader转换成Excel文档流
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
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

            AutoSizeColumns(sheet);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
        }
        return ms;
    }

    /// <summary>
    /// DataReader转换成Excel文档流，并保存到文件
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="fileName">保存的路径</param>
    public static void RenderToExcel(IDataReader reader, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(reader))
        {
            SaveToFile(ms, fileName);
        }
    }

    /// <summary>
    /// DataReader转换成Excel文档流，并输出到客户端
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="context">HTTP上下文</param>
    /// <param name="fileName">输出的文件名</param>
    public static void RenderToExcel(IDataReader reader, HttpContext context, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(reader))
        {
            RenderToBrowser(ms, context, fileName);
        }
    }
    public static MemoryStream RenderToExcel_Format(DataTable table, string title)
    {
        MemoryStream ms = new MemoryStream();

        using (table)
        {
            IWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet();

            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in table.Columns)
            {
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
            }


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
            AutoSizeColumns(sheet);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;


        }
        return ms;
    }
    /// <summary>
    /// DataTable转换成Excel文档流
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public static MemoryStream RenderToExcel(DataTable table)
    {
        MemoryStream ms = new MemoryStream();

        using (table)
        {
            IWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("data element");
            //sheet.SheetName = "data element";
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in table.Columns)
            {
                ICell cell = headerRow.CreateCell(column.Ordinal);
                cell.SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value

                //cell.CellStyle.BorderTop = BorderStyle.Thin;
                //cell.CellStyle.BorderLeft = BorderStyle.Thin;
                //cell.CellStyle.BorderRight = BorderStyle.Thin;
                //cell.CellStyle.BorderBottom = BorderStyle.Thin;

                NPOI.SS.UserModel.ICellStyle styleRed = workbook.CreateCellStyle();
                styleRed.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.BlueGrey.Index;
                styleRed.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                styleRed.BorderTop = BorderStyle.Thin;
                styleRed.BorderLeft = BorderStyle.Thin;
                styleRed.BorderRight = BorderStyle.Thin;
                styleRed.BorderBottom = BorderStyle.Thin;
                cell.CellStyle = styleRed;

            }


            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in table.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in table.Columns)
                {
                    if (column.DataType == DbType.DateTime.GetType())
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(DateTime.Parse(row[column].ToString()).ToString("yyyy/MM/dd HH:mm"));
                    }
                    else
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                }

                rowIndex++;
            }
            AutoSizeColumns(sheet);

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;


        }
        return ms;
    }

    /// <summary>
    /// DataTable转换成Excel文档流，并保存到文件
    /// </summary>
    /// <param name="table"></param>
    /// <param name="fileName">保存的路径</param>
    public static void RenderToExcel(DataTable table, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(table))
        {
            SaveToFile(ms, fileName);
        }
    }

    /// <summary>
    /// DataTable转换成Excel文档流，并输出到客户端
    /// </summary>
    /// <param name="table"></param>
    /// <param name="response"></param>
    /// <param name="fileName">输出的文件名</param>
    public static void RenderToExcel(DataTable table, HttpContext context, string fileName)
    {
        using (MemoryStream ms = RenderToExcel(table))
        {
            RenderToBrowser(ms, context, fileName);
        }
    }

    /// <summary>
    /// Excel文档流是否有数据
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <returns></returns>
    public static bool HasData(Stream excelFileStream)
    {
        return HasData(excelFileStream, 0);
    }

    /// <summary>
    /// Excel文档流是否有数据
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <returns></returns>
    public static bool HasData(Stream excelFileStream, int sheetIndex)
    {
        using (excelFileStream)
        {
            IWorkbook workbook = new HSSFWorkbook(excelFileStream);

            if (workbook.NumberOfSheets > 0)
            {
                if (sheetIndex < workbook.NumberOfSheets)
                {
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);

                    return sheet.PhysicalNumberOfRows > 0;

                }
            }

        }
        return false;
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetName">表名称</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, string sheetName)
    {
        return RenderFromExcel(excelFileStream, sheetName, 0);
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetName">表名称</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex)
    {
        DataTable table = null;

        using (excelFileStream)
        {
            IWorkbook workbook = new HSSFWorkbook(excelFileStream);

            ISheet sheet = workbook.GetSheet(sheetName);
            {
                table = RenderFromExcel(sheet, headerRowIndex);
            }

        }
        return table;
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// 默认转换Excel的第一个表
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream)
    {
        return RenderFromExcel(excelFileStream, 0, 0);
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, int sheetIndex)
    {
        return RenderFromExcel(excelFileStream, sheetIndex, 0);
    }

    /// <summary>
    /// Excel文档流转换成DataTable
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    public static DataTable RenderFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex)
    {
        DataTable table = null;

        using (excelFileStream)
        {
            IWorkbook workbook = new HSSFWorkbook(excelFileStream);

            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            table = RenderFromExcel(sheet, headerRowIndex);


        }
        return table;
    }

    /// <summary>
    /// Excel表格转换成DataTable
    /// </summary>
    /// <param name="sheet">表格</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    private static DataTable RenderFromExcel(ISheet sheet, int headerRowIndex)
    {
        DataTable table = new DataTable();

        IRow headerRow = sheet.GetRow(headerRowIndex);
        int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
        int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

        //handling header.
        for (int i = headerRow.FirstCellNum; i < cellCount; i++)
        {
            DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
            table.Columns.Add(column);
        }

        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
        {
            IRow row = sheet.GetRow(i);
            DataRow dataRow = table.NewRow();

            if (row != null)
            {
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = GetCellValue(row.GetCell(j));
                }
            }

            table.Rows.Add(dataRow);
        }

        return table;
    }

    /// <summary>
    /// Excel文档导入到数据库
    /// 默认取Excel的第一个表
    /// 第一行必须为标题行
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="insertSql">插入语句</param>
    /// <param name="dbAction">更新到数据库的方法</param>
    /// <returns></returns>
    public static int RenderToDb(Stream excelFileStream, string insertSql, DBAction dbAction)
    {
        return RenderToDb(excelFileStream, insertSql, dbAction, 0, 0);
    }

    public delegate int DBAction(string sql, params IDataParameter[] parameters);

    /// <summary>
    /// Excel文档导入到数据库
    /// </summary>
    /// <param name="excelFileStream">Excel文档流</param>
    /// <param name="insertSql">插入语句</param>
    /// <param name="dbAction">更新到数据库的方法</param>
    /// <param name="sheetIndex">表索引号，如第一个表为0</param>
    /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
    /// <returns></returns>
    public static int RenderToDb(Stream excelFileStream, string insertSql, DBAction dbAction, int sheetIndex, int headerRowIndex)
    {
        int rowAffected = 0;
        using (excelFileStream)
        {
            IWorkbook workbook = new HSSFWorkbook(excelFileStream);

            ISheet sheet = workbook.GetSheetAt(sheetIndex);

            StringBuilder builder = new StringBuilder();

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    builder.Append(insertSql);
                    builder.Append(" values (");
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        builder.AppendFormat("'{0}',", GetCellValue(row.GetCell(j)).Replace("'", "''"));
                    }
                    builder.Length = builder.Length - 1;
                    builder.Append(");");
                }

                if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                {
                    //每50条记录一次批量插入到数据库
                    rowAffected += dbAction(builder.ToString());
                    builder.Length = 0;
                }

            }
        }
        return rowAffected;
    }
}