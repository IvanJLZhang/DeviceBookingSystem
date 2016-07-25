using DataBaseModel;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Settings_ProjectView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowProjectTable();
    }

    void ShowProjectTable()
    {
        DataTable project_table = settingsHandler.GetProjectTable();

        if (project_table != null)
        {
            this.gv_project_view.DataSource = project_table.DefaultView;
            this.gv_project_view.KeyFieldName = "PJ_Code";
            this.gv_project_view.DataBind();
        }
    }
    protected void gv_project_view_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        string key = e.Keys["PJ_Code"].ToString();
        if (settingsHandler.DeleteProject(key) != -1)
        {
            gv_project_view.JSProperties["cpAlertMsg"] = "Deleted the project:" + key;
        }
        else
        {
            gv_project_view.JSProperties["cpAlertMsg"] = "Can not deleted the project:" + key;
        }

        ShowProjectTable();
    }
    protected void gv_project_view_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        e.Cancel = true;
        string key = e.Keys["PJ_Code"].ToString();
        tbl_Project oldValues = new tbl_Project();
        tbl_Project newValues = new tbl_Project();

        oldValues.PJ_Code = e.OldValues["PJ_Code"].ToString();
        oldValues.PJ_Name = e.OldValues["PJ_Name"].ToString();
        oldValues.Cust_Name = e.OldValues["Cust_Name"].ToString();

        newValues.PJ_Code = e.NewValues["PJ_Code"].ToString();
        newValues.PJ_Name = e.NewValues["PJ_Name"].ToString();
        newValues.Cust_Name = e.NewValues["Cust_Name"].ToString();

        if (settingsHandler.UpdateProject(oldValues, newValues) > 0)
        {
            this.gv_project_view.JSProperties["cpAlertMsg"] = "Update the project:" + key;
        }
        else
        {
            gv_project_view.JSProperties["cpAlertMsg"] = "Can not update the project:" + key;
        }
        this.gv_project_view.CancelEdit();
        e.Cancel = true;
        ShowProjectTable();
    }
    protected void btn_addps_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "EditPS")
        {
            string pj_code = e.CommandArgument.ToString();
            var pjTable = settingsHandler.GetProjectTable(pj_code);
            if (pjTable != null && pjTable.Rows.Count > 0)
            {
                string openwindow = @"<script>window.showModalDialog('./ProjectStageView.aspx?pj_code=" + pj_code + @"', 'resizable=yes;help=yes');window.location.href = window.location.href;</script>";
                Response.Write(openwindow);
            }
        }
    }
    protected void gv_project_view_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
        gv_project_view.Columns["Project Stage"].Visible = false;
    }
    protected void gv_project_view_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        tbl_Project project = new tbl_Project();
        project.PJ_Code = e.NewValues["PJ_Code"].ToString();
        project.PJ_Name = e.NewValues["PJ_Name"].ToString();
        project.Cust_Name = e.NewValues["Cust_Name"].ToString();

        if (settingsHandler.InitNewRow(project) > 0)
        {
            this.gv_project_view.JSProperties["cpAlertMsg"] = "Insert the project:" + project.PJ_Code + ", you can edit the project stage by click the edit button.";
        }
        else
        {
            gv_project_view.JSProperties["cpAlertMsg"] = "Can not insert the project:" + project.PJ_Code;
        }
        e.Cancel = true;
        this.gv_project_view.CancelEdit();
        ShowProjectTable();
    }
    protected void gv_project_view_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
    {
    }
    protected void gv_project_view_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        gv_project_view.Columns["Project Stage"].Visible = true;
    }
    protected void gv_project_view_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        gv_project_view.Columns["Project Stage"].Visible = true;
    }
}