using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_BookingSteps_Step3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitControlState();
            InitTimeComboData();
            ShowSelectedDevices();
            ShowDeviceRecord();
        }
    }
    #region session value
    List<string> SelectedIDList
    {
        get
        {
            if (Session["SelectedIDList"] == null)
                return new List<string>();
            return (List<string>)Session["SelectedIDList"];
        }
        set
        {
            Session["SelectedIDList"] = value;
        }
    }

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
    #region methods
    protected void chb_allDayevent_CheckedChanged(object sender, EventArgs e)
    {
        this.cb_start.Enabled = this.cb_end.Enabled = !chb_allDayevent.Checked;
        if (chb_allDayevent.Checked)
        {
            this.cb_start.SelectedIndex = this.cb_end.SelectedIndex = 0;
        }
    }
    protected void chb_recurrence_CheckedChanged(object sender, EventArgs e)
    {
        InitControlState();
        InitTimeComboData();
    }

    void InitControlState()
    {
        this.cb_start.Enabled = this.cb_end.Enabled = !this.chb_allDayevent.Checked;

        if (chb_recurrence.Checked)
        {
            pnl_rangeOfdate.Visible = true;
            pnl_appointmentTime.Visible = true;
            pnl_appointmentDatetime.Visible = false;

        }
        else
        {
            pnl_rangeOfdate.Visible = false;
            pnl_appointmentTime.Visible = false;
            pnl_appointmentDatetime.Visible = true;
        }
        this.lbl_everyDay.Enabled = this.rb_daily.Enabled = this.chbl_weekly.Enabled = this.rb_weekly.Enabled = chb_recurrence.Checked;
        rb_daily_weekly_CheckedChanged(null, null);



        this.de_from.ValidationSettings.RequiredField.IsRequired =
            this.de_to.ValidationSettings.RequiredField.IsRequired =
            this.cb_start.ValidationSettings.RequiredField.IsRequired =
            this.cb_end.ValidationSettings.RequiredField.IsRequired = this.chb_recurrence.Checked;

        this.de_from2.ValidationSettings.RequiredField.IsRequired =
            this.de_to2.ValidationSettings.RequiredField.IsRequired =
            this.cb_start2.ValidationSettings.RequiredField.IsRequired =
            this.cb_end2.ValidationSettings.RequiredField.IsRequired = !this.chb_recurrence.Checked;


    }

    void InitTimeComboData()
    {
        this.cb_start.Items.Clear();
        this.cb_start2.Items.Clear();
        this.cb_end.Items.Clear();
        this.cb_end2.Items.Clear();

        TimeSpan time = TimeSpan.Zero;
        for (int index = 0; index < 48; index++)
        {
            string timeStr = time.ToString(@"hh\:mm");
            this.cb_start.Items.Add(timeStr, timeStr);
            this.cb_start2.Items.Add(timeStr, timeStr);
            this.cb_end.Items.Add(timeStr, timeStr);
            this.cb_end2.Items.Add(timeStr, timeStr);

            time = time.Add(new TimeSpan(0, 30, 0));
        }

        this.cb_start.Value = this.cb_start2.Value = this.cb_end.Value = this.cb_end2.Value = null;
        this.de_from.Value = this.de_from2.Value = this.de_to.Value = this.de_to2.Value = null;
    }

    void ShowSelectedDevices()
    {
        DataTable deviceList = BookingPageFactory.GetDeviceTable(this.SelectedIDList);
        if (deviceList != null)
        {
            this.rbtnl_deviceList.DataSource = deviceList.DefaultView;
            this.rbtnl_deviceList.TextField = "s_name";
            this.rbtnl_deviceList.ValueField = "s_id";
            this.rbtnl_deviceList.DataBind();

            this.rbtnl_deviceList.SelectedIndex = 0;
        }
    }

    protected void rb_daily_weekly_CheckedChanged(object sender, EventArgs e)
    {
        this.chbl_weekly.Enabled = this.rb_weekly.Enabled ? this.rb_weekly.Checked : this.rb_weekly.Enabled;
        this.lbl_everyDay.Enabled = this.rb_daily.Enabled ? this.rb_daily.Checked : this.rb_daily.Enabled;
    }


    void ShowDeviceRecord()
    {
        string device_id = null;
        string date_from = null;
        string date_to = null;

        if (this.rbtnl_deviceList.SelectedItem != null)
        {
            device_id = this.rbtnl_deviceList.Value.ToString();
        }
        if (pnl_rangeOfdate.Visible)
        {
            if (this.de_from.Value != null)
                date_from = Convert.ToDateTime(this.de_from.Value).ToString("yyyy-MM-dd");

            if (this.de_to.Value != null)
            {
                date_to = Convert.ToDateTime(this.de_to.Value).ToString("yyyy-MM-dd");
            }
        }
        else if (pnl_appointmentDatetime.Visible)
        {
            if (this.de_from2.Value != null)
            {
                date_from = Convert.ToDateTime(this.de_from2.Value).ToString("yyyy-MM-dd");
            }

            if (this.de_to2.Value != null)
            {
                date_to = Convert.ToDateTime(this.de_to2.Value).ToString("yyyy-MM-dd");
            }
        }

        this.devViewFrame.Attributes["src"] = "./RecordPatternShow.aspx?device_id=" + device_id + "&date_from=" + date_from + "&date_to=" + date_to;

    }
    #endregion
    protected void btn_new_Click(object sender, EventArgs e)
    {
        if (this.NewBookingData == null)
        {
            #region init table
            this.NewBookingData = new DataTable();
            this.NewBookingData.Columns.Add("id");

            this.NewBookingData.Columns["id"].Unique = true;
            this.NewBookingData.Columns["id"].AutoIncrement = true;
            this.NewBookingData.Columns["id"].AutoIncrementSeed = 1;
            this.NewBookingData.Columns["id"].AutoIncrementStep = 1;

            this.NewBookingData.Columns.Add("s_id");
            this.NewBookingData.Columns.Add("s_name");
            this.NewBookingData.Columns.Add("db_recurrence");
            this.NewBookingData.Columns.Add("db_is_recurrence");
            this.NewBookingData.Columns.Add("Loan_DateTime");
            this.NewBookingData.Columns.Add("Plan_To_ReDateTime");
            this.NewBookingData.Columns.Add("db_start");
            this.NewBookingData.Columns.Add("db_end");
            #endregion
        }
        DataRow newRow = this.NewBookingData.NewRow();
        if (this.rbtnl_deviceList.Items.Count <= 0)
        {
            newRow["s_id"] = "1111";
            newRow["s_name"] = "1111";
            return;
        }
        else
        {
            newRow["s_id"] = this.rbtnl_deviceList.SelectedItem.Value;
            newRow["s_name"] = this.rbtnl_deviceList.SelectedItem.Text;
        }
        newRow["db_is_recurrence"] = this.chb_recurrence.Checked;
        if (this.chb_recurrence.Checked)
        {
            if (this.rb_daily.Checked)
            {
                newRow["db_recurrence"] = (Int32)RegularType.EveryDay;

            }
            else if (this.rb_weekly.Checked)
            {
                string weekdayStr = String.Empty;
                foreach (var weekday in this.chbl_weekly.SelectedValues)
                {
                    weekdayStr += weekday + ",";
                }
                weekdayStr = weekdayStr.Substring(0, weekdayStr.Length - 1);
                newRow["db_recurrence"] = weekdayStr;
            }

            newRow["Loan_DateTime"] = Convert.ToDateTime(this.de_from.Value);
            newRow["Plan_To_ReDateTime"] = Convert.ToDateTime(this.de_to.Value);

            newRow["db_start"] = TimeSpan.Parse(this.cb_start.Value.ToString());
            newRow["db_end"] = TimeSpan.Parse(this.cb_end.Value.ToString());

        }
        else
        {
            newRow["db_recurrence"] = (Int32)RegularType.NONE;

            DateTime dt_from = Convert.ToDateTime(this.de_from2.Value);
            dt_from = dt_from.Add(TimeSpan.Parse(this.cb_start2.Value.ToString()));

            DateTime dt_to = Convert.ToDateTime(this.de_to2.Value);
            dt_to = dt_to.Add(TimeSpan.Parse(this.cb_end2.Value.ToString()));

            newRow["Loan_DateTime"] = dt_from;
            newRow["Plan_To_ReDateTime"] = dt_to;

        }
        if (!RecordManagment.CheckRecord(newRow, this.NewBookingData))
        {
            this.NewBookingData.Rows.Add(newRow);
            this.recordView.Attributes["src"] = "../RecordSimpleView.aspx";
        }
        else
        {
            string scriptText = "alert('Datetime conflit, please check the booking record and pick new time duration.')";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", scriptText, true);
        }
    }
    protected void DateChanged(object sender, EventArgs e)
    {
        ShowDeviceRecord();
    }
}