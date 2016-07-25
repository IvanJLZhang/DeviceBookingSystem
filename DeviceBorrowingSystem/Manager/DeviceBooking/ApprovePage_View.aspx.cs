using BLL;
using GlobalClassNamespace;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        }
    }
    protected void btn_Approve_Click(object sender, EventArgs e)
    {
        string bookingId = Request["bookingId"].ToString().Trim();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        deviceBooking.ID = bookingId;
        if (Session["UserID"] != null)
            deviceBooking.Reviewer_ID = Session["UserID"].ToString().Trim();

        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        if (deviceBookingManage.ApproveByID(deviceBooking))
        {
            GlobalClass.PopMsg(this.Page, "Approved: " + bookingId);
            Response.Write("<script>window.close();opener.location.href = opener.location.href;</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
        }
    }
    protected void btn_Reject_Click(object sender, EventArgs e)
    {
        string bookingId = Request["bookingId"].ToString().Trim();
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        deviceBooking.ID = bookingId;
        if (Session["UserID"] != null)
            deviceBooking.Reviewer_ID = Session["UserID"].ToString().Trim();
        //deviceBooking.Review_Comment = this.tb_Reason.Text.Trim();

        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        if (deviceBookingManage.RejectByID(deviceBooking))
        {
            GlobalClass.PopMsg(this.Page, "Rejected: " + bookingId);
            Response.Write("<script>window.close();opener.location.href = opener.location.href;</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
        }
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");//opener.location.href = opener.location.href;
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