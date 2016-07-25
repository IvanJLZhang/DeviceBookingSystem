using BLL;
using GlobalClassNamespace;
using Model;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_ApprovePage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["bookingId"] == null)
            {
                Response.Write("<script>alert('There is no booking item!');window.open('', '_self', ''); window.close();</script>");
                return;
            }
            ShowBookingSheet();
        }
    }

    void ShowBookingSheet() {
        this.SqlDataSource1.SelectCommand = @"SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Device_ID, tbl_Person.P_Name, tbl_Person.P_Department, 
tbl_Person.P_ExNumber, tbl_Project.PJ_Name, tbl_Project.Cust_Name, tbl_DeviceBooking.Loan_DateTime, 
tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Comment, summary.s_name as Device_Name, summary.s_id as Device_ID, 
tbl_Project.PJ_Code, tbl_Person.P_ID, tbl_TestCategory.Name, tbl_DeviceBooking.PJ_Stage, tbl_DeviceBooking.Reviewer_ID, 
tbl_DeviceBooking.Real_ReDateTime AS [Return DT] 
FROM tbl_DeviceBooking 
inner join tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID 
INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code 
INNER JOIN tbl_TestCategory ON tbl_DeviceBooking.TestCategory_ID = tbl_TestCategory.ID 
WHERE (tbl_DeviceBooking.Booking_ID = @Booking_ID)";

        this.DetailsView1.DataKeyNames = new string[] {"Booking_ID", "Device_ID" };
        this.DetailsView1.DataBind();
    }


    protected void btn_Approve_Click(object sender, EventArgs e)
    {
        string bookingId = Request["bookingId"].ToString().Trim();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        deviceBooking.ID = bookingId;
        deviceBooking.Reviewer_ID = Session["UserID"].ToString().Trim();
        deviceBooking.Review_Comment = this.tb_Reason.Text.Trim();
        deviceBooking.Device_ID = this.DetailsView1.DataKey.Values["Device_ID"].ToString();
        devBookingManagementFac booking_factory = new devBookingManagementFac();
        var result = booking_factory.ApproveBookingSheetByBookigID(deviceBooking);
        if ((bool)result.returnCode)
        {
            // approve 成功
            {// Mail Service
                List<string> ids = new List<string>();
                ids.Add(bookingId);
                cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
                DataTable bookingList = bookingList = deviceBookingManage.GetBookingItemsByIDs(ids);
                string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string MailTo = bookingList.Rows[0]["P_Email"].ToString();//Chris_Tsung@Wistron.com;@wistron.local;May_Lai@wistron.com
                string cc = "";//"mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;";//mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;
                string LoanerName = String.Empty;
                if (Session["UserName"] != null)
                    LoanerName = Session["UserName"].ToString();

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
            Response.Write("<script>alert('Approve:" + bookingId + "');window.close();opener.location.href = opener.location.href;</script>");
        }
        else if (result.returnMsg == Resource.msg_device_status_borrow_out)
        {
            string scriptText = @"
<script>
    var ret = confirm('" + result.returnMsg + @"');
    window.close();
    if(ret){
        opener.location.href = '../RecordManagement.aspx?device_id=" + deviceBooking.Device_ID + @"&status=2'
    }
</script>";
            Response.Write(scriptText);
        }
        else
        {
            GlobalClass.PopMsg(this.Page, result.returnMsg);
        }
    }
    protected void btn_Reject_Click(object sender, EventArgs e)
    {
        string bookingId = Request["bookingId"].ToString().Trim();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        deviceBooking.ID = bookingId;
        deviceBooking.Reviewer_ID = Session["UserID"].ToString().Trim();
        deviceBooking.Review_Comment = this.tb_Reason.Text.Trim();

        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        if (deviceBookingManage.RejectByID(deviceBooking))
        {
            //GlobalClass.PopMsg(this.Page, "Rejected: " + bookingId);

            {// Mail Service
                List<string> ids = new List<string>();
                ids.Add(bookingId);
                DataTable bookingList = bookingList = deviceBookingManage.GetBookingItemsByIDs(ids);
                string id = DateTime.Now.ToString("yyyyMMddHHmmssttt");
                string MailTo = bookingList.Rows[0]["P_Email"].ToString();//Chris_Tsung@Wistron.com;@wistron.local;May_Lai@wistron.com
                string cc = "";//"mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;";//mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;
                string LoanerName = String.Empty;
                if (Session["UserName"] != null)
                    LoanerName = Session["UserName"].ToString();

                StringBuilder Subject = new StringBuilder();
                Subject.Append("[Device Borrowing System]--Borrow Reject");

                StringBuilder body = new StringBuilder();
                body.Append("<h2>Borrow Information: </h2><br/><br/>");
                body.Append("<table border='1'><tr>");
                body.Append("<th>Booking ID</th>");
                body.Append("<th>Device Name</th>");
                //body.Append("<th>Loaner Name</th>");
                body.Append("<th>Start DateTime</th>");
                body.Append("<th>End DateTime</th>");
                body.Append("<th>Reject Reason</th>");
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


            Response.Write("<script>alert('Reject:" + bookingId + "');window.close();opener.location.href = opener.location.href;</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");
    }
    public string GetReviewerName(string id)
    {
        cl_PersonManage personManage = new cl_PersonManage();
        var result = personManage.GetPersonInfoByID(id);
        if (result != null && result.Rows.Count > 0)
            return result.Rows[0]["P_Name"].ToString();

        return String.Empty;
    }
    public string GetTimeZoneDateTimeString(string dateTime)
    {
        if (dateTime.CompareTo(String.Empty) != 0)
        {
            DateTime time = DateTime.Parse(dateTime);
            time = time.AddHours(GetClientTimeZone() - 8);

            return time.ToString("yyyy/MM/dd HH:mm");
        }
        return String.Empty;
    }
    private double GetClientTimeZone()
    {
        string personSite = "";
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(userID);
            if (personInfo != null)
            {
                personSite = personInfo.Rows[0]["P_Location"].ToString();
            }
        }
        double clientTimeZone = 0.0;
        if (personSite.CompareTo("WKS") == 0 || personSite.CompareTo("WHC") == 0)
        {
            clientTimeZone = 8.0;
        }
        else
        {
            clientTimeZone = -6.0;
        }

        return clientTimeZone;
    }
}