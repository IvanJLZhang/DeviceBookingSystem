using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserCtrl_DatePicker : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.SelDate = DateTime.Now.ToString("yyyy/MM/dd");
        }
    }
    protected void imgbtn_date_Click(object sender, ImageClickEventArgs e)
    {
        this.Calendar1.Visible = !this.Calendar1.Visible;
    }
    protected void Calendar1_SelectionChanged(object sender, EventArgs e)
    {
        this.tb_Date.Text = Calendar1.SelectedDate.ToString("yyyy/MM/dd");
        this.Calendar1.Visible = false;
    }

    public string SelDate {
        get { return this.tb_Date.Text; }
        set { this.tb_Date.Text = value; }
    }
    protected void Calendar1_PreRender(object sender, EventArgs e)
    {
        System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US", false);
        Thread.CurrentThread.CurrentCulture = cultureInfo;
    }
}