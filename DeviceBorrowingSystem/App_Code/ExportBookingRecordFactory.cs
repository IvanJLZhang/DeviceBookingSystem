using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BLL;
using SystemDAO;
using System.Text;
using System.Data.SqlClient;
using Model;
using System.IO;
using System.Collections;
using NPOI.SS.UserModel;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using System.Xml;
using NPOI.HSSF.Util;
/// <summary>
/// ExportBookingRecordFactory 的摘要说明
/// </summary>
public class ExportBookingRecordFactory
{
    public ExportBookingRecordFactory()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public ExportBookingRecordFactory(string cateory)
    {
        this._Category = cateory;
    }
    string _Category = "1";

    public DataTableCollection GetLocationList()
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(@"select distinct Owner.P_Location as site from tbl_DeviceBooking 
inner join tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
inner join tbl_Person as Owner on summary.s_ownerid = Owner.P_ID where summary.s_category = @Category;");
        try
        {
            var result = SqlHelper.GetTableText(sql.ToString(), new SqlParameter[] { new SqlParameter("@Category", this._Category) });
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public DataTableCollection GetDepartmentList()
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(@"select distinct Owner.P_Department as department from tbl_DeviceBooking 
inner join tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
inner join tbl_Person as Owner on summary.s_ownerid = Owner.P_ID where summary.s_category = @Category;");
        try
        {
            var result = SqlHelper.GetTableText(sql.ToString(), new SqlParameter[] { new SqlParameter("@Category", this._Category) });
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //    public DataTableCollection GetChamberList()
    //    {
    //        StringBuilder sql = new StringBuilder();
    //        sql.Append(@"select distinct tbl_Device.Lab_Location as chamber from tbl_DeviceBooking 
    //inner join tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
    //where tbl_Device.Device_category = @Category and tbl_Device.Lab_Location is not null;");
    //        try
    //        {
    //            var result = SqlHelper.GetTableText(sql.ToString(), new SqlParameter[] { new SqlParameter("@Category", this._Category) });
    //            return result;
    //        }
    //        catch (Exception ex)
    //        {
    //            return null;
    //        }
    //    }

    public DataTableCollection GetProjectList()
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(@"select distinct tbl_Project.PJ_Code as pid, tbl_Project.PJ_Name as pname from tbl_DeviceBooking 
inner join tbl_Project on tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code 
inner join tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
where summary.s_category=@Category;");
        try
        {
            var result = SqlHelper.GetTableText(sql.ToString(), new SqlParameter[] { new SqlParameter("@Category", this._Category) });
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public DataTableCollection GetTestCategoryList()
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(@"select distinct tbl_TestCategory.ID as tcid, tbl_TestCategory.Name as tcname from tbl_DeviceBooking 
inner join tbl_TestCategory on tbl_DeviceBooking.TestCategory_ID = tbl_TestCategory.ID 
inner join tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
where summary.s_category=@Category;");
        try
        {
            var result = SqlHelper.GetTableText(sql.ToString(), new SqlParameter[] { new SqlParameter("@Category", this._Category) });
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public DataTableCollection GetYearList(int category)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append(@"select distinct YEAR(tbl_DeviceBooking.Loan_DateTime) as year from tbl_DeviceBooking 
inner join tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
where summary.s_category=@Category 
order by year;");

        try
        {
            var result = SystemDAO.SqlHelperEx.GetTableText(sql.ToString(), new SqlParameter[] { new SqlParameter("@Category", category) });
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
    }



    public ExcelRenderNode GetYearlyReport(int year)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        ExcelRender xlsApp = new ExcelRender();
        xlsApp.NewExcel();
        xlsApp.CreateSheet("Cellular Breakdown");

        int nStartRow = 0;
        int nStartCol = 0;
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        List<ProjectColorNode> projectColorList = new List<ProjectColorNode>();
        List<YearReportSheet> YearReportList = new List<YearReportSheet>();
        var results = deviceBookingManage.GetBookingRecod(year);
        if (results == null || results.Count <= 0)
        {
            result.ms = null;
            result.errMsg = deviceBookingManage.errMsg;
            return result;
        }

        YearReportList = ModifyData(results[0], year);

        {// 设置Project图例

            DataTableCollection projectLists = deviceBookingManage.GetProjectList(year);
            if (projectLists == null || projectLists.Count <= 0)
            {
                result.ms = null;
                result.errMsg = deviceBookingManage.errMsg;
                return result;
            }

            int StartColorIndex = 3;
            for (int index = 0; index != projectLists[0].Rows.Count; index++)
            {
                ProjectColorNode projectColor = new ProjectColorNode();
                projectColor.projectName = projectLists[0].Rows[index]["PJ_Name"].ToString().Trim();
                projectColor.ColorIndex = StartColorIndex++;
                projectColorList.Add(projectColor);
            }

            for (int index = 0; index != projectColorList.Count; index++)
            {
                xlsApp.SetCellValue(nStartRow, nStartCol, projectColorList[index].projectName, TypeCode.String);
                xlsApp.SetCellStyle(nStartRow, nStartCol, (short)projectColorList[index].ColorIndex, NPOI.SS.UserModel.HorizontalAlignment.Left, true);
                nStartRow++;
            }
            xlsApp.SetCellWidth(nStartCol, 25);
        }



        nStartCol++; nStartCol++;
        nStartRow++;
        DateTime theYear = new DateTime(year, 1, 1);
        int weekCnt = 1;
        if (theYear.DayOfWeek == DayOfWeek.Monday)
        {
            weekCnt = 1;
        }
        else
        {
            weekCnt = 2;
        }
        for (DateTime monthIndex = theYear; monthIndex < theYear.AddMonths(12); monthIndex = monthIndex.AddMonths(1))
        {
            int cnt = 0;
            DateTime weekIndex = new DateTime(monthIndex.Year, monthIndex.Month, monthIndex.Day);
            for (; weekIndex <= monthIndex.AddMonths(1); weekIndex = weekIndex.AddDays(7))
            {
                DateTime dayIndex = new DateTime(weekIndex.Year, weekIndex.Month, weekIndex.Day);
                for (; dayIndex <= weekIndex.AddDays(7); dayIndex = dayIndex.AddDays(1))
                {
                    if (dayIndex.DayOfWeek == DayOfWeek.Monday && dayIndex.Year == theYear.Year && dayIndex.Month == monthIndex.Month)
                    {
                        xlsApp.SetCellValue(nStartRow, nStartCol, "W" + weekCnt, TypeCode.String);
                        //xlsApp.SetCellFontSize(nStartRow, nStartCol, 8);
                        //xlsApp.HorAligment(nStartRow, nStartCol, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
                        xlsApp.SetCellValue(nStartRow + 1, nStartCol, dayIndex.ToString("M/d"), TypeCode.String);
                        xlsApp.SetOrientation(nStartRow + 1, nStartCol, 90);
                        xlsApp.SetCellStyle(nStartRow + 1, nStartCol, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
                        //xlsApp.SetCellbackgroundStyle(nStartRow + 1, nStartCol, ColorIndex.浅黄);
                        //xlsApp.SetCellFontSize(nStartRow + 1, nStartCol, 12);
                        xlsApp.SetCellWidth(nStartCol, 5);
                        weekCnt++; nStartCol++;
                        cnt++;
                        break;
                    }
                }
            }
            string s1 = MyExcel.GetLetter(nStartCol - cnt) + (nStartRow - 1);
            string s2 = MyExcel.GetLetter(nStartCol - 1) + (nStartRow - 1);
            xlsApp.MergeCells(nStartRow - 1, nStartRow - 1, nStartCol - cnt, nStartCol - 1);
            xlsApp.SetCellValue(nStartRow - 1, nStartCol - cnt, monthIndex.ToString("M-yyyy"), TypeCode.String);
            xlsApp.SetCellStyle(nStartRow - 1, nStartCol - cnt, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, false);
            //xlsApp.HorAligment(nStartRow - 1, nStartCol - cnt, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
            //xlsApp.SetCellFontSize(nStartRow - 1, nStartCol - cnt, 12);
        }

        {
            int DateRow = nStartRow;
            nStartRow++;
            nStartRow++;
            nStartCol = 1;
            xlsApp.SetCellWidth(nStartCol, 55);
            var locationLists = deviceBookingManage.GetLocationList(year);
            if (locationLists == null || locationLists.Count <= 0)
            {
                result.ms = null;
                result.errMsg = deviceBookingManage.errMsg;
                return result;
            }
            for (int indey = 0; indey != locationLists[0].Rows.Count; indey++)
            {
                string locationStr = locationLists[0].Rows[indey]["Location"].ToString();
                int lCnt = 0;
                int TempRow = nStartRow;
                for (int index = 0; index != YearReportList.Count; index++)
                {
                    if (YearReportList[index].location.CompareTo(locationStr) == 0)
                    {
                        xlsApp.SetCellValue(nStartRow, nStartCol, YearReportList[index].DevicenName, TypeCode.String);

                        //xlsApp.HorAligment(nStartRow, nStartCol, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                        //xlsApp.WrapText(nStartRow, nStartCol);
                        {// 填入数据
                            var DeviceDateList = YearReportList[index].dateTimeList;
                            for (int indez = 0; indez != DeviceDateList.Count; indez++)
                            {
                                var oneDateTime = DeviceDateList[indez];
                                int dateRow = DateRow;
                                int dateCol = 3;
                                int sCol = 3;
                                int eCol = 3;
                                string dateStr = xlsApp.GetCellValue(dateRow, dateCol);
                                while (dateStr.CompareTo(String.Empty) != 0)
                                {
                                    if (("W" + GetWeekOfYear(oneDateTime.startDateTime)).CompareTo(dateStr) == 0)
                                    {
                                        sCol = dateCol;
                                    }
                                    if (("W" + GetWeekOfYear(oneDateTime.endDateTime)).CompareTo(dateStr) == 0)
                                    {
                                        eCol = dateCol;
                                    }
                                    //if (oneDateTime.endDateTime.Year > oneDateTime.startDateTime.Year)
                                    //{
                                    //    eCol = sCol;
                                    //}
                                    dateCol++;
                                    dateStr = xlsApp.GetCellValue(dateRow, dateCol);
                                }
                                if (eCol < sCol)
                                    eCol = sCol;
                                //xlsApp.MergeCells(MyExcel.GetLetter(sCol) + nStartRow, MyExcel.GetLetter(eCol) + nStartRow);
                                xlsApp.MergeCells(nStartRow, nStartRow, sCol, eCol);
                                xlsApp.SetCellValue(nStartRow, sCol, oneDateTime.purpose, TypeCode.String);
                                int colorIndex = GetColorIndex(projectColorList, oneDateTime.projectName);
                                xlsApp.SetCellStyle(nStartRow, sCol, (short)colorIndex, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
                                //xlsApp.SetCellbackgroundStyle(nStartRow, sCol, colorIndex);
                                //xlsApp.SetBordersLineStyle(nStartRow, sCol, LineStyle.连续直线);
                            }
                        }

                        nStartRow++; lCnt++;
                    }
                }
                //xlsApp.MergeCells("A" + (TempRow), "A" + (TempRow + lCnt - 1));
                xlsApp.MergeCells(TempRow, TempRow + lCnt - 1, 0, 0);
                xlsApp.SetCellValue(TempRow, 0, locationStr, TypeCode.String);
                xlsApp.SetCellStyle(TempRow, 0, NPOI.HSSF.Util.HSSFColor.COLOR_NORMAL, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
                //xlsApp.HorAligment(TempRow, 1, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
            }

        }
        xlsApp._MyWorkbook.Write(result.ms);
        return result;

    }

    public ExcelRenderNode GetYearlyReport(RecordFilter filter)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        ExcelRender xlsApp = new ExcelRender();
        xlsApp.NewExcel();
        xlsApp.CreateSheet("Cellular Breakdown");

        int nStartRow = 0;
        int nStartCol = 0;
        List<ProjectColorNode> projectColorList = new List<ProjectColorNode>();
        List<YearReportSheet> YearReportList = new List<YearReportSheet>();
        var results = RecordManagment.GetRecords(filter);
        if (results == null || results.Rows.Count <= 0)
        {
            result.ms = null;
            result.errMsg = "No record data";
            return result;
        }
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        YearReportList = ModifyData(results, filter.df_year);

        {// 设置Project图例

            DataTableCollection projectLists = deviceBookingManage.GetProjectList(filter.df_year);
            if (projectLists == null || projectLists.Count <= 0)
            {
                result.ms = null;
                result.errMsg = deviceBookingManage.errMsg;
                return result;
            }

            int StartColorIndex = 3;
            for (int index = 0; index != projectLists[0].Rows.Count; index++)
            {
                ProjectColorNode projectColor = new ProjectColorNode();
                projectColor.projectName = projectLists[0].Rows[index]["PJ_Name"].ToString().Trim();
                projectColor.ColorIndex = StartColorIndex++;
                projectColorList.Add(projectColor);
            }

            for (int index = 0; index != projectColorList.Count; index++)
            {
                xlsApp.SetCellValue(nStartRow, nStartCol, projectColorList[index].projectName, TypeCode.String);
                xlsApp.SetCellStyle(nStartRow, nStartCol, (short)projectColorList[index].ColorIndex, NPOI.SS.UserModel.HorizontalAlignment.Left, true);
                nStartRow++;
            }
            xlsApp.SetCellWidth(nStartCol, 25);
        }



        nStartCol++; nStartCol++;
        nStartRow++;
        DateTime theYear = new DateTime(filter.df_year, 1, 1);
        int weekCnt = 1;
        if (theYear.DayOfWeek == DayOfWeek.Monday)
        {
            weekCnt = 1;
        }
        else
        {
            weekCnt = 2;
        }
        for (DateTime monthIndex = theYear; monthIndex < theYear.AddMonths(12); monthIndex = monthIndex.AddMonths(1))
        {
            int cnt = 0;
            DateTime weekIndex = new DateTime(monthIndex.Year, monthIndex.Month, monthIndex.Day);
            for (; weekIndex <= monthIndex.AddMonths(1); weekIndex = weekIndex.AddDays(7))
            {
                DateTime dayIndex = new DateTime(weekIndex.Year, weekIndex.Month, weekIndex.Day);
                for (; dayIndex <= weekIndex.AddDays(7); dayIndex = dayIndex.AddDays(1))
                {
                    if (dayIndex.DayOfWeek == DayOfWeek.Monday && dayIndex.Year == theYear.Year && dayIndex.Month == monthIndex.Month)
                    {
                        xlsApp.SetCellValue(nStartRow, nStartCol, "W" + weekCnt, TypeCode.String);
                        //xlsApp.SetCellFontSize(nStartRow, nStartCol, 8);
                        //xlsApp.HorAligment(nStartRow, nStartCol, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
                        xlsApp.SetCellValue(nStartRow + 1, nStartCol, dayIndex.ToString("M/d"), TypeCode.String);
                        xlsApp.SetOrientation(nStartRow + 1, nStartCol, 90);
                        xlsApp.SetCellStyle(nStartRow + 1, nStartCol, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
                        //xlsApp.SetCellbackgroundStyle(nStartRow + 1, nStartCol, ColorIndex.浅黄);
                        //xlsApp.SetCellFontSize(nStartRow + 1, nStartCol, 12);
                        xlsApp.SetCellWidth(nStartCol, 5);
                        weekCnt++; nStartCol++;
                        cnt++;
                        break;
                    }
                }
            }
            string s1 = MyExcel.GetLetter(nStartCol - cnt) + (nStartRow - 1);
            string s2 = MyExcel.GetLetter(nStartCol - 1) + (nStartRow - 1);
            xlsApp.MergeCells(nStartRow - 1, nStartRow - 1, nStartCol - cnt, nStartCol - 1);
            xlsApp.SetCellValue(nStartRow - 1, nStartCol - cnt, monthIndex.ToString("M-yyyy"), TypeCode.String);
            xlsApp.SetCellStyle(nStartRow - 1, nStartCol - cnt, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, false);
            //xlsApp.HorAligment(nStartRow - 1, nStartCol - cnt, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
            //xlsApp.SetCellFontSize(nStartRow - 1, nStartCol - cnt, 12);
        }

        {
            int DateRow = nStartRow;
            nStartRow++;
            nStartRow++;
            nStartCol = 1;
            xlsApp.SetCellWidth(nStartCol, 55);
            var locationLists = deviceBookingManage.GetLocationList(filter.df_year);
            if (locationLists == null || locationLists.Count <= 0)
            {
                result.ms = null;
                result.errMsg = deviceBookingManage.errMsg;
                return result;
            }
            for (int indey = 0; indey != locationLists[0].Rows.Count; indey++)
            {
                string locationStr = locationLists[0].Rows[indey]["Location"].ToString();
                int lCnt = 0;
                int TempRow = nStartRow;
                for (int index = 0; index != YearReportList.Count; index++)
                {
                    if (YearReportList[index].location.CompareTo(locationStr) == 0)
                    {
                        xlsApp.SetCellValue(nStartRow, nStartCol, YearReportList[index].DevicenName, TypeCode.String);

                        //xlsApp.HorAligment(nStartRow, nStartCol, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft);
                        //xlsApp.WrapText(nStartRow, nStartCol);
                        {// 填入数据
                            var DeviceDateList = YearReportList[index].dateTimeList;
                            for (int indez = 0; indez != DeviceDateList.Count; indez++)
                            {
                                var oneDateTime = DeviceDateList[indez];
                                int dateRow = DateRow;
                                int dateCol = 3;
                                int sCol = 3;
                                int eCol = 3;
                                string dateStr = xlsApp.GetCellValue(dateRow, dateCol);
                                while (dateStr.CompareTo(String.Empty) != 0)
                                {
                                    if (("W" + GetWeekOfYear(oneDateTime.startDateTime)).CompareTo(dateStr) == 0)
                                    {
                                        sCol = dateCol;
                                    }
                                    if (("W" + GetWeekOfYear(oneDateTime.endDateTime)).CompareTo(dateStr) == 0)
                                    {
                                        eCol = dateCol;
                                    }
                                    //if (oneDateTime.endDateTime.Year > oneDateTime.startDateTime.Year)
                                    //{
                                    //    eCol = sCol;
                                    //}
                                    dateCol++;
                                    dateStr = xlsApp.GetCellValue(dateRow, dateCol);
                                }
                                if (eCol < sCol)
                                    eCol = sCol;
                                //xlsApp.MergeCells(MyExcel.GetLetter(sCol) + nStartRow, MyExcel.GetLetter(eCol) + nStartRow);
                                xlsApp.MergeCells(nStartRow, nStartRow, sCol, eCol);
                                xlsApp.SetCellValue(nStartRow, sCol, oneDateTime.purpose, TypeCode.String);
                                int colorIndex = GetColorIndex(projectColorList, oneDateTime.projectName);
                                xlsApp.SetCellStyle(nStartRow, sCol, (short)colorIndex, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
                                //xlsApp.SetCellbackgroundStyle(nStartRow, sCol, colorIndex);
                                //xlsApp.SetBordersLineStyle(nStartRow, sCol, LineStyle.连续直线);
                            }
                        }

                        nStartRow++; lCnt++;
                    }
                }
                //xlsApp.MergeCells("A" + (TempRow), "A" + (TempRow + lCnt - 1));
                xlsApp.MergeCells(TempRow, TempRow + lCnt - 1, 0, 0);
                xlsApp.SetCellValue(TempRow, 0, locationStr, TypeCode.String);
                xlsApp.SetCellStyle(TempRow, 0, NPOI.HSSF.Util.HSSFColor.COLOR_NORMAL, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
                //xlsApp.HorAligment(TempRow, 1, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
            }

        }
        xlsApp._MyWorkbook.Write(result.ms);
        return result;

    }

    public ExcelRenderNode GetGlobleReport(RecordFilter filter)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        ExcelRender xlsApp = new ExcelRender();
        xlsApp.NewExcel();
        xlsApp.CreateSheet("Cellular Breakdown");

        int nStartRow = 0;
        int nStartCol = 0;
        var results = RecordManagment.GetRecords(filter);
        Dictionary<string, int> siteColorList = new Dictionary<string, int>();
        #region 设置site图例
        {
            //DataTable siteList = settingsHandler.GetSiteList();

            int StartColorIndex = 3;
            siteColorList.Add("Plan to be Farmed outside of WCH", StartColorIndex++);
            siteColorList.Add("WKS", StartColorIndex++);
            siteColorList.Add("WCH", StartColorIndex++);
            siteColorList.Add("WHC", StartColorIndex++);
            siteColorList.Add("ExternalLab", StartColorIndex++);
            siteColorList.Add("Troubleshooting & Fix testing", StartColorIndex++);

            foreach (var site in siteColorList)
            {
                xlsApp.SetCellValue(nStartRow, nStartCol + 1, site.Key, TypeCode.String);
                xlsApp.SetCellStyle(nStartRow, nStartCol, (short)site.Value, NPOI.SS.UserModel.HorizontalAlignment.Left, true);
                nStartRow++;
            }
        }
        #endregion

        #region 设置日历
        nStartCol++; nStartCol++;
        nStartRow--; nStartRow--;
        DateTime theYear = new DateTime(filter.df_year, 1, 1);
        int weekCnt = 1;
        int startWeekNo = 1;
        if (theYear.DayOfWeek == DayOfWeek.Monday)
        {
            weekCnt = 1;
        }
        else
        {
            weekCnt = 2;
            startWeekNo = 2;
        }
        for (DateTime monthIndex = theYear; monthIndex < theYear.AddMonths(12); monthIndex = monthIndex.AddMonths(1))
        {
            int cnt = 0;
            DateTime weekIndex = new DateTime(monthIndex.Year, monthIndex.Month, monthIndex.Day);
            for (; weekIndex <= monthIndex.AddMonths(1); weekIndex = weekIndex.AddDays(7))
            {
                DateTime dayIndex = new DateTime(weekIndex.Year, weekIndex.Month, weekIndex.Day);
                for (; dayIndex <= weekIndex.AddDays(7); dayIndex = dayIndex.AddDays(1))
                {
                    if (dayIndex.DayOfWeek == DayOfWeek.Monday && dayIndex.Year == theYear.Year && dayIndex.Month == monthIndex.Month)
                    {
                        xlsApp.SetCellValue(nStartRow, nStartCol, "W" + weekCnt, TypeCode.String);
                        //xlsApp.SetCellFontSize(nStartRow, nStartCol, 8);
                        //xlsApp.HorAligment(nStartRow, nStartCol, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
                        xlsApp.SetCellValue(nStartRow + 1, nStartCol, dayIndex.ToString("M/d"), TypeCode.String);
                        xlsApp.SetOrientation(nStartRow + 1, nStartCol, 90);
                        xlsApp.SetCellStyle(nStartRow + 1, nStartCol, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
                        //xlsApp.SetCellbackgroundStyle(nStartRow + 1, nStartCol, ColorIndex.浅黄);
                        //xlsApp.SetCellFontSize(nStartRow + 1, nStartCol, 12);
                        xlsApp.SetCellWidth(nStartCol, 5);
                        weekCnt++; nStartCol++;
                        cnt++;
                        break;
                    }
                }
            }
            string s1 = MyExcel.GetLetter(nStartCol - cnt) + (nStartRow - 1);
            string s2 = MyExcel.GetLetter(nStartCol - 1) + (nStartRow - 1);
            xlsApp.MergeCells(nStartRow - 1, nStartRow - 1, nStartCol - cnt, nStartCol - 1);
            xlsApp.SetCellValue(nStartRow - 1, nStartCol - cnt, monthIndex.ToString("M-yyyy"), TypeCode.String);
            xlsApp.SetCellStyle(nStartRow - 1, nStartCol - cnt, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, false);
            //xlsApp.HorAligment(nStartRow - 1, nStartCol - cnt, Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter);
            //xlsApp.SetCellFontSize(nStartRow - 1, nStartCol - cnt, 12);
        }
        #endregion

        #region 设置project
        DataTable projectList = settingsHandler.GetProjectTable();
        int startRow = nStartRow + 2;
        int endCol = nStartCol;
        nStartCol = 0;
        foreach (DataRow project in projectList.Rows)
        {
            xlsApp.SetCellValue(startRow, nStartCol, "Project");
            //xlsApp.SetCellFont(startRow, nStartCol, 20, true);
            xlsApp.SetCellValue(startRow, nStartCol + 1, project["PJ_Name"]);
            // xlsApp.SetCellFont(startRow, nStartCol, 20, true);
            for (int index = 0; index != endCol; index++)
            {
                xlsApp.SetCellStyle(startRow, index, NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index, HorizontalAlignment.Left, false);
            }
            xlsApp.SetRowHeight(startRow, 20);
            xlsApp.SetCellValue(startRow + 1, nStartCol + 1, "Development Phase");

            DataTable pj_stageList = settingsHandler.GetProjectStage(project["PJ_Code"].ToString());
            if (pj_stageList != null && pj_stageList.Rows.Count > 0)
            {
                foreach (DataRow pj_Stage in pj_stageList.Rows)
                {
                    DateTime start = Convert.ToDateTime(pj_Stage["ps_from"]);
                    DateTime to = Convert.ToDateTime(pj_Stage["ps_to"]);

                    if (start.Year == filter.df_year)
                    {
                        int w1 = GetWeekOfYear(start);
                        int w2 = GetWeekOfYear(to);
                        int colorIndex = 3;
                        bool Set = false;
                        for (int indez = w1; indez <= w2; indez++)
                        {
                            if (!Set)
                            {
                                xlsApp.SetCellValue(startRow + 1, indez - startWeekNo + 3, pj_Stage["ps_stage"].ToString());
                                Set = true;
                            }
                            xlsApp.SetCellStyle(startRow + 1, indez - startWeekNo + 3, (short)colorIndex, HorizontalAlignment.Center, false);
                        }
                        colorIndex++;
                    }
                }
            }
            startRow++;
            int sRow = startRow;
            startRow++;
            #region 填入数据
            #region WKS
            {
                var deviceIDList = GetDeviceIDList(project["PJ_Code"].ToString(), "WKS", results);
                if (deviceIDList != null && deviceIDList.Count > 0)
                {
                    foreach (var deviceid in deviceIDList)
                    {
                        // 填入device
                        foreach (DataRow record in results.Rows)
                        {
                            if (record["Device_ID"].ToString() == deviceid)
                            {
                                //xlsApp.SetCellValue(startRow++, 1, deviceid);
                                xlsApp.SetCellValue(startRow++, 1, record["Device_Name"].ToString());
                                break;
                            }
                        }
                        //xlsApp.SetCellFont(startRow - 1, 1, 15, true);
                        // 填入purpose
                        var purposeList = GetPurposeIDList(project["PJ_Code"].ToString(), "WKS", deviceid, results);
                        if (purposeList != null)
                        {
                            foreach (var purpose in purposeList)
                            {
                                foreach (DataRow record in results.Rows)
                                {
                                    if (record["TestCategory_ID"].ToString() == purpose)
                                    {
                                        //xlsApp.SetCellValue(startRow++, 1, deviceid);
                                        xlsApp.SetCellValue(startRow++, 1, "    " + record["TestCategory"].ToString());
                                        // 填入记录数据
                                        foreach (DataRow record1 in results.Rows)
                                        {
                                            if (record1["Project_ID"].ToString() == project["PJ_Code"].ToString()
                                                && record1["Site"].ToString() == "WKS"
                                                && record1["Device_ID"].ToString() == deviceid
                                                && record1["TestCategory_ID"].ToString() == purpose)
                                            {
                                                DateTime startTime = Convert.ToDateTime(record1["Loan_DateTime"]);
                                                DateTime endTime = Convert.ToDateTime(record1["Plan_To_ReDateTime"]);
                                                int w1 = GetWeekOfYear(startTime);
                                                int w2 = GetWeekOfYear(endTime);
                                                for (int indez = w1; indez <= w2; indez++)
                                                {
                                                    xlsApp.SetCellStyle(startRow, indez - startWeekNo + 2, (short)siteColorList["WKS"], HorizontalAlignment.Center, false);
                                                }
                                            }
                                        }
                                        startRow++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (startRow > sRow + 1)
                {
                    xlsApp.MergeCells(sRow, startRow - 1, 0, 0);
                    xlsApp.SetCellValue(sRow, 0, "WKS");
                }
            }
            #endregion
            #region WHC
            {
                sRow = startRow;
                var deviceIDList = GetDeviceIDList(project["PJ_Code"].ToString(), "WHC", results);
                if (deviceIDList != null && deviceIDList.Count > 0)
                {
                    foreach (var deviceid in deviceIDList)
                    {
                        // 填入device
                        foreach (DataRow record in results.Rows)
                        {
                            if (record["Device_ID"].ToString() == deviceid)
                            {
                                //xlsApp.SetCellValue(startRow++, 1, deviceid);
                                xlsApp.SetCellValue(startRow++, 1, record["Device_Name"].ToString());
                                break;
                            }
                        }

                        //xlsApp.SetCellFont(startRow - 1, 1, 15, true);
                        // 填入purpose
                        var purposeList = GetPurposeIDList(project["PJ_Code"].ToString(), "WHC", deviceid, results);
                        if (purposeList != null)
                        {
                            foreach (var purpose in purposeList)
                            {
                                foreach (DataRow record in results.Rows)
                                {
                                    if (record["TestCategory_ID"].ToString() == purpose)
                                    {
                                        //xlsApp.SetCellValue(startRow++, 1, deviceid);
                                        xlsApp.SetCellValue(startRow, 1, "    " + record["TestCategory"].ToString());
                                        // 填入记录数据
                                        foreach (DataRow record1 in results.Rows)
                                        {
                                            if (record1["Project_ID"].ToString() == project["PJ_Code"].ToString()
                                                && record1["Site"].ToString() == "WHC"
                                                && record1["Device_ID"].ToString() == deviceid
                                                && record1["TestCategory_ID"].ToString() == purpose)
                                            {
                                                DateTime startTime = Convert.ToDateTime(record1["Loan_DateTime"]);
                                                DateTime endTime = Convert.ToDateTime(record1["Plan_To_ReDateTime"]);
                                                int w1 = GetWeekOfYear(startTime);
                                                int w2 = GetWeekOfYear(endTime);
                                                for (int indez = w1; indez <= w2; indez++)
                                                {
                                                    xlsApp.SetCellStyle(startRow, indez - startWeekNo + 2, (short)siteColorList["WHC"], HorizontalAlignment.Center, false);
                                                }
                                            }
                                        }
                                        startRow++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                if (startRow > sRow)
                {
                    xlsApp.MergeCells(sRow, startRow - 1, 0, 0);
                    xlsApp.SetCellValue(sRow, 0, "WHC");
                }
            }
            #endregion
            #region WCH
            {
                sRow = startRow;
                var deviceIDList = GetDeviceIDList(project["PJ_Code"].ToString(), "WCH", results);
                if (deviceIDList != null && deviceIDList.Count > 0)
                {
                    foreach (var deviceid in deviceIDList)
                    {
                        // 填入device
                        foreach (DataRow record in results.Rows)
                        {
                            if (record["Device_ID"].ToString() == deviceid)
                            {
                                //xlsApp.SetCellValue(startRow++, 1, deviceid);
                                xlsApp.SetCellValue(startRow++, 1, record["Device_Name"].ToString());
                                break;
                            }
                        }
                        //xlsApp.SetCellFont(startRow - 1, 1, 15, true);
                        // 填入purpose
                        var purposeList = GetPurposeIDList(project["PJ_Code"].ToString(), "WCH", deviceid, results);
                        if (purposeList != null)
                        {
                            foreach (var purpose in purposeList)
                            {
                                foreach (DataRow record in results.Rows)
                                {
                                    if (record["TestCategory_ID"].ToString() == purpose)
                                    {
                                        xlsApp.SetCellValue(startRow, 1, "    " + record["TestCategory"].ToString());
                                        // 填入记录数据
                                        foreach (DataRow record1 in results.Rows)
                                        {
                                            if (record1["Project_ID"].ToString() == project["PJ_Code"].ToString()
                                                && record1["Site"].ToString() == "WCH"
                                                && record1["Device_ID"].ToString() == deviceid
                                                && record1["TestCategory_ID"].ToString() == purpose)
                                            {
                                                DateTime startTime = Convert.ToDateTime(record1["Loan_DateTime"]);
                                                DateTime endTime = Convert.ToDateTime(record1["Plan_To_ReDateTime"]);
                                                int w1 = GetWeekOfYear(startTime);
                                                int w2 = GetWeekOfYear(endTime);
                                                for (int indez = w1; indez <= w2; indez++)
                                                {
                                                    xlsApp.SetCellStyle(startRow, indez - startWeekNo + 2, (short)siteColorList["WCH"], HorizontalAlignment.Center, false);
                                                }
                                            }
                                        }
                                        startRow++;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (startRow > sRow)
                {
                    xlsApp.MergeCells(sRow, startRow - 1, 0, 0);
                    xlsApp.SetCellValue(sRow, 0, "WCH");
                    xlsApp.SetCellStyle(sRow, 0, -1, HorizontalAlignment.Center, false);
                }
            }
            #endregion
            #endregion
        }
        #endregion
        xlsApp.AutoFitAll();
        xlsApp._MyWorkbook.Write(result.ms);
        return result;

    }


    public static ExcelRenderNode GetFilterReport(RecordFilter filter)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        ExcelRender xlsApp = new ExcelRender();
        xlsApp.NewExcel();
        xlsApp.CreateSheet("data");

        int nStartRow = 0;
        int nStartCol = 0;
        var results = RecordManagment.GetRecords(filter);
        if (results == null)
            return null;

        // Column title
        #region
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Device");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Device Dpt.");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Loaner");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Loaner Dpt.");
        if (filter.category == Category.Device)
            xlsApp.SetCellValue(nStartRow, nStartCol++, "Custom ID");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Project");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Test Category");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "From");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "TO");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Return");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Status");

        xlsApp.SetRowHeight(nStartRow, 20);
        for (int index = 0; index != nStartCol; index++)
        {
            xlsApp.SetCellStyle(nStartRow, index, NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index, HorizontalAlignment.Center, true);
        }
        #endregion

        nStartRow++;
        for (int index = 0; index != results.Rows.Count; index++)
        {
            nStartCol = 0;
            DataRow record = results.Rows[index];
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Device_Name"], TypeCode.String);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Owner_Dpt"], TypeCode.String);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["LoanerName"], TypeCode.String);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Loaner_Dpt"], TypeCode.String);
            if (filter.category == Category.Device)
                xlsApp.SetCellValue(nStartRow, nStartCol++, record["Custom_ID"], TypeCode.String);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["PJ_Name"], TypeCode.String);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["TestCategory"], TypeCode.String);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Loan_DateTime"], TypeCode.DateTime);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Plan_To_ReDateTime"], TypeCode.DateTime);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Real_ReDateTime"], TypeCode.DateTime);

            RecordStatus status = (RecordStatus)Convert.ToInt32(record["Status"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, status.ToString(), TypeCode.String);
            nStartRow++;
        }

        xlsApp.AutoFitAll();
        xlsApp._MyWorkbook.Write(result.ms);
        return result;
    }

    public static List<string> GetDeviceIDList(string project, string site, DataTable results)
    {
        if (results == null)
            return null;
        List<string> deviceIDList = new List<string>();
        foreach (DataRow result in results.Rows)
        {
            if (result["Project_ID"].ToString() == project &&
                result["Site"].ToString() == site
                && !deviceIDList.Contains(result["Device_ID"].ToString()))
            {
                deviceIDList.Add(result["Device_ID"].ToString());
            }
        }

        return deviceIDList;
    }
    public static List<string> GetPurposeIDList(string project, string site, string device, DataTable results)
    {
        if (results == null)
            return null;
        List<string> purposeIDList = new List<string>();
        foreach (DataRow result in results.Rows)
        {
            if (result["Project_ID"].ToString() == project
                && result["Site"].ToString() == site
                && result["Device_ID"].ToString() == device
                && !purposeIDList.Contains(result["TestCategory_ID"].ToString()))
            {
                purposeIDList.Add(result["TestCategory_ID"].ToString());
            }
        }

        return purposeIDList;
    }


    public ExcelRenderNode GetmonthlyReport(DateTime startDate, DateTime endDate, string tempPath)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        ExcelRender xlsApp = new ExcelRender();


        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        cl_PersonManage personManage = new cl_PersonManage();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        DataTableCollection tables = deviceBookingManage.GetBookingRecod(startDate, endDate);
        if (tables == null)
        {
            result.errMsg = deviceBookingManage.errMsg;
            result.ms = null;
            return result;
        }
        if (tables.Count <= 0 || tables[0].Rows.Count <= 0)
        {
            result.errMsg = "There is no record!";
            result.ms = null;
            return result;
        }
        NOPI_SetFirstSheet(xlsApp, startDate, endDate, result, tables[0]);
        NOPI_SetSecondSheet(xlsApp, startDate, endDate, tempPath, result, tables[0]);
        NPOI_SetThirdSheet(xlsApp, startDate, endDate, tempPath, result, tables[0]);


        MemoryStream ms = new MemoryStream();
        xlsApp._MyWorkbook.Write(ms);

        result.ms = ms;
        result.errMsg = "";
        return result;
    }
    public static ExcelRenderNode GetmonthlyReport(RecordFilter filter, string tempPath)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        ExcelRender xlsApp = new ExcelRender();
        DataTable records = RecordManagment.GetRecords(filter);
        if (records == null || records.Rows.Count <= 0)
        {
            result.errMsg = "There is no record!";
            result.ms = null;
            return result;
        }
        NOPI_SetFirstSheet(xlsApp, filter.df_start, filter.df_end, result, records);
        //NOPI_SetSecondSheet(xlsApp, filter.df_start, filter.df_end, tempPath, result, records);
        //NPOI_SetThirdSheet(xlsApp, filter.df_start, filter.df_end, tempPath, result, records);


        MemoryStream ms = new MemoryStream();
        xlsApp._MyWorkbook.Write(ms);

        result.ms = ms;
        result.errMsg = "";
        return result;
    }


    public ExcelRenderNode GetAllRecordByRecord(DateTime startDate, DateTime endDate)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        cl_PersonManage personManage = new cl_PersonManage();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();

        DataTableCollection tables = deviceBookingManage.GetBookingRecod_Device(startDate, endDate, 1);
        if (tables == null)
        {
            result.errMsg = deviceBookingManage.errMsg;
            result.ms = null;
            return result;
        }
        if (tables.Count <= 0 || tables[0].Rows.Count <= 0)
        {
            result.errMsg = "There is no record!";
            result.ms = null;
            return result;
        }
        MemoryStream ms = ExcelRender.RenderToExcel(tables[0]);
        result.ms = ms;
        result.errMsg = "";
        return result;
        //System.Data.DataTable 
        //ExcelRender.RenderToExcel(tables[0], Context, "Device_Borrowing_Record_From_" + startDate.ToString("yyyyMMdd") + "_To_" + endDate.ToString("yyyyMMdd") + ".xls");
    }

    public ExcelRenderNode GetAllRecord()
    {

        ExcelRenderNode result = new ExcelRenderNode();
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        cl_PersonManage personManage = new cl_PersonManage();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();

        var records = deviceBookingManage.GetRecordsBySEDate(null, null);
        if (records == null)
        {
            result.errMsg = deviceBookingManage.errMsg;
            result.ms = null;
            return result;
        }
        MemoryStream ms = ExcelRender.RenderToExcel(records);
        result.ms = ms;
        result.errMsg = "";
        return result;
    }

    public ExcelRenderNode GetAllRecordByRecord(DateTime? startDate, DateTime? endDate)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        cl_PersonManage personManage = new cl_PersonManage();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();

        var records = deviceBookingManage.GetRecordsBySEDate(startDate, endDate);
        if (records == null)
        {
            result.errMsg = deviceBookingManage.errMsg;
            result.ms = null;
            return result;
        }
        MemoryStream ms = ExcelRender.RenderToExcel(records);
        result.ms = ms;
        result.errMsg = "";
        return result;

    }
    public static ExcelRenderNode GetAllRecordByRecord(RecordFilter filter)
    {
        var records = RecordManagment.GetRecords(filter);
        ExcelRenderNode result = new ExcelRenderNode();
        if (records == null)
        {
            result.errMsg = "can not get the records.";
            result.ms = null;
            return result;
        }
        MemoryStream ms = ExcelRender.RenderToExcel(records);
        result.ms = ms;
        result.errMsg = "";
        return result;

    }
    public ExcelRenderNode GetAllRecordByDevice(DateTime startDate, DateTime endDate)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        cl_DeviceManage deviceMana = new cl_DeviceManage();
        DataTable deviceArr = deviceMana.GetDeviceList();
        cl_DeviceBookingManage deviceBookingMana = new cl_DeviceBookingManage();


        ExcelRender xlsApp = new ExcelRender();
        xlsApp.NewExcel();
        xlsApp.CreateSheet("Data");

        int nStartRow = 1;
        bool title = false;
        for (int index = 0; index != deviceArr.Rows.Count; index++)
        {
            DataRow device = deviceArr.Rows[index];
            string deviceid = device["Device_ID"].ToString();

            var record = deviceBookingMana.GetBookingRecordByDeviceID(startDate, endDate, "Device", deviceid);
            if (record == null || record.Count <= 0 || record[0].Rows.Count <= 0)
            {
                xlsApp.SetCellValue(nStartRow, 0, device["Device_Name"], TypeCode.String);
                nStartRow++;
                continue;
            }

            for (int indey = 0; indey != record[0].Rows.Count; indey++)
            {
                if (!title)
                {
                    for (int col = 0; col != record[0].Columns.Count; col++)
                    {
                        DataColumn column = record[0].Columns[col];
                        xlsApp.SetCellValue(0, col, column.Caption, TypeCode.String);
                        xlsApp.SetCellStyle(0, col, NPOI.HSSF.Util.HSSFColor.BlueGrey.Index, HorizontalAlignment.Center, true);
                        title = true;
                    }
                }
                DataRow oneRecord = record[0].Rows[indey];
                for (int col = 0; col != record[0].Columns.Count; col++)
                {
                    if (record[0].Columns[col].DataType == DbType.DateTime.GetType())
                    {
                        xlsApp.SetCellValue(nStartRow, col, DateTime.Parse(oneRecord[col].ToString()).ToString("yyyy/MM/dd HH:mm"), TypeCode.String);
                    }
                    else
                    {
                        xlsApp.SetCellValue(nStartRow, col, oneRecord[col].ToString(), TypeCode.String);
                    }

                }
                nStartRow++;
            }
        }
        xlsApp._MyWorkbook.Write(result.ms);
        result.errMsg = "";
        return result;
    }
    public static ExcelRenderNode GetAllRecordByDevice(RecordFilter filter)
    {
        ExcelRenderNode result = new ExcelRenderNode();
        DataTable deviceArr = devManagementFac.GetDeviceList(filter);
        DataTable recordList = RecordManagment.GetRecords(filter);
        ExcelRender xlsApp = new ExcelRender();
        xlsApp.NewExcel();
        xlsApp.CreateSheet("Data");
        int nStartRow = 1;
        bool title = false;
        for (int index = 0; index != deviceArr.Rows.Count; index++)
        {
            DataRow device = deviceArr.Rows[index];
            string deviceid = device["Device_ID"].ToString();

            DataTable deviceRecord = recordList.Clone();
            if (!title)
            {
                for (int col = 0; col != deviceRecord.Columns.Count; col++)
                {
                    DataColumn column = deviceRecord.Columns[col];
                    xlsApp.SetCellValue(0, col, column.Caption, TypeCode.String);
                    xlsApp.SetCellStyle(0, col, NPOI.HSSF.Util.HSSFColor.BlueGrey.Index, HorizontalAlignment.Center, true);
                    title = true;
                }
            }
            foreach (DataRow record in recordList.Rows)
            {// 20150526153039215
                string r_deviceid = record["Device_ID"].ToString();
                if (deviceid == r_deviceid)
                {
                    deviceRecord.Rows.Add(record.ItemArray);
                }
            }
            if (deviceRecord.Rows.Count <= 0)
            {
                xlsApp.SetCellValue(nStartRow, 0, device["Device_Name"], TypeCode.String);
                nStartRow++;
                continue;
            }

            for (int indey = 0; indey != deviceRecord.Rows.Count; indey++)
            {
                DataRow oneRecord = deviceRecord.Rows[indey];
                for (int col = 0; col != deviceRecord.Columns.Count; col++)
                {
                    if (deviceRecord.Columns[col].DataType == DbType.DateTime.GetType())
                    {
                        xlsApp.SetCellValue(nStartRow, col, DateTime.Parse(oneRecord[col].ToString()).ToString("yyyy/MM/dd HH:mm"), TypeCode.String);
                    }
                    else
                    {
                        xlsApp.SetCellValue(nStartRow, col, oneRecord[col].ToString(), TypeCode.String);
                    }

                }
                nStartRow++;
            }
        }

        xlsApp._MyWorkbook.Write(result.ms);
        result.errMsg = "";
        return result;
    }
    public ExcelRenderNode GetChamberDashboard(int year)
    {
        ExcelRenderNode result = new ExcelRenderNode();

        ExcelRender xlsApp = new ExcelRender();
        xlsApp.NewExcel();
        xlsApp.CreateSheet("Dashboard");
        int nStartRow = 0;
        InitExcelTable(ref nStartRow, "Chambers", "Chamber", xlsApp);

        cl_DeviceManage chamberMana = new cl_DeviceManage();
        var chamberList = chamberMana.GetChamberList();
        if (chamberList == null)
        {
            result.ms = null;
            result.errMsg = "can not get chamber list";
        }
        // chamber 的使用统计
        for (int index = 0; index != chamberList.Rows.Count; index++)
        {
            DataRow chamber = chamberList.Rows[index];
            xlsApp.SetCellValue(nStartRow, 0, chamber["Chamber_Name"]);
            xlsApp.SetCellStyle(nStartRow, 0, -1, HorizontalAlignment.Center, true);
            for (int indey = 1; indey <= 12; indey++)
            {
                DateTime date = new DateTime(year, indey, 1);
                double per = GetBorrowingPercent(chamber["Chamber_ID"].ToString(), date);
                if (per != 0.00)
                    xlsApp.SetCellValue(nStartRow, indey, String.Format("{0:P0}", per));

                xlsApp.SetCellStyle(nStartRow, indey, -1, HorizontalAlignment.Center, true);
            }
            nStartRow++;
        }

        // chamber 按project的使用统计
        nStartRow += 4;

        cl_DeviceBookingManage bookingMana = new cl_DeviceBookingManage();

        var projects = bookingMana.GetProjectListByChamberRecord();
        if (projects.Count <= 0)
        {
            result.ms = null;
            result.errMsg = "can not get project list";
            return result;
        }

        for (int index = 0; index != chamberList.Rows.Count; index++)
        {
            DataRow chamber = chamberList.Rows[index];
            InitExcelTable(ref nStartRow, "Project - " + chamber["Chamber_Name"], "Project", xlsApp);
            for (int indey = 0; indey != projects[0].Rows.Count; indey++)
            {
                string project = projects[0].Rows[indey]["PJ_Name"].ToString();
                string pjid = projects[0].Rows[indey]["Project_ID"].ToString();
                xlsApp.SetCellValue(nStartRow, 0, project);
                xlsApp.SetCellStyle(nStartRow, 0, -1, HorizontalAlignment.Center, true);

                for (int indez = 1; indez <= 12; indez++)
                {
                    DateTime date = new DateTime(year, indez, 1);
                    double duration = GetBorrowingDuration(chamber["Chamber_ID"].ToString(), date, pjid);

                    if (duration != 0.0)
                        xlsApp.SetCellValue(nStartRow, indez, String.Format("{0:0.0}", duration));

                    xlsApp.SetCellStyle(nStartRow, indez, -1, HorizontalAlignment.Center, true);
                }
                nStartRow++;
            }

            nStartRow += 2;
        }
        MemoryStream ms = new MemoryStream();
        xlsApp._MyWorkbook.Write(ms);
        result.ms = ms;
        result.errMsg = "";
        return result;
    }



    double GetBorrowingDuration(string chamberid, DateTime date, string projectID)
    {
        cl_DeviceBookingManage bookingMana = new cl_DeviceBookingManage();
        var result = bookingMana.GetBookingRecordByYearMonthLabnamePJ(chamberid, date.Year, date.Month, projectID);
        if (result == null || result.Count <= 0 || result[0].Rows.Count <= 0)
        {
            return 0;
        }
        double sum = 0.0;
        foreach (DataRow row in result[0].Rows)
        {
            try
            {
                sum += Convert.ToDouble(row["duration"]);
            }
            catch
            {
                sum += 0.0;
            }
        }

        return sum;// 换算成小时
    }
    double GetBorrowingPercent(string chamberid, DateTime date)
    {
        cl_DeviceBookingManage bookingMana = new cl_DeviceBookingManage();
        var result = bookingMana.GetBookingRecordByYearMonthLabname(chamberid, date.Year, date.Month);
        if (result == null || result.Count <= 0 || result[0].Rows.Count <= 0)
        {
            return 0;
        }
        double sum = 0.0;
        foreach (DataRow row in result[0].Rows)
        {
            try
            {
                sum += Convert.ToDouble(row["duration"]);
            }
            catch
            {
                sum += 0.0;
            }
        }

        double record = GetMonthHour(date);
        record = sum / record;
        return record;
    }
    double GetMonthHour(DateTime date)
    {
        DateTime date0 = new DateTime(date.Year, date.Month, 1);
        DateTime date1 = date0.AddMonths(1);

        TimeSpan times = date1.Subtract(date0);

        return times.Days * 8.0;// 每天8小时
    }
    enum Month
    {
        Jan = 1, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec
    }
    void InitExcelTable(ref int nStartRow, string title, string secTitle, ExcelRender xlsApp)
    {
        xlsApp.SetCellValue(nStartRow, 0, title);
        xlsApp.SetCellStyle(nStartRow, 0, HSSFColor.LightOrange.Index, HorizontalAlignment.Left, false);
        xlsApp.SetCellFont(nStartRow, 0, 22, true);
        xlsApp.SetRowHeight(nStartRow, 25);
        xlsApp.AutoFit(0);
        nStartRow++;
        xlsApp.SetCellValue(nStartRow, 0, secTitle);
        xlsApp.SetCellStyle(nStartRow, 0, HSSFColor.SkyBlue.Index, HorizontalAlignment.Center, true);
        xlsApp.SetCellFont(nStartRow, 0, 14, true);

        for (int index = 1; index <= 12; index++)
        {
            Month month = (Month)index;
            xlsApp.SetCellValue(nStartRow, index, System.Enum.GetName(month.GetType(), month));
            xlsApp.SetCellStyle(nStartRow, index, HSSFColor.LightBlue.Index, HorizontalAlignment.Center, true);
            xlsApp.SetCellStyle(nStartRow - 1, index, HSSFColor.LightOrange.Index, HorizontalAlignment.Left, false);
        }
        nStartRow++;
    }


    private static void NOPI_SetFirstSheet(ExcelRender xlsApp, DateTime startDate, DateTime endDate, ExcelRenderNode result, DataTable records)
    {
        xlsApp.CreateSheet("data Element");
        int nStartCol = 0;
        int nStartRow = 0;
        #region // Column title
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Projecct ID");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Projecct Name");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Start DateTime");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "End DateTime");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Device Name");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "duration");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Purpose");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Applicant");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Extension");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Department Name");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Tag");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Reviewer");
        xlsApp.SetCellValue(nStartRow, nStartCol++, "Record Date");

        xlsApp.SetRowHeight(nStartRow, 20);
        for (int index = 0; index != nStartCol; index++)
        {
            xlsApp.SetCellStyle(nStartRow, index, NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index, HorizontalAlignment.Center, true);
        }

        #endregion
        nStartRow++;
        nStartCol = 0;
        #region 填入数据
        for (int index = 0; index != records.Rows.Count; index++)
        {
            nStartCol = 0;
            DataRow record = records.Rows[index];
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Project_ID"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["PJ_Name"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Loan_DateTime"], TypeCode.DateTime);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Plan_To_ReDateTime"], TypeCode.DateTime);

            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Device_Name"]);
            DateTime start = Convert.ToDateTime(record["Loan_DateTime"]);
            DateTime end = Convert.ToDateTime(record["Plan_To_ReDateTime"]);
            double duration = end.Subtract(start).TotalMinutes / 60.0;

            xlsApp.SetCellValue(nStartRow, nStartCol++, duration, TypeCode.Double);

            xlsApp.SetCellValue(nStartRow, nStartCol++, record["TestCategory"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["LoanerName"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["LoanerExtension"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Loaner_Dpt"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Comment"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["ReviewerName"]);
            xlsApp.SetCellValue(nStartRow, nStartCol++, record["Date"], TypeCode.DateTime);

            nStartRow++;
        }
        #endregion

        xlsApp.AutoFitAll();
    }
    private static void NOPI_SetSecondSheet(ExcelRender xlsApp, DateTime startDate, DateTime endDate, string tempPath, ExcelRenderNode result, DataTable tables0)
    {
        List<ProjectInfo> projectNameList = new List<ProjectInfo>();
        List<DepartmentInfo> departmentNameList = new List<DepartmentInfo>();
        var resultList = ModifyData(tables0, ref projectNameList, ref departmentNameList);

        string sheetName = "Summary of ";
        ArrayList monthList = GetMonthList(startDate, endDate);
        for (int index = 0; index != monthList.Count; index++)
        {
            sheetName += monthList[index].ToString() + ",";
        }
        sheetName = sheetName.Trim().Substring(0, sheetName.Length - 1);
        xlsApp.CreateSheet(sheetName);
        int nStartRow = 0;
        int nStartCol = 0;
        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Equipment", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 40);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        nStartCol++;

        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Total booking hours\n(HR)", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 35);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        nStartCol++;

        xlsApp.MergeCells(nStartRow, nStartRow, nStartCol, nStartCol + departmentNameList.Count - 1);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Departments booking hours", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 40);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        for (int index = 0; index != departmentNameList.Count; index++)
        {
            xlsApp.SetCellValue(nStartRow + 1, nStartCol + index, departmentNameList[index].DepartmentName, TypeCode.String);
            xlsApp.SetCellStyle(nStartRow + 1, nStartCol + index, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
            xlsApp.SetCellWidth(nStartCol + index, departmentNameList[index].DepartmentName.Length + 5);
        }
        nStartCol += departmentNameList.Count;


        xlsApp.MergeCells(nStartRow, nStartRow, nStartCol, nStartCol + projectNameList.Count - 1);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Projects booking hours", TypeCode.String);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        for (int index = 0; index != projectNameList.Count; index++)
        {
            xlsApp.SetCellValue(nStartRow + 1, nStartCol + index, projectNameList[index].ProjectName, TypeCode.String);
            xlsApp.SetCellStyle(nStartRow + 1, nStartCol + index, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
            xlsApp.SetCellWidth(nStartCol + index, projectNameList[index].ProjectName.Length + 5);
        }
        nStartCol += projectNameList.Count;

        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Idle time", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 15);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        nStartCol++;

        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Equipment working hours\n(Daily)", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 45);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);

        nStartCol++;

        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Equipment working hours\n(monthly)", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 45);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        nStartCol++;

        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Cost for Equipment\n(HR / $USD)", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 45);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        nStartCol++;

        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Totally cost\n($USD)", TypeCode.String);
        xlsApp.SetCellWidth(nStartCol, 20);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);

        nStartRow++; nStartRow++;

        {// 填入数据
            for (int index = 0; index != resultList.Count; index++)
            {
                nStartCol = 0;
                var one = resultList[index];
                xlsApp.SetCellValue(nStartRow, nStartCol, one.DeviceName, TypeCode.String);
                nStartCol++;

                xlsApp.SetCellValue(nStartRow, nStartCol, one.sumHours, TypeCode.String);
                nStartCol++;

                for (int indey = 0; indey != one.departmentList.Count; indey++)
                {
                    xlsApp.SetCellValue(nStartRow, nStartCol + indey, one.departmentList[indey].hours, TypeCode.String);
                    //nStartCol++;
                }
                nStartCol += one.departmentList.Count;
                for (int indey = 0; indey != one.projectList.Count; indey++)
                {
                    xlsApp.SetCellValue(nStartRow, nStartCol + indey, one.projectList[indey].hours, TypeCode.String);
                    //nStartCol++;
                }
                nStartCol += one.projectList.Count;
                double monthHours = GetWorkHours(startDate, endDate, one.Avg_HR);
                one.MonthHours = monthHours;

                // 未使用时数
                xlsApp.SetCellValue(nStartRow, nStartCol, one.MonthHours - one.sumHours, TypeCode.String);
                nStartCol++;

                xlsApp.SetCellValue(nStartRow, nStartCol, one.Avg_HR, TypeCode.String);
                nStartCol++;

                xlsApp.SetCellValue(nStartRow, nStartCol, one.MonthHours, TypeCode.String);
                nStartCol++;

                xlsApp.SetCellValue(nStartRow, nStartCol, one.Cost, TypeCode.String);
                nStartCol++;

                xlsApp.SetCellValue(nStartRow, nStartCol, one.Cost * one.sumHours, TypeCode.String);

                nStartRow++;
            }
        }
        #region 画图（专案）
        {
            // Project
            int nRowIndex = nStartRow + 2;
            for (int index = 0; index != resultList.Count; index++)
            {
                nStartCol = 0;
                BookingSheetInfo device = resultList[index];

                System.Data.DataTable table = new System.Data.DataTable();
                //DataColumn column = new DataColumn("ProjectName", typeof(System.Data.SqlTypes.SqlString));
                table.Columns.Add(new DataColumn("ProjectName", typeof(System.String)));
                table.Columns.Add(new DataColumn("Hours", typeof(System.Double)));

                table.TableName = device.DeviceName + "(Project)";

                DataRow row = null;
                for (int indey = 0; indey != device.projectList.Count; indey++)
                {
                    row = table.NewRow();
                    row["ProjectName"] = device.projectList[indey].ProjectName;
                    row["Hours"] = device.projectList[indey].hours;

                    table.Rows.Add(row);
                }

                double workHours = GetWorkHours(startDate, endDate, device.Avg_HR);
                double UnusedHours = workHours - device.sumHours;
                row = table.NewRow();
                row["ProjectName"] = "Idle time";
                row["Hours"] = UnusedHours;
                table.Rows.Add(row);

                string picName = tempPath + DateTime.Now.ToString("yyyyMMddHHmmssP") + index + ".png";
                GetChartImage(table, device.DeviceName + "(Project)", picName);

                xlsApp.AddPicture(picName, nRowIndex, nStartCol);
                nRowIndex += 20;
            }
        }
        #endregion
        {// 画图（部门） 
            int nRowIndex = nStartRow + 2;

            for (int index = 0; index != resultList.Count; index++)
            {
                nStartCol = 2;
                BookingSheetInfo device = resultList[index];

                System.Data.DataTable table = new System.Data.DataTable();
                //DataColumn column = new DataColumn("ProjectName", typeof(System.Data.SqlTypes.SqlString));
                table.Columns.Add(new DataColumn("DepartmentName", typeof(System.String)));
                table.Columns.Add(new DataColumn("Hours", typeof(System.Double)));

                table.TableName = device.DeviceName + "(Department)";

                DataRow row = null;
                for (int indey = 0; indey != device.departmentList.Count; indey++)
                {
                    row = table.NewRow();
                    row["DepartmentName"] = device.departmentList[indey].DepartmentName;
                    row["Hours"] = device.departmentList[indey].hours;

                    table.Rows.Add(row);
                }

                double workHours = GetWorkHours(startDate, endDate, device.Avg_HR);
                double UnusedHours = workHours - device.sumHours;
                row = table.NewRow();
                row["DepartmentName"] = "Idle time";
                row["Hours"] = UnusedHours;
                table.Rows.Add(row);

                string picName = tempPath + DateTime.Now.ToString("yyyyMMddHHmmssD") + index + ".png";
                GetChartImage(table, device.DeviceName + "(Department)", picName);

                xlsApp.AddPicture(picName, nRowIndex, nStartCol);
                nRowIndex += 20;
            }
        }

        //xlsApp.AutoFitAll();
    }
    private static void NPOI_SetThirdSheet(ExcelRender xlsApp, DateTime startDate, DateTime endDate, string tempPath, ExcelRenderNode result, DataTable tables0)
    {
        xlsApp.CreateSheet("utilization");

        int nStartRow = 0;
        int nStartCol = 0;

        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Equipment List", TypeCode.String);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        xlsApp.SetCellWidth(nStartCol, 20);
        nStartCol++;


        xlsApp.MergeCells(nStartRow, nStartRow + 1, nStartCol, nStartCol);
        xlsApp.SetCellValue(nStartRow, nStartCol, "Working Hrs\r\nDaily", TypeCode.String);
        xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        xlsApp.SetCellWidth(nStartCol, 20);
        nStartCol++;
        int col = nStartCol;

        List<DateHoursNode> dateHoursList = new List<DateHoursNode>();
        for (DateTime dateIndex = startDate; dateIndex <= endDate; dateIndex = dateIndex.AddDays(1))
        {
            xlsApp.SetCellValue(nStartRow + 1, nStartCol, dateIndex.ToString("MM/dd"), TypeCode.String);
            if (dateIndex.DayOfWeek == DayOfWeek.Saturday || dateIndex.DayOfWeek == DayOfWeek.Sunday)
            {
                //xlsApp.SetCellbackgroundStyle(nStartRow + 1, nStartCol, ColorIndex.黄色);
                xlsApp.SetCellStyle(nStartRow + 1, nStartCol, NPOI.HSSF.Util.HSSFColor.Yellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
            }
            else
            {
                xlsApp.SetCellStyle(nStartRow + 1, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
            }
            DateHoursNode dateHoursNode = new DateHoursNode();
            dateHoursNode.date = dateIndex;
            dateHoursNode.hours = 0;

            dateHoursList.Add(dateHoursNode);

            nStartCol++;
        }

        //xlsApp.MergeCells(MyExcel.GetLetter(col) + nStartRow, MyExcel.GetLetter(nStartCol - 1) + nStartRow);
        xlsApp.MergeCells(nStartRow, nStartRow, col, nStartCol - 1);
        xlsApp.SetCellValue(nStartRow, col, "Utilization", TypeCode.String);
        xlsApp.SetCellStyle(nStartRow, col, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);


        List<BookingSheet3> resultSheet3 = ModifyData(tables0, dateHoursList);
        {// 填入数据
            nStartRow = 2;
            nStartCol = 0;

            for (int index = 0; index != resultSheet3.Count; index++)
            {
                nStartCol = 0;
                var resultOne = resultSheet3[index];

                xlsApp.SetCellValue(nStartRow, nStartCol, resultOne.DevicenName, TypeCode.String);
                nStartCol++;

                xlsApp.SetCellValue(nStartRow, nStartCol, resultOne.Avg_HR, TypeCode.String);
                nStartCol++;

                for (int indey = 0; indey != resultOne.dateHoursList.Count; indey++)
                {
                    var dateHoursOne = resultOne.dateHoursList[indey];

                    xlsApp.SetCellValue(nStartRow, nStartCol + indey, String.Format("{0:P2}", (dateHoursOne.hours / resultOne.Avg_HR)), TypeCode.String);
                }
                nStartRow++;
            }
        }

        nStartRow += 4;
        {
            int rowIndex = nStartRow;
            nStartCol = 1;
            DataSet dataSet = new DataSet();
            for (int index = 0; index != resultSheet3.Count; index++)
            {
                BookingSheet3 sheet3 = resultSheet3[index];

                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add(new DataColumn("DateTime", typeof(System.String)));
                table.Columns.Add(new DataColumn("Hours", typeof(System.Double)));
                DataRow row = null;
                for (int indey = 0; indey != sheet3.dateHoursList.Count; indey++)
                {
                    row = table.NewRow();
                    row["DateTime"] = sheet3.dateHoursList[indey].date.ToString("M/d");
                    row["Hours"] = sheet3.dateHoursList[indey].hours / sheet3.Avg_HR;
                    table.Rows.Add(row);
                }
                table.TableName = sheet3.DevicenName;
                dataSet.Tables.Add(table);
            }
            string picName = tempPath + DateTime.Now.ToString("yyyyMMddHHmmss") + "_Utilization.png";
            GetChartImage_Column(dataSet.Tables, "Utilization", picName);

            xlsApp.AddPicture(picName, rowIndex, nStartCol);
        }
        {// 删除Temp文件夹中的内容
            string[] files = Directory.GetFiles(tempPath);
            for (int index = 0; index != files.Length; index++)
            {
                File.Delete(files[index]);
            }
        }

        //xlsApp.AutoFitAll();
    }

    private static void GetChartImage_Column(DataTableCollection tables, string title, string fileName)
    {
        System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
        //chart.DataSource = table.DefaultView;
        chart.BorderlineColor = Color.Black;
        chart.BorderlineWidth = 1;
        chart.BorderlineDashStyle = ChartDashStyle.Solid;
        chart.Width = 1000;

        Title chartTitle = new Title();
        chartTitle.Font = new System.Drawing.Font("Microsoft JhengHei", 12, FontStyle.Bold);
        chartTitle.Text = title;
        chart.Titles.Add(chartTitle);

        System.Web.UI.DataVisualization.Charting.Axis AxisX = new System.Web.UI.DataVisualization.Charting.Axis();
        AxisX.Interval = 1;
        AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;// = TextOrientation.Rotated270;
        AxisX.IsStartedFromZero = false;

        System.Web.UI.DataVisualization.Charting.Axis AxisY = new System.Web.UI.DataVisualization.Charting.Axis();
        AxisY.Interval = 0.2;
        AxisY.Minimum = 0;
        AxisY.LabelStyle = new LabelStyle() { Format = "{0:P0}" };

        System.Web.UI.DataVisualization.Charting.ChartArea chartArea = new System.Web.UI.DataVisualization.Charting.ChartArea("area1");
        chartArea.Area3DStyle.Enable3D = false;
        chartArea.AxisX.Interval = 1;
        chartArea.AxisX.Minimum = 1;
        chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
        chartArea.AxisY.Interval = 0.2;
        chartArea.AxisY.LabelStyle = new LabelStyle() { Format = "{0:P0}" };

        chart.ChartAreas.Add(chartArea);

        System.Web.UI.DataVisualization.Charting.Legend legend = new System.Web.UI.DataVisualization.Charting.Legend();
        legend.Alignment = StringAlignment.Center;
        legend.Docking = Docking.Right;
        legend.LegendStyle = LegendStyle.Column;
        legend.IsTextAutoFit = true;
        chart.Legends.Add(legend);

        for (int index = 0; index != tables.Count; index++)
        {
            System.Data.DataTable table = tables[index];

            System.Web.UI.DataVisualization.Charting.Series series = new System.Web.UI.DataVisualization.Charting.Series();
            series.Points.DataBindXY(table.DefaultView, table.Columns[0].ColumnName, table.DefaultView, table.Columns[1].ColumnName);
            series.LegendText = table.TableName;
            chart.Series.Add(series);
        }
        chart.SaveImage(fileName);

        //this.Chart1 = chart;
    }

    private List<YearReportSheet> ModifyData(System.Data.DataTable table, int year)
    {
        List<YearReportSheet> yearReportList = new List<YearReportSheet>();
        for (int index = 0; index != table.Rows.Count; index++)
        {
            string device_id = table.Rows[index]["Device_ID"].ToString();
            string device_Name = table.Rows[index]["Device_Name"].ToString();
            string location = table.Rows[index]["Location"].ToString();

            DateNode dateNode = new DateNode();
            dateNode.startDateTime = DateTime.Parse(table.Rows[index]["Start DateTime"].ToString());
            dateNode.endDateTime = DateTime.Parse(table.Rows[index]["End DataTime"].ToString());
            dateNode.purpose = table.Rows[index]["Purpose"].ToString();
            dateNode.projectName = table.Rows[index]["Device_Name"].ToString();
            int resultIndex = Contains(yearReportList, device_id);
            if (resultIndex != -1)
            {

                yearReportList[resultIndex].dateTimeList.Add(dateNode);
            }
            else
            {
                YearReportSheet yearReport = new YearReportSheet();
                yearReport.DeviceID = device_id;
                yearReport.DevicenName = device_Name;
                yearReport.location = location;
                yearReport.dateTimeList.Add(dateNode);

                yearReportList.Add(yearReport);
            }
        }

        return yearReportList;
    }

    private static List<BookingSheet3> ModifyData(System.Data.DataTable table, List<DateHoursNode> dateHoursList)
    {
        List<BookingSheet3> bookingSheet3List = new List<BookingSheet3>();
        for (int index = 0; index != table.Rows.Count; index++)
        {
            BookingSheetInfo bookingSheetInfo = new BookingSheetInfo();
            var row = table.Rows[index];
            string deviceID = row["Device_ID"].ToString();
            string deviceName = row["Device Name"].ToString();
            double Avg_HR = double.Parse(row["Avg_HR"].ToString());

            int result = Contains(bookingSheet3List, deviceID);
            if (result == -1)
            {
                BookingSheet3 bookingSheet3 = new BookingSheet3();
                bookingSheet3.DeviceID = deviceID;
                bookingSheet3.DevicenName = deviceName;
                bookingSheet3.Avg_HR = Avg_HR;

                bookingSheet3List.Add(bookingSheet3);
            }
        }

        for (int index = 0; index != bookingSheet3List.Count; index++)
        {
            var bookingSheet = bookingSheet3List[index];
            for (int indey = 0; indey != table.Rows.Count; indey++)
            {
                var row = table.Rows[indey];
                string deviceID = row["Device_ID"].ToString();
                if (deviceID.CompareTo(bookingSheet.DeviceID) == 0)
                {
                    DateHoursNode[] dateHoursListCpy = new DateHoursNode[dateHoursList.Count];
                    for (int i = 0; i != dateHoursListCpy.Length; i++)
                    {
                        DateHoursNode o = new DateHoursNode();
                        o.date = dateHoursList[i].date;
                        dateHoursListCpy[i] = o;
                    }
                    DateTime startDateTime = DateTime.Parse(row["Start DateTime"].ToString());
                    DateTime endDateTime = DateTime.Parse(row["End DataTime"].ToString());
                    if (startDateTime.Hour == 8)
                        startDateTime = startDateTime.AddHours(-8);
                    if (endDateTime.Hour == 20)
                        endDateTime = endDateTime.AddHours(4);


                    for (DateTime dateIndex = startDateTime.AddHours(0 - startDateTime.Hour); dateIndex <= endDateTime; dateIndex = dateIndex.AddDays(1))
                    {
                        if (dateIndex.DayOfWeek != DayOfWeek.Saturday && dateIndex.DayOfWeek != DayOfWeek.Sunday)
                        {
                            double hrs = 0;
                            for (DateTime timeIndex = dateIndex; timeIndex < dateIndex.AddHours(24); timeIndex = timeIndex.AddHours(1))
                            {
                                if (timeIndex >= startDateTime && timeIndex < endDateTime)
                                {
                                    hrs += 1;
                                }
                            }
                            int dateHrsIndex = Contains(dateHoursListCpy, dateIndex);
                            if (dateHrsIndex != -1)
                            {
                                dateHoursListCpy[dateHrsIndex].hours = hrs;
                            }
                        }
                    }
                    if (bookingSheet.dateHoursList.Count <= 0)
                        bookingSheet.dateHoursList.AddRange(dateHoursListCpy);
                    else
                    {
                        for (int indea = 0; indea != dateHoursListCpy.Length; indea++)
                        {
                            bookingSheet.dateHoursList[indea].hours += dateHoursListCpy[indea].hours;
                        }
                    }
                }
            }
        }
        return bookingSheet3List;
    }
    private static List<BookingSheetInfo> ModifyData(System.Data.DataTable table, ref List<ProjectInfo> projectNameList, ref List<DepartmentInfo> departmentNameLsit)
    {
        List<BookingSheetInfo> bookingSheetInfoList = new List<BookingSheetInfo>();
        for (int index = 0; index != table.Rows.Count; index++)
        {
            ProjectInfo projectInfo = new ProjectInfo();
            DepartmentInfo departmentInfo = new DepartmentInfo();
            BookingSheetInfo bookingSheetInfo = new BookingSheetInfo();

            var row = table.Rows[index];
            string deviceID = row["Device_ID"].ToString();
            string deviceName = row["Device Name"].ToString();
            double Avg_HR = double.Parse(row["Avg_HR"].ToString());

            double Cost = double.Parse(row["Device_Cost"].ToString());

            int result = Contains(bookingSheetInfoList, deviceID);

            projectInfo.ProjectName = row["Project Name"].ToString();
            departmentInfo.DepartmentName = row["Department Name"].ToString();


            if (Contains(projectNameList, projectInfo.ProjectName) == -1)
            {
                projectNameList.Add(projectInfo);
            }
            if (Contains(departmentNameLsit, departmentInfo.DepartmentName) == -1)
            {
                departmentNameLsit.Add(departmentInfo);
            }
            if (result == -1)
            {

                bookingSheetInfo.DeviceID = deviceID;
                bookingSheetInfo.DeviceName = deviceName;
                bookingSheetInfo.Avg_HR = Avg_HR;
                bookingSheetInfo.Cost = Cost;
                bookingSheetInfoList.Add(bookingSheetInfo);
            }
        }
        for (int index = 0; index != bookingSheetInfoList.Count; index++)
        {
            var bookingSheet = bookingSheetInfoList[index];
            ProjectInfo[] projectList = new ProjectInfo[projectNameList.Count];
            for (int i = 0; i != projectNameList.Count; i++)
            {
                ProjectInfo p = new ProjectInfo();
                p.ProjectName = projectNameList[i].ProjectName;
                projectList[i] = p;
            }
            DepartmentInfo[] departmentList = new DepartmentInfo[departmentNameLsit.Count];
            for (int i = 0; i != departmentNameLsit.Count; i++)
            {
                DepartmentInfo p = new DepartmentInfo();
                p.DepartmentName = departmentNameLsit[i].DepartmentName;
                departmentList[i] = p;
            }
            for (int indey = 0; indey != table.Rows.Count; indey++)
            {
                var row = table.Rows[indey];
                string deviceID = row["Device_ID"].ToString();
                if (bookingSheet.DeviceID.CompareTo(deviceID) == 0)
                {
                    string projectName = row["Project Name"].ToString();
                    string departmentName = row["Department Name"].ToString();

                    DateTime startDateTime = DateTime.Parse(row["Start DateTime"].ToString());
                    DateTime endDateTime = DateTime.Parse(row["End DataTime"].ToString());
                    double hours = GetTimeCount(startDateTime, endDateTime);

                    int projectIndex = Contains(projectList, projectName);
                    projectList[projectIndex].hours += hours;

                    int departmentIndex = Contains(departmentList, departmentName);
                    departmentList[departmentIndex].hours += hours;
                    departmentNameLsit[departmentIndex].hours = 0;
                    bookingSheet.sumHours += hours;
                }
            }
            bookingSheet.projectList.AddRange(projectList);
            bookingSheet.departmentList.AddRange(departmentList);
        }
        return bookingSheetInfoList;
    }

    private int Contains(List<YearReportSheet> yearReportList, string deviceID)
    {
        foreach (var o in yearReportList)
        {
            if (o.DeviceID.CompareTo(deviceID) == 0)
                return yearReportList.IndexOf(o);
        }
        return -1;
    }

    private static int Contains(DateHoursNode[] dateHoursList, DateTime date)
    {
        for (int index = 0; index != dateHoursList.Length; index++)
        {
            if (dateHoursList[index].date.Year == date.Year && dateHoursList[index].date.DayOfYear == date.DayOfYear)
                return index;
        }
        return -1;
    }
    private int Contains(List<DateHoursNode> dateHoursList, DateTime date)
    {
        foreach (var o in dateHoursList)
        {
            if (o.date.Year == date.Year && o.date.DayOfYear == date.DayOfYear)
                return dateHoursList.IndexOf(o);
        }
        return -1;
    }
    private static int Contains(List<BookingSheet3> bookingSheet3List, string deviceID)
    {
        foreach (var o in bookingSheet3List)
        {
            if (o.DeviceID.CompareTo(deviceID) == 0)
                return bookingSheet3List.IndexOf(o);
        }
        return -1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectInfoList"></param>
    /// <param name="projectName"></param>
    /// <returns></returns>
    private static int Contains(DepartmentInfo[] projectInfoList, string projectName)
    {
        for (int index = 0; index != projectInfoList.Length; index++)
        {
            if (projectInfoList[index].DepartmentName.CompareTo(projectName) == 0)
                return index;
        }
        //foreach (var o in projectInfoList)
        //{
        //    if (o.ProjectName.CompareTo(projectName) == 0)
        //        return projectInfoList.IndexOf(o);
        //}
        return -1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectInfoList"></param>
    /// <param name="projectName"></param>
    /// <returns></returns>
    private static int Contains(ProjectInfo[] projectInfoList, string projectName)
    {
        for (int index = 0; index != projectInfoList.Length; index++)
        {
            if (projectInfoList[index].ProjectName.CompareTo(projectName) == 0)
                return index;
        }
        //foreach (var o in projectInfoList)
        //{
        //    if (o.ProjectName.CompareTo(projectName) == 0)
        //        return projectInfoList.IndexOf(o);
        //}
        return -1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectInfoList"></param>
    /// <param name="projectName"></param>
    /// <returns></returns>
    private static int Contains(List<ProjectInfo> projectInfoList, string projectName)
    {
        foreach (var o in projectInfoList)
        {
            if (o.ProjectName.CompareTo(projectName) == 0)
                return projectInfoList.IndexOf(o);
        }
        return -1;
    }
    private static int Contains(List<DepartmentInfo> departmentInfoList, string departmentName)
    {
        foreach (var o in departmentInfoList)
        {
            if (o.DepartmentName.CompareTo(departmentName) == 0)
                return departmentInfoList.IndexOf(o);
        }
        return -1;
    }
    private static int Contains(List<BookingSheetInfo> bookingSheetInfoList, string deviceID)
    {
        foreach (var o in bookingSheetInfoList)
        {
            if (o.DeviceID.CompareTo(deviceID) == 0)
                return bookingSheetInfoList.IndexOf(o);
        }
        return -1;
    }

    /// <summary>
    /// 获取借用时数
    /// </summary>
    /// <returns></returns>
    public static double GetTimeCount(DateTime startDateTime, DateTime endDateTime)
    {
        // 当起始时间为早上8点时， 则认为借用时间为当天零点开始借用
        if (startDateTime.Hour == 8)
        {
            startDateTime = startDateTime.AddHours(-8);
        }
        // 当截止时间为晚上20点时， 则认为借用时间至当天晚上24点结束
        if (endDateTime.Hour == 20)
        {
            endDateTime = endDateTime.AddHours(4);
        }
        double cnt = 0;

        TimeSpan time = endDateTime.Subtract(startDateTime);
        cnt = time.TotalHours;

        return cnt;
    }

    private int GetColorIndex(List<ProjectColorNode> projectColorList, string projectName)
    {
        foreach (var o in projectColorList)
            if (o.projectName.CompareTo(projectName) == 0)
                return o.ColorIndex;

        return 2;// 白色
    }
    private int GetWeekOfYear(DateTime dateTime)
    {
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(dateTime, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        return weekOfYear;
    }

    /// <summary>
    /// 获取时间段内的总工作时数
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="HoursOfDay"></param>
    /// <returns></returns>
    private static double GetWorkHours(DateTime startDate, DateTime endDate, double HoursOfDay)
    {
        double cnt = 0;
        for (DateTime index = startDate; index <= endDate; index = index.AddDays(1))
        {
            if (!(index.DayOfWeek == DayOfWeek.Saturday || index.DayOfWeek == DayOfWeek.Sunday))
            {
                cnt += HoursOfDay;
            }
        }
        return cnt;
    }

    private static ArrayList GetMonthList(DateTime startDate, DateTime endDate)
    {
        ArrayList monthList = new ArrayList();
        DateTime dateIndex = new DateTime(startDate.Year, startDate.Month, 1);
        for (; dateIndex <= endDate; dateIndex = dateIndex.AddMonths(1))
            monthList.Add(dateIndex.Month);

        return monthList;
    }

    private static void GetChartImage(System.Data.DataTable table, string title, string fileName)
    {
        System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();
        chart.DataSource = table.DefaultView;
        chart.BorderlineColor = Color.Black;
        chart.BorderlineWidth = 1;
        chart.BorderlineDashStyle = ChartDashStyle.Solid;
        chart.Width = 355;

        Title chartTitle = new Title();
        chartTitle.Font = new System.Drawing.Font("Microsoft JhengHei", 12, FontStyle.Bold);
        chartTitle.Text = title;
        chart.Titles.Add(chartTitle);

        System.Web.UI.DataVisualization.Charting.Series series = new System.Web.UI.DataVisualization.Charting.Series("series1");
        series.ChartType = SeriesChartType.Pie;
        series.XValueMember = table.Columns[0].ColumnName;
        series.YValueMembers = table.Columns[1].ColumnName;
        series.IsValueShownAsLabel = true;
        chart.Series.Add(series);
        //chart.Series["Series1"].IsValueShownAsLabel = true;

        System.Web.UI.DataVisualization.Charting.ChartArea chartArea = new System.Web.UI.DataVisualization.Charting.ChartArea("area1");
        chartArea.Area3DStyle.Enable3D = true;
        chart.ChartAreas.Add(chartArea);

        System.Web.UI.DataVisualization.Charting.Legend legend = new System.Web.UI.DataVisualization.Charting.Legend();
        legend.Alignment = StringAlignment.Center;
        //legend.AutoFitMinFontSize = true;
        legend.Docking = Docking.Right;
        legend.LegendStyle = LegendStyle.Column;
        legend.IsTextAutoFit = true;
        chart.Legends.Add(legend);

        chart.SaveImage(fileName);


    }
}