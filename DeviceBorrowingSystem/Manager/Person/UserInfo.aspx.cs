using BLL;
using DataBaseModel;
using GlobalClassNamespace;
//using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Person_UserInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewType viewType = GetViewType();
            if (viewType == ViewType.ADD)
            {
                if (Request["uid"] != null)
                {
                    Response.Redirect("AddPerson.aspx?uid=" + Request["uid"] + "&type=" + (Int32)viewType + "");
                }
                else
                {
                    Response.Redirect("AddPerson.aspx?type=" + (Int32)viewType + "");
                }
            }
            else
            {
                ShowUserDetail();
            }
        }
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
                    this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (Int32)UserRole.REVIEWER);

                    this.chb_ativate.Enabled = true;

                    break;
                case UserRole.ADMIN:
                    this.cb_role.Items.Add(UserRole.USER.ToString(), (Int32)UserRole.USER);
                    this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (Int32)UserRole.REVIEWER);
                    this.cb_role.Items.Add(UserRole.LEADER.ToString(), (Int32)UserRole.LEADER);
                    this.cb_role.Items.Add(UserRole.ADMIN.ToString(), (Int32)UserRole.ADMIN);

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
                break;
            case ViewType.DETELE:
                break;
            case ViewType.NULL:
                break;
            default:
                break;
        }
    }
    void ShowUserDetail()
    {
        string uid = String.Empty;
        if (Request["uid"] != null)
        {
            uid = Request["uid"].ToString();
        }
        DataRow personInfo = personManageFac.GetPersonDetailByID(uid);
        if (personInfo == null)
        {
            this.tb_uid.Text = String.Empty;
            this.tb_uname.Text = String.Empty;
            this.tb_email.Text = String.Empty;
            this.tb_telphone.Text = String.Empty;

            this.cb_dpt.Text = String.Empty;
            this.cb_chargeDpt.Text = String.Empty;
            this.cb_role.Value = string.Empty;
            this.cb_site.Value = String.Empty;

            this.chb_ativate.Checked = false;
        }
        else
        {
            this.tb_uid.Text = personInfo["P_ID"].ToString();
            this.tb_uname.Text = personInfo["P_Name"].ToString();
            this.tb_email.Text = personInfo["P_Email"].ToString();
            this.tb_telphone.Text = personInfo["P_ExNumber"].ToString();

            this.cb_dpt.Text = personInfo["P_Department"].ToString();
            this.cb_chargeDpt.Text = personInfo["P_ChargeDepartment"].ToString();


            this.cb_role.Value = ((UserRole)Int32.Parse(personInfo["P_Role"].ToString())).ToString();
            //this.cb_role.Value = personInfo["P_Role"].ToString();

            this.cb_site.Value = personInfo["P_Location"].ToString();

            this.chb_ativate.Checked = Convert.ToBoolean(personInfo["P_Activate"]);
        }

        ViewType viewType = GetViewType();
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
                break;
            case ViewType.DETELE:
                break;
            case ViewType.NULL:
                break;
            default:
                break;
        }
    }

    ViewType GetViewType()
    {
        // check role
        ViewType viewType = ViewType.VIEW;// default is view
        this.cb_role.Items.Clear();
        this.cb_role.Items.Add(UserRole.USER.ToString(), (int)UserRole.USER);
        if (Session["Role"] != null)
        {
            UserRole role = (UserRole)Convert.ToInt32(Session["Role"]);
            switch (role)
            {
                case UserRole.USER:// if the role is user, view type is view only.
                    viewType = ViewType.VIEW;
                    break;
                case UserRole.REVIEWER:
                    //this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (int)UserRole.REVIEWER);
                    if (Request["type"] != null)
                    {
                        viewType = (ViewType)Convert.ToInt32(Request["type"]);
                    }
                    break;
                case UserRole.LEADER:
                    this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (int)UserRole.REVIEWER);
                    //this.cb_role.Items.Add(UserRole.LEADER.ToString(), (int)UserRole.LEADER);

                    if (Request["type"] != null)
                    {
                        viewType = (ViewType)Convert.ToInt32(Request["type"]);
                    }
                    break;
                case UserRole.ADMIN:
                    this.cb_role.Items.Add(UserRole.REVIEWER.ToString(), (int)UserRole.REVIEWER);
                    this.cb_role.Items.Add(UserRole.LEADER.ToString(), (int)UserRole.LEADER);
                    //this.cb_role.Items.Add(UserRole.ADMIN.ToString(), (int)UserRole.ADMIN);
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
    // update, add submit
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        var viewType = GetViewType();
        switch (viewType)
        {
            case ViewType.VIEW:
                break;
            case ViewType.EDIT:
                tbl_Person person = new tbl_Person();
                person.P_ID = this.tb_uid.Text;
                person.P_ChargeDepartment = this.cb_chargeDpt.Text.Trim();
                person.P_Department = this.cb_dpt.Text;
                person.P_Location = this.cb_site.Value.ToString();

                UserRole role;
                if (Enum.TryParse<UserRole>(this.cb_role.Value.ToString(), true, out role))
                {
                    person.P_Role = (Int32)role;
                }
                else
                {
                    person.P_Role = (Int32)UserRole.USER;
                }
                //person.P_Role = Convert.ToInt32(this.cb_role.Value);
                person.P_ExNumber = this.tb_telphone.Text;
                person.P_Activate = this.chb_ativate.Checked;
                person.P_Email = this.tb_email.Text;
                person.P_Name = this.tb_uname.Text;

                UserRole oper_role = (UserRole)Convert.ToInt32(Session["Role"]);
                string oper_id = Session["UserID"].ToString();

                this.btn_Save.JSProperties.Remove("cpMsg");
                if (personManageFac.UpdatePersonInfo(person, oper_role, oper_id) > 0)
                {
                    //bool activate = Convert.ToBoolean(this.hf_activate["Activate"]);
                    //if (!activate && this.chb_ativate.Checked)
                    //{// 帐号被激活， 发送通知邮件
                    //    string id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + cnt;
                    //    //SendConfirmEmail(id, person);
                    //}
                    if (oper_id == person.P_ID)
                    {
                        Session["Role"] = (Int32)person.P_Role;
                        DataTable LoginUserInfo = (DataTable)Session["LoginUserInfo"];
                        DataRow login_user = LoginUserInfo.Rows[0];
                        login_user["P_Role"] = (Int32)person.P_Role;
                        login_user["P_Department"] = person.P_Department;
                        login_user["P_ChargeDepartment"] = person.P_ChargeDepartment;
                        if (person.P_Role == 0)
                        {
                            Session["PrePage"] = null;
                        }
                    }
                    this.btn_Save.JSProperties["cpMsg"] = "update information successfully!";
                }
                else
                {
                    this.btn_Save.JSProperties["cpMsg"] = "something was wrong, update failure!";
                }
                Response.Write("<script>alert('" + this.btn_Save.JSProperties["cpMsg"] + "'); window.opener.location.reload();window.close();</script>");

                ShowUserDetail();
                break;
            case ViewType.ADD:
                #region Add
                //if (this.AddUserTable == null)
                //{
                //    AddUserTable = new DataTable();
                //    #region person table init
                //    AddUserTable.Columns.Add("P_ID");
                //    AddUserTable.Columns["P_ID"].Unique = true;
                //    AddUserTable.Columns.Add("P_Name");
                //    AddUserTable.Columns.Add("P_Department");
                //    AddUserTable.Columns.Add("P_ChargeDepartment");
                //    AddUserTable.Columns.Add("P_Email");
                //    AddUserTable.Columns.Add("P_Role");
                //    AddUserTable.Columns.Add("P_ExNumber");
                //    AddUserTable.Columns.Add("P_Location");
                //    AddUserTable.Columns.Add("P_Activate");
                //    #endregion
                //}
                //DataRow PersonOne = this.AddUserTable.NewRow();
                //PersonOne["P_ID"] = this.tb_uid.Text;
                //PersonOne["P_Name"] = this.tb_uname.Text;
                //PersonOne["P_Department"] = this.cb_dpt.Text;
                //PersonOne["P_ChargeDepartment"] = this.cb_chargeDpt.Text;
                //PersonOne["P_Email"] = this.tb_email.Text;
                //PersonOne["P_ExNumber"] = this.tb_telphone.Text;
                //PersonOne["P_Role"] = Convert.ToInt32(this.cb_role.Value);
                //PersonOne["P_Location"] = this.cb_site.Value.ToString();
                //PersonOne["P_Activate"] = this.chb_ativate.Checked;

                //try
                //{
                //    this.pnl_AddUserTable.Visible = true;
                //    this.AddUserTable.Rows.Add(PersonOne);

                //    this.gv_addedUser.DataSource = this.AddUserTable.DefaultView;
                //    this.gv_addedUser.KeyFieldName = "P_ID";
                //    this.gv_addedUser.DataBind();
                //}
                //catch { }
                #endregion
                break;

        }

    }
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

    private void SendConfirmEmail(string id, tbl_Person personInfo)
    {

        string MailTo = personInfo.P_Email;
        string subject = String.Empty;
        subject += "[Device Borrowing System]--Account Activate Verification";
        string body = String.Empty;
        body += @"
<div>
    
</div>
";
        //StringBuilder Subject = new StringBuilder();
        //Subject.Append("[Device Borrowing System]--Register Confirm");

        //StringBuilder body = new StringBuilder();
        //body.Append("<h2>Hi " + personInfo["P_Name"] + "</h2><br/>");
        //body.Append("Welcome to Device Borrowing System Web. This is a register confirm eamil, please click the link below to finish your register information: <br/>");
        //string url = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/Login.aspx?uname=" + personInfo["P_Name"] + "&email=" + MailTo + "&first=1";
        //body.Append("<a href='" + url + "'>" + GlobalClass.GetMD5Str(id) + GlobalClass.GetMD5Str(personInfo["P_Name"].ToString()) + "</a><br/><br/><br/>");
        //body.Append("This mail was sended by system automatically, please do not try to reply, thanks for you coorperation!");

        //MailService.AddOneMailService(id, MailTo, "", Subject.ToString(), body.ToString(), true, "");
    }
}