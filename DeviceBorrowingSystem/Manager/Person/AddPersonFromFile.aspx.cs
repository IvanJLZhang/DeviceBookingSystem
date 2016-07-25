using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Person_AddPersonFromFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btn_AddAll.Attributes.Add("onclick", "javascript:return confirm('Are sure to add the these users?');");
        if (!IsPostBack)
        {
            if (Request["type"] == null)
            {
                Response.Write("<script>window.open('', '_self', ''); window.close();</script>");
            }
            ShowPostedPersonList();
        }
    }
    private void ShowPostedPersonList()
    {
        if (Session["PersonData"] != null)
        {
            this.gv_PersonList.DataSource = Session["PersonData"];
            this.gv_PersonList.DataKeyNames = new string[] { "P_ID" };
            this.gv_PersonList.DataBind();
        }
    }
    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session.Remove("PersonData");
        Response.Write("<script>alert('close window');window.close();</script>");
    }
    protected void btn_AddAll_Click(object sender, EventArgs e)
    {
        cl_PersonManage personManage = new cl_PersonManage();
        DataTable personList = (DataTable)Session["PersonData"];
        int cnt = personManage.AddPersonByTable(personList);
        Session.Remove("PersonData");
        Response.Write("<script>alert('Add " + cnt + " users successfully!');window.opener.location.href = window.opener.location.href;window.close();</script>");
    }
    protected void btn_upload_Click(object sender, EventArgs e)
    {
        if (FileSelect.PostedFile != null && FileSelect.PostedFile.ContentLength > 0)
        {
            string fileName = FileSelect.PostedFile.FileName;
            DataTable personTable = ExcelRender.RenderFromExcel(FileSelect.PostedFile.InputStream, 0);
            personTable.Rows.RemoveAt(0);
            if (personTable != null && personTable.Rows.Count > 0)
            {
                //personTable.Columns.Add(new DataColumn("P_Role", typeof(System.Int32)));
                personTable.Columns.Add(new DataColumn("P_Password", typeof(System.String)));
                foreach (DataRow row in personTable.Rows)
                {
                    //row["P_Role"] = GetRoleInt(row["Role"].ToString());
                    row["P_Password"] = "123456";
                }
                Session["PersonData"] = personTable;
                this.gv_PersonList.DataSource = Session["PersonData"];
                this.gv_PersonList.DataKeyNames = new string[] { "P_ID" };
                this.gv_PersonList.DataBind();
            }
        }
    }

    public string GetRoleStr(int role)
    {
        switch (role)
        {
            case 0:
                return "User";
            case 10:
                return "Reviewer";
            case 11:
                return "Reviewer";
            case 20:
                return "Admin";
            default:
                return String.Empty;
        }
    }
    private int GetRoleInt(string role)
    {
        if (role.ToLower().CompareTo("admin") == 0)
        {
            return 20;
        }
        else if (role.ToLower().CompareTo("reviewer") == 0)
        {
            return 10;
        }
        else if (role.ToLower().CompareTo("reviewer(equipment)") == 0)
        {
            return 11;
        }
        else
            return 0;
    }
    protected void gv_PersonList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("PersonDetail") == 0)
        {
            //string scriptText = "window.showModalDialog('Person/UserInfo.aspx?uid=" + e.CommandArgument.ToString() + "&edit=true', '', ''); window.location.href = window.location.href;";
            Response.Write("<script>window.showModalDialog('./UserInfo.aspx?uid=" + e.CommandArgument.ToString() + "&edit=true&type=addview', '', ''); window.location.href = window.location.href;</script>");
        }
        if (e.CommandName.CompareTo("DeleteItem") == 0)
        {
            DataTable deviceList = (DataTable)Session["PersonData"];
            GridViewRow row = ((GridViewRow)(((LinkButton)(e.CommandSource)).Parent.Parent));
            deviceList.Rows.RemoveAt(row.RowIndex);
            Session["PersonData"] = deviceList;
            gv_PersonList.DataSource = Session["PersonData"];
            this.gv_PersonList.DataKeyNames = new string[] { "P_ID" };
            gv_PersonList.DataBind();

        }
    }
    protected void btn_GetTemplate_Click(object sender, EventArgs e)
    {
        personManageFac dev = new personManageFac();
        ExcelRenderNode result = dev.GenerateTemplateFile(Server.MapPath("~\\Temp\\"));

        GlobalClassNamespace.GlobalClass.DeleteDir(Server.MapPath("~\\Temp\\"));

        string fileName = "Person_Data_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
        ExcelRender.RenderToBrowser(result.ms, Context, fileName);

        result.ms.Close();
        result.ms.Dispose();
    }
}