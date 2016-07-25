using GlobalClassNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_OpenPage_AddTestCategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.tb_PJCode.Text = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {
        Model.tbl_TestCategory projectInfo = new Model.tbl_TestCategory();
        projectInfo.ID = this.tb_PJCode.Text.Trim();
        projectInfo.Name = this.tb_pjname.Text.Trim();

        BLL.cl_TestCategoryManage projectManage = new BLL.cl_TestCategoryManage();

        if (projectManage.AddTestCategory(projectInfo))
        {
            GlobalClass.PopMsg(this.Page, "添加成功！");

            Response.Write("<script>opener.location.href = opener.location.href; window.close();</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, projectManage.errMsg);
            this.lbl_errMsg.Text = projectManage.errMsg;
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            log.Error("Add test category:" + projectManage.errMsg);
        }
    }
    protected void btn_calcel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");
    }
}