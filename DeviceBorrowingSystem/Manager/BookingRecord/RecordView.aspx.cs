using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_BookingRecord_RecordView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    void ShowGridViewData()
    {
        string booking_id = null;
        string loaner_id = null;
        string device_id = null;
        DateTime? date_from = null;
        DateTime? date_to = null;

        Category category = Category.Device;
        if (Session["Category"] != null)
            category = (Category)Convert.ToInt32(Session["Category"]);

        //int category = 2;
        if (Request["booking_id"] != null)
        {
            booking_id = Request["booking_id"].ToString();
        }

        if (Request["loaner_id"] != null)
        {
            loaner_id = Request["loaner_id"].ToString();
        }

        if (Request["device_id"] != null && Request["device_id"].ToString() != String.Empty)
        {
            device_id = Request["device_id"].ToString();
        }
        if (Request["date_from"] != null && Request["date_from"].ToString() != String.Empty)
        {
            date_from = Convert.ToDateTime(Request["date_from"]);
        }
        if (Request["date_to"] != null && Request["date_to"].ToString() != String.Empty)
        {
            date_to = Convert.ToDateTime(Request["date_to"]);
        }
        List<RecordStatus> status = new List<RecordStatus>();
        if (Request["status"] != null && Request["status"].ToString() != String.Empty)
        {
            status.Add((RecordStatus)Convert.ToInt32(Request["status"]));
        }
        else
        {
            status = new List<RecordStatus>() { RecordStatus.NEW_SUBMIT, RecordStatus.APPROVE_NORETURN, RecordStatus.REJECT, RecordStatus.RETURN };
        }


        DataTable record = RecordManagment.GetRecords((Int32)category
            , status
            , booking_id
            , loaner_id
            , device_id
            , date_from
            , date_to);

        if (record != null)
        {
            DataView recordView = record.DefaultView;
            recordView.Sort = "Loan_DateTime asc, Status desc";

            this.gv_RecordView.DataSource = recordView;
            this.gv_RecordView.KeyFieldName = "Booking_ID";
            this.gv_RecordView.DataBind();
        }
    }
    protected void gv_RecordView_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        string id = e.Keys["Booking_ID"].ToString();
        int result = RecordManagment.DeleteRecords(new List<string>() { id });

        if (result > 0)
        {
            gv_RecordView.JSProperties["cpAlertMsg"] = "Deleted the record:" + id;
        }
        else
        {
            gv_RecordView.JSProperties["cpAlertMsg"] = "Can not delete the record:" + id;
        }

        ShowGridViewData();
    }
    protected void gv_RecordView_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var intID = Convert.ToString(gv_RecordView.GetRowValues(gv_RecordView.FocusedRowIndex, "Booking_ID"));
        var view = sender as ASPxGridView;
        view.JSProperties["cpType"] = e.ButtonID;
        view.JSProperties["cpBookingId"] = intID;
        view.JSProperties["cpViewType"] = (Int32)GetViewType();
    }
    protected void gv_RecordView_DataBound(object sender, EventArgs e)
    {
        Category category = Category.Device;
        if (Session["Category"] != null)
            category = (Category)Convert.ToInt32(Session["Category"]);
        if (category != Category.Device)
        {
            this.gv_RecordView.Columns["Custom_ID"].Visible = false;
        }

        ViewType type = ViewType.VIEW;
        if (Request["type"] != null)
        {
            type = (ViewType)Convert.ToInt32(Request["type"]);
        }
        switch (type)
        {
            case ViewType.VIEW:
                this.gv_RecordView.Columns["Command"].Visible = false;
                break;
        }

    }
    protected void gv_RecordView_Init(object sender, EventArgs e)
    {
        ShowGridViewData();
    }

    /// <summary>
    /// Get view type
    /// </summary>
    /// <returns></returns>
    ViewType GetViewType()
    {
        // check role
        ViewType viewType = ViewType.VIEW;// default is view
        if (Session["Role"] != null)
        {
            UserRole role = (UserRole)Convert.ToInt32(Session["Role"]);
            switch (role)
            {
                case UserRole.USER:// if the role is user, view type is view only.
                    viewType = ViewType.VIEW;
                    break;
                case UserRole.REVIEWER:
                case UserRole.LEADER:
                case UserRole.ADMIN:
                    if (Request["type"] != null)
                    {
                        viewType = (ViewType)Convert.ToInt32(Request["type"]);
                    }
                    break;
                default:
                    break;
            }
        }
        return viewType;
    }
}