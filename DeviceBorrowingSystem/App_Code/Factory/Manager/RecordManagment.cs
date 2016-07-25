using BLL;
using DataBaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using SystemDAO;

/// <summary>
/// RecordManagment 的摘要说明
/// </summary>
public class RecordManagment
{

    log4net.ILog log = log4net.LogManager.GetLogger("loginfo");
    //HttpBrowserCapabilities bc = Request.Browser;
    //log.Info("Browser: " + bc.Browser + "; IP: " + Request.UserHostAddress);

    public RecordManagment()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //

    }

    public static DataTable GetDeviceByIDName(string idName)
    {

        string sql = @"select base.* from tbl_summary_dev_title as base 
where (base.s_id = @idName or base.s_name like ('%' + @idName + '%')) 
and (base.s_status = 1)";
        try
        {
            var result = SystemDAO.SqlHelperEx.GetTableText(sql, new SqlParameter[] { new SqlParameter("@idName", idName) });
            return result[0];
        }
        catch
        {
            return null;
        }

    }

    public static DataTable GetBookingTableStruct()
    {
        string sql = @"select top 1 booking.* from tbl_DeviceBooking as booking ";
        try
        {
            var result = SqlHelper.GetTableText(sql, null);
            return result[0].Clone();
        }
        catch
        {
            return null;
        }

    }

    public static int AddRecords(DataTable records)
    {
        string sql = @"INSERT INTO tbl_DeviceBooking(Booking_ID, Loaner_ID, Device_ID, Project_ID, TestCategory_ID, PJ_Stage, Loan_DateTime 
, Plan_To_ReDateTime, Real_ReDateTime, Status, Comment, Reviewer_ID, Review_Comment, Date) 
  VALUES (@Booking_ID,@Loaner_ID,@Device_ID,@Project_ID, @TestCategory_ID, @PJ_Stage, @Loan_DateTime 
, @Plan_To_ReDateTime,@Real_ReDateTime,@Status,@Comment,@Reviewer_ID, @Review_Comment, GETDATE())";
        int cnt = 0;
        List<string> ids = new List<string>();
        foreach (DataRow record in records.Rows)
        {
            SqlParameter[] param = new SqlParameter[] { 
            new SqlParameter("@Booking_ID", record["Booking_ID"]), 
            new SqlParameter("@Loaner_ID", record["Loaner_ID"]), 
            new SqlParameter("@Device_ID", record["Device_ID"]), 
            new SqlParameter("@Project_ID", record["Project_ID"]), 
            new SqlParameter("@TestCategory_ID", record["TestCategory_ID"]), 
            new SqlParameter("@PJ_Stage", record["PJ_Stage"]), 
            new SqlParameter("@Loan_DateTime", Convert.ToDateTime(record["Loan_DateTime"])), 
            new SqlParameter("@Plan_To_ReDateTime", Convert.ToDateTime(record["Plan_To_ReDateTime"])), 
            new SqlParameter("@Real_ReDateTime", Convert.ToDateTime(record["Real_ReDateTime"])), 
            new SqlParameter("@Status", record["Status"]), 
            new SqlParameter("@Comment", record["Comment"]), 
            new SqlParameter("@Reviewer_ID", record["Reviewer_ID"]), 
            new SqlParameter("@Review_Comment", record["Review_Comment"])
            };
            try
            {
                int result = SqlHelper.ExecteNonQueryText(sql, param);
                if (result >= 1)
                {
                    ids.Add(record["Booking_ID"].ToString());
                    #region Mail Service
                    if (ids.Count >= 1)
                    {
                        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
                        DataTable bookingList = deviceBookingManage.GetBookingItemsByIDs(ids);
                        if (bookingList != null)
                        {
                            string id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + cnt;
                            string MailTo = bookingList.Rows[0]["P_Email"].ToString();//Chris_Tsung@Wistron.com;@wistron.local;May_Lai@wistron.com
                            string cc = "";//"mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;";//mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;

                            StringBuilder Subject = new StringBuilder();
                            Subject.Append("[Device Borrowing System]--Borrow Approve");

                            StringBuilder body = new StringBuilder();
                            body.Append("<h2>Borrow Information: </h2><br/><br/>");
                            body.Append("<table border='1'><tr>");
                            body.Append("<th>Booking ID</th>");
                            body.Append("<th>Device Name</th>");
                            //body.Append("<th>Loaner Name</th>");
                            body.Append("<th>Start DateTime</th>");
                            body.Append("<th>End DateTime</th>");
                            body.Append("<th>Approve Reason</th>");
                            body.Append("</tr>");

                            //cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
                            for (int index = 0; index != bookingList.Rows.Count; index++)
                            {
                                body.Append("<tr>");
                                body.Append("<td>" + bookingList.Rows[index]["Booking_ID"].ToString() + "</td>");
                                body.Append("<td>" + bookingList.Rows[index]["Device_Name"].ToString() + "</td>");
                                //body.Append("<td>" + bookingList.Rows[index]["P_Name"].ToString() + "</td>");
                                body.Append("<td>" + bookingList.Rows[index]["Loan_DateTime"].ToString() + "</td>");
                                body.Append("<td>" + bookingList.Rows[index]["Plan_To_ReDateTime"].ToString() + "</td>");
                                body.Append("<td>" + bookingList.Rows[index]["Review_Comment"].ToString() + "</td>");
                                body.Append("</tr>");
                            }
                            body.Append("</table><br/>");
                            body.Append("Please sign in the borrowing system to check the detail record!<br/>");
                            body.Append("http://tpeota01.whq.wistron:88/");

                            MailService.AddOneMailService(id, MailTo.ToString(), cc.ToString(), Subject.ToString(), body.ToString(), true, "");
                        }
                    }
                    #endregion
                }
                cnt += result;
            }
            catch
            {

            }
        }

        return cnt;
    }
    /// <summary>
    /// do not show reviewer
    /// </summary>
    /// <param name="category"></param>
    /// <param name="status"></param>
    /// <param name="booking_id"></param>
    /// <param name="loaner_id"></param>
    /// <param name="device_id"></param>
    /// <param name="date_from"></param>
    /// <param name="date_to"></param>
    /// <param name="showReviewer"></param>
    /// <returns></returns>
    public static DataTable GetRecordsNotShowReviewer(Category category, List<RecordStatus> status
        , string booking_id = null, string loaner_id = null
        , string device_id = null, DateTime? date_from = null
        , DateTime? date_to = null, bool showReviewer = false)
    {
        string sql = @"select record.* 
, summary.s_name as Device_Name 
, Loaner.P_Name as LoanerName, Loaner.P_Department as [Loaner_Dpt] 
, [Owner].P_Department as [Owner_Dpt] 
, P.PJ_Name, P.Cust_Name 
, TC.Name as TestCategory ";
        switch ((Category)category)
        {
            case Category.Device:
                sql += @"
, D.d_customid as Custom_ID ";
                break;
            case Category.Equipment:
                sql += @"
, 1 as Custom_ID ";
                break;
            case Category.Chamber:
                sql += @"
, 1 as Custom_ID ";
                break;
            default:
                break;
        }

        sql += @"
from tbl_DeviceBooking as record 
INNER JOIN tbl_Person as Loaner ON record.Loaner_ID = Loaner.P_ID 
INNER JOIN tbl_Project as P ON record.Project_ID = P.PJ_Code 
INNER JOIN tbl_TestCategory as TC ON record.TestCategory_ID = TC.ID 
INNER JOIN tbl_summary_dev_title summary ON record.Device_ID = summary.s_id 
INNER JOIN tbl_Person as [Owner] on summary.s_ownerid = [Owner].P_ID ";
        switch ((Category)category)
        {
            case Category.Device:
                sql += @"
INNER JOIN tbl_device_detail as D on record.Device_ID = D.d_id  ";
                break;
            case Category.Equipment:
                sql += @" ";
                break;
            case Category.Chamber:
                sql += @" ";
                break;
            default:
                break;
        }
        #region do filter
        List<SqlParameter> paramss = new List<SqlParameter>();
        // category
        sql += @"
where (summary.s_category = @category) ";
        paramss.Add(new SqlParameter("@category", category));
        // booking id
        if (booking_id != null)
        {
            sql += @"
and (record.Booking_ID = @booking_id) ";
            paramss.Add(new SqlParameter("@booking_id", booking_id));
        }
        // status
        string r_status = String.Empty;
        r_status += @"
and (record.Status in ( ";
        foreach (RecordStatus recordStatus in status)
        {
            r_status += (int)recordStatus + ",";
        }
        r_status = r_status.Substring(0, r_status.Length - 1);
        r_status += ")) ";

        sql += r_status;
        // loaner
        if (loaner_id != null)
        {
            sql += @"
and (record.Loaner_ID = @Loaner_ID)";
            paramss.Add(new SqlParameter("@Loaner_ID", loaner_id));
        }

        // device id
        if (device_id != null)
        {
            sql += @"
and (summary.s_id = @Device_ID) ";
            paramss.Add(new SqlParameter("@Device_ID", device_id));
        }

        // date_from
        if (date_from != null)
        {
            sql += @"
and (@date_from <= Plan_To_ReDateTime) ";
            paramss.Add(new SqlParameter("@date_from", date_from));
        }

        // date_to
        if (date_to != null)
        {
            sql += @"
and (@date_to >= Loan_DateTime) ";
            paramss.Add(new SqlParameter("@date_to", date_to));
        }
        #endregion

        try
        {
            DataTableCollection result = SystemDAO.SqlHelperEx.GetTableText(sql, paramss.ToArray());
            return result[0];
        }
        catch { }
        return null;
    }
    /// <summary>
    /// show reviewer
    /// </summary>
    /// <param name="category"></param>
    /// <param name="status"></param>
    /// <param name="booking_id"></param>
    /// <param name="loaner_id"></param>
    /// <param name="device_id"></param>
    /// <param name="date_from"></param>
    /// <param name="date_to"></param>
    /// <returns></returns>
    public static DataTable GetRecords(int category, List<RecordStatus> status
        , string booking_id = null, string loaner_id = null
        , string device_id = null, DateTime? date_from = null
        , DateTime? date_to = null)
    {
        string sql = @"select record.* 
, summary.s_name as Device_Name 
, Loaner.P_Name as LoanerName, Loaner.P_Department as [Loaner_Dpt] 
, [Owner].P_Department as [Owner_Dpt] 
, Reviewer.P_Name as ReviewerName 
, P.PJ_Name, P.Cust_Name 
, TC.Name as TestCategory ";
        switch ((Category)category)
        {
            case Category.Device:
                sql += @"
, D.d_customid as Custom_ID ";
                break;
            case Category.Equipment:
                sql += @"
, 1 as Custom_ID ";
                break;
            case Category.Chamber:
                sql += @"
, 1 as Custom_ID ";
                break;
            default:
                break;
        }

        sql += @"
from tbl_DeviceBooking as record 
INNER JOIN tbl_Person as Loaner ON record.Loaner_ID = Loaner.P_ID 
INNER JOIN tbl_Project as P ON record.Project_ID = P.PJ_Code 
INNER JOIN tbl_TestCategory as TC ON record.TestCategory_ID = TC.ID 
INNER JOIN tbl_Person AS Reviewer ON record.Reviewer_ID = Reviewer.P_ID 
INNER JOIN tbl_summary_dev_title summary ON record.Device_ID = summary.s_id 
INNER JOIN tbl_Person as [Owner] on summary.s_ownerid = [Owner].P_ID ";
        switch ((Category)category)
        {
            case Category.Device:
                sql += @"
INNER JOIN tbl_device_detail as D on record.Device_ID = D.d_id  ";
                break;
            case Category.Equipment:
                sql += @" ";
                break;
            case Category.Chamber:
                sql += @" ";
                break;
            default:
                break;
        }
        #region do filter
        List<SqlParameter> paramss = new List<SqlParameter>();
        // category
        sql += @"
where (summary.s_category = @category) ";
        paramss.Add(new SqlParameter("@category", category));
        // booking id
        if (booking_id != null)
        {
            sql += @"
and (record.Booking_ID = @booking_id) ";
            paramss.Add(new SqlParameter("@booking_id", booking_id));
        }
        // status
        string r_status = String.Empty;
        r_status += @"
and (record.Status in ( ";
        foreach (RecordStatus recordStatus in status)
        {
            r_status += (int)recordStatus + ",";
        }
        r_status = r_status.Substring(0, r_status.Length - 1);
        r_status += ")) ";

        sql += r_status;
        // loaner
        if (loaner_id != null)
        {
            sql += @"
and (record.Loaner_ID = @Loaner_ID)";
            paramss.Add(new SqlParameter("@Loaner_ID", loaner_id));
        }

        // device id
        if (device_id != null)
        {
            sql += @"
and (summary.s_id = @Device_ID) ";
            paramss.Add(new SqlParameter("@Device_ID", device_id));
        }

        // date_from
        if (date_from != null)
        {
            sql += @"
and (@date_from <= Plan_To_ReDateTime) ";
            paramss.Add(new SqlParameter("@date_from", date_from));
        }

        // date_to
        if (date_to != null)
        {
            sql += @"
and (@date_to >= Loan_DateTime) ";
            paramss.Add(new SqlParameter("@date_to", date_to));
        }
        #endregion

        try
        {
            DataTableCollection result = SystemDAO.SqlHelperEx.GetTableText(sql, paramss.ToArray());
            return result[0];
        }
        catch { }
        return null;
    }
    public static DataTable GetRecords(int category, List<RecordStatus> status, string booking_id = null, string loaner_id = null)
    {
        string sql = @"select record.* 
, summary.s_name as Device_Name 
, Loaner.P_Name as LoanerName, Loaner.P_Department as [Loaner_Dpt] 
, [Owner].P_Department as [Owner_Dpt] 
, Reviewer.P_Name as ReviewerName 
, P.PJ_Name, P.Cust_Name 
, TC.Name as TestCategory ";
        switch ((Category)category)
        {
            case Category.Device:
                sql += @"
, D.d_customid as Custom_ID ";
                break;
            case Category.Equipment:
                sql += @"
, 1 as Custom_ID ";
                break;
            case Category.Chamber:
                sql += @"
, 1 as Custom_ID ";
                break;
            default:
                break;
        }

        sql += @"
from tbl_DeviceBooking as record 
INNER JOIN tbl_Person as Loaner ON record.Loaner_ID = Loaner.P_ID 
INNER JOIN tbl_Project as P ON record.Project_ID = P.PJ_Code 
INNER JOIN tbl_TestCategory as TC ON record.TestCategory_ID = TC.ID 
INNER JOIN tbl_Person AS Reviewer ON record.Reviewer_ID = Reviewer.P_ID 
INNER JOIN tbl_summary_dev_title summary ON record.Device_ID = summary.s_id 
INNER JOIN tbl_Person as [Owner] on summary.s_ownerid = [Owner].P_ID ";
        switch ((Category)category)
        {
            case Category.Device:
                sql += @"
INNER JOIN tbl_device_detail as D on record.Device_ID = D.d_id  ";
                break;
            case Category.Equipment:
                sql += @" ";
                break;
            case Category.Chamber:
                sql += @" ";
                break;
            default:
                break;
        }
        #region do filter
        List<SqlParameter> paramss = new List<SqlParameter>();
        // category
        sql += @"
where (summary.s_category = @category) ";
        paramss.Add(new SqlParameter("@category", category));
        // booking id
        if (booking_id != null)
        {
            sql += @"
and (record.Booking_ID = @booking_id) ";
            paramss.Add(new SqlParameter("@booking_id", booking_id));
        }
        // status
        string r_status = String.Empty;
        r_status += @"
and (record.Status in ( ";
        foreach (RecordStatus recordStatus in status)
        {
            r_status += (int)recordStatus + ",";
        }
        r_status = r_status.Substring(0, r_status.Length - 1);
        r_status += ")) ";

        sql += r_status;
        // loaner
        if (loaner_id != null)
        {
            sql += @"
and (record.Loaner_ID = @Loaner_ID)";
            paramss.Add(new SqlParameter("@Loaner_ID", loaner_id));
        }
        #endregion

        try
        {
            DataTableCollection result = SystemDAO.SqlHelperEx.GetTableText(sql, paramss.ToArray());
            return result[0];
        }
        catch { }
        return null;
    }
    public static DataTable GetRecords(RecordFilter filter)
    {
        string sql = @"select 
summary.s_name as Device_Name 
, Loaner.P_Name as LoanerName, Loaner.P_Department as [Loaner_Dpt], Loaner.P_ExNumber as LoanerExtension  
, [Owner].P_Department as [Owner_Dpt], [Owner].P_Location as [Site] 
, Reviewer.P_Name as ReviewerName 
, P.PJ_Name, P.Cust_Name 
, TC.Name as TestCategory 
, record.* ";
        switch (filter.category)
        {
            case Category.Device:
                sql += @"
, D.d_customid as Custom_ID ";
                break;
            case Category.Equipment:
                sql += @"
, 1 as Custom_ID ";
                break;
            case Category.Chamber:
                sql += @"
, 1 as Custom_ID ";
                break;
            default:
                break;
        }
        sql += @"
from tbl_DeviceBooking as record 
INNER JOIN tbl_Person as Loaner ON record.Loaner_ID = Loaner.P_ID 
INNER JOIN tbl_Project as P ON record.Project_ID = P.PJ_Code 
INNER JOIN tbl_TestCategory as TC ON record.TestCategory_ID = TC.ID 
INNER JOIN tbl_Person AS Reviewer ON record.Reviewer_ID = Reviewer.P_ID 
INNER JOIN tbl_summary_dev_title summary ON record.Device_ID = summary.s_id 
INNER JOIN tbl_Person as [Owner] on summary.s_ownerid = [Owner].P_ID ";
        switch (filter.category)
        {
            case Category.Device:
                sql += @"
INNER JOIN tbl_device_detail as D on record.Device_ID = D.d_id  ";
                break;
            case Category.Equipment:
                sql += @" ";
                break;
            case Category.Chamber:
                sql += @" ";
                break;
            default:
                break;
        }

        #region do filters
        string filterStr = string.Empty;
        List<SqlParameter> paramss = new List<SqlParameter>();
        // category
        filterStr += @"
where (summary.s_category = @category) ";
        paramss.Add(new SqlParameter("@category", (int)filter.category));
        // device filter
        if (filter.deviceFilter)
        {
            if (filter.d_site != string.Empty)
            {
                filterStr += @"
and ([Owner].P_Location = @Location) ";

                paramss.Add(new SqlParameter("@Location", filter.d_site));
            }

            if (filter.d_department != string.Empty)
            {
                filterStr += @"
and ([Owner].P_Department = @Department) ";
                paramss.Add(new SqlParameter("@Department", filter.d_department));
            }
        }
        // record filter
        if (filter.recordFilter)
        {
            if (filter.r_project != string.Empty)
            {
                filterStr += @"
and (P.PJ_Code = @PJ_Code) ";
                paramss.Add(new SqlParameter("@PJ_Code", filter.r_project));
            }

            if (filter.r_purpose != string.Empty)
            {
                filterStr += @"
and (TC.ID = @TC_ID) ";
                paramss.Add(new SqlParameter("@TC_ID", filter.r_purpose));
            }
        }
        // duration filter
        if (filter.durationFilter)
        {
            if (filter.df_year != 0)
            {
                filterStr += @"
and (YEAR(record.Loan_DateTime) = @year) ";
                paramss.Add(new SqlParameter("@year", filter.df_year));
            }
            if (filter.df_start != new DateTime())
            {
                filterStr += @"
and (record.Loan_DateTime >= @startDate) ";
                paramss.Add(new SqlParameter("@startDate", filter.df_start));
            }
            if (filter.df_end != new DateTime())
            {
                filterStr += @"
and (record.Loan_DateTime <= @endDate) ";
                paramss.Add(new SqlParameter("@endDate", filter.df_end));
            }
        }

        // status
        List<RecordStatus> status = new List<RecordStatus>();
        status.Add(RecordStatus.APPROVE_NORETURN);
        status.Add(RecordStatus.RETURN);
        string r_status = String.Empty;
        r_status += @"
and (record.Status in ( ";
        foreach (RecordStatus recordStatus in status)
        {
            r_status += (int)recordStatus + ",";
        }
        r_status = r_status.Substring(0, r_status.Length - 1);
        r_status += ")) ";

        filterStr += r_status;
        #endregion

        sql += filterStr;

        sql += @"
order by Loan_DateTime ";
        try
        {
            DataTableCollection result = SystemDAO.SqlHelperEx.GetTableText(sql, paramss.ToArray());
            return result[0];
        }
        catch { }
        return null;

    }
    public static DataRow GetRecord(string booking_id)
    {
        string sql = @"
select record.* 
, summary.s_name as Device_Name 
, tbl_Project.PJ_Name, tbl_Project.Cust_Name 
, Loaner.P_Name as Loaner_Name 
from tbl_DeviceBooking as record 
inner join tbl_Person as Loaner on record.Loaner_ID = Loaner.P_ID 
inner join tbl_summary_dev_title as summary on record.Device_ID = summary.s_id 
inner join tbl_Project on tbl_Project.PJ_Code = record.Project_ID 
where (record.Booking_ID = @Booking_ID) ";

        try
        {
            var result = SystemDAO.SqlHelperEx.GetTableText(sql, new SqlParameter[] { new SqlParameter("@Booking_ID", booking_id) });
            return result[0].Rows[0];
        }
        catch
        {
            return null;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cat"></param>
    /// <param name="status"></param>
    /// <param name="chargeOfDptList"></param>
    /// <returns></returns>
    public static DataTable GetRecords(Category cat, List<RecordStatus> status, DataTable chargeOfDptList)
    {
        string sql = @"select record.* 
, summary.s_name as Device_Name 
, Loaner.P_Name as LoanerName, Loaner.P_Department as [Loaner_Dpt] 
, [Owner].P_Department as [Owner_Dpt] 
, P.PJ_Name, P.Cust_Name 
, TC.Name as TestCategory ";
        switch (cat)
        {
            case Category.Device:
                sql += @"
, D.d_customid as Custom_ID ";
                break;
            case Category.Equipment:
                sql += @"
, 1 as Custom_ID ";
                break;
            case Category.Chamber:
                sql += @"
, 1 as Custom_ID ";
                break;
            default:
                break;
        }

        sql += @"
from tbl_DeviceBooking as record 
INNER JOIN tbl_Person as Loaner ON record.Loaner_ID = Loaner.P_ID 
INNER JOIN tbl_Project as P ON record.Project_ID = P.PJ_Code 
INNER JOIN tbl_TestCategory as TC ON record.TestCategory_ID = TC.ID 
INNER JOIN tbl_summary_dev_title summary ON record.Device_ID = summary.s_id 
INNER JOIN tbl_Person as [Owner] on summary.s_ownerid = [Owner].P_ID ";
        switch (cat)
        {
            case Category.Device:
                sql += @"
INNER JOIN tbl_device_detail as D on record.Device_ID = D.d_id  ";
                break;
            case Category.Equipment:
                sql += @" ";
                break;
            case Category.Chamber:
                sql += @" ";
                break;
            default:
                break;
        }
        sql += @"
where (summary.s_category = @category) ";
        #region filter
        // status
        string r_status = String.Empty;
        r_status += @"
and (record.Status in ( ";
        foreach (RecordStatus recordStatus in status)
        {
            r_status += (int)recordStatus + ",";
        }
        r_status = r_status.Substring(0, r_status.Length - 1);
        r_status += ")) ";

        sql += r_status;

        // owner department
        if (chargeOfDptList != null)
        {
            string owner_dpt = String.Empty;
            owner_dpt += @"
and (Owner.P_Department in( ";

            foreach (DataRow dpt in chargeOfDptList.Rows)
            {
                owner_dpt += "'" + dpt["DptValue"].ToString() + "',";
            }
            if (owner_dpt != String.Empty)
            {
                owner_dpt = owner_dpt.Substring(0, owner_dpt.Length - 1);
            }
            owner_dpt += "))";

            sql += owner_dpt;
        }
        #endregion
        try
        {
            return SystemDAO.SqlHelperEx.GetTableText(sql, new SqlParameter[] { new SqlParameter("@category", (Int32)cat) })[0];
        }
        catch
        {
            return null;
        }
    }

    public static int DeleteRecords(List<string> booking_id)
    {
        string sql = @"delete from tbl_DeviceBooking 
where Booking_ID in ( ";
        string ids = String.Empty;
        foreach (string id in booking_id)
        {
            ids += "'" + id + "',";
        }
        if (ids == String.Empty)
            return -1;
        ids = ids.Substring(0, ids.Length - 1);
        sql += ids + @" )";

        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, null);
        }
        catch
        {
            return -1;
        }
    }

    public static int CheckStatus(string booing_id)
    {
        string sql = @"select record.Status from tbl_DeviceBooking as record 
where record.Booking_ID = @Booking_ID ";

        try
        {
            return (Int32)SystemDAO.SqlHelperEx.ExecuteScalarText(sql, new SqlParameter[] {
                                                                        new SqlParameter("@Booking_ID", booing_id)
            
            });
        }
        catch
        {
            return -2;
        }
    }

    public static int EditBookingRecord(tbl_DeviceBooking booking)
    {

        string sql = @"
update tbl_DeviceBooking set 
Loaner_ID = @Loaner_ID, Device_ID = @Device_ID 
, Project_ID = @Project_ID, TestCategory_ID = @TestCategory_ID, PJ_Stage = @PJ_Stage 
, Loan_DateTime = @Loan_DateTime, Plan_To_ReDateTime = @Plan_To_ReDateTime
, db_is_recurrence = @db_is_recurrence, db_recurrence = @db_recurrence
, db_start = @db_start, db_end = @db_end
, Real_ReDateTime = @Real_ReDateTime, Status = @Status , Comment = @Comment 
, Reviewer_ID = @Reviewer_ID, Review_Comment = @Review_Comment , Date = GetDate()  

where Booking_ID = @Booking_ID 

";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@Booking_ID", booking.Booking_ID));
        if (booking.Loaner_ID == String.Empty)
            return -1;
        paramss.Add(new SqlParameter("@Loaner_ID", booking.Loaner_ID));
        paramss.Add(new SqlParameter("@Device_ID", booking.Device_ID));
        paramss.Add(new SqlParameter("@Project_ID", booking.Project_ID));
        paramss.Add(new SqlParameter("@TestCategory_ID", booking.TestCategory_ID));
        paramss.Add(new SqlParameter("@PJ_Stage", booking.PJ_Stage));
        paramss.Add(new SqlParameter("@Loan_DateTime", booking.Loan_DateTime));
        paramss.Add(new SqlParameter("@Plan_To_ReDateTime", booking.Plan_To_ReDateTime));
        paramss.Add(new SqlParameter("@db_is_recurrence", booking.db_is_recurrence));
        if (booking.db_is_recurrence)
        {
            paramss.Add(new SqlParameter("@db_recurrence", booking.db_recurrence));
            paramss.Add(new SqlParameter("@db_start", booking.db_start));
            paramss.Add(new SqlParameter("@db_end", booking.db_end));
        }
        else
        {
            paramss.Add(new SqlParameter("@db_recurrence", DBNull.Value));
            paramss.Add(new SqlParameter("@db_start", DBNull.Value));
            paramss.Add(new SqlParameter("@db_end", DBNull.Value));
        }

        if (booking.Real_ReDateTime == null)
        {
            paramss.Add(new SqlParameter("@Real_ReDateTime", DBNull.Value));
        }
        else
        {
            paramss.Add(new SqlParameter("@Real_ReDateTime", booking.Real_ReDateTime));
        }
        paramss.Add(new SqlParameter("@Status", booking.Status));
        paramss.Add(new SqlParameter("@Comment", booking.Comment));

        if (booking.Reviewer_ID == null)
        {
            paramss.Add(new SqlParameter("@Reviewer_ID", DBNull.Value));
            paramss.Add(new SqlParameter("@Review_Comment", DBNull.Value));
        }
        else
        {
            paramss.Add(new SqlParameter("@Reviewer_ID", booking.Reviewer_ID));
            paramss.Add(new SqlParameter("@Review_Comment", booking.Review_Comment));
        }
        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
        }
        catch
        {
            return -1;
        }
    }
    //public static DataTable Get

    /// <summary>
    /// 检测记录是否冲突
    /// </summary>
    /// <param name="newRecord"></param>
    public static bool CheckRecord(DataRow newRecord, DataTable NewedRows)
    {
        string sql = String.Empty;
        sql += @"
select record.* from tbl_DeviceBooking as record 
where (Status = 1 or Status = 2) 
and (Device_ID = @Device_ID) 
and (@Loan_DateTime <= Plan_To_ReDateTime) 
and (@Plan_To_ReDateTime >= Loan_DateTime) ";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@Device_ID", newRecord["s_id"].ToString()));

        DateTime from = Convert.ToDateTime(newRecord["Loan_DateTime"]);
        from = new DateTime(from.Year, from.Month, from.Day);

        DateTime to = Convert.ToDateTime(newRecord["Plan_To_ReDateTime"]);
        to = new DateTime(to.Year, to.Month, to.Day + 1);
        paramss.Add(new SqlParameter("@Loan_DateTime", from));
        paramss.Add(new SqlParameter("@Plan_To_ReDateTime", to));
        try
        {
            DataTable result = SystemDAO.SqlHelperEx.GetTableText(sql, paramss.ToArray())[0];
            if (result != null)
            {
                foreach (DataRow record in result.Rows)
                {
                    var timeList = GetTimeList(record);
                    var newTimeList = GetTimeList(newRecord);
                    foreach (DateTime time in timeList)
                    {
                        if (newTimeList.Contains(time))
                        {
                            return true;
                        }
                    }
                }
                foreach (DataRow record in NewedRows.Rows)
                {
                    var timeList = GetTimeList(record);
                    var newTimeList = GetTimeList(newRecord);
                    foreach (DateTime time in timeList)
                    {
                        if (newTimeList.Contains(time))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        catch
        {
            return true;
        }
    }
    private static List<DateTime> GetTimeList(DataRow record)
    {
        bool isRecurrence = Convert.ToBoolean(record["db_is_recurrence"]);
        DateTime from = Convert.ToDateTime(record["Loan_DateTime"]);
        DateTime to = Convert.ToDateTime(record["Plan_To_ReDateTime"]);
        if (!isRecurrence)
        {
            return GetTimeList(from, to);
        }
        else
        {
            string[] recurrence = record["db_recurrence"].ToString().Split(',');
            TimeSpan start = TimeSpan.Parse(record["db_start"].ToString());
            TimeSpan end = TimeSpan.Parse(record["db_end"].ToString());
            if (start == TimeSpan.Zero && end == TimeSpan.Zero)
            {// 表示allday event
                end = TimeSpan.FromHours(24);
            }
            List<DateTime> timeList = new List<DateTime>();
            foreach (string r in recurrence)
            {
                RegularType regularType = (RegularType)Convert.ToInt32(r);
                switch (regularType)
                {
                    case RegularType.EveryDay:
                        for (DateTime index = from; index <= to; index = index.AddDays(1))
                        {
                            DateTime dt_start = index.Add(start);
                            DateTime dt_end = index.Add(end);
                            timeList.AddRange(GetTimeList(dt_start, dt_end));
                        }
                        break;
                    case RegularType.MONDAY:
                    case RegularType.THURSDAY:
                    case RegularType.TUESDAY:
                    case RegularType.WEDNESDAY:
                    case RegularType.FRIDAY:
                    case RegularType.SATURDAY:
                    case RegularType.SUNDAY:
                        for (DateTime index = from; index <= to; index = index.AddDays(1))
                        {
                            if (index.DayOfWeek.ToString().ToUpper() == regularType.ToString())
                            {
                                DateTime dt_start = index.Add(start);
                                DateTime dt_end = index.Add(end);
                                timeList.AddRange(GetTimeList(dt_start, dt_end));
                            }
                        }
                        break;
                }
            }
            return timeList;
        }
    }
    private static List<DateTime> GetTimeList(DateTime from, DateTime to)
    {
        List<DateTime> timeList = new List<DateTime>();
        for (DateTime index = from; index <= to; index = index.AddMinutes(30))
        {
            timeList.Add(index);
        }
        return timeList;
    }
}