using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Equipment_OwnerSelect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetDeptList();
            ShowOwnerList();
        }
    }
    private void SetDeptList()
    {
        cl_PersonManage personManage = new cl_PersonManage();
        int role = 0;
        if (Request["maxrole"] != null)
            role = Convert.ToInt32(Request["maxrole"]);
        DataTable table = personManage.GetDepartmentList(role);

        if (table != null)
        {
            table.DefaultView.Sort = "DptValue asc";
            this.ddl_department.DataSource = table.DefaultView;
            this.ddl_department.DataTextField = "DptName";
            this.ddl_department.DataValueField = "DptValue";
            this.ddl_department.DataBind();

            if (Request["dpt"] != null) {
                this.ddl_department.SelectedValue = Request["dpt"].ToString();
            }

        }
    }
    private void ShowOwnerList()
    {
        int role = 0;
        if (Request["maxrole"] != null)
        {
            role = Convert.ToInt32(Request["maxrole"]);
        }
        else
            role = 0;
        cl_PersonManage personManage = new cl_PersonManage();
        DataTable table = personManage.GetOwnerList(this.ddl_department.SelectedValue, role);

        this.dl_UserList.DataSource = table.DefaultView;
        this.dl_UserList.DataBind();
    }
    protected void dl_UserList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("SelectUser") == 0)
            Response.Write("<script>window.returnValue = '" + e.CommandArgument.ToString() + "'; window.close();</script>");
    }
    protected void ddl_department_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowOwnerList();
    }
}