using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_ProjectManage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["type"] != null) {
                if (Request["type"].ToString().Equals("project", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.ddl_Type.SelectedValue = "Project";
                }
                else {
                    this.ddl_Type.SelectedValue = "Test Category";
                
                }
            }
            ddl_Type_SelectedIndexChanged(null, null);
        }
    }
    protected void linkbutton1_Click(object sender, EventArgs e)
    {
        if (this.ddl_Type.SelectedValue.CompareTo("Project") == 0)
        {
            //ImageButton4.ToolTip = "Add a project item";
            string scriptText = "window.open('AddProject.aspx', '', 'width=400px,height=400px,location=no, top=100px, left=100px');";
            ScriptManager.RegisterStartupScript(this.up_main, this.GetType(), "click", scriptText, true);
        }
        else if (this.ddl_Type.SelectedValue.CompareTo("Test Category") == 0)
        {
            //ImageButton4.ToolTip = "Add a Test category item";
            string scriptText = "window.open('OpenPage/AddTestCategory.aspx', '', 'width=400px,height=250px,location=no, top=100px, left=100px');";
            ScriptManager.RegisterStartupScript(this.up_main, this.GetType(), "click", scriptText, true);
        }
        //Response.Write("<script>window.open('AddProject.aspx', '', 'width=400px,height=400px,location=no, top=100px, left=100px');</script>");
    }
    protected void linkbutton2_Click(object sender, EventArgs e)
    {
        string scriptText = "window.open('OpenPage/AddTestCategory.aspx', '', 'width=400px,height=250px,location=no, top=100px, left=100px');";
        ScriptManager.RegisterStartupScript(this.up_main, this.GetType(), "click", scriptText, true);
        //Response.Write("<script>window.open('OpenPage/AddTestCategory.aspx', '', 'width=400px,height=250px,location=no, top=100px, left=100px');</script>");
    }
    protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddl_Type.SelectedValue.CompareTo("Project") == 0)
        {
            this.panel_ProjectManagement.Visible = true;
            this.panel_TestCatManagement.Visible = false;
            ImageButton4.ToolTip = "Add a project item";
        }
        else
        {
            this.panel_TestCatManagement.Visible = true;
            this.panel_ProjectManagement.Visible = false;
            ImageButton4.ToolTip = "Add a Test category item";
        }
    }
}