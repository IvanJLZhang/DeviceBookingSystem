using DataBaseModel;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Settings_ProjectStageView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
            ShowProjectStageTable();
        //}
    }

    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
    }

    void ShowProjectStageTable()
    {
        if (Request["pj_code"] != null)
        {
            string pj_code = Request["pj_code"].ToString();
            this.lbl_pj_code.Text = pj_code;
            DataTable pjName = settingsHandler.GetProjectTable(pj_code);
            if (pjName != null && pjName.Rows.Count > 0)
            {
                this.lb_pj_namel.Text = pjName.Rows[0]["PJ_Name"].ToString();
            }
            else
            {
                Response.Write("<script>window.close();</script>");
            }
            DataTable pjTable = settingsHandler.GetProjectStage(pj_code);
            pjTable.Columns.Add("ps_status");

            DataTable activatePJ = pjTable.Clone();
            DataTable noActivatePJ = pjTable.Clone();

            foreach (DataRow pj in pjTable.Rows)
            {
                DateTime start = Convert.ToDateTime(pj["ps_from"]);
                DateTime to = Convert.ToDateTime(pj["ps_to"]);
                if (DateTime.Now >= start && DateTime.Now <= to)
                {
                    pj["ps_status"] = 1;
                    activatePJ.Rows.Add(pj.ItemArray);

                }
                else
                {
                    pj["ps_status"] = 0;
                    noActivatePJ.Rows.Add(pj.ItemArray);
                }
            }
            DataView dataView = pjTable.DefaultView;
            dataView.Sort = "ps_status desc";
            this.gv_project_stage_view.DataSource = dataView;
            this.gv_project_stage_view.KeyFieldName = "ps_id";
            this.gv_project_stage_view.DataBind();
        }
    }

    protected void gv_project_stage_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        string ps_id = e.Keys["ps_id"].ToString();
        if (settingsHandler.DeleteProjectStage(ps_id) != -1)
        {
            gv_project_stage_view.JSProperties["cpAlertMsg"] = "Deleted the project stage:" + ps_id;
        }
        else
        {
            gv_project_stage_view.JSProperties["cpAlertMsg"] = "Can not deleted the project stage:" + ps_id;
        }
        e.Cancel = true;
        ShowProjectStageTable();
    }
    protected void gv_project_stage_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        int key = Convert.ToInt32(e.Keys["ps_id"].ToString());
        tbl_project_stage oldValues = new tbl_project_stage();
        tbl_project_stage newValues = new tbl_project_stage();

        oldValues.ps_id = key;
        oldValues.ps_stage = e.OldValues["ps_stage"].ToString();
        oldValues.ps_from = Convert.ToDateTime(e.OldValues["ps_from"]);
        oldValues.ps_to = Convert.ToDateTime(e.OldValues["ps_to"]);

        newValues.ps_id = key;
        newValues.ps_stage = e.NewValues["ps_stage"].ToString();
        newValues.ps_from = Convert.ToDateTime(e.NewValues["ps_from"]);
        newValues.ps_to = Convert.ToDateTime(e.NewValues["ps_to"]);

        if (settingsHandler.UpdateProjectStage(oldValues, newValues) > 0)
        {
            this.gv_project_stage_view.JSProperties["cpAlertMsg"] = "Update the project stage:" + key;
        }
        else
        {
            gv_project_stage_view.JSProperties["cpAlertMsg"] = "Can not update the project stage:" + key;
        }
        this.gv_project_stage_view.CancelEdit();
        e.Cancel = true;

        ShowProjectStageTable();
    }
    protected void gv_project_stage_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
    }
    protected void gv_project_stage_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        ASPxGridView gv_ps = sender as ASPxGridView;

        tbl_project_stage pj_stage = new tbl_project_stage();
        pj_stage.ps_pj_id = this.lbl_pj_code.Text;
        pj_stage.ps_stage = e.NewValues["ps_stage"].ToString();
        pj_stage.ps_from = Convert.ToDateTime(e.NewValues["ps_from"]);
        pj_stage.ps_to = Convert.ToDateTime(e.NewValues["ps_to"]);

        if (settingsHandler.InitNewRow(pj_stage) > 0)
        {
            gv_ps.JSProperties["cpAlertMsg"] = "Insert the project stage:" + pj_stage.ps_stage;
        }
        else
        {
            gv_ps.JSProperties["cpAlertMsg"] = "Can not insert the project stage:" + pj_stage.ps_stage;
        }
        e.Cancel = true;
        gv_ps.CancelEdit();

        ShowProjectStageTable();

    }
    protected void gv_project_stage_view_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
    {
        //ShowProjectStageTable();
    }
}