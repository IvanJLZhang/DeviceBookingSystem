using DevExpress.Web.ASPxEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_BookingSteps_Step1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    Category cat
    {
        get
        {
            if (Session["Category"] == null)
                return Category.Device;
            else
            {
                return (Category)Convert.ToInt32(Session["Category"]);
            }
        }
        set
        {
            Session["Category"] = (Int32)value;
        }
    }
    protected void ASPxRadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        cat = (Category)Convert.ToInt32(this.ASPxRadioButtonList1.Value);
    }
    protected void ASPxRadioButtonList1_Init(object sender, EventArgs e)
    {
        ASPxRadioButtonList1.SelectedIndex = (Int32)cat - 1;
    }
}