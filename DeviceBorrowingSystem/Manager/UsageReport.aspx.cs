using BLL;
using Microsoft.Office.Interop.Excel;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Manager_UsageReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckCategory();
            InitFilterParams();
            DoFilter();
        }
    }
    private void CheckCategory()
    {
        if (Session["Category"] != null)
        {
            this.DropDownList1.SelectedValue = Session["Category"].ToString();
        }
    }
    protected List<string> SelectedItems
    {
        get { return ViewState["selecteditems"] != null ? (List<string>)ViewState["selecteditems"] : null; }
        set { ViewState["selecteditems"] = value; }
    }
    private void GetSelectedItem()
    {
        List<string> selecteditems = null;
        if (this.SelectedItems == null)
            selecteditems = new List<string>();
        else
            selecteditems = this.SelectedItems;

        // 获取选中的记录
        for (int index = 0; index != this.GridView1.Rows.Count; index++)
        {
            System.Web.UI.WebControls.CheckBox cbx = (System.Web.UI.WebControls.CheckBox)this.GridView1.Rows[index].FindControl("chkSelect");
            string id = this.GridView1.DataKeys[index].Value.ToString();

            if (selecteditems.Contains(id) && !cbx.Checked)
            {
                selecteditems.Remove(id);
            }
            if (!selecteditems.Contains(id) && cbx.Checked)
            {
                selecteditems.Add(id);
            }
        }

        this.SelectedItems = selecteditems;
    }

    int CurrentPage {
        get {
            if (Session["page_report"] == null)
            {
                return 0;

            }
            else {
                return Convert.ToInt32(Session["page_report"]);
            
            }
        }
        set {
            Session["page_report"] = value;
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("Approve") == 0)
        {
            string bookingId = e.CommandArgument.ToString();
            string scriptText = "window.open('./DeviceBooking/ApprovePage_View.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes' );";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script></script>");
        }
        if (e.CommandName.CompareTo("DeviceDetail") == 0)
        {
            string deviceId = e.CommandArgument.ToString();
            //Response.Write("<script>var obj = window.showModalDialog('./Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;</script>");
            //Response.Write("<script>window.open('Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'width=580px, height=500px'); </script>");

            string scriptText = "var obj = window.showModalDialog('./Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            //string scriptText = "window.open('./Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'width=580px, height=500px, top=100px, left=100px'); ";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        }
        if (e.CommandName.CompareTo("Return") == 0)
        {
            string booking_id = e.CommandArgument.ToString();
            //Response.Write("<script>window.showModalDialog('OpenPage/ReturnDevice.aspx?bookingid=" + booking_id + "', '', 'dialogHeight = 300px; dialogWeight = 200px;');</script>");
            string scriptText = "window.showModalDialog('OpenPage/ReturnDevice.aspx?bookingid=" + booking_id + "', '', 'dialogHeight = 300px;');window.location.reload();";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        }

        if (e.CommandName.CompareTo("Modify") == 0)
        {
            string id = e.CommandArgument.ToString();
            string scriptText = "var obj = window.showModalDialog('./DeviceBooking/ModifyDeviceBooking.aspx?id=" + id + "','', 'dialogWidth=750px; resizable=yes'); if(obj != null && obj == 'OK') window.location.href = window.location.href";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
        }

        if (e.CommandName.Equals("DeleteRecord", StringComparison.CurrentCultureIgnoreCase))
        {
            string id = e.CommandArgument.ToString();
            cl_DeviceBookingManage bookingMana = new cl_DeviceBookingManage();
            string scroptText = String.Empty;
            if (bookingMana.DeleteBookingItemByID(id))
            {
                scroptText = "alert('delete the{ " + id + "} record successfully!'); window.location.reload();";
            }
            else {

                scroptText = "alert('sorry, something is wrong when delete the record, error message: " + bookingMana.errMsg + "');";
            }
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scroptText, true);
        }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        DoFilter();
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.SelectedItems != null)
        {
            this.SelectedItems.Clear();
            this.SelectedItems = null;
        }
        Session["Category"] = this.DropDownList1.SelectedValue;
        Response.Redirect(Request.Url.ToString()); 
    }
    protected void btn_ExportExcel_Click(object sender, EventArgs e)
    {
        //string scriptText = "window.open('ExportPage.aspx?cat=" + this.DropDownList1.SelectedItem.Text + "', '', 'width=800px; height=550px;');";
        string scriptText = "window.open('BookingRecord/ExportBookingRecord.aspx?cat=" + this.DropDownList1.SelectedItem.Value + "', '', 'width=800px; height=550px;');";
        ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1 && this.SelectedItems != null)
        {
            string id = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
            System.Web.UI.WebControls.CheckBox cbx = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("chkSelect");
            if (this.SelectedItems.Contains(id))
            {
                cbx.Checked = true;
            }
            else
                cbx.Checked = false;
        }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex;
        DoFilter();
        GetSelectedItem();
    }
    protected void GridView1_DataBounding(object sender, EventArgs e)
    {
        GetSelectedItem();
    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        CheckBookingStatus();
    }

    private void CheckBookingStatus()
    {
        for (int index = 0; index != GridView1.Rows.Count; index++)
        {
            string booking_id = GridView1.DataKeys[index].Value.ToString();
            cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();

            System.Data.DataTable deviceBookingInfo = deviceBookingManage.GetBookingItemsByIDs(new List<string>() { booking_id });
            if (deviceBookingInfo != null)
            {
                int status = Convert.ToInt32(deviceBookingInfo.Rows[0]["Status"].ToString());
                if (status > 2)
                {
                    ImageButton lbtn = (ImageButton)GridView1.Rows[index].FindControl("lbtn_Return");
                    ImageButton lbtn1 = (ImageButton)GridView1.Rows[index].FindControl("img_Modify");
                    lbtn.Enabled = false;
                    lbtn.BackColor = System.Drawing.Color.DarkGray;

                    lbtn1.Enabled = false;
                    lbtn1.BackColor = System.Drawing.Color.DarkGray;
                }
            }
            ImageButton lbtn2 = (ImageButton)GridView1.Rows[index].FindControl("ibtn_Delete");
            if (Session["Role"] != null)
            {
                int role = Convert.ToInt32(Session["Role"]);

                lbtn2.Visible = (role / 10) >= 2 ? true : false;
            }
            else {
                lbtn2.Visible = false;
            }
            if (lbtn2.Visible) {
                lbtn2.Attributes.Add("onclick", "javascript:return confirm('Are you sure to delete the record?');");
            }

            System.Web.UI.WebControls.Label lbl_customid = (System.Web.UI.WebControls.Label)GridView1.Rows[index].FindControl("lbl_customid");
            DataControlField customidField = GridView1.Columns[3];
            if (this.DropDownList1.SelectedValue == "1")
            {
                customidField.HeaderStyle.Width = 80;
                customidField.HeaderText = "Custom_ID";
            }
            else
            {

                customidField.HeaderStyle.Width = 0;
                customidField.HeaderText = "";
                customidField.HeaderStyle.BorderStyle = BorderStyle.None;
                customidField.ItemStyle.BorderStyle = BorderStyle.None;
                lbl_customid.Text = "";
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
            System.Data.DataTable personInfo = person.GetPersonInfoByID(userID);
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
    protected void ddl_pagesize_SelectedIndexChanged(object sender, EventArgs e)
    {
        DoFilter();
    }
    protected void ddl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        DoFilter();
    }


    void DoFilter()
    {
        if (Request["currentid"] == null)
        {
            #region 处理filter事件
            // filter-Status
            int status = Convert.ToInt32(this.ddl_status.SelectedValue);

            string sqlCommand = @"SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, tbl_DeviceBooking.Device_ID, 
tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.TestCategory_ID, tbl_DeviceBooking.PJ_Stage, tbl_DeviceBooking.Loan_DateTime, 
tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Real_ReDateTime, tbl_DeviceBooking.Status, 
tbl_DeviceBooking.Comment, tbl_DeviceBooking.Reviewer_ID, tbl_DeviceBooking.Date, tbl_DeviceBooking.Review_Comment, 
tbl_Project.PJ_Name, Loaner.P_Name, tbl_TestCategory.Name, tbl_Person_1.P_Name AS Reviewer_Name, 
summary.s_name AS Device_Name ";

            if (this.DropDownList1.SelectedValue == "1")
            {
                sqlCommand += @"
, device.d_customid as Custom_ID ";

            }
            else
            {
                sqlCommand += @"
, 1 as Custom_ID ";

            }
            sqlCommand += @"
FROM tbl_DeviceBooking 
INNER JOIN tbl_Person as Loaner ON tbl_DeviceBooking.Loaner_ID = Loaner.P_ID 
INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code 
INNER JOIN tbl_TestCategory ON tbl_DeviceBooking.TestCategory_ID = tbl_TestCategory.ID 
INNER JOIN tbl_Person AS tbl_Person_1 ON tbl_DeviceBooking.Reviewer_ID = tbl_Person_1.P_ID 
INNER JOIN tbl_summary_dev_title summary ON tbl_DeviceBooking.Device_ID = summary.s_id 
INNER JOIN tbl_Person as Owner on summary.s_ownerid = Owner.P_ID ";


            if (this.DropDownList1.SelectedValue == "1")
            {
                sqlCommand += @"
INNER JOIN tbl_device_detail as device on tbl_DeviceBooking.Device_ID = device.d_id ";
            }

            sqlCommand += "WHERE (summary.s_category = @s_category) ";

            switch (status)
            {
                case 2:
                    sqlCommand += @"
AND (tbl_DeviceBooking.Status = 2) ";
                    break;
                case 3:
                    sqlCommand += @"
AND (tbl_DeviceBooking.Status = 3) ";
                    break;
                default:
                    sqlCommand += @"
AND (tbl_DeviceBooking.Status >= 2) ";
                    break;
            }

            // filter- class/interface
            if (this.DropDownList1.SelectedValue == "1")
            {
                if (this.ddl_class.SelectedValue != "ALL")
                {
                    sqlCommand += @"
AND (device.d_class = '" + this.ddl_class.SelectedValue + "') ";
                }
                if (this.ddl_interface.SelectedValue != "ALL")
                {
                    sqlCommand += @"
AND (device.d_interface = '" + this.ddl_interface.SelectedValue + "')";
                }
            }

            // filter search text 
            if (this.tb_searchText.Text.Trim() != String.Empty)
            {
                string search = this.tb_searchText.Text.Trim();
                string searchLikeText = "'%" + search + "%'";
                string searchEqulText = "'" + search + "'";
                sqlCommand += @"
AND((tbl_DeviceBooking.Booking_ID LIKE " + searchLikeText + @") 
OR (summary.s_name LIKE " + searchLikeText + @") 
OR (Owner.P_ID = " + searchEqulText + @") 
OR (Owner.P_Name = " + searchEqulText + @") 
OR (Loaner.P_ID = " + searchEqulText + @") 
OR (Loaner.P_Name = " + searchEqulText + @") ";

                if (this.DropDownList1.SelectedValue == "1")
                {
                    sqlCommand += @"
OR (device.d_customid = " + searchEqulText + @") ";
                }

                sqlCommand += ")";
            }

            this.SqlDataSource1.SelectCommand = sqlCommand;
            // filter- page size
            this.GridView1.PageSize = Convert.ToInt32(this.ddl_pagesize.SelectedValue);
            this.GridView1.DataSourceID = this.SqlDataSource1.ID;
            this.GridView1.DataKeyNames = new string[] { "Booking_ID" };
            this.GridView1.DataBind();

            this.GridView1.PageIndex = this.CurrentPage;
            this.tb_searchText.Text = "";
            #endregion
        }
        else {// 处理单独请求事件
            string currentBookingID = Request["currentid"].ToString();
            devBookingManagementFac record_factory = new devBookingManagementFac();
            var result = record_factory.GetNoReturnRecordByCurrentBookingID(currentBookingID, this.DropDownList1.SelectedValue);
            if (result.returnCode != null)
            {
                System.Data.DataTable records = (System.Data.DataTable)result.returnCode;
                if (records.Rows.Count <= 0)
                {// 若没有以前的未还记录， 则修改device状态
                    record_factory.UpdateDeviceStatusByBookingID(currentBookingID, 1, this.DropDownList1.SelectedValue);
                    Response.Redirect("~/Manager/UsageReport.aspx");
                }
                else
                {
                    this.GridView1.PageSize = Convert.ToInt32(this.ddl_pagesize.SelectedValue);
                    this.GridView1.DataSource = records.DefaultView;
                    this.GridView1.DataKeyNames = new string[] { "Booking_ID" };
                    this.GridView1.DataBind();
                    this.GridView1.PageIndex = 0;
                }
            }
            else {
                Response.Redirect("~/Manager/UsageReport.aspx");
            }
        }
    }


    void InitFilterParams()
    {
        this.ddl_pagesize.SelectedIndex = 0;
        this.ddl_status.SelectedIndex = 1;

        if (this.DropDownList1.SelectedValue == "1")
        {
            this.pnl_deviceFilter.Visible = true;
            cl_DeviceBookingManage bookingMana = new cl_DeviceBookingManage();
            var dclass = bookingMana.GetDeviceClassViaBookingRecord();
            this.ddl_class.DataSource = dclass.DefaultView;
            this.ddl_class.DataTextField = "class";
            this.ddl_class.SelectedValue = "ALL";
            this.ddl_class.DataBind();

            var dinterface = bookingMana.GetDeviceInterfaceViaBookingRecord();
            this.ddl_interface.DataSource = dinterface.DefaultView;
            this.ddl_interface.DataTextField = "interface";
            this.ddl_interface.SelectedValue = "ALL";
            this.ddl_interface.DataBind();
        }
        else
        {
            this.pnl_deviceFilter.Visible = false;
        }
    }

    /// <summary>
    /// 添加一条借用纪录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void linkbutton_Click(object sender, ImageClickEventArgs e)
    {
        string scriptText = @"window.open('./DeviceBooking/AddRecord.aspx');";
        ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
    }
    /// <summary>
    /// 通过文件批量上传
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgb_AddFromFile_Click(object sender, ImageClickEventArgs e)
    {

    }
}