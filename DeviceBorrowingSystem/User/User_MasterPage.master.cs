using BLL;
using GlobalClassNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_User_MasterPage : System.Web.UI.MasterPage
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
    private void ShowNotification()
    {
        string loanerID = Session["UserID"].ToString();
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
        this.lbl_unSubmit.Text = deviceBookingManage.GetUnSubmitCntByLoanerID(loanerID).ToString();
        this.lbl_Unapproved.Text = deviceBookingManage.GetUnApprovedCntByLoanerID(loanerID).ToString();
        this.lbl_approved.Text = deviceBookingManage.GetApprovedCntByLoanerID(loanerID).ToString();
        this.lbl_Rejected.Text = deviceBookingManage.GetRejectedCntByLoanerID(loanerID).ToString();
        this.lbl_Sum.Text = deviceBookingManage.GetAllRequestCntByLoanerID(loanerID).ToString();
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
        //int role = Int32.Parse(Session["Role"].ToString());
        //// 确定角色访问权限
        //if ((role / 10) <= 0)
        //{// 权限为普通用户
        //    Response.Redirect("../UserLogin.aspx");
        //    return false;
        //}
        return true;
    }
    public HiddenField hf_TimeZoneCtrl
    {
        get { return hf_TimeZone; }
        set { hf_TimeZone = value; }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        double timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
        if (hf_TimeZone.Value.CompareTo(String.Empty) != 0)
        {
            double clientTimeZone = Convert.ToDouble(hf_TimeZone.Value);

            this.lbl_Time.Text = DateTime.Now.AddHours(clientTimeZone - timezone).ToString("yyyy-MM-dd HH:mm");

            //this.Timer1.Interval = 300000;
        }
    }

    public ScriptManager scriptManager1
    {
        get { return this.scriptManager1; }
        set { this.scriptManager1 = value; }
    }
    protected void lb_UserName_Click(object sender, EventArgs e)
    {
        string id = Session["UserID"].ToString();
        ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "window.open('../Manager/Person/UserInfo.aspx?type=0&uid=" + id + "', '_blank', '');", true);
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
        UserRole role = (UserRole)Convert.ToInt32(Session["Role"]);
        if (role != UserRole.USER)
        {
            Response.Redirect("../Manager/Default.aspx");
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
            string bookingParameters = e.CommandArgument.ToString() + "$";
            Response.Write("<script>window.open('FillBorrowingInfo.aspx?device_id=" + bookingParameters + "', '', 'width=730px, scrollbars=yes');</script>");//width=750px
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
