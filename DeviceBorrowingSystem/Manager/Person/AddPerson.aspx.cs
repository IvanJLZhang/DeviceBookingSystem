using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using BLL;
using GlobalClassNamespace;
using System.Text;
using System.Data;
//[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public partial class Manager_AddPerson : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitControlStateAndPropertyData();
            ShowUserDetail();
        }
    }
    #region property
    DataTable AddUserTable
    {
        get
        {
            if (Session["AddUserTable"] == null)
            {

                return null;
            }
            return (DataTable)Session["AddUserTable"];
        }
        set
        {
            Session["AddUserTable"] = value;
        }

    }
    #endregion
    #region control event
    protected void ibtn_check_Click(object sender, ImageClickEventArgs e)
    {
        string uid = this.tb_uid.Text.Trim();
        LDAPHelper ldap = new LDAPHelper();
        string errMsg = String.Empty;

        var person = ldap.CheckOneUserInDB(uid);
        if (person != null)
        {
            this.tb_uid.Text = string.Empty;
            errMsg = "The person is in the system already, plz try someone else!";
            Response.Write("<script>alert('" + errMsg + "')</script>");
        }
    }
    /// <summary>
    /// new a person
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        ibtn_check_Click(null, null);
        var viewType = GetViewType();
        switch (viewType)
        {
            case ViewType.VIEW:
                break;
            case ViewType.EDIT:
                break;
            case ViewType.ADD:

                if (this.AddUserTable == null)
                {
                    AddUserTable = new DataTable();
                    #region person table init
                    AddUserTable.Columns.Add("P_ID");
                    AddUserTable.Columns["P_ID"].Unique = true;
                    AddUserTable.Columns.Add("P_Name");
                    AddUserTable.Columns.Add("P_Department");
                    AddUserTable.Columns.Add("P_ChargeDepartment");
                    AddUserTable.Columns.Add("P_Email");
                    AddUserTable.Columns.Add("P_Role");
                    AddUserTable.Columns.Add("P_ExNumber");
                    AddUserTable.Columns.Add("P_Location");
                    AddUserTable.Columns.Add("P_Activate");
                    #endregion
                }
                DataRow PersonOne = this.AddUserTable.NewRow();
                PersonOne["P_ID"] = this.tb_uid.Text;
                PersonOne["P_Name"] = String.Empty;
                PersonOne["P_Department"] = this.cb_dpt.Text;
                PersonOne["P_ChargeDepartment"] = this.cb_chargeDpt.Text;
                PersonOne["P_Email"] = this.tb_email.Text;
                PersonOne["P_ExNumber"] = String.Empty;
                PersonOne["P_Role"] = Convert.ToInt32(this.cb_role.Value);
                PersonOne["P_Location"] = this.cb_site.Value.ToString();
                PersonOne["P_Activate"] = this.chb_ativate.Checked;

                try
                {
                    this.pnl_AddUserTable.Visible = true;
                    this.AddUserTable.Rows.Add(PersonOne);

                    this.gv_addedUser.DataSource = this.AddUserTable.DefaultView;
                    this.gv_addedUser.KeyFieldName = "P_ID";
                    this.gv_addedUser.DataBind();
                }
                catch { }
                break;

        }
    }
    /// <summary>
    /// submit the person table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        if (this.AddUserTable == null || this.AddUserTable.Rows.Count <= 0)
            return;
        int cnt = 0;
        foreach (DataRow person in this.AddUserTable.Rows)
        {
            bool activate = Convert.ToBoolean(person["P_Activate"]);
            if (activate)
            {// 只有帐号被激活以后才添加进用户表并发送通知邮件
                if (personManageFac.InsertOnePerson(person) > 0)
                {// send confirm email
                    // 需要将new person中的条目删除
                    personManageFac.DeleteNewPersonByID(person["P_ID"].ToString());
                    cnt++;
                    string id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + cnt;
                    SendConfirmEmail(id, person);
                }
                else
                {// show some error message

                }
            }
        }
        if (cnt > 0)
        {
            Response.Write(@"
<script>
    alert('Add " + cnt + @" person successfully!');
    window.opener.location.href = window.opener.location.href;
    window.close();
</script>");
        }
    }

    protected void gv_addedUser_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        string uid = e.Keys["P_ID"].ToString();
        foreach (DataRow person in this.AddUserTable.Rows)
        {
            if (person["P_ID"].Equals(uid))
            {
                this.AddUserTable.Rows.Remove(person);
                break;
            }
        }
        ShowAddUser();
    }
    #endregion
    #region methods
    void ShowAddUser()
    {
        if (this.AddUserTable == null || this.pnl_AddUserTable.Visible == false)
            return;
        this.gv_addedUser.DataSource = this.AddUserTable.DefaultView;
        this.gv_addedUser.KeyFieldName = "P_ID";
        this.gv_addedUser.DataBind();
    }
    void InitControlStateAndPropertyData()
    {
        // site data
        DataTable siteList = settingsHandler.GetSiteList();
        if (siteList != null)
        {
            this.cb_site.DataSource = siteList.DefaultView;
            this.cb_site.ValueField = "site_addr";
            this.cb_site.TextField = "site_addr";
            this.cb_site.DataBind();
        }
        // role data
        if (Session["Role"] != null)
        {
            this.cb_role.Items.Clear();
            UserRole role = (UserRole)Convert.ToInt32(Session["Role"]);
            switch (role)
            {
                case UserRole.USER:// if the role is user, view type is view only.
                    this.cb_role.Items.Add(UserRole.USER.ToString(), (Int32)UserRole.USER);

                    this.chb_ativate.Enabled = false;
                    break;
                case UserRole.REVIEWER:
                    this.cb_role.Items.Add(UserRole.USER.ToString(), (Int32)UserRole.USER);
                    //this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (Int32)UserRole.REVIEWER);
                    this.chb_ativate.Enabled = true;
                    break;
                case UserRole.LEADER:
                    this.cb_role.Items.Add(UserRole.USER.ToString(), (Int32)UserRole.USER);
                    this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (Int32)UserRole.REVIEWER);
                    this.chb_ativate.Enabled = true;
                    break;
                case UserRole.ADMIN:
                    this.cb_role.Items.Add(UserRole.USER.ToString(), (Int32)UserRole.USER);
                    this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (Int32)UserRole.REVIEWER);
                    this.cb_role.Items.Add(UserRole.LEADER.ToString(), (Int32)UserRole.LEADER);
                    //this.cb_role.Items.Add(UserRole.ADMIN.ToString(), (Int32)UserRole.ADMIN);
                    this.chb_ativate.Enabled = true;
                    break;
                default:
                    break;
            }
        }
        ViewType viewType = GetViewType();
        this.btn_Save.Text = viewType.ToString();
        switch (viewType)
        {
            case ViewType.VIEW:
                this.btn_Save.Enabled = false;
                this.ibtn_check.Visible = false;
                this.tb_uid.Width = 250;
                break;
            case ViewType.EDIT:
                this.btn_Save.Enabled = true;
                this.ibtn_check.Visible = false;
                this.tb_uid.Width = 250;
                break;
            case ViewType.ADD:
                this.btn_Save.Enabled = true;
                this.btn_Save.Text = "NEW";
                this.ibtn_check.Visible = true;
                this.tb_uid.Width = 210;
                this.tb_uid.ReadOnly = false;
                this.AddUserTable = null;
                this.pnl_AddUserTable.Visible = false;
                break;
            case ViewType.DETELE:
                break;
            case ViewType.NULL:
                break;
            default:
                break;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="personInfo"></param>
    private void SendConfirmEmail(string id, DataRow personInfo)
    {
        string MailTo = personInfo["P_Email"].ToString();

        // 生成验证码
        LoginHelper loginHelper = new LoginHelper();
        string cleartext = loginHelper.GenerateCaptchaCleartext(6);
        string ciphertext = loginHelper.GenerateCaptchaCiphertext(cleartext, personInfo["P_ID"].ToString());
        loginHelper.InsertCaptchaToDB(cleartext, ciphertext);

        string subject = "[Device Borrowing System]--Account Activated Verification";
        string html = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/Login.aspx";

        string link = html + "?uid=" + personInfo["P_ID"].ToString() + @"&captcha=" + cleartext;
        string body = String.Empty;
        body += @"
<h3>Hi, " + personInfo["P_ID"] + @"</h2>
<div style='margin-left:20px'>

	<p>Your account was activated by administrator, please sign in the system by click the below hyperlink to finish your register information!
	    <br/>
	    <br/>
	    <a href='" + link + @"'>" + link + @"</a>
	</p>
	<hr style='margin-top:50px'/>
	<p>This mail was sent automatically, please do not try to reply, thanks for your coorperation!</p>
</div>";
        MailService.AddOneMailService(id, MailTo, "", subject, body.ToString(), true, "");
    }
    /// <summary>
    /// 显示已经存在的用户的信息
    /// </summary>
    void ShowUserDetail()
    {
        string uid = String.Empty;
        if (Request["uid"] != null)
        {
            uid = Request["uid"].ToString();
        }
        DataRow personInfo = personManageFac.GetNewPersonDetailByID(uid);
        if (personInfo == null)
        {
            this.tb_uid.Text = String.Empty;
            this.tb_email.Text = String.Empty;
            this.cb_dpt.Text = String.Empty;
            this.cb_chargeDpt.Text = String.Empty;
            this.cb_role.Value = string.Empty;
            this.cb_site.Value = String.Empty;

            this.chb_ativate.Checked = false;
        }
        else
        {
            this.tb_uid.Text = personInfo["P_ID"].ToString();
            this.tb_email.Text = personInfo["P_Email"].ToString();

            this.cb_dpt.Text = personInfo["P_Department"].ToString();
            //this.cb_chargeDpt.Text = personInfo["P_ChargeDepartment"].ToString();
            this.cb_role.Value = personInfo["P_Role"].ToString();
            //this.cb_site.Value = personInfo["P_Location"].ToString();

            //this.chb_ativate.Checked = Convert.ToBoolean(personInfo["P_Activate"]);
        }
    }
    #endregion


}