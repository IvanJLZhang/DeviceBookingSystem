using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

enum SystemSettings
{
    Project = 1, Pourpse
}
public partial class Manager_SystemSettings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            ShowPage();
        }
    }

    void ShowPage()
    {
        if (Request["type"] != null)
        {
            SystemSettings type = (SystemSettings)(Convert.ToInt32(Request["type"]));
            switch (type)
            {
                case SystemSettings.Project:
                    this.devViewFrame.Attributes["src"] = "Settings/ProjectView.aspx";
                    break;
                case SystemSettings.Pourpse:
                    this.devViewFrame.Attributes["src"] = "Settings/TestCategoryView.aspx";
                    break;
                default:
                    break;
            }
        }
    }
}