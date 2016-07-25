using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_DeviceBooking_AddRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitWebControl();
        }
    }
    public DataTable Records
    {
        get
        {
            if (Session["Records"] == null)
                return null;
            else
                return (DataTable)Session["Records"];
        }
        set
        {
            Session["Records"] = value;
        }

    }

    private void ShowCustName()
    {
        string pjid = this.ddl_Project.SelectedItem.Value.ToString();
        cl_ProjectManage projectManage = new cl_ProjectManage();
        this.tb_CustName.Text = projectManage.GetCustNameByPJID(pjid);

        this.tb_PjStage.Text = settingsHandler.GetActivateProjectStage(pjid);
    }

    protected void tb_deviceID_TextChanged(object sender, EventArgs e)
    {
        string deviceIdName = this.tb_deviceID.Text.Trim();
        DataTable deviceArr = RecordManagment.GetDeviceByIDName(deviceIdName);
        if (deviceArr != null)
        {
            Session["d_category"] = deviceArr.Rows[0]["s_category"];
            this.cb_DeviceName.DataSource = deviceArr.DefaultView;
            this.cb_DeviceName.ValueField = "s_id";
            this.cb_DeviceName.TextField = "s_name";
            this.cb_DeviceName.DataBind();
        }
    }
    protected void ddl_loanerDpt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {

    }
    protected void ddl_loanerDpt_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.ddl_LoanerName.Value = "";
    }
    protected void btn_new_Click(object sender, EventArgs e)
    {
        tbl_DeviceBooking booking = new tbl_DeviceBooking();
        booking.ID = GenBookingID("BO");
        booking.Device_ID = this.cb_DeviceName.SelectedItem.Value.ToString();
        booking.Loaner_ID = this.ddl_LoanerName.SelectedItem.Value.ToString();
        booking.Reviewer_ID = Session["UserID"].ToString();

        booking.Status = Convert.ToInt32(this.cb_Status.SelectedItem.Value);
        if (booking.Status >= 3)
        {
            booking.Real_ReDateTime = DateTime.Now;
        }

        booking.Review_Comment = this.tb_ReviewComment.Text;
        booking.Comment = this.tb_Comment.Text;
        booking.Project_ID = this.ddl_Project.SelectedItem.Value.ToString();
        booking.TestCategory_ID = this.cb_TestCategory.SelectedItem.Value.ToString();
        booking.PJ_Stage = this.tb_PjStage.Text;

        double clientTimeZone = GetClientTimeZone();
        DateTime from = DateTime.Parse(this.tb_startDT.Text + " " + this.ddl_startTime.SelectedValue);
        DateTime to = DateTime.Parse(this.tb_EndDate.Text + " " + this.ddl_EndTime.SelectedValue);
        booking.Loan_DateTime = from;
        booking.Plan_TO_ReDateTime = to;
        if (this.Records == null) InitRecordsTable();

        DataRow record = this.Records.NewRow();
        record["Booking_ID"] = booking.ID;
        record["Loaner_ID"] = booking.Loaner_ID;
        record["Device_ID"] = booking.Device_ID;
        record["Project_ID"] = booking.Project_ID;
        record["TestCategory_ID"] = booking.TestCategory_ID;
        record["PJ_Stage"] = booking.PJ_Stage;
        record["Loan_DateTime"] = booking.Loan_DateTime;
        record["Plan_To_ReDateTime"] = booking.Plan_TO_ReDateTime;
        if (booking.Real_ReDateTime == null)
        {
            record["Real_ReDateTime"] = DBNull.Value;
        }
        else
        {
            record["Real_ReDateTime"] = booking.Real_ReDateTime;
        }
        record["Status"] = booking.Status;
        record["Comment"] = booking.Comment;
        record["Reviewer_ID"] = booking.Reviewer_ID;
        record["Date"] = DateTime.Now;
        record["Review_Comment"] = booking.Review_Comment;
        record["Device Name"] = this.cb_DeviceName.SelectedItem.Text;

        if (booking.Loan_DateTime > booking.Plan_TO_ReDateTime)
        {
            string msg = "please pick correct start and end datetime!";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", msg, true);
            return;
        }
        if (!RecordManagment.CheckRecord(record, this.Records))
        {
            this.Records.Rows.Add(record);
            ShowAddingRecords();
            InitWebControl();
        }
        else
        {
            string msg = "datetime conflict, please pick correct start and end datetime!";
            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", msg, true);
            return;
        }
    }
    private void InitRecordsTable()
    {

        if (this.Records == null)
        {
            this.Records = RecordManagment.GetBookingTableStruct();
        }
        this.Records.Columns.Add("Device Name");
    }
    private void ShowAddingRecords()
    {
        if (this.Records != null)
        {
            this.gv_records.DataSource = this.Records.DefaultView;
            this.gv_records.KeyFieldName = "Booking_ID";
            this.gv_records.DataBind();
        }
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        string msg = String.Empty;
        if (this.Records == null || this.Records.Rows.Count <= 0)
        {
            msg = "alert('Please add at least one record!');";
        }
        else
        {
            int cnt = RecordManagment.AddRecords(this.Records);
            msg = "alert('Add " + cnt + " records!');";
        }
        ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "click", msg, true);
        InitWebControl();
        if (this.Records != null)
            this.Records.Rows.Clear();
        this.ShowAddingRecords();
    }

    private string GenBookingID(string device_class)
    {
        if (this.Records == null || this.Records.Rows.Count <= 0)
        {
            cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();

            string serialNumber = deviceBookingManage.GetMaxBookingID(device_class);
            if (serialNumber != "0" && serialNumber != String.Empty)
            {
                string IDType = serialNumber.Substring(0, 2);
                string headDate = serialNumber.Substring(2, 8);
                int lastNumber = int.Parse(serialNumber.Substring(10));

                if (headDate == DateTime.Now.ToString("yyyyMMdd"))
                {
                    lastNumber++;
                    return IDType + headDate + lastNumber.ToString("0000");
                }
            }
            return device_class + DateTime.Now.ToString("yyyyMMdd") + "0001";
        }
        else
        {
            string lastBookingID = this.Records.Rows[this.Records.Rows.Count - 1]["Booking_ID"].ToString();
            int lastNumber = int.Parse(lastBookingID.Substring(10));
            lastNumber++;
            string newBookingID = lastBookingID.Substring(0, 10) + lastNumber.ToString("0000");
            return newBookingID;
        }
    }

    private double GetClientTimeZone()
    {
        string personSite = "";
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(userID);
            if (personInfo != null)
            {
                personSite = personInfo.Rows[0]["P_Location"].ToString();
            }
        }
        double clientTimeZone = 0.0;
        if (personSite.CompareTo("WKS") == 0 || personSite.CompareTo("WHC") == 0)
        {
            clientTimeZone = 8.0;
        }
        else
        {
            clientTimeZone = -6.0;
        }

        return clientTimeZone;
    }
    protected void gv_records_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        string key = e.Keys["Booking_ID"].ToString();
        foreach (DataRow record in this.Records.Rows)
        {
            if (key.Equals(record["Booking_ID"]))
            {
                this.Records.Rows.Remove(record);
                break;
            }
        }
        ShowAddingRecords();
    }

    private void InitWebControl()
    {
        this.tb_deviceID.Text = String.Empty;
        this.cb_DeviceName.Items.Clear();
        this.cb_DeviceName.Value = null;
        this.tb_startDT.Text = String.Empty;
        this.ddl_startTime.SelectedIndex = 0;
        this.tb_EndDate.Text = String.Empty;
        this.ddl_EndTime.SelectedIndex = 0;

        this.ddl_Project.Value = String.Empty;
        this.ddl_Project.SelectedIndex = -1;
        this.tb_CustName.Text = null;

        this.cb_TestCategory.Value = String.Empty;
        this.cb_TestCategory.SelectedIndex = -1;

        this.ddl_loanerDpt.Value = null;
        this.ddl_loanerDpt.SelectedIndex = -1;
        this.ddl_LoanerName.Value = null;
        this.ddl_LoanerName.SelectedIndex = -1;

        this.tb_Comment.Text = String.Empty;
        this.tb_ReviewComment.Text = String.Empty;
        this.tb_PjStage.Text = String.Empty;

        this.cb_Status.Value = null;
        this.cb_Status.SelectedIndex = 1;

    }
    protected void ddl_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowCustName();
    }
}