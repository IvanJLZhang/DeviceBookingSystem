using BLL;
using DataBaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// BookingPageFactory 的摘要说明
/// </summary>
public abstract class BookingPageFactory
{
    public BookingPageFactory()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //


    }

    public static DataTable GetDeviceTable(List<string> idList)
    {
        if (idList == null || idList.Count <= 0)
            return null;

        string sql = @"
select base.s_id, base.s_name 
from tbl_summary_dev_title as base 
where base.s_id in ( ";

        foreach (var id in idList)
        {
            sql += "'" + id + "',";
        }

        sql = sql.Substring(0, sql.Length - 1);
        sql += @"
) ";
        try
        {
            return SystemDAO.SqlHelperEx.GetTableText(sql, null)[0];
        }
        catch
        {
            return null;
        }
    }

    public static string GenNewBookingID(string device_class)
    {
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();

        string serialNumber = deviceBookingManage.GetMaxBookingID(device_class);
        if (serialNumber != "0" && serialNumber != String.Empty)
        {
            string IDType = serialNumber.Substring(0, 2);
            string headDate = serialNumber.Substring(2, 8);
            int lastNumber = int.Parse(serialNumber.Substring(10));

            if (headDate == DateTime.Now.ToString("yyyyMMdd"))
            {
                lastNumber++;
                return IDType + headDate + lastNumber.ToString("0000");
            }
        }
        return device_class + DateTime.Now.ToString("yyyyMMdd") + "0001";
    }


    public static int NewBookingRecord(tbl_DeviceBooking booking)
    {

        string sql = @"
insert into tbl_DeviceBooking (Booking_ID, Loaner_ID, Device_ID, Project_ID, TestCategory_ID, PJ_Stage, Loan_DateTime, Plan_To_ReDateTime
, db_is_recurrence, db_recurrence, db_start, db_end, Real_ReDateTime, Status, Comment, Reviewer_ID, Review_Comment, Date) 
Values(@Booking_ID, @Loaner_ID, @Device_ID, @Project_ID, @TestCategory_ID, @PJ_Stage, @Loan_DateTime, @Plan_To_ReDateTime
, @db_is_recurrence, @db_recurrence, @db_start, @db_end, @Real_ReDateTime, @Status, @Comment, @Reviewer_ID, @Review_Comment, GetDate()) ";

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

    public static DataTable GetBookingItemsByIDs(List<string> ids)
    {
        StringBuilder cmdText = new StringBuilder();
        cmdText = cmdText.Remove(0, cmdText.Length);
        cmdText.Append("SELECT tbl_DeviceBooking.*, tbl_Person.P_Name, tbl_Person.P_Email, summary.s_name as Device_Name, summary.s_category , summary.s_ownerid as Owner_ID FROM tbl_DeviceBooking ");
        cmdText.Append("INNER JOIN tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id ");
        cmdText.Append("INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID ");
        cmdText.Append("Where Booking_ID in ( ");
        for (int index = 0; index != ids.Count; index++)
        {
            cmdText.Append("'" + ids[index] + "', ");
        }
        cmdText.Remove(cmdText.Length - 2, 2);
        cmdText.Append(")");

        DataTableCollection tables = null;
        try
        {
            tables = SystemDAO.SqlHelperEx.GetTableText(cmdText.ToString(), null);
            return tables[0];
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}