using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_RecordManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckCategory();
            ShowRecord();
        }
    }
    void ShowRecord()
    {
        if (Request["device_id"] != null)
        {
            this.devViewFrame.Attributes["src"] = "./BookingRecord/RecordView.aspx?type=1&device_id=" + Request["device_id"] + "&status=" + Request["status"];
        }
        else
        {
            this.devViewFrame.Attributes["src"] = "./BookingRecord/RecordView.aspx?type=1";
        }
    }


    private void CheckCategory()
    {
        if (Session["Category"] != null)
        {
            //this.cb_category.SelectedIndex = Convert.ToInt32(Session["Category"]) - 1;
            this.DropDownList1.SelectedValue = Session["Category"].ToString();
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Category"] = this.DropDownList1.SelectedValue;
        Response.Redirect(Request.Url.ToString());
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
    protected void cb_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Session["Category"] = this.cb_category.SelectedItem.Value.ToString();
        //Response.Redirect(Request.Url.ToString());
    }
    protected void btn_ExportExcel_Click(object sender, EventArgs e)
    {
        //string scriptText = "window.open('ExportPage.aspx?cat=" + this.DropDownList1.SelectedItem.Text + "', '', 'width=800px; height=550px;');";
        string scriptText = "window.open('./BookingRecord/CustomizeRecord.aspx?cat=" + this.DropDownList1.SelectedItem.Value + "', '_blank', '');";
        ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
    }
}