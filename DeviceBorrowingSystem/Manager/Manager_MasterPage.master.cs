using BLL;
using GlobalClassNamespace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Manager_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.Timer1.Interval = 1;
            CheckLogin();
            ShowNotification();
        }
    }
    DataTable LoginUserInfo
    {
        get
        {
            if (Session["LoginUserInfo"] == null)
                return null;
            else
                return (DataTable)Session["LoginUserInfo"];
        }
    }
    DataTable ChargeOfDptList
    {
        get
        {
            if (ViewState["ChargeOfDptList"] == null)
                return null;
            return (DataTable)ViewState["ChargeOfDptList"];
        }
        set
        {
            ViewState["ChargeOfDptList"] = value;
        }
    }
    private void ShowNotification()
    {
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        this.lbl_approved.Text = deviceBookingManage.GetApprovedCntByReviewerID(Session["UserID"].ToString()).ToString();
        this.lbl_Rejected.Text = deviceBookingManage.GetRejectedCnt().ToString();

        cl_PersonManage personManage = new cl_PersonManage();
        ChargeOfDptList = personManage.GetChargeOfDptListByUserInfo(LoginUserInfo);
        List<string> dptList = new List<string>();
        foreach (DataRow row in ChargeOfDptList.Rows)
        {
            if (row["DptValue"].ToString() != "0")
            {
                dptList.Add(row["DptValue"].ToString());
            }
        }
        this.lbl_newRequest.Text = deviceBookingManage.GetNewRequestCnt_Ex(dptList).ToString();
        if (Session["Category"] != null)
        {
            try
            {
                int category = Convert.ToInt32(Session["Category"]);
                this.lbl_allRecord.Text = deviceBookingManage.GetAllRecordByCategory(category).ToString();
                this.lbl_overdueRecord.Text = deviceBookingManage.GetOverdueingRecordByCategory(category).ToString();
            }
            catch
            {
                this.lbl_allRecord.Text = deviceBookingManage.GetAllRecordByCategory(1).ToString();
                this.lbl_overdueRecord.Text = deviceBookingManage.GetOverdueingRecordByCategory(1).ToString();

            }
        }
    }
    private bool CheckLogin()
    {
        if (Session["UserName"] == null || Session["UserID"] == null)
        {
            Session["UserName"] = null;
            Session["UserID"] = null;
            Session["PrePage"] = Request.Url.ToString();
            Response.Redirect("../Default.aspx");

            return false;
        }
        this.lb_UserName.Text = Session["UserName"].ToString();
        int role = Int32.Parse(Session["Role"].ToString());
        // 确定角色访问权限
        if ((role / 10) <= 0)
        {// 权限为普通用户
            Response.Redirect("../Default.aspx");
            return false;
        }

        if (Session["Category"] == null)
            Session["Category"] = 1;
        return true;
    }
    protected void lb_UserName_Click(object sender, EventArgs e)
    {
        string id = Session["UserID"].ToString();
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "window.open('./Person/UserInfo.aspx?type=1&uid=" + id + "', '_blank', '');", true);
    }
    protected void lb_QuitLogin_Click(object sender, EventArgs e)
    {
        Session["UserName"] = null;
        Session["UserID"] = null;
        Session["PrePage"] = null;
        Response.Redirect("../Default.aspx");

    }
    protected void lb_GoToBorrowingPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("../User/Home.aspx");
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        double timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
        if (hf_TimeZone.Value.CompareTo(String.Empty) != 0)
        {
            double clientTimeZone = Convert.ToDouble(hf_TimeZone.Value);
            this.lbl_Time.Text = DateTime.Now.AddHours(timezone - clientTimeZone).ToString("yyyy-MM-dd HH:mm");
            //this.Timer1.Interval = 60000;
        }

    }

    public string GetStr(string s1, string s2)
    {
        return s1 + ": " + s2;
    }
    public string GetToolTipStr(string name, string startdt, string enddt)
    {
        return name + ": " + startdt + "~" + enddt;
    }
    protected void dg_Remind_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("Approve") == 0)
        {
            string bookingId = e.CommandArgument.ToString();
            Response.Write("<script>window.open('ApprovePage.aspx?bookingId=" + bookingId + "', '', 'width=760px, scrollbars=yes, top=100px, left=100px' );</script>");
        }
    }
    protected void form1_Load(object sender, EventArgs e)
    {
        //if (this.hf_TimeZone.Value.CompareTo(String.Empty) != 0)
        //{
        //    int timeZone = Convert.ToInt32(this.hf_TimeZone.Value);
        //    if (timeZone <= 0)
        //        lbl_TimeZone.Text = "-GMT " + timeZone;
        //    else
        //        lbl_TimeZone.Text = "-GMT +" + timeZone;
        //}
        int timeZone = (int)GetClientTimeZone();
        if (timeZone <= 0)
            lbl_TimeZone.Text = "-GMT " + timeZone;
        else
            lbl_TimeZone.Text = "-GMT +" + timeZone;

        if (this.hf_TimeZone.Value.CompareTo(String.Empty) == 0)
        {
            this.hf_TimeZone.Value = timeZone.ToString();
        }
    }

    private double GetClientTimeZone()
    {
        string personSite = "";
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            System.Data.DataTable personInfo = person.GetPersonInfoByID(userID);
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
