using BLL;
using GlobalClassNamespace;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_FillBorrowingInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.PickDateTimeList == null)
            {
                Response.Write("<script>alert('select no device/equipment!'); window.close();</script>");
            }
            ShowDeviceBooking_Ex();
        }
    }
    private DataTable PickDateTimeList
    {
        get
        {
            if (Session["PickDateTimeList"] == null)
            {
                //DataTable table = new DataTable();
                //table.Columns.Add(new DataColumn("StartDateTime", typeof(DateTime)));
                //table.Columns.Add(new DataColumn("EndDateTime", typeof(DateTime)));
                //return table;
                return null;
            }
            else
                return Session["PickDateTimeList"] as DataTable;
        }
        set
        {
            Session["PickDateTimeList"] = value;
        }
    }
    private DataTable bookingList = new DataTable();
    private void ShowDeviceBooking()
    {
        string device_idStr = Request["device_id"].ToString().Substring(0, Request["device_id"].ToString().Length - 1);
        string[] device_idArr = device_idStr.Split('$');
        List<string> ids = new List<string>(device_idArr);

        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        bookingList = deviceBookingManage.GetBookingItemsByIDs(ids);
        double clientTimeZone = GetClientTimeZone();
        foreach (DataRow row in bookingList.Rows)
        {
            DateTime time = DateTime.Parse(row["Loan_DateTime"].ToString());
            time = time.AddHours(clientTimeZone - 8);
            row["Loan_DateTime"] = time;
            time = DateTime.Parse(row["Plan_To_ReDateTime"].ToString());
            time = time.AddHours(clientTimeZone - 8);
            row["Plan_To_ReDateTime"] = time;
        }
        this.GridView1.DataSource = bookingList.DefaultView;
        string[] dataKey = new string[1];
        dataKey[0] = "Booking_ID";
        this.GridView1.DataKeyNames = dataKey;

        this.GridView1.DataBind();

        ShowUserInfo();
    }
    private void ShowDeviceBooking_Ex() {
        if (this.PickDateTimeList != null)
        {
            if (this.PickDateTimeList.Rows.Count <= 0)
            {
                string text = "alert('select no device/equipment!'); window.close();window.opener.location.href = window.opener.location.href;";
                ScriptManager.RegisterStartupScript(upd2, this.GetType(), "onclick", text, true);
            }
            GridView1.DataSource = PickDateTimeList.DefaultView;
            GridView1.DataKeyNames = new string[] { "id" };
            GridView1.DataBind();
            ShowUserInfo();
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
    private void ShowUserInfo()
    {
        if (Session["UserID"] == null || Session["UserName"] == null)
            return;
        cl_PersonManage personManage = new cl_PersonManage();
        DataTable user = personManage.GetPersonInfoByID(Session["UserID"].ToString());
        if (user == null)
        {
            GlobalClass.PopMsg(this.Page, personManage.errMsg);
            return;
        }

        this.tb_Loanerid.Text = user.Rows[0]["P_ID"].ToString();
        this.tb_Loanername.Text = user.Rows[0]["P_Name"].ToString();
        this.tb_dpt.Text = user.Rows[0]["P_Department"].ToString();
        this.tb_email.Text = user.Rows[0]["P_Email"].ToString();
        this.tb_exnumber.Text = user.Rows[0]["P_Email"].ToString();
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.DropDownList1.SelectedItem.Value.ToString() == (String.Empty))
        {
            this.tb_custname.Text = this.DropDownList1.SelectedItem.Value.ToString();
        }
        else
            ShowCustName();
    }

    private void ShowCustName()
    {
        string pjid = this.DropDownList1.SelectedItem.Value.ToString();
        cl_ProjectManage projectManage = new cl_ProjectManage();
        this.tb_custname.Text = projectManage.GetCustNameByPJID(pjid);

        this.tb_pjStage.Text = settingsHandler.GetActivateProjectStage(pjid);
    }
    protected void DropDownList1_DataBound(object sender, EventArgs e)
    {
        //this.DropDownList1.SelectedIndex = 0;
        //ShowCustName();
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        #region 旧的
        //ShowDeviceBooking();
        //for (int index = 0; index != this.bookingList.Rows.Count; index++)
        //{
        //    DataRow row = this.bookingList.Rows[index];
        //    tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
        //    deviceBooking.ID = row["Booking_ID"].ToString().Trim();
        //    if (Session["UserID"] != null)
        //        deviceBooking.Loaner_ID = Session["UserID"].ToString();
        //    deviceBooking.Device_ID = row["Device_ID"].ToString().Trim();
        //    deviceBooking.Project_ID = this.DropDownList1.SelectedValue;
        //    deviceBooking.Loan_DateTime = DateTime.Parse(row["Loan_DateTime"].ToString());
        //    deviceBooking.Plan_TO_ReDateTime = DateTime.Parse(row["Plan_To_ReDateTime"].ToString());

        //    deviceBooking.TestCategory_ID = this.DropDownList2.SelectedValue;
        //    deviceBooking.PJ_Stage = this.tb_pjStage.Text.Trim();

        //    deviceBooking.Status = 1;
        //    deviceBooking.Comment = this.tb_Comment.Text.Trim();
        //    cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        //    if (!deviceBookingManage.UpdateBookingItemByID(deviceBooking))
        //    {
        //        GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
        //        return;
        //    }
        //}
        #endregion
        List<string> ids = new List<string>();
        for (int index = 0; index != this.PickDateTimeList.Rows.Count; index++)
        {
            DataRow row = this.PickDateTimeList.Rows[index];
            tbl_DeviceBooking deviceBooking = new tbl_DeviceBooking();
            if (Request["deviceClass"].ToString() == "2")
                deviceBooking.ID = GetBookingID("BE");
            else if (Request["deviceClass"].ToString() == "1")
                deviceBooking.ID = GetBookingID("BD");
            else if (Request["deviceClass"].ToString() == "3")
                deviceBooking.ID = GetBookingID("BC");
            if (Session["UserID"] != null)
                deviceBooking.Loaner_ID = Session["UserID"].ToString();
            deviceBooking.Device_ID = row["Device_ID"].ToString().Trim();
            deviceBooking.Project_ID = this.DropDownList1.SelectedItem.Value.ToString();

            double clientTimeZone = GetClientTimeZone();


            // 存储在database中数据统一采用服务器中的+8时区信息
            deviceBooking.Loan_DateTime = DateTime.Parse(row["StartDateTime"].ToString()).AddHours(8 - clientTimeZone);
            deviceBooking.Plan_TO_ReDateTime = DateTime.Parse(row["EndDateTime"].ToString()).AddHours(8 - clientTimeZone);
            deviceBooking.TestCategory_ID = this.DropDownList2.SelectedItem.Value.ToString();
            deviceBooking.PJ_Stage = this.tb_pjStage.Text.Trim();
            deviceBooking.Status = 1;
            deviceBooking.Comment = this.tb_Comment.Text.Trim();

            cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
            if (!deviceBookingManage.AddBookingItem(deviceBooking))
            {
                GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
                return;
            }

            ids.Add(deviceBooking.ID);
        }
        {// Mail Service
            cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
            DataTable bookingList1 = deviceBookingManage.GetBookingItemsByIDs(ids);
            SendMail(bookingList1);
        }
        this.PickDateTimeList.Clear();
        this.PickDateTimeList = null;
        //GlobalClass.PopMsg(this.Page, "Submit application successfully, plz wait for proposing.");
        //返回首页
        Response.Write("<script>alert('Submit application successfully, plz wait for proposing.');window.close();opener.location.href = opener.location.href;</script>");
    }
    private void SendMail(DataTable bookingList)
    {
        if (bookingList.Rows.Count <= 0)
            return;

        string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string cc = "";
        cl_PersonManage personManage = new cl_PersonManage();
        DataTable adminList = personManage.GetPersonListByRole(20);
        if (adminList.Rows.Count <= 0)
        {
            cc = "Ivan_JL_Zhang@wistron.com";
        }
        else
        {
            foreach (DataRow row in adminList.Rows)
            {
                if (row["P_Name"].ToString() != "admin")
                    cc += row["P_Email"].ToString() + ";";
            }
            //MailTo = "Chris_Tsung@Wistron.com;Ivan_JL_Zhang@wistron.local;May_Lai@wistron.com";//Chris_Tsung@Wistron.com;K1207A49@wistron.local;May_Lai@wistron.com
        }

        string MailTo = "";// "mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;";//mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;
        int role = 1;
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
            DataTable personInfo = personManage.GetPersonInfoByID(ownerID);
            if (personInfo != null && personInfo.Rows.Count > 0)
            {
                dptList.Add(personInfo.Rows[0]["P_Department"]);
            }
        }
        body.Append("</table><br/>");
        body.Append("Please sign in the borrowing system and approve/reject the item!<br/>");
        body.Append("http://tpeota01.whq.wistron:88/");

        DataTable reviewerList = null;
        if (dptList.Count > 0)
        {
            reviewerList = personManage.GetPersonListByRoleDpt(role, dptList);
        }
        if (reviewerList.Rows.Count <= 0)
        {
            MailTo = "Ivan_JL_Zhang@wistron.com";
        }
        else
        {
            foreach (DataRow row in reviewerList.Rows)
            {
                MailTo += row["P_Email"].ToString() + ";";
            }
        }
        MailService.AddOneMailService(id, MailTo.ToString(), cc.ToString(), Subject.ToString(), body.ToString(), true, "");
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");
    }
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string booking_id = GridView1.DataKeys[e.RowIndex].Value.ToString();
        for (int index = 0; index != this.PickDateTimeList.Rows.Count; index++) { 
            DataRow row = this.PickDateTimeList.Rows[index];
            if(row["id"].ToString().Equals(booking_id)){
                this.PickDateTimeList.Rows.RemoveAt(index);
                break;
            }
        }
        ShowDeviceBooking_Ex();
    }


    private string GetBookingID(string device_class)
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
}