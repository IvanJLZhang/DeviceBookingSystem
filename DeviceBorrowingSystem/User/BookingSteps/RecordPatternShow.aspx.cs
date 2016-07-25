using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_BookingSteps_RecordPatternShow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            NewInit();
    }
    #region properties
    Category cat
    {
        get
        {
            if (Session["Category"] == null)
                return Category.Device;
            else
            {
                return (Category)Convert.ToInt32(Session["Category"]);
            }
        }
        set
        {
            Session["Category"] = (Int32)value;
        }
    }
    #endregion
    void NewInit()
    {
        this.de_from.Value = DateTime.Now;
        if (Request["device_id"] != null && Request["device_id"].ToString() != string.Empty)
        {
            this.tb_deviceID.Text = Request["device_id"].ToString();
            tb_deviceID_TextChanged(null, null);
        }
        if (Request["date_from"] != null && Request["date_from"].ToString() != String.Empty)
        {
            this.de_from.Value = Convert.ToDateTime(Request["date_from"]);
        }
        if (Request["date_to"] != null && Request["date_to"].ToString() != string.Empty)
        {
            this.de_to.Value = Convert.ToDateTime(Request["date_to"]);
        }
        ShowRecordTable();
    }
    protected void tb_deviceID_TextChanged(object sender, EventArgs e)
    {
        string deviceIdName = this.tb_deviceID.Text.Trim();
        DataTable deviceArr = RecordManagment.GetDeviceByIDName(deviceIdName);
        if (deviceArr != null)
        {
            this.cb_DeviceName.DataSource = deviceArr.DefaultView;
            this.cb_DeviceName.ValueField = "s_id";
            this.cb_DeviceName.TextField = "s_name";
            this.cb_DeviceName.DataBind();
            this.cb_DeviceName.SelectedIndex = 0;
        }
    }
    protected void DateChanged(object sender, EventArgs e)
    {
        ShowRecordTable();
    }

    void ShowRecordTable()
    {
        ViewType type = ViewType.VIEW;

        string device_id = null;
        DateTime? date_from = null;
        DateTime? date_to = null;
        if (this.cb_DeviceName.Value != null)
        {
            device_id = this.cb_DeviceName.Value.ToString();
        }
        if (this.de_from.Value != null)
        {
            date_from = Convert.ToDateTime(this.de_from.Value);
        }

        if (this.de_to.Value != null)
        {
            date_to = Convert.ToDateTime(this.de_to.Value);
        }

        var status = new List<RecordStatus>() { RecordStatus.NEW_SUBMIT, RecordStatus.APPROVE_NORETURN };

        DataTable record = RecordManagment.GetRecordsNotShowReviewer(this.cat,
             status,
             null, null,
             device_id,
             date_from,
             date_to, false);
        if (record != null)
        {
            this.gv_RecordView.DataSource = record.DefaultView;
            this.gv_RecordView.KeyFieldName = "Booking_ID";
            this.gv_RecordView.DataBind();
        }
    }
    protected void cb_DeviceName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowRecordTable();
    }
    protected void gv_RecordView_DataBound(object sender, EventArgs e)
    {
        if (Session["Category"] != null)
        {
            Category cat = (Category)Convert.ToInt32(Session["Category"]);
            if (cat == Category.Device)
            {
                this.gv_RecordView.Columns["Custom_ID"].Visible = true;
            }
            else
            {
                this.gv_RecordView.Columns["Custom_ID"].Visible = false;
            }
        }
    }
    protected void gv_RecordView_Init(object sender, EventArgs e)
    {
        ShowRecordTable();
    }
}