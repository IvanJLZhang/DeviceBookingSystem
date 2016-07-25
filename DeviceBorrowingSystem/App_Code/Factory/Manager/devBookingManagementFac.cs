using BLL;
using Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SystemDAO;

/// <summary>
/// devBookingManagementFac 的摘要说明
/// </summary>
public class devBookingManagementFac
{
    public devBookingManagementFac()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public ReturnResult ApproveBookingSheetByBookigID(tbl_DeviceBooking deviceBooking)
    {
        int borrow_status = checkDeviceStatus(deviceBooking.ID);

        ReturnResult result = new ReturnResult();

        switch ((BorrowStatus)borrow_status)
        {
            case BorrowStatus.Borrow_Out:
                result.returnMsg = Resource.msg_device_status_borrow_out;
                result.returnCode = false;
                break;
            case BorrowStatus.OK:
                {
                    cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
                    result.returnCode = deviceBookingManage.ApproveByID(deviceBooking);
                    result.returnMsg = deviceBookingManage.errMsg;
                    if ((bool)result.returnCode) {
                        UpdateDeviceStatusByBookingID(deviceBooking.ID, 0, this._category.ToString());
                    }
                }
                break;
        }
        return result;
    }
    int _category = 0;
    public int checkDeviceStatus(string bookingid)
    {
        string sql = @"select summary.s_category cate 
from tbl_summary_dev_title summary 
inner join tbl_DeviceBooking record on summary.s_id = record.Device_ID 
where record.Booking_ID = @bookingid ";
        SqlParameter[] param = new SqlParameter[] { new SqlParameter("@bookingid", bookingid) };

        try
        {
            int cat = (int)SqlHelper.ExecuteScalarText(sql, param);
            this._category = cat;
            switch (cat)
            {
                case 1:
                    sql = @"select device.d_status borrow_status 
from tbl_device_detail as device 
inner join tbl_DeviceBooking record on device.d_id = record.Device_ID 
where record.Booking_ID = @bookingid ";
                    return (int)SqlHelper.ExecuteScalarText(sql, param);
                case 2:
                case 3:
                default:
                    return 1;

            }
        }
        catch (Exception ex)
        {
            return 0;
        }
    }


    public ReturnResult GetNoReturnRecordByCurrentBookingID(string currentBookingID, string category)
    {
        ReturnResult result = new ReturnResult();
        string sql = @"SELECT record.Booking_ID, record.Loaner_ID, record.Device_ID, 
record.Project_ID, record.TestCategory_ID, record.PJ_Stage, record.Loan_DateTime, 
record.Plan_To_ReDateTime, record.Real_ReDateTime, record.Status, 
record.Comment, record.Reviewer_ID, record.Date, record.Review_Comment, 
tbl_Project.PJ_Name, Loaner.P_Name, tbl_TestCategory.Name, tbl_Person_1.P_Name AS Reviewer_Name, 
summary.s_name AS Device_Name ";

        if (category == "1")
        {
            sql += @"
, device.d_customid as Custom_ID ";

        }
        else
        {
            sql += @"
, 1 as Custom_ID ";

        }
        sql += @"
from tbl_DeviceBooking record 
INNER JOIN tbl_Person as Loaner ON record.Loaner_ID = Loaner.P_ID 
INNER JOIN tbl_Project ON record.Project_ID = tbl_Project.PJ_Code 
INNER JOIN tbl_TestCategory ON record.TestCategory_ID = tbl_TestCategory.ID 
INNER JOIN tbl_Person AS tbl_Person_1 ON record.Reviewer_ID = tbl_Person_1.P_ID 
INNER JOIN tbl_summary_dev_title summary ON record.Device_ID = summary.s_id 
INNER JOIN tbl_Person as Owner on summary.s_ownerid = Owner.P_ID ";

        if (category == "1")
        {
            sql += @"
INNER JOIN tbl_device_detail as device on record.Device_ID = device.d_id ";
        }
        sql += @"
where (record.Device_ID = (select record.Device_ID from tbl_DeviceBooking record 
where record.Booking_ID = @Booking_ID))
AND (record.Status = 2) ";
        SqlParameter[] param = new SqlParameter[] { new SqlParameter("@Booking_ID", currentBookingID) };
        try
        {
            result.returnCode = SqlHelper.GetTableText(sql, param)[0];
        }
        catch (Exception ex)
        {
            result.returnMsg = ex.Message;
        }
        return result;
    }


    public void UpdateDeviceStatusByBookingID(string currentBookingID, int status, string category) {
        int cat = Int32.Parse(category);
        string sql = String.Empty;
        switch (cat) { 
            case 1:
                sql += @"
update tbl_device_detail set d_status = @status 
where tbl_device_detail.d_id = (select record.Device_ID from tbl_DeviceBooking as record 
where record.Booking_ID = @bookingid) ";
                break;
            case 2:
            case 3:
            default:
                break;
        }
        sql += @"";
        SqlParameter[] param = new SqlParameter[] { new SqlParameter("@status", status), new SqlParameter("@bookingid", currentBookingID) };
        try
        {
            SqlHelper.ExecteNonQueryText(sql, param);
        }
        catch { }
    }
}