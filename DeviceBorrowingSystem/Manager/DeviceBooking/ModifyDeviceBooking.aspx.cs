using BLL;
using GlobalClassNamespace;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_DeviceBooking_ModifyDeviceBooking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowDeviceBookingInfo();
        }
    }


    private void ShowDeviceBookingInfo()
    {
        if (ViewState["BookingData"] == null)
        {
            if (Request["id"] == null)
                return;
            string deviceBookingID = Request["id"].ToString();
            cl_DeviceBookingManage deviceBooking = new cl_DeviceBookingManage();
            DataTable deviceBookingInfo = deviceBooking.GetBookingInfoByID(deviceBookingID);
            if (deviceBookingInfo != null)
            {
                deviceBookingInfo.Columns.Add("Loaner_Dpt", typeof(String));
                DataRow row = deviceBookingInfo.Rows[0];
                string loanerID = deviceBookingInfo.Rows[0]["Loaner_ID"].ToString();
                cl_PersonManage person = new cl_PersonManage();
                DataTable personInfo = person.GetPersonInfoByID(loanerID);
                if (personInfo != null)
                {
                    row["Loaner_Dpt"] = personInfo.Rows[0]["P_Department"];
                }
                //row["Booking_ID"] = deviceBookingID + "_0";

                cl_DeviceManage device = new cl_DeviceManage();
                DataTable deviceInfo = device.GetDeviceDetailByID(deviceBookingInfo.Rows[0]["Device_ID"].ToString());
                if (deviceInfo != null)
                {
                    this.lbtn_DeviceName.Text = deviceInfo.Rows[0]["s_name"].ToString();
                    this.lbtn_DeviceName.Attributes.Add("DeviceID", deviceBookingInfo.Rows[0]["Device_ID"].ToString());
                }



                DateTime startDateTime = Convert.ToDateTime(row["Loan_DateTime"]);
                DateTime endDateTime = Convert.ToDateTime(row["Plan_To_ReDateTime"]);
                row["Loan_DateTime"] = Convert.ToDateTime(GetTimeZoneDateTimeString(startDateTime.ToString("yyyy/MM/dd HH:mm")));
                row["Plan_To_ReDateTime"] = Convert.ToDateTime(GetTimeZoneDateTimeString(endDateTime.ToString("yyyy/MM/dd HH:mm")));

                this.hf_endDateTime.Value = Convert.ToDateTime(row["Plan_To_ReDateTime"]).ToString("yyyy/MM/dd HH:mm");


                ViewState["BookingData"] = deviceBookingInfo;
                this.dl_FillBorrowingInfo.DataSource = deviceBookingInfo.DefaultView;
                this.dl_FillBorrowingInfo.DataKeyField = "Booking_ID";
                this.dl_FillBorrowingInfo.DataBind();


            }
        }
        else
        {
            this.dl_FillBorrowingInfo.DataSource = ViewState["BookingData"];
            this.dl_FillBorrowingInfo.DataKeyField = "Booking_ID";
            this.dl_FillBorrowingInfo.DataBind();
        }
    }
    protected void lbtn_DeviceName_Click(object sender, EventArgs e)
    {
        string deviceId = lbtn_DeviceName.Attributes["DeviceID"];
        string scriptText = "var obj = window.showModalDialog('../Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
        Response.Write("<script>" + scriptText + "</script>");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddl_EndTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl_EndTime = (DropDownList)sender;
        DataListItem item = (DataListItem)((ddl_EndTime).Parent);

        Label lbl_startDateTime = (Label)item.FindControl("lbl_startDT");
        TextBox tb_endDate = (TextBox)item.FindControl("tb_EndDate");

        //HiddenField hf_EndDateTime = (HiddenField)item.FindControl("hf_endDateTime");

        DateTime startDateTime = Convert.ToDateTime(lbl_startDateTime.Text);
        DateTime newEndDateTime = Convert.ToDateTime(tb_endDate.Text.Trim() + " " + ddl_EndTime.SelectedValue);
        DateTime EndDateTime = Convert.ToDateTime(hf_endDateTime.Value);

        if (newEndDateTime <= startDateTime)
        {
            Response.Write("<script>alert('new End DateTime can not earlier than start DateTime!')</script>");
            SetEndDateTime(tb_endDate, ddl_EndTime, EndDateTime);
            return;
        }
        if (newEndDateTime > EndDateTime)
        {
            Response.Write("<script>alert('new End DateTime can not later than end DateTime!')</script>");
            SetEndDateTime(tb_endDate, ddl_EndTime, EndDateTime);
            return;
        }

        DataTable bookingInfo = (DataTable)ViewState["BookingData"];
        if (bookingInfo != null)
        {
            DataRow row = bookingInfo.Rows[item.ItemIndex];
            for (int index = item.ItemIndex + 1; index < bookingInfo.Rows.Count; index++)
            {// 删除当前行后面所有的行， 重新组织
                bookingInfo.Rows.RemoveAt(index);
                index--;
            }
            row["Plan_To_ReDateTime"] = newEndDateTime;

            if (Request["id"] != null)
            {
                string newBookingid = Request["id"].ToString() + "_" + (item.ItemIndex + 1);
                //string newBookingid = Request["id"].ToString() + "_" + (item.ItemIndex + 1);
                DataRow newRow = bookingInfo.Rows.Add(row.ItemArray);
                newRow["Booking_ID"] = newBookingid;
                newRow["Loan_DateTime"] = newEndDateTime.AddMinutes(30);
                newRow["Plan_To_ReDateTime"] = EndDateTime;
            }

            ViewState["BookingData"] = bookingInfo;
            ShowDeviceBookingInfo();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void tb_EndDate_TextChanged(object sender, EventArgs e)
    {
        TextBox tb_endDate = (TextBox)sender;
        DataListItem item = (DataListItem)((tb_endDate).Parent);

        Label lbl_startDateTime = (Label)item.FindControl("lbl_startDT");
        DropDownList ddl_EndTime = (DropDownList)item.FindControl("ddl_EndTime");

        //HiddenField hf_EndDateTime = (HiddenField)item.FindControl("hf_endDateTime");

        DateTime startDateTime = Convert.ToDateTime(lbl_startDateTime.Text);
        DateTime newEndDateTime = Convert.ToDateTime(tb_endDate.Text.Trim() + " " + ddl_EndTime.SelectedValue);
        DateTime EndDateTime = Convert.ToDateTime(hf_endDateTime.Value);

        if (newEndDateTime <= startDateTime)
        {
            Response.Write("<script>alert('new End DateTime can not earlier than start DateTime!')</script>");
            SetEndDateTime(tb_endDate, ddl_EndTime, EndDateTime);
            return;
        }
        if (newEndDateTime > EndDateTime)
        {
            Response.Write("<script>alert('new End DateTime can not later than end DateTime!')</script>");
            SetEndDateTime(tb_endDate, ddl_EndTime, EndDateTime);
            return;
        }

        DataTable bookingInfo = (DataTable)ViewState["BookingData"];
        if (bookingInfo != null)
        {
            DataRow row = bookingInfo.Rows[item.ItemIndex];
            for (int index = item.ItemIndex + 1; index < bookingInfo.Rows.Count; index++)
            {// 删除当前行后面所有的行， 重新组织
                bookingInfo.Rows.RemoveAt(index);
                index--;
            }
            row["Plan_To_ReDateTime"] = newEndDateTime;
            //string bookingid = row["Booking_ID"].ToString();
            if (Request["id"] != null)
            {
                string newBookingid = Request["id"].ToString() + "_" + (item.ItemIndex + 1);
                //string newBookingid = Request["id"].ToString() + "_" + (item.ItemIndex + 1);
                DataRow newRow = bookingInfo.Rows.Add(row.ItemArray);
                newRow["Booking_ID"] = newBookingid;
                newRow["Loan_DateTime"] = newEndDateTime.AddMinutes(30);
                newRow["Plan_To_ReDateTime"] = EndDateTime;
            }
            ViewState["BookingData"] = bookingInfo;
            ShowDeviceBookingInfo();
        }
    }

    private void CheckEndDateTime(DateTime startDateTime, DateTime newEndDateTime, DateTime EndDateTime)
    {

    }
    protected void dl_FillBorrowingInfo_ItemCreated(object sender, DataListItemEventArgs e)
    {
        DataListItem item = e.Item;
        if (ViewState["BookingData"] != null)
        {
            DataTable bookingData = (DataTable)ViewState["BookingData"];
            DataRow row = bookingData.Rows[item.ItemIndex];

            DropDownList ddl_EndTime = (DropDownList)item.FindControl("ddl_EndTime");
            DateTime endDateTime = Convert.ToDateTime(row["Plan_To_ReDateTime"]);
            ddl_EndTime.SelectedValue = endDateTime.ToString("HH:mm");

            DropDownList ddl_Project = (DropDownList)item.FindControl("ddl_Project");
            ddl_Project.SelectedValue = row["Project_ID"].ToString();

            DropDownList ddl_TestCategory = (DropDownList)item.FindControl("ddl_TestCategory");
            string tid = row["TestCategory_ID"].ToString();
            ddl_TestCategory.SelectedValue = row["TestCategory_ID"].ToString();

            DropDownList ddl_loanerDpt = (DropDownList)item.FindControl("ddl_loanerDpt");
            ddl_loanerDpt.SelectedValue = row["Loaner_Dpt"].ToString();

            DropDownList ddl_LoanerName = (DropDownList)item.FindControl("ddl_LoanerName");
            ddl_LoanerName.SelectedValue = row["Loaner_ID"].ToString();
        }
    }

    private void SetEndDateTime(TextBox tb_endDate, DropDownList ddl_endTime, DateTime endDateTime)
    {
        tb_endDate.Text = endDateTime.ToString("yyyy/MM/dd");
        ddl_endTime.SelectedValue = endDateTime.ToString("HH:mm");
    }



    protected void btn_Approve_Click(object sender, EventArgs e)
    {
        if (Request["id"] != null)
        {
            string bookingID = Request["id"].ToString();
            cl_DeviceBookingManage devicebooking = new cl_DeviceBookingManage();
            // 删除原有借用记录
            if (devicebooking.DeleteBookingItemByID(bookingID))
            {
                for (int index = 0; index != this.dl_FillBorrowingInfo.Items.Count; index++)
                {
                    DataListItem item = this.dl_FillBorrowingInfo.Items[index];

                    tbl_DeviceBooking bookingInfo = new tbl_DeviceBooking();
                    bookingInfo.ID = this.dl_FillBorrowingInfo.DataKeys[index].ToString();

                    var ddl_LoanerName = (DropDownList)item.FindControl("ddl_LoanerName");
                    bookingInfo.Loaner_ID = ddl_LoanerName.SelectedValue;

                    bookingInfo.Device_ID = this.lbtn_DeviceName.Attributes["DeviceID"];

                    var ddl_Project = (DropDownList)item.FindControl("ddl_Project");
                    bookingInfo.Project_ID = ddl_Project.SelectedValue;

                    var ddl_TestCategory = (DropDownList)item.FindControl("ddl_TestCategory");
                    bookingInfo.TestCategory_ID = ddl_TestCategory.SelectedValue;

                    var tb_PJstage = (TextBox)item.FindControl("tb_PjStage");
                    bookingInfo.PJ_Stage = tb_PJstage.Text.Trim();

                    Label lbl_startDateTime = (Label)item.FindControl("lbl_startDT");
                    TextBox tb_endDate = (TextBox)item.FindControl("tb_EndDate");
                    DropDownList ddl_EndTime = (DropDownList)item.FindControl("ddl_EndTime");

                    double clientTimeZone = GetClientTimeZone();
                    DateTime startDateTime = Convert.ToDateTime(lbl_startDateTime.Text);
                    DateTime endDateTime = Convert.ToDateTime(tb_endDate.Text + " " + ddl_EndTime.SelectedValue);
                    startDateTime = startDateTime.AddHours(clientTimeZone - 8);
                    endDateTime = endDateTime.AddHours(clientTimeZone - 8);

                    bookingInfo.Loan_DateTime = startDateTime;
                    bookingInfo.Plan_TO_ReDateTime = endDateTime;

                    bookingInfo.Status = 2;

                    TextBox tb_comment = (TextBox)item.FindControl("tb_Comment");
                    bookingInfo.Comment = tb_comment.Text.Trim();

                    bookingInfo.Reviewer_ID = Session["UserID"].ToString();

                    TextBox tb_ReviewComment = (TextBox)item.FindControl("tb_ReviewComment");
                    bookingInfo.Review_Comment = tb_ReviewComment.Text.Trim();

                    cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
                    if (deviceBookingManage.AddBookingItem(bookingInfo))
                    {
                        {// Mail Service
                            string id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + index;
                            cl_PersonManage person = new cl_PersonManage();
                            DataTable tbl = person.GetPersonInfoByID(bookingInfo.Loaner_ID);
                            if (tbl == null)
                                continue;
                            string MailTo = tbl.Rows[0]["P_Email"].ToString();//Chris_Tsung@Wistron.com;@wistron.local;May_Lai@wistron.com
                            string cc = "";
                            tbl = person.GetPersonInfoByID(Session["UserID"].ToString());
                            if (tbl != null)
                            {
                                cc = tbl.Rows[0]["P_Email"].ToString();
                            }
                            //"mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;";//mike_yt_chen@wistron.com;gary_ky_li@wistron.com;wennie_weng@wistron.com;Bruce_CH_Yang@wistron.com;Lucien_Tseng@wistron.com;Simon_Hsieh@wistron.com;Edmund_Huang@wistron.com;Ling_Huang@wistron.com;

                            StringBuilder Subject = new StringBuilder();
                            Subject.Append("[Device Borrowing System]--Borrow Modify, Approve");

                            StringBuilder body = new StringBuilder();
                            body.Append("<h2>Borrow Information: </h2><br/><br/>");
                            body.Append("<table border='1'><tr>");
                            body.Append("<th>Booking ID</th>");
                            body.Append("<th>Device Name</th>");
                            //body.Append("<th>Loaner Name</th>");
                            body.Append("<th>Start DateTime</th>");
                            body.Append("<th>End DateTime</th>");
                            body.Append("<th>Approve Reason</th>");
                            body.Append("</tr>");

                            //cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
                            //for (int index = 0; index != bookingList.Rows.Count; index++)
                            //{
                                body.Append("<tr>");
                                body.Append("<td>" + bookingInfo.ID+ "</td>");
                                body.Append("<td>" + this.lbtn_DeviceName.Text + "</td>");
                                //body.Append("<td>" + bookingList.Rows[index]["P_Name"].ToString() + "</td>");
                                body.Append("<td>" + bookingInfo.Loan_DateTime + "</td>");
                                body.Append("<td>" + bookingInfo.Plan_TO_ReDateTime + "</td>");
                                body.Append("<td>" + bookingInfo.Review_Comment + "</td>");
                                body.Append("</tr>");
                            //}
                            body.Append("</table><br/>");
                            body.Append("Please sign in the borrowing system to check the detail record!<br/>");
                            body.Append("http://tpeota01.whq.wistron:88/");

                            MailService.AddOneMailService(id, MailTo.ToString(), cc.ToString(), Subject.ToString(), body.ToString(), true, "");
                        }
                        //GlobalClass.PopMsg(this.Page, deviceBookingManage.errMsg);
                        //return;
                    }
                }
                Response.Write("<script>alert('Modify Booking sheet successfully!');window.returnValue = 'OK'; window.close();</script>");
            }
        }

    }

    public string GetTimeZoneDateTimeString(string dateTime)
    {
        if (dateTime.CompareTo(String.Empty) != 0)
        {
            DateTime time = DateTime.Parse(dateTime);
            time = time.AddHours(GetClientTimeZone() - 8);

            return time.ToString("yyyy/MM/dd HH:mm");
        }
        return String.Empty;
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
}