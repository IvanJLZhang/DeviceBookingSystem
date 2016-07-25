using DataBaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Settings_TestCategoryView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowPurposeTable();
    }

    void ShowPurposeTable()
    {
        DataTable purpose_table = settingsHandler.GetPurposeTable();

        if (purpose_table != null)
        {
            this.gv_purpose_view.DataSource = purpose_table.DefaultView;
            this.gv_purpose_view.KeyFieldName = "ID";
            this.gv_purpose_view.DataBind();
        }
    }
    protected void gv_purpose_view_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        string key = e.Keys["ID"].ToString();
        if (settingsHandler.DeletePurpose(key) != -1)
        {
            gv_purpose_view.JSProperties["cpAlertMsg"] = "Deleted the test category:" + key;
        }
        else
        {
            gv_purpose_view.JSProperties["cpAlertMsg"] = "Can not deleted the test category:" + key;
        }

        ShowPurposeTable();
    }
    protected void gv_purpose_view_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {

        string key = e.Keys["ID"].ToString();
        tbl_TestCategory oldValues = new tbl_TestCategory();
        tbl_TestCategory newValues = new tbl_TestCategory();

        oldValues.ID = e.OldValues["ID"].ToString();
        oldValues.Name = e.OldValues["Name"].ToString();

        newValues.ID = e.NewValues["ID"].ToString();
        newValues.Name = e.NewValues["Name"].ToString();

        if (settingsHandler.UpdatePurse(oldValues, newValues) > 0)
        {
            gv_purpose_view.JSProperties["cpAlertMsg"] = "Update the test category:" + key;
        }
        else
        {
            gv_purpose_view.JSProperties["cpAlertMsg"] = "Can not update the test category:" + key;
        }
        this.gv_purpose_view.CancelEdit();
        e.Cancel = true;
        ShowPurposeTable();
    }
    protected void gv_purpose_view_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
    {
        e.NewValues["ID"] = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    }
    protected void gv_purpose_view_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
    {
        tbl_TestCategory purpose = new tbl_TestCategory();
        purpose.ID = e.NewValues["ID"].ToString();
        purpose.Name = e.NewValues["Name"].ToString();

        if (settingsHandler.InitNewRow(purpose) > 0)
        {
            this.gv_purpose_view.JSProperties["cpAlertMsg"] = "Insert the test category:" + purpose.ID;
        }
        else
        {
            gv_purpose_view.JSProperties["cpAlertMsg"] = "Can not insert the test category:" + purpose.ID;
        }
        e.Cancel = true;
        this.gv_purpose_view.CancelEdit();
        ShowPurposeTable();
    }
}