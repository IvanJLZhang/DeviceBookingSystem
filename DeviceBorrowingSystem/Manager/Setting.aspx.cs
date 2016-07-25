using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Configuration;
using System.Web.Configuration;
using BLL;
using Model;
using System.Xml;
using System.Data;
public partial class Manager_Setting : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //SetConfigDataXml();
            //SetLabLocationXml();
            //ShowProxyRecord();
        } 
    }
    //private void SetConfigDataXml()
    //{
    //    WebParams webParams = new WebParams();
    //    webParams.xmlDocPathName = Server.MapPath("~/ConfigData/WebParams.xml");
    //    this.tb_WarnDaysDevice.Text = webParams.GetNodeValue("WarnDays", "Device_WarnDays");
    //    this.tb_WarnDaysEquip.Text = webParams.GetNodeValue("WarnDays", "Equipment_WarnDays");
    //}
    //private void SaveConfigData_Xml()
    //{
    //    WebParams webParams = new WebParams();
    //    webParams.xmlDocPathName = Server.MapPath("~/ConfigData/WebParams.xml");
    //    webParams.SetNodeValue("WarnDays", "Device_WarnDays", this.tb_WarnDaysDevice.Text);
    //    webParams.SetNodeValue("WarnDays", "Equipment_WarnDays", this.tb_WarnDaysEquip.Text);
    //}

    //private void ShowProxyRecord()
    //{
    //    if (Session["UserID"] != null)
    //    {

    //        cl_ProxyUserManage proxyUserManage = new cl_ProxyUserManage();
    //        proxyUserManage.FinishedProxy("0", Session["UserID"].ToString());
    //        DataTable result = proxyUserManage.GetProxyUserListByUserID(Session["UserID"].ToString());
    //        this.gv_ProxyRecord.DataSource = result.DefaultView;
    //        this.gv_ProxyRecord.DataKeyNames = new string[] { "id" };
    //        this.gv_ProxyRecord.DataBind();
    //    }
    //}
    //protected void btn_Save_Click(object sender, EventArgs e)
    //{
    //    //SaveConfigData();
    //    SaveConfigData_Xml();
    //    SetProxyUserData();
    //    ShowProxyRecord();
    //}
    //private void SetProxyUserData()
    //{
    //    if (tb_ProxyName.Value.Trim().CompareTo(String.Empty) != 0)
    //    {
    //        tbl_ProxyUser proxyUser = new tbl_ProxyUser();
    //        cl_PersonManage personManage = new cl_PersonManage();
    //        proxyUser.UID = Session["UserID"].ToString();
    //        proxyUser.ProxyUID = personManage.GetPersonIDByName(this.tb_ProxyName.Value.Trim());
    //        proxyUser.StartDate = Convert.ToDateTime(this.tb_ProxyFrom.Text);
    //        proxyUser.EndDate = Convert.ToDateTime(this.tb_ProxyTO.Text);

    //        cl_ProxyUserManage proxyUserManage = new cl_ProxyUserManage();
    //        if (!proxyUserManage.AddProxyUser(proxyUser))
    //        {
    //            //
    //        }
    //    }
    //}
    //protected void dl_labLocation_ItemCommand(object source, DataListCommandEventArgs e)
    //{
    //    if (e.CommandName.CompareTo("DeleteItem") == 0)
    //    {
    //        WebParams webParams = new WebParams();
    //        webParams.xmlDocPathName = Server.MapPath("~/ConfigData/WebParams.xml");
    //        if (webParams.DeleteNode("Lab_Location", "Lab_Name", e.CommandArgument.ToString()))
    //        {
    //            SetLabLocationXml();
    //        }
    //    }
    //}

    //private void SetLabLocationXml()
    //{
    //    WebParams webParams = new WebParams();
    //    webParams.xmlDocPathName = Server.MapPath("~/ConfigData/WebParams.xml");

    //    XmlNodeList labNameList = webParams.GetNodeValueList("Lab_Location", "Lab_Name");
    //    DataTable dataTable = new DataTable();
    //    dataTable.Columns.Add(new DataColumn("LabName", typeof(System.String)));
    //    DataRow row = null;
    //    foreach (XmlNode node in labNameList)
    //    {
    //        row = dataTable.NewRow();
    //        row["LabName"] = node.InnerText;
    //        dataTable.Rows.Add(row);
    //    }

    //    this.dl_labLocation.DataSource = dataTable.DefaultView;
    //    this.dl_labLocation.DataKeyField = "LabName";
    //    this.dl_labLocation.DataBind();
    //}
    //protected void imgb_Add_Click(object sender, ImageClickEventArgs e)
    //{
    //    if (this.tb_LabName.Text.Trim().CompareTo(String.Empty) != 0)
    //    {
    //        WebParams webParams = new WebParams();
    //        webParams.xmlDocPathName = Server.MapPath("~/ConfigData/WebParams.xml");
    //        if (webParams.InsertNode("Lab_Location", "Lab_Name", this.tb_LabName.Text))
    //        {
    //            SetLabLocationXml();
    //            this.tb_LabName.Text = String.Empty;
    //        }
    //    }
    //}
    //protected void gv_ProxyRecord_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.CompareTo("CancelProxy") == 0)
    //    {
    //        cl_ProxyUserManage proxyUserManage = new cl_ProxyUserManage();
    //        if (proxyUserManage.FinishedProxy(e.CommandArgument.ToString(), Session["UserID"].ToString()))
    //        {
    //            ShowProxyRecord();
    //        }
    //    }
    //}
}