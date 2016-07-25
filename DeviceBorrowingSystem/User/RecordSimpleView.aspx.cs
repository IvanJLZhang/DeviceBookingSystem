using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_RecordSampleView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region session data
    DataTable NewBookingData
    {
        get
        {
            if (Session["NewBookingData"] == null)
                return null;
            return (DataTable)Session["NewBookingData"];
        }
        set
        {
            Session["NewBookingData"] = value;
        }
    }
    #endregion

    void ShowTableData()
    {
        if (NewBookingData == null)
            return;

        this.gv_recordView.DataSource = NewBookingData.DefaultView;
        this.gv_recordView.KeyFieldName = "id";
        this.gv_recordView.DataBind();
    }
    protected void gv_recordView_Init(object sender, EventArgs e)
    {
        ShowTableData();
    }
    protected void gv_recordView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        string id = e.Keys["id"].ToString();
        foreach (DataRow record in this.NewBookingData.Rows)
        {
            if (id.Equals(record["id"].ToString()))
            {
                this.NewBookingData.Rows.Remove(record);
                break;
            }
        }

        ShowTableData();
    }
}