using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using BLL;
using GlobalClassNamespace;
using System.Data;
using System.Xml;
using System.Web.UI.HtmlControls;
public partial class Manager_EquipmentDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["id"] == null || Session["Category"] == null)
                Response.Write("<script>window.open('', '_self', ''); window.close();</script>");
            InitTabPages();
        }
    }


    void InitTabPages()
    {
        HtmlGenericControl if_detail = this.ASPxPageControl1.TabPages[0].FindControl("if_detail") as HtmlGenericControl;
        string id = Request["id"];


        string url = "DetailInfo.aspx?";
        url += "id=" + id;


        if (Request["type"] != null)
        {
            string type = Request["type"];
            url += "&type=" + type;
        }
        if_detail.Attributes["src"] = url;



        url = "Calibration.aspx?";
        url += "id=" + id;
        if (Request["type"] != null)
        {
            string type = Request["type"];
            url += "&type=" + type;
        }
        HtmlGenericControl if_cali = this.ASPxPageControl1.TabPages[0].FindControl("if_cali") as HtmlGenericControl;
        if_cali.Attributes["src"] = url;

        this.ASPxPageControl1.ActiveTabIndex = 0;
    }
}