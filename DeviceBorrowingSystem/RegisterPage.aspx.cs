using BLL;
using DataBaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegisterPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitControlStateAndData();
        }
    }
    void InitControlStateAndData()
    {
        this.tb_uid.Text = this.tb_email.Text = this.tb_dpt.Text = String.Empty;
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        tbl_Person person = new tbl_Person();
        person.P_ID = this.tb_uid.Text.Trim();
        person.P_Email = this.tb_email.Text.Trim();
        person.P_Department = this.tb_dpt.Text.Trim().ToUpper();
        LDAPHelper ldap = new LDAPHelper();
        DataRow checkPerson = ldap.CheckOneUserInDB(person.P_ID);
        if (checkPerson != null)
        {
            Response.Write(@"
<script>
    alert('invalid uid!');
</script>");
            return;
        }
        if (personManageFac.NewUser(person) >= 1)
        {
            SendNewUserRequestEmail(person);

            Response.Write(@"
<script>
    alert('Your request was submit to administrator, please wait for feedback.');
    window.close();
</script>");
        }
        else
        {
            Response.Write("<script>alert('Sorry, something was wrong!');</script>");
        }
    }
    /// <summary>
    /// 发送新增用户请求邮件
    /// </summary>
    /// <param name="person"></param>
    void SendNewUserRequestEmail(tbl_Person person)
    {
        string mailTo = String.Empty;
        string cc = String.Empty;
        // get reviewer mail
        DataTable reviewerList = personManageFac.GetPersonList(UserRole.REVIEWER, person.P_Department);
        if (reviewerList != null)
        {
            foreach (DataRow reviewer in reviewerList.Rows)
            {
                string mail = reviewer["P_Email"].ToString();
                if (!mailTo.Contains(mail))
                {
                    mailTo += mail + ";";
                }
            }
        }
        else
        {
            mailTo = "Ivan_JL_Zhang@wistron.com";
        }
        // get admin mail
        DataTable adminTable = personManageFac.GetPersonList(UserRole.ADMIN, "");
        foreach (DataRow admin in adminTable.Rows)
        {
            cc += admin["P_Email"].ToString() + ";";
        }
        string html = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/";
        string subject = "[Device Borrowing System]--New User Request";
        string body = @"
<h2>Hi, All</h2>
<div style='margin-left:20px'>
	There is a new user request:
	<br/>
	<br/>
	<table border='1'>
		<tr>
			<th>UID</th>
			<th>Email</th>
			<th>Department</th>
		</tr>
		<tr>
			<td>" + person.P_ID + @"</td>
			<td>" + person.P_Email + @"</td>
			<td>" + person.P_Department + @"</td>
		</tr>
	</table>
	<br/>
	Please ligin to Device Borrowing system to deal with the request.
	<a href='" + html + @"'>" + html + @"</a>
	<br/>
	<br/>
	<font color='red'>Note: this mail was sent by system automatically, please do not try to reply, thanks for your coorperation.</font>
</div>";
        string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        MailService.AddOneMailService(id, mailTo, cc, subject, body, true, "");
    }
}