using BLL;
using GlobalClassNamespace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Equipment_EquipmentView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Category"] != null)
        {
            this.category = Session["Category"].ToString();
        }
        else
        {
            this.category = "2";
        }
        if (!IsPostBack)
        {
            if (this.category == "2" || this.category == "3")
            {
                InitDDLLocationData();
                InitDDLDepartmentData();
                this.PagedDataBind();
            }
        }
    }

    private void InitDDLLocationData()
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable result = null;
        // Set this.ddl_Location
        result = null;
        result = deviceManage.GetLocationListByCa(this.category);
        if (result == null)
        {
            GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            return;
        }
        this.ddl_location.DataSource = result.DefaultView;
        this.ddl_location.DataTextField = "Text";
        this.ddl_location.DataValueField = "Value";
        this.ddl_location.DataBind();
    }
    private void InitDDLDepartmentData()
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable result = null;
        // Set Department
        result = null;
        result = deviceManage.GetDepartmentListByCaLo(this.category, this.ddl_location.SelectedValue);
        if (result == null)
        {
            GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            return;
        }
        this.ddl_Department.DataSource = result.DefaultView;
        this.ddl_Department.DataTextField = "Text";
        this.ddl_Department.DataValueField = "Value";
        this.ddl_Department.DataBind();
    }

    protected void ddl_location_SelectedIndexChanged(object sender, EventArgs e)
    {
        E_CurrentPage = 1;
        InitDDLDepartmentData();
        PagedDataBind();
    }
    protected void ddl_Department_SelectedIndexChanged(object sender, EventArgs e)
    {
        E_CurrentPage = 1;

        PagedDataBind();
    }
    #region Equipment Manage
    private int E_CurrentPage
    {
        get
        {
            return Session["E_CurrentPage"] == null ? 1 : Convert.ToInt32(Session["E_CurrentPage"]);
        }
        set
        {
            Session["E_CurrentPage"] = value;
        }
    }

    string category;
    private void PagedDataBind()
    {
        int curpage = E_CurrentPage;
        this.labPage.Text = E_CurrentPage.ToString();
        PagedDataSource ps = new PagedDataSource();
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable ds = deviceManage.GetDeviceNameList(category, "0", "0", this.ddl_location.SelectedValue, this.ddl_Department.SelectedValue);
        if (ds == null)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            log.Error("Add Project:" + deviceManage.errMsg);
            return;
        }
        ps.DataSource = ds.DefaultView;
        ps.AllowPaging = true;
        ps.PageSize = 6;
        ps.CurrentPageIndex = curpage - 1;
        this.lnkbtnOne.Enabled = true;
        this.lnkbtnBack.Enabled = true;
        this.lnkbtnNext.Enabled = true;
        this.lnkbtnUp.Enabled = true;

        if (curpage == 1)
        {
            this.lnkbtnOne.Enabled = false;
            this.lnkbtnUp.Enabled = false;
        }
        if (curpage == ps.PageCount)
        {
            this.lnkbtnNext.Enabled = false;
            this.lnkbtnBack.Enabled = false;
        }

        this.labBackPage.Text = Convert.ToString(ps.PageCount);

        this.DataList1.DataSource = ps;
        this.DataList1.DataKeyField = "id";
        this.DataList1.DataBind();

    }
    protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("EquipmentDetail") == 0)
        {
            //Response.Write("<script>window.open('EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'width=580px, height=500px, top=100px, left=100px'); </script>");
            string scriptText = "var obj = window.showModalDialog('./EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'resizable = yes; dialogWidth = 750px; dialogHeight = 650px'); if(obj != null && obj == 'OK') window.location.reload();";
            //string scriptText = "window.open('./Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'width=580px, height=500px, top=100px, left=100px, scrollbars=yes');";
            ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);

        }

        if (e.CommandName.CompareTo("DeleteItem") == 0)
        {
            cl_DeviceManage deviceManage = new cl_DeviceManage();
            if (deviceManage.DeleteDeviceByID(e.CommandArgument.ToString()))
            {
                string scriptText = "alert('Delete Successfully！')";
                ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
                PagedDataBind();
            }
            else
            {
                string scriptText = "alert('" + deviceManage.errMsg + "')";
                ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
            }

        }
    }

    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item)
        {
            LinkButton lbtn_Detete = (LinkButton)e.Item.FindControl("lbtn_Delete");
            lbtn_Detete.Attributes.Add("onclick", "javascript:return confirm('Are sure to delete the Equipment?');");
        }
    }
    /// <summary>
    /// 添加设备
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void linkbutton_Click(object sender, EventArgs e)
    {
        string scriptText = @"var ret = window.showModalDialog('./Equipment/AddEquipment.aspx?cat=" + category + @"', '', 'dialogWidth = 685px; dialogHeight = 800px;resizable = yes;');
if(ret == 'OK'){
    alert('Add a device successfully!'); 
    window.location.reload();
}";
        ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
    }
    /// <summary>
    /// 第一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnOne_Click(object sender, EventArgs e)
    {
        E_CurrentPage = 1;
        this.labPage.Text = "1";
        this.PagedDataBind();
    }
    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnUp_Click(object sender, EventArgs e)
    {
        E_CurrentPage--;
        this.labPage.Text = Convert.ToString(Convert.ToInt32(this.labPage.Text) - 1);
        this.PagedDataBind();
    }
    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        E_CurrentPage++;
        this.labPage.Text = Convert.ToString(Convert.ToInt32(this.labPage.Text) + 1);
        this.PagedDataBind();
    }
    /// <summary>
    /// 最后一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        E_CurrentPage = Convert.ToInt32(labBackPage.Text);
        this.labPage.Text = labBackPage.Text;
        this.PagedDataBind();
    }
    #endregion
}