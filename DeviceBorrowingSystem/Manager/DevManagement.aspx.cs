using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_DevManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            CheckCategory();
        }
    }

    private void CheckCategory()
    {
        if (Request["cat"] != null)
        {
            Session["Category"] = Request["cat"];
        }
        if (Session["Category"] != null)
        {
            this.ddl_category.SelectedValue = Session["Category"].ToString();
        }

        if (ddl_category.SelectedValue == "1")
        {
            this.devViewFrame.Attributes["src"] = "Device/deviceView.aspx?type=1";
        }
        else
        {
            this.devViewFrame.Attributes["src"] = "Equipment/EquipmentViewEx.aspx?type=1";
        }
    }
    protected void ddl_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Category"] = ddl_category.SelectedValue;
        Response.Redirect("~/Manager/DevManagement.aspx?cat=" + Session["Category"]); 
    }
    protected void imgb_AddFromFile_Click(object sender, ImageClickEventArgs e)
    {
        Session.Remove("DeviceData");
        string scriptText = "window.open('./Equipment/AddDeviceListFromFile.aspx?type=add', '', '')";
        ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
    }
    protected void imgbtn_PrintDeviceList_Click(object sender, ImageClickEventArgs e)
    {
        devManagementFac devManage = new devManagementFac();
        devManage.serverPath = Server.MapPath("~/");
        ExcelRenderNode result = devManage.PrintDeviceTable(this.ddl_category.SelectedValue);
        ExcelRender.RenderToBrowser(result.ms, Context, this.ddl_category.SelectedItem.Text + "_Data_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");

        result.ms.Close();
        result.ms.Dispose();
    }
    protected void imgbtn_Search_Click(object sender, ImageClickEventArgs e)
    {

    }
    /// <summary>
    /// 添加设备
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void linkbutton_Click(object sender, EventArgs e)
    {
        string cat = this.ddl_category.SelectedValue;
        string scriptText = @"var ret = window.showModalDialog('./Equipment/AddEquipment.aspx?cat=" + cat + @"', '', 'dialogWidth = 685px; dialogHeight = 800px;resizable = yes;');
if(ret == 'OK'){
    alert('Add a device successfully!'); 
    window.location.reload();
}";
        ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
    }
}