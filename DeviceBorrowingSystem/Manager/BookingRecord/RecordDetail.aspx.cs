using DataBaseModel;
using DevExpress.Web.ASPxEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_BookingRecord_RecordDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitControlState();
            InitTimeComboData();
            InitControlData();
            ShowBookingData();
        }
    }

    #region proterties
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
            Session["Category"] = value;
        }
    }
    #endregion

    #region 控件事件
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        if (Request["booking_id"] == null)
            return;
        string booking_id = Request["booking_id"].ToString();
        DataRow record = RecordManagment.GetRecord(booking_id);
        if (record == null)
            return;
        tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        deviceBooking.Booking_ID = Request["booking_id"].ToString();
        if (Session["UserID"] == null) return;
        deviceBooking.Loaner_ID = record["Loaner_ID"].ToString();
        deviceBooking.Reviewer_ID = Session["UserID"].ToString();
        deviceBooking.Device_ID = record["Device_ID"].ToString();
        deviceBooking.Project_ID = this.cb_project.Value.ToString();
        deviceBooking.PJ_Stage = this.tb_pjStage.Text;
        deviceBooking.TestCategory_ID = this.cb_testCategory.Value.ToString();
        deviceBooking.Status = Convert.ToInt32(this.cb_status.Value);

        deviceBooking.Comment = this.mo_comment.Text;
        deviceBooking.Review_Comment = this.mo_reviewComment.Text;

        deviceBooking.db_is_recurrence = this.chb_recurrence.Checked;
        if (deviceBooking.db_is_recurrence)
        {
            if (this.rb_daily.Checked)
            {
                deviceBooking.db_recurrence = ((Int32)RegularType.EveryDay).ToString();

            }
            else if (this.rb_weekly.Checked)
            {
                string weekdayStr = String.Empty;
                foreach (var weekday in this.chbl_weekly.SelectedValues)
                {
                    weekdayStr += weekday + ",";
                }
                weekdayStr = weekdayStr.Substring(0, weekdayStr.Length - 1);
                deviceBooking.db_recurrence = weekdayStr;
            }

            deviceBooking.Loan_DateTime = Convert.ToDateTime(this.de_from.Value);
            deviceBooking.Plan_To_ReDateTime = Convert.ToDateTime(this.de_to.Value);

            deviceBooking.db_start = TimeSpan.Parse(this.cb_start.Value.ToString());
            deviceBooking.db_end = TimeSpan.Parse(this.cb_end.Value.ToString());
        }
        else
        {
            DateTime from = Convert.ToDateTime(this.de_from2.Value);
            from = from.Add(TimeSpan.Parse(this.cb_start2.Value.ToString()));

            DateTime to = Convert.ToDateTime(this.de_to2.Value);
            to = to.Add(TimeSpan.Parse(this.cb_end2.Value.ToString()));

            deviceBooking.Loan_DateTime = from;
            deviceBooking.Plan_To_ReDateTime = to;
        }
        if (record["Real_ReDateTime"] != null
            && record["Real_ReDateTime"].ToString() != string.Empty)
        {
            deviceBooking.Real_ReDateTime = Convert.ToDateTime(record["Real_ReDateTime"]);
        }
        var result = RecordManagment.EditBookingRecord(deviceBooking);
        string scriptText = @"
alert('update successfully!');
window.opener.location.reload();
window.close();";
        if (result <= 0)
        {// error
            scriptText = @"
alert('Can not submit the request, plz check the raw data and try again!');
";
            return;
        }
        ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {

    }
    protected void chb_recurrence_CheckedChanged(object sender, EventArgs e)
    {
        InitControlState();
        InitTimeComboData();
        InitControlData();
    }
    protected void rb_daily_weekly_CheckedChanged(object sender, EventArgs e)
    {
        this.chbl_weekly.Enabled = this.rb_weekly.Enabled ? this.rb_weekly.Checked : this.rb_weekly.Enabled;
        this.lbl_everyDay.Enabled = this.rb_daily.Enabled ? this.rb_daily.Checked : this.rb_daily.Enabled;
    }
    protected void chb_allDayevent_CheckedChanged(object sender, EventArgs e)
    {
        this.cb_start.Enabled = this.cb_end.Enabled = !chb_allDayevent.Checked;
        if (chb_allDayevent.Checked)
        {
            this.cb_start.SelectedIndex = this.cb_end.SelectedIndex = 0;
        }
    }

    protected void cb_project_SelectedIndexChanged(object sender, EventArgs e)
    {
        string pj_code = this.cb_project.Value.ToString();
        DataTable detail = settingsHandler.GetProjectTable(pj_code);
        if (detail != null)
        {
            foreach (DataRow row in detail.Rows)
            {
                this.tb_custName.Text = row["Cust_Name"].ToString();
                this.tb_pjStage.Text = row["pj_stage"].ToString();
                break;
            }
        }
    }
    #endregion
    #region private methods
    /// <summary>
    /// 初始化控件状态
    /// </summary>
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

        ViewType viewType = GetViewType();
        this.btn_submit.Text = viewType.ToString();
        switch (viewType)
        {
            case ViewType.VIEW:
                this.btn_submit.Enabled = false;
                break;
            case ViewType.EDIT:
                this.btn_submit.Enabled = true;
                break;
        }
    }
    /// <summary>
    /// 初始化时间列表数据
    /// </summary>
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
    /// <summary>
    /// 初始化控件绑定数据
    /// </summary>
    void InitControlData()
    {
        DataTable projectTable = settingsHandler.GetProjectTable();
        if (projectTable != null)
        {
            this.cb_project.DataSource = projectTable.DefaultView;
            this.cb_project.TextField = "PJ_Name";
            this.cb_project.ValueField = "PJ_Code";
            this.cb_project.DataBind();
        }

        DataTable purpose = settingsHandler.GetPurposeTable();
        if (purpose != null)
        {
            this.cb_testCategory.DataSource = purpose.DefaultView;
            this.cb_testCategory.TextField = "Name";
            this.cb_testCategory.ValueField = "ID";
            this.cb_testCategory.DataBind();
        }

        this.cb_status.Items.Clear();
        this.cb_status.Items.Add(RecordStatus.APPROVE_NORETURN.ToString(), (Int32)RecordStatus.APPROVE_NORETURN);
        this.cb_status.Items.Add(RecordStatus.NEW_NOSUBMIT.ToString(), (Int32)RecordStatus.NEW_NOSUBMIT);
        this.cb_status.Items.Add(RecordStatus.NEW_SUBMIT.ToString(), (Int32)RecordStatus.NEW_SUBMIT);
        this.cb_status.Items.Add(RecordStatus.REJECT.ToString(), (Int32)RecordStatus.REJECT);
        this.cb_status.Items.Add(RecordStatus.RETURN.ToString(), (Int32)RecordStatus.RETURN);
    }

    void ShowBookingData()
    {
        if (Request["booking_id"] != null)
        {
            string booking_id = Request["booking_id"].ToString();
            DataRow record = RecordManagment.GetRecord(booking_id);

            bool isRecurrence = Convert.ToBoolean(record["db_is_recurrence"]);
            this.chb_recurrence.Checked = isRecurrence;
            chb_recurrence_CheckedChanged(null, null);
            if (isRecurrence)
            {
                string[] recurrence = record["db_recurrence"].ToString().Split(',');
                foreach (string r in recurrence)
                {
                    RegularType regularType = (RegularType)Convert.ToInt32(r);
                    switch (regularType)
                    {
                        case RegularType.EveryDay:
                            this.rb_daily.Checked = true;
                            this.rb_weekly.Checked = false;
                            break;
                        case RegularType.MONDAY:
                        case RegularType.THURSDAY:
                        case RegularType.TUESDAY:
                        case RegularType.WEDNESDAY:
                        case RegularType.FRIDAY:
                        case RegularType.SATURDAY:
                        case RegularType.SUNDAY:
                            this.rb_daily.Checked = false;
                            this.rb_weekly.Checked = true;
                            foreach (ListEditItem item in chbl_weekly.Items)
                            {
                                if (Convert.ToInt32(item.Value) ==
                                    (Int32)regularType)
                                {
                                    item.Selected = true;
                                }
                            }
                            break;
                    }
                }
                rb_daily_weekly_CheckedChanged(null, null);

                DateTime from = Convert.ToDateTime(record["Loan_DateTime"]);
                DateTime to = Convert.ToDateTime(record["Plan_To_ReDateTime"]);
                TimeSpan start = TimeSpan.Parse(record["db_start"].ToString());
                TimeSpan end = TimeSpan.Parse(record["db_end"].ToString());

                this.de_from.Value = Convert.ToDateTime(from.ToString("yyyy/MM/dd"));
                this.de_to.Value = Convert.ToDateTime(to.ToString("yyyy/MM/dd"));
                this.cb_start.Value = start.ToString(@"hh\:mm");
                this.cb_end.Value = end.ToString(@"hh\:mm");
            }
            else
            {
                DateTime from = Convert.ToDateTime(record["Loan_DateTime"]);
                DateTime to = Convert.ToDateTime(record["Plan_To_ReDateTime"]);

                this.de_from2.Value = Convert.ToDateTime(from.ToString("yyyy/MM/dd"));
                this.de_to2.Value = Convert.ToDateTime(to.ToString("yyyy/MM/dd"));

                this.cb_start2.Value = from.ToString(@"hh\:mm");
                this.cb_end2.Value = to.ToString(@"hh\:mm");
            }
            this.tb_loaner.Text = record["Loaner_Name"].ToString();
            this.tb_deviceName.Text = record["Device_Name"].ToString();

            this.cb_status.Value = Convert.ToInt32(record["Status"]);
            this.cb_project.Value = record["Project_ID"];
            this.tb_custName.Text = record["Cust_Name"].ToString();
            this.tb_pjStage.Text = record["PJ_Stage"].ToString();

            this.cb_testCategory.Value = record["TestCategory_ID"];

            this.mo_comment.Text = record["Comment"].ToString();
            this.mo_reviewComment.Text = record["Review_Comment"].ToString();

        }
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
    #endregion

}