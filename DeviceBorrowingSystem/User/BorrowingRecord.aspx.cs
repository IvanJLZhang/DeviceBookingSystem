using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_BorrowingRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public string GetStatusStr(int status)
    {
        switch (status)
        {
            case 0:
                return "Un-submit";
            case 1:
                return "Submit";
            case 2:
                return "Approved";
            case -1:
                return "Rejected";
            default:
                return "";
        }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        if (this.tb_bookingId.Text.Trim().CompareTo(String.Empty) == 0)
            return;
        this.SqlDataSource1.SelectCommand = @"SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, 
tbl_DeviceBooking.Device_ID, tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.TestCategory_ID, 
tbl_DeviceBooking.PJ_Stage, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, 
tbl_DeviceBooking.Real_ReDateTime, tbl_DeviceBooking.Status, tbl_DeviceBooking.Comment, tbl_DeviceBooking.Reviewer_ID, 
tbl_DeviceBooking.Date, tbl_DeviceBooking.Review_Comment, tbl_Device.s_name as Device_Name, tbl_Person.P_Name 
FROM tbl_DeviceBooking 
INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID 
INNER JOIN tbl_summary_dev_title as tbl_Device ON tbl_DeviceBooking.Device_ID = tbl_Device.s_id  
WHERE (tbl_Device.s_category = @Category) 
AND (tbl_Person.P_ID = @UserID) 
AND (tbl_DeviceBooking.Booking_ID = '" + tb_bookingId.Text + "')";
        this.GridView1.DataSourceID = this.SqlDataSource1.ID;
        this.GridView1.DataKeyNames = new string[1] { "Booking_ID" };
        this.GridView1.DataBind();
    }
    protected void btn_ExportExcel_Click(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("Approve") == 0)
        {
            GridViewRow row = ((GridViewRow)(((LinkButton)(e.CommandSource)).Parent.Parent));
            Label lbl = (Label)row.FindControl("lbl_Status");
            string bookingId = e.CommandArgument.ToString();
            string scriptText = String.Empty;
            if (lbl != null && lbl.Text.CompareTo("Un-submit") == 0)
            {
                scriptText = "window.open('FillBorrowingInfo.aspx?device_id=" + bookingId + "$" + "', '', 'width=730px, scrollbars=yes, top=100px, left=100px');";
            }
            else
            {
                scriptText = "window.open('../Manager/DeviceBooking/ApprovePage_View.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes' );";
            }
            ScriptManager.RegisterStartupScript(this.upd_Main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>window.open('ApprovePage_View.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes' );</script>");
        }
        if (e.CommandName.CompareTo("DeviceDetail") == 0)
        {
            //string deviceId = e.CommandArgument.ToString();
            //string scriptText = "window.open('../Manager/Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'width=580px, height=500px'); ";
            //ScriptManager.RegisterStartupScript(this.upd_Main, this.GetType(), "click", scriptText, true);

            string scriptText = "var obj = window.showModalDialog('../Manager/Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            //string scriptText = "window.open('../Manager/Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'width=580px, height=500px, top=100px, left=100px, scrollbars=yes');";
            ScriptManager.RegisterStartupScript(this.upd_Main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>window.open('../User/Detail.aspx?id=" + deviceId + "', '', 'width=580px, height=500px'); </script>");
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void GridView1_DataBinding(object sender, EventArgs e)
    {

    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        CheckGridViewStatus();
    }

    private void CheckGridViewStatus()
    {
        for (int index = 0; index != this.GridView1.Rows.Count; index++)
        {
            GridViewRow row = this.GridView1.Rows[index];
            LinkButton lbtn = (LinkButton)row.FindControl("lbtn_Delete");
            Label lbl = (Label)row.FindControl("lbl_Status");
            if (lbl != null && lbl.Text.CompareTo("Approved") == 0)
            {
                if (lbtn != null)
                    lbtn.Enabled = false;
            }
        }
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