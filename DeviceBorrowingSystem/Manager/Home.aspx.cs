using BLL;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckCategory();
            CheckDepartment();
        }
        ShowDeviceBookingList();
    }
    DataTable LoginUserInfo
    {
        get
        {
            if (Session["LoginUserInfo"] == null)
                return null;
            else
                return (DataTable)Session["LoginUserInfo"];
        }
    }
    DataTable ChargeOfDptList
    {
        get
        {
            if (ViewState["ChargeOfDptList"] == null)
                return null;
            return (DataTable)ViewState["ChargeOfDptList"];
        }
        set
        {
            ViewState["ChargeOfDptList"] = value;
        }
    }
    private void ShowDeviceBookingList()
    {
        List<string> dptList = new List<string>();
        if (this.ddl_dpt.SelectedValue == "0")
        {
            foreach (DataRow row in ChargeOfDptList.Rows)
            {
                if (row["DptValue"].ToString() != "0")
                {
                    dptList.Add(row["DptValue"].ToString());
                }
            }
        }
        else
        {
            dptList.Add(this.ddl_dpt.SelectedValue);
        }
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        var BookingList = deviceBookingManage.GetNewRequestByCategoryOwnerDpt(this.DropDownList1.SelectedValue, dptList);
        if (BookingList != null)
        {
            //this.GridView1.DataSource = BookingList;
            //this.GridView1.DataKeyNames = new string[] { "Booking_ID" };
            //this.GridView1.DataBind();

            this.gv_requestView.DataSource = BookingList;
            this.gv_requestView.KeyFieldName = "Booking_ID";
            this.gv_requestView.DataBind();

            string catestr = Session["Category"].ToString();
            int cat = Int32.Parse(catestr);
            if (BookingList.Rows.Count > 0 && cat != 1)
            {
                this.btn_Approved.Visible = true;
                this.btn_Reject.Visible = true;
            }
            else
            {
                this.btn_Approved.Visible = false;
                this.btn_Reject.Visible = false;

            }
        }
        else
        {
            this.btn_Approved.Visible = false;
            this.btn_Reject.Visible = false;
        }
    }

    private void CheckCategory()
    {
        if (Session["Category"] != null && Session["Category"].ToString() != "")
        {
            this.DropDownList1.SelectedValue = Session["Category"].ToString();
            //int m_role = Int32.Parse(Session["Role"].ToString());
            //if (m_role / 10 <= 1)
            //{// 如果是非admin账户需要禁用Category选项
            //    this.DropDownList1.Enabled = false;
            //}
        }
        else
        {
            Session["Category"] = 1;
        }
    }

    private void CheckDepartment()
    {
        if (LoginUserInfo != null)
        {
            cl_PersonManage personManage = new cl_PersonManage();
            DataTable dptList = personManage.GetChargeOfDptListByUserInfo(LoginUserInfo);
            ChargeOfDptList = dptList;
            this.ddl_dpt.DataSource = dptList.DefaultView;
            this.ddl_dpt.DataTextField = "DptName";
            this.ddl_dpt.DataValueField = "DptValue";
            this.ddl_dpt.DataBind();

            int role = Convert.ToInt32(LoginUserInfo.Rows[0]["P_Role"]);
            if (role / 10 >= 2)
            {
                this.ddl_dpt.SelectedValue = "0";
            }
            else
            {
                this.ddl_dpt.SelectedValue = LoginUserInfo.Rows[0]["P_Department"].ToString();
            }
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("Approve") == 0)
        {
            string bookingId = e.CommandArgument.ToString();
            string scriptText = "window.open('./DeviceBooking/ApprovePage.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes, left=100px' );";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>window.open('ApprovePage.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes, left=100px' );</script>");
        }
        if (e.CommandName.CompareTo("DeviceDetail") == 0)
        {
            string deviceId = e.CommandArgument.ToString();
            string scriptText = "var obj = window.showModalDialog('./Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            //string scriptText = "window.open('./Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'width=580px, height=500px, top=100px, left=100px'); ";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>window.open('../User/Detail.aspx?id=" + deviceId + "', '', 'width=580px, height=500px, top=100px, left=100px'); </script>");
        }
        if (e.CommandName.CompareTo("UserDetail") == 0)
        {
            string id = e.CommandArgument.ToString().Trim();
            string scriptText = "window.showModalDialog('./Person/UserInfo.aspx?uid=" + id + "&edit=false','', '');";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        }

        if (e.CommandName.CompareTo("Modify") == 0)
        {
            string id = e.CommandArgument.ToString();
            string scriptText = "var obj = window.showModalDialog('./DeviceBooking/ModifyDeviceBooking.aspx?id=" + id + "','', 'dialogWidth=750px; resizable=yes'); if(obj != null && obj == 'OK') window.location.href = window.location.href";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Category"] = this.DropDownList1.SelectedValue;
        Response.Redirect(Request.Url.ToString());
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        if (DropDownList1.SelectedValue.CompareTo("1") == 0)
        {
            this.SqlDataSource1.SelectCommand = @"SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, 
tbl_DeviceBooking.Device_ID, tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, 
tbl_DeviceBooking.Comment, tbl_Person.P_Name, tbl_Person.P_ID, summary.s_name as Device_Name, tbl_Project.PJ_Name 
FROM tbl_DeviceBooking 
INNER JOIN tbl_Device ON tbl_DeviceBooking.Device_ID = tbl_Device.Device_ID 
INNER JOIN tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID 
INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code 
WHERE (tbl_DeviceBooking.Status = 1) AND (tbl_DeviceBooking.Booking_ID = '" + this.tb_bookingId.Text.Trim() + "') AND (summary.s_category = 1)";
        }
        else if (DropDownList1.SelectedValue.CompareTo("2") == 0)
        {
            this.SqlDataSource1.SelectCommand = @"SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, tbl_DeviceBooking.Device_ID, tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Comment, tbl_Person.P_Name, tbl_Person.P_ID, summary.s_name as Device_Name, tbl_Project.PJ_Name 
FROM tbl_DeviceBooking 
INNER JOIN tbl_Device ON tbl_DeviceBooking.Device_ID = tbl_Device.Device_ID 
INNER JOIN tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID 
INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code 
WHERE (tbl_DeviceBooking.Status = 1) AND (tbl_DeviceBooking.Booking_ID = '" + this.tb_bookingId.Text.Trim() + "') AND (summary.s_category = 2)";
        }
        else if (DropDownList1.SelectedValue.CompareTo("3") == 0)
        {
            this.SqlDataSource1.SelectCommand = @"SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, tbl_DeviceBooking.Device_ID, tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Comment, tbl_Person.P_Name, tbl_Person.P_ID, summary.s_name as Device_Name, tbl_Project.PJ_Name 
FROM tbl_DeviceBooking 
INNER JOIN tbl_Device ON tbl_DeviceBooking.Device_ID = tbl_Device.Device_ID 
INNER JOIN tbl_summary_dev_title as summary on tbl_DeviceBooking.Device_ID = summary.s_id 
INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID 
INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code 
WHERE (tbl_DeviceBooking.Status = 1) AND (tbl_DeviceBooking.Booking_ID = '" + this.tb_bookingId.Text.Trim() + "') AND (summary.s_category = 3)";
        }

        //this.GridView1.DataSourceID = this.SqlDataSource1.ID;
        //this.GridView1.DataBind();
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
    protected void ddl_dpt_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDeviceBookingList();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //this.GridView1.PageIndex = e.NewPageIndex;
        ShowDeviceBookingList();
    }
    protected void btn_Approved_Click(object sender, EventArgs e)
    {
        var selectedId = this.gv_requestView.GetSelectedFieldValues("Booking_ID");
        string scriptText = String.Empty;
        foreach (string id in selectedId) {
            scriptText += id + "$";
        }
        scriptText = scriptText.Substring(0, scriptText.Length - 1);
        scriptText = "var obj = window.showModalDialog('./DeviceBooking/FillReason.aspx?type=Approve&id=" + scriptText + "','', 'dialogWidth=750px; resizable=yes'); if(obj != null && obj == 'OK') window.location.href = window.location.href";
        ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        //string scriptText = String.Empty;
        //foreach (GridViewRow row in this.GridView1.Rows)
        //{
        //    CheckBox cb = row.FindControl("cb_Select") as CheckBox;
        //    if (cb.Checked)
        //    {
        //        scriptText += this.GridView1.DataKeys[row.RowIndex].Value.ToString() + "$";
        //    }
        //}
        //if (scriptText == String.Empty)
        //{
        //    string temp = "alert('Please select almost one booking item.')";
        //    ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", temp, true);
        //    return;
        //}
        //scriptText = scriptText.Substring(0, scriptText.Length - 1);
        //scriptText = "var obj = window.showModalDialog('./DeviceBooking/FillReason.aspx?type=Approve&id=" + scriptText + "','', 'dialogWidth=750px; resizable=yes'); if(obj != null && obj == 'OK') window.location.href = window.location.href";
        //ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
    }
    protected void btn_Reject_Click(object sender, EventArgs e)
    {
        var selectedId = this.gv_requestView.GetSelectedFieldValues("Booking_ID");
        string scriptText = String.Empty;
        foreach (string id in selectedId)
        {
            scriptText += id + "$";
        }
        scriptText = scriptText.Substring(0, scriptText.Length - 1);
        scriptText = "var obj = window.showModalDialog('./DeviceBooking/FillReason.aspx?type=Reject&id=" + scriptText + "','', 'dialogWidth=750px; resizable=yes'); if(obj != null && obj == 'OK') window.location.href = window.location.href";
        ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        //string scriptText = String.Empty;
        //foreach (GridViewRow row in this.GridView1.Rows)
        //{
        //    CheckBox cb = row.FindControl("cb_Select") as CheckBox;
        //    if (cb.Checked)
        //    {
        //        scriptText += this.GridView1.DataKeys[row.RowIndex].Value.ToString() + "$";
        //    }
        //}
        //if (scriptText == String.Empty)
        //{
        //    string temp = "alert('Please select almost one booking item.')";
        //    ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", temp, true);
        //    return;
        //}
        //scriptText = scriptText.Substring(0, scriptText.Length - 1);
        //scriptText = "var obj = window.showModalDialog('./DeviceBooking/FillReason.aspx?type=Reject&id=" + scriptText + "','', 'dialogWidth=750px; resizable=yes'); if(obj != null && obj == 'OK') window.location.href = window.location.href";
        //ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        CheckBookingStatus();
    }

    private void CheckBookingStatus()
    {
        var customidField = this.gv_requestView.AllColumns[2];
        if (this.DropDownList1.SelectedValue == "1")
        {
            customidField.Visible = true;
        }
        else
        {
            customidField.Visible = false;
        }

        //for (int index = 0; index != GridView1.Rows.Count; index++)
        //{
        //    System.Web.UI.WebControls.Label lbl_customid = (System.Web.UI.WebControls.Label)GridView1.Rows[index].FindControl("lbl_customid");
        //    DataControlField customidField = GridView1.Columns[2];
        //    if (this.DropDownList1.SelectedValue == "1")
        //    {
        //        customidField.HeaderStyle.Width = 80;
        //        customidField.HeaderText = "Custom_ID";
        //    }
        //    else
        //    {

        //        customidField.HeaderStyle.Width = 0;
        //        customidField.HeaderText = "";
        //        customidField.HeaderStyle.BorderStyle = BorderStyle.None;
        //        customidField.ItemStyle.BorderStyle = BorderStyle.None;
        //        lbl_customid.Text = "";
        //    }
        //}
    }
    protected void gv_requestView_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
    {
        if (e.CommandArgs.CommandName.CompareTo("Approve") == 0)
        {
            string bookingId = e.CommandArgs.CommandArgument.ToString();
            string scriptText = "window.open('./DeviceBooking/ApprovePage.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes, left=100px' );";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>window.open('ApprovePage.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes, left=100px' );</script>");
        }
        if (e.CommandArgs.CommandName.CompareTo("DeviceDetail") == 0)
        {
            string deviceId = e.CommandArgs.CommandArgument.ToString();
            string scriptText = "var obj = window.showModalDialog('./Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            //string scriptText = "window.open('./Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'width=580px, height=500px, top=100px, left=100px'); ";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>window.open('../User/Detail.aspx?id=" + deviceId + "', '', 'width=580px, height=500px, top=100px, left=100px'); </script>");
        }
        if (e.CommandArgs.CommandName.CompareTo("UserDetail") == 0)
        {
            string id = e.CommandArgs.CommandArgument.ToString().Trim();
            string scriptText = "window.showModalDialog('./Person/UserInfo.aspx?uid=" + id + "&edit=false','', '');";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        }

        if (e.CommandArgs.CommandName.CompareTo("Modify") == 0)
        {
            string id = e.CommandArgs.CommandArgument.ToString();
            string scriptText = "var obj = window.showModalDialog('./DeviceBooking/ModifyDeviceBooking.aspx?id=" + id + "','', 'dialogWidth=750px; resizable=yes'); if(obj != null && obj == 'OK') window.location.href = window.location.href";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        }
    }
    protected void gv_requestView_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var intID = Convert.ToString(gv_requestView.GetRowValues(gv_requestView.FocusedRowIndex, "Booking_ID"));
        var view = sender as ASPxGridView;
        view.JSProperties["cpType"] = e.ButtonID;
        view.JSProperties["cpBookingId"] = intID;
    }
}