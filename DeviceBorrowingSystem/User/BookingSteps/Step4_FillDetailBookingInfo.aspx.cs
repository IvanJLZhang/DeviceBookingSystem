using BLL;
using DataBaseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_BookingSteps_Step4_FillDetailBookingInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitUIControl();
        }
    }
    #region SessionData
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
    #region methods
    void InitUIControl()
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
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["STEP"] = 1;
        Response.Write("<script>parent.location.href = '../BookingPage.aspx'</script>");
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        List<string> ids = new List<string>();
        foreach (DataRow row in this.NewBookingData.Rows)
        {
            tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
            switch (this.cat)
            {
                case Category.Device:
                    deviceBooking.Booking_ID = BookingPageFactory.GenNewBookingID("BD");
                    break;
                case Category.Equipment:
                    deviceBooking.Booking_ID = BookingPageFactory.GenNewBookingID("BE");
                    break;
                case Category.Chamber:
                    deviceBooking.Booking_ID = BookingPageFactory.GenNewBookingID("BC");
                    break;
                default:
                    break;
            }
            if (Session["UserID"] != null)
                deviceBooking.Loaner_ID = Session["UserID"].ToString();

            deviceBooking.Device_ID = row["s_id"].ToString();
            deviceBooking.Project_ID = this.cb_project.Value.ToString();
            deviceBooking.PJ_Stage = this.tb_pjStage.Text;
            deviceBooking.TestCategory_ID = this.cb_testCategory.Value.ToString();
            deviceBooking.Comment = this.mo_comment.Text.Trim();

            deviceBooking.Status = (Int32)RecordStatus.NEW_SUBMIT;

            deviceBooking.Loan_DateTime = Convert.ToDateTime(row["Loan_DateTime"]);
            deviceBooking.Plan_To_ReDateTime = Convert.ToDateTime(row["Plan_To_ReDateTime"]);

            deviceBooking.db_is_recurrence = Convert.ToBoolean(row["db_is_recurrence"]);
            if (deviceBooking.db_is_recurrence)
            {
                deviceBooking.db_recurrence = row["db_recurrence"].ToString();
                deviceBooking.db_start = TimeSpan.Parse(row["db_start"].ToString());
                deviceBooking.db_end = TimeSpan.Parse(row["db_end"].ToString());
            }

            var result = BookingPageFactory.NewBookingRecord(deviceBooking);
            if (result <= 0)
            {// error
                Response.Write(@"
<script>
    alert('Can not submit the request, plz check the raw data and try again!');
</script>");
                return;
            }
            else
            {
                ids.Add(deviceBooking.Booking_ID);
            }
        }

        {// Mail service 
            DataTable bookingList = BookingPageFactory.GetBookingItemsByIDs(ids);
            SendMail(bookingList);
        }

        // 返回首页
        Session["STEP"] = 1;
        Response.Write(@"
<script>
    alert('Submit application successfully, plz wait for proposing.');
    parent.location.href = '../BookingPage.aspx';
</script>");
    }

    private void SendMail(DataTable bookingList)
    {
        if (bookingList == null || bookingList.Rows.Count <= 0)
            return;

        string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string MailTo = "";
        string LoanerName = String.Empty;
        if (Session["UserName"] != null)
            LoanerName = Session["UserName"].ToString();

        StringBuilder Subject = new StringBuilder();
        Subject.Append("[Device Borrowing System]--Device/Equipment borrow");

        StringBuilder body = new StringBuilder();
        body.Append("<h2>Borrow Information: </h2><br/><br/>");
        body.Append("<table border='1'><tr>");
        body.Append("<th>Booking ID</th>");
        body.Append("<th>Device Name</th>");
        body.Append("<th>Category</th>");
        body.Append("<th>Loaner Name</th>");
        body.Append("<th>Start DateTime</th>");
        body.Append("<th>End DateTime</th>");
        body.Append("<th>Borrow Reason</th>");
        body.Append("</tr>");
        ArrayList dptList = new ArrayList();
        for (int index = 0; index != bookingList.Rows.Count; index++)
        {
            body.Append("<tr>");
            body.Append("<td>" + bookingList.Rows[index]["Booking_ID"].ToString() + "</td>");
            body.Append("<td>" + bookingList.Rows[index]["Device_Name"].ToString() + "</td>");

            int cat = Convert.ToInt32(bookingList.Rows[index]["s_category"]);
            string category = Enum.GetName(typeof(Category), cat);

            body.Append("<td>" + category + "</td>");
            body.Append("<td>" + bookingList.Rows[index]["P_Name"].ToString() + "</td>");

            DateTime sdt = Convert.ToDateTime(bookingList.Rows[index]["Loan_DateTime"]);
            DateTime edt = Convert.ToDateTime(bookingList.Rows[index]["Plan_To_ReDateTime"]);

            string sdts = sdt.ToString("yyyy/MM/dd HH:mm");
            string edts = edt.ToString("yyyy/MM/dd HH:mm");

            body.Append("<td>" + sdts + "</td>");
            body.Append("<td>" + edts + "</td>");
            //string comment = bookingList.Rows[index]["Comment"].ToString();
            body.Append("<td>" + bookingList.Rows[index]["Comment"].ToString() + "</td>");
            body.Append("</tr>");

            string ownerID = bookingList.Rows[index]["Owner_ID"].ToString();

            DataRow personInfo = personManageFac.GetPersonDetailByID(ownerID);
            if (personInfo != null)
            {
                // get reviewer list
                DataTable reviewerList = personManageFac.GetPersonList(UserRole.REVIEWER, personInfo["P_Department"].ToString());
                if (reviewerList != null)
                {
                    foreach (DataRow reviewer in reviewerList.Rows)
                    {
                        string email = reviewer["P_Email"].ToString();
                        if (!MailTo.Contains(email))
                        {
                            MailTo += email + ";";
                        }
                    }
                }
            }
        }
        body.Append("</table><br/>");
        body.Append("Please sign in the borrowing system and approve/reject the item!<br/>");

        string html = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/";
        body.Append(html);
        if (MailTo == String.Empty)
        {
            MailTo = "Ivan_JL_Zhang@wistron.com";
        }

        string cc = "";
        DataTable adminList = personManageFac.GetPersonListByRole(UserRole.ADMIN);
        if (adminList != null)
        {
            foreach (DataRow admin in adminList.Rows)
            {
                string email = admin["P_Email"].ToString();
                if (!cc.Contains(email))
                {
                    cc += email + ";";
                }
            }
        }
        MailService.AddOneMailService(id, MailTo, cc.ToString(), Subject.ToString(), body.ToString(), true, "");
    }

    #endregion
}