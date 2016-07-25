using BLL;
using GlobalClassNamespace;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitLogin();

            log4net.ILog log = log4net.LogManager.GetLogger("loginfo");
            HttpBrowserCapabilities bc = Request.Browser;
            log.Info("Browser: " + bc.Browser + "; IP: " + Request.UserHostAddress);
        }
    }
    private void InitLogin()
    {
        if (Request["uid"] != null)
        {
            string userName = Request["uid"].ToString();
            this.tb_UserName.Text = userName;
        }
    }
    protected void btn_Login_Click(object sender, EventArgs e)
    {
        string uid = this.tb_UserName.Text;
        string pwd = this.tb_Pwd.Text;
        // 先进行登录检查
        string errMsg = String.Empty;
        LDAPHelper ldap_login = new LDAPHelper();
        DataRow user = ldap_login.UserLogin(uid, pwd, out errMsg);
        if (user == null)
        {
            GlobalClass.PopMsg(this.Page, errMsg);
            return;
        }
        if (Request["captcha"] != null)
        {
            #region captcha check
            string captcha = Request["captcha"].ToString();

            LoginHelper loginHelper = new LoginHelper();
            string ciphertext = loginHelper.GenerateCaptchaCiphertext(captcha, uid);

            var result = loginHelper.CheckCaptcha(ciphertext);
            if (result == null)
            {
                errMsg = "Wrong captcha or expiration, please check the captcha again or contact with system administrator.";
                common.PopMsg(this.Page, errMsg);
                return;
            }
            bool check = Convert.ToBoolean(result["cp_check"]);
            if (check)
            {
                //errMsg = "Your captcha was been checked, please sign in directly, if still can not sign in, please contact with system administrator.";
                //string html = "" + Request.Url.Host + ":" + Request.Url.Port + "/Login.aspx";
                //common.PopMsg(this.Page, errMsg);
                Response.Redirect("Login.aspx?uid=" + uid);
                return;
            }
            DateTime expiration = Convert.ToDateTime(result["cp_expiration"]);
            if (DateTime.Now > expiration)
            {
                errMsg = "Wrong captcha or expiration, please check the captcha again or contact with system administrator.";
                common.PopMsg(this.Page, errMsg);
                return;
            }
            int result1 = loginHelper.UpdateCaptchaTable(ciphertext);
            if (result1 <= 0)
            {
                errMsg = "Wrong captcha or expiration, please check the captcha again or contact with system administrator.";
                common.PopMsg(this.Page, errMsg);
                return;
            }
            #endregion
            result1 = LoginHelper.RegisterStatusUpdate(uid);
            if (result1 <= 0)
            {
                errMsg = "Update Register Status wrong, please contact with system administrator.";
                common.PopMsg(this.Page, errMsg);
                return;
            }
        }
        else
        {
            bool RegisterStatus = Convert.ToBoolean(user["P_RegisterStatus"]);
            if (!RegisterStatus)
            {
                errMsg = "Your register status was false, which means you must sign in the system by linking from account activate verification email you recieved, if you did not recieve the email, please contact with system administrator.";
                common.PopMsg(this.Page, errMsg);

                return;
            }
        }
        #region session given value
        DataTable userTable = user.Table.Clone();
        userTable.Rows.Add(user.ItemArray);
        Session["LoginUserInfo"] = userTable;
        Session["UserName"] = user["P_Name"].ToString();
        Session["UserID"] = user["P_ID"].ToString();

        int role = int.Parse(user["P_Role"].ToString());
        Session["Role"] = role;

        if (role / 10 <= 0)
        {// normal user
            string prePage = String.Empty;
            if (Session["PrePage"] == null)
                prePage = null;
            else
                prePage = Session["PrePage"].ToString();

            if (prePage != null && prePage.CompareTo(String.Empty) != 0 && prePage.ToString() != "Default.aspx")
            {
                Response.Redirect(prePage.ToString());
                prePage = null;
            }
            else
            {
                Response.Redirect("User/Home.aspx");
            }
        }
        else
        { // reviewer or admin
            // 确定管理类别
            string prePage = String.Empty;
            if (Session["PrePage"] == null)
                prePage = null;
            else
                prePage = Session["PrePage"].ToString();
            if (prePage != null && prePage.CompareTo(String.Empty) != 0 && prePage.ToString() != "Default.aspx")
            {
                Response.Redirect(prePage.ToString());
                prePage = null;
            }
            else
            {
                Response.Redirect("Manager/Default.aspx");
            }
        }
        #endregion
    }
    protected void lb_Regeister_Click(object sender, EventArgs e)
    {
        Response.Write(@"
<script>
    window.open('RegisterPage.aspx');
</script>");
    }
}