using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_PersonManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, ImageClickEventArgs e)
    {
        string scriptText = "window.open('./Person/UserInfo.aspx?type=3', '', '');";
        ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
    }
    protected void imgb_AddFromFile_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void imgbtn_PrintPersonList_Click(object sender, ImageClickEventArgs e)
    {

    }
}