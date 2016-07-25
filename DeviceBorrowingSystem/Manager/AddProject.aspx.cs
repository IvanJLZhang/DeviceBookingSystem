using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using BLL;
using GlobalClassNamespace;
//[assembly:log4net.Config.XmlConfigurator(Watch = true)]
public partial class Manager_AddProject : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_calcel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {
        Model.tbl_Project projectInfo = new Model.tbl_Project();
        //projectInfo.ID = DateTime.Now.ToString("yyyyMMddHHmmss");
        projectInfo.ID = this.tb_PJCode.Text.Trim();
        projectInfo.PJ_Name = this.tb_pjname.Text.Trim();
        projectInfo.Cust_Name = this.tb_custname.Text.Trim();

        BLL.cl_ProjectManage projectManage = new BLL.cl_ProjectManage();

        if (projectManage.AddProject(projectInfo))
        {
            GlobalClass.PopMsg(this.Page, "添加成功！");

            Response.Write("<script>opener.location.href = opener.location.href; window.close();</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, projectManage.errMsg);
            this.lbl_errMsg.Text = projectManage.errMsg;
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            log.Error("Add Project:" + projectManage.errMsg);
        }
    }
}