using BLL;
using GlobalClassNamespace;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_DeviceBooking_FillReason : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["type"] != null && Request["id"] != null)
            {
                this.btn_ApproveOrReject.Text = Request["type"].ToString();
            }
            else
            {
                Response.Write("<script>window.open('', '_self', ''); window.close();</script>");
            }
        }

    }
    protected void btn_ApproveOrReject_Click(object sender, EventArgs e)
    {
        if (Request["id"] == null)
            return;
        string[] idArr = Request["id"].ToString().Split('$');
        foreach (var id in idArr)
        {
            if (Request["type"].CompareTo("Approve") == 0)
                Approve(id, this.tb_Reason.Text.Trim());
            else
                Reject(id, this.tb_Reason.Text.Trim());
        }
        Response.Write("<script>window.returnValue = 'OK';window.close();</script>");
    }
    private void Approve(string id, string reason)
    {
        string bookingId = id;
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        deviceBooking.ID = bookingId;
        deviceBooking.Reviewer_ID = Session["UserID"].ToString().Trim();
        deviceBooking.Review_Comment = reason;
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        if (deviceBookingManage.ApproveByID(deviceBooking))
        {
            //GlobalClass.PopMsg(this.Page, "Approved: " + bookingId);

            {// Mail Service
                List<string> ids = new List<string>();
                ids.Add(bookingId);
                DataTable bookingList = bookingList = deviceBookingManage.GetBookingItemsByIDs(ids);
                string MailID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
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

                MailService.AddOneMailService(MailID, MailTo.ToString(), cc.ToString(), Subject.ToString(), body.ToString(), true, "");
            }

            //Response.Write("<script>alert('Approve:" + bookingId + "');window.close();opener.location.href = opener.location.href;</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
        }
    }
    private void Reject(string id, string reaseon)
    {
        string bookingId = id;
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        deviceBooking.ID = bookingId;
        deviceBooking.Reviewer_ID = Session["UserID"].ToString().Trim();
        deviceBooking.Review_Comment = reaseon;

        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        if (deviceBookingManage.RejectByID(deviceBooking))
        {
            //GlobalClass.PopMsg(this.Page, "Rejected: " + bookingId);
            {// Mail Service
                List<string> ids = new List<string>();
                ids.Add(bookingId);
                DataTable bookingList = bookingList = deviceBookingManage.GetBookingItemsByIDs(ids);
                string MailID = DateTime.Now.ToString("yyyyMMddHHmmssttt");
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

                MailService.AddOneMailService(MailID, MailTo.ToString(), cc.ToString(), Subject.ToString(), body.ToString(), true, "");
            }
            //Response.Write("<script>alert('Reject:" + bookingId + "');window.close();opener.location.href = opener.location.href;</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
        }
    }
}