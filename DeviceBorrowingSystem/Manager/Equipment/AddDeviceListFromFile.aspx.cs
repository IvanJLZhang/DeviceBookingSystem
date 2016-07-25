using BLL;
using DevExpress.Web.ASPxUploadControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZipHelper;

public partial class Manager_Equipment_AddDeviceListFromFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["type"] == null)
            {
                Response.Write("<script>window.open('', '_self', ''); window.close();</script>");
            }

            int category = Convert.ToInt32(Session["Category"]);
            this.lbl_title.Text = "Add " + Enum.GetName(typeof(Category), category) + " list from file";
            //btn_AddAll.Attributes[""]("onclick", "javascript:return confirm('Are sure to add the these devices?');");
            //ShowPostedDeviceList();
        }
    }
    private void ShowPostedDeviceList()
    {
        int category = Convert.ToInt32(Session["Category"]);
        this.lbl_title.Text = "Add " + Enum.GetName(typeof(Category), category) + " list from file";


        if (DeviceData == null)
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, Resources.Resource.msg_device_upload_cannot_load_devicetable);
            return;
        }

        CheckDeviceData();

        if (DeviceData != null)
        {
            DataTable CannotAddDeviceData = DeviceData.Clone();
            foreach (DataRow row in DeviceData.Rows)
            {
                if (row["errnote"] != null && Convert.ToString(row["errnote"]) != String.Empty)
                    CannotAddDeviceData.Rows.Add(row.ItemArray);
            }
            this.gv_deviceView.DataSource = CannotAddDeviceData;
            this.gv_deviceView.DataKeyNames = new string[] { "id" };
            this.gv_deviceView.DataBind();
        }

        if (CanAddDeviceData != null)
        {
            this.gv_addDeviceView.DataSource = CanAddDeviceData;
            this.gv_addDeviceView.DataKeyNames = new string[] { "id" };
            this.gv_addDeviceView.DataBind();
        }
    }
    /// <summary>
    /// 原始数据
    /// </summary>
    private DataTable DeviceData
    {
        get
        {
            if (Session["DeviceData"] != null)
                return (DataTable)Session["DeviceData"];
            else
                return null;

        }
        set
        {
            Session["DeviceData"] = value;
        }
    }
    /// <summary>
    /// 可以被添加的数据
    /// </summary>
    private DataTable CanAddDeviceData
    {
        get
        {
            if (Session["CanAddDeviceData"] != null)
                return (DataTable)Session["CanAddDeviceData"];
            else
                return null;

        }
        set
        {
            Session["CanAddDeviceData"] = value;
        }
    }
    private void CheckDeviceData()
    {
        if (DeviceData == null)
            return;
        Session.Remove("CanAddDeviceData");
        CanAddDeviceData = DeviceData.Clone();
        for (int index = 0; index != DeviceData.Rows.Count; index++)
        {
            DataRow deviceInfo = DeviceData.Rows[index];
            deviceInfo["errnote"] = "";
            if (CheckAssetID(deviceInfo))
            {
                deviceInfo["errnote"] += "duplicated Asset ID;";
            }

            int cat = Int32.Parse(deviceInfo["s_category"].ToString());
            if (cat == 1)
            {
                if (CheckCustomID(deviceInfo))
                {
                    deviceInfo["errnote"] += "duplicated Custom ID;";
                }
            }

            if (Convert.ToString(deviceInfo["s_ownerid"]) == String.Empty)
            {
                deviceInfo["errnote"] += "Owner Name is NULL";
            }

            if (deviceInfo["errnote"] == null || Convert.ToString(deviceInfo["errnote"]) == String.Empty)
            {
                CanAddDeviceData.Rows.Add(deviceInfo.ItemArray);
            }
        }
    }

    protected void gv_deviceView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("DeviceDetailInfo") == 0)
        {
            string scriptText = "var obj = window.showModalDialog('./Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            Response.Write("<script>var obj = window.showModalDialog('./EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=edit', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;</script>");
        }
        if (e.CommandName.CompareTo("DeleteItem") == 0)
        {
            DataTable deviceList = (DataTable)DeviceData;
            GridViewRow row = ((GridViewRow)(((LinkButton)(e.CommandSource)).Parent.Parent));
            deviceList.Rows.RemoveAt(row.RowIndex);
            ShowPostedDeviceList();
        }
    }
    public string GetPersonName(string userID)
    {
        cl_PersonManage personManage = new cl_PersonManage();
        DataTable personInfo = personManage.GetPersonInfoByID(userID);
        if (personInfo != null && personInfo.Rows.Count > 0)
        {
            return personInfo.Rows[0]["P_Name"].ToString();
        }
        return String.Empty;
    }
    protected void btn_AddAll_Click(object sender, EventArgs e)
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        if (CanAddDeviceData == null)
            return;
        DataTable deviceList = (DataTable)CanAddDeviceData;
        // 将存放在Temp中的device image另存到deviceImages目录下
        for (int index = 0; index != deviceList.Rows.Count; index++)
        {
            DataRow device = deviceList.Rows[index];
            string imageurl = Convert.ToString(device["s_image_url"]);
            if (imageurl == String.Empty)
                continue;

            FileInfo imagefile = new FileInfo(Server.MapPath(imageurl));
            string newimagefile = DateTime.Now.ToString("yyyyMMddHHmmssfff") + index + imagefile.Extension;
            newimagefile = "~/DeviceImgs/" + newimagefile;

            imagefile.CopyTo(Server.MapPath(newimagefile), true);
            device["s_image_url"] = newimagefile;
        }
        int cnt = deviceManage.AddDeviceByDataTable(deviceList);
        Session.Remove("DeviceData");
        Session.Remove("CanAddDeviceData");

        {// 删除Temp文件夹中的内容
            GlobalClassNamespace.GlobalClass.DeleteDir(Server.MapPath("~/Temp/"));
        }
        Response.Write("<script>alert('Add " + cnt + " devices successfully!');window.opener.location.href = window.opener.location.href;window.close();</script>");
    }
    /// <summary>
    /// Cancel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session.Remove("DeviceData");
        Session.Remove("CanAddDeviceData");
        Response.Write("<script>window.close();</script>");
    }
    protected void gv_addDeviceView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("DeviceDetailInfo") == 0)
        {
            string scriptText = "var obj = window.showModalDialog('./Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            Response.Write("<script>var obj = window.showModalDialog('./EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=edit', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;</script>");
        }
        if (e.CommandName.CompareTo("DeleteItem") == 0)
        {
            DataTable deviceList = (DataTable)CanAddDeviceData;
            GridViewRow row = ((GridViewRow)(((LinkButton)(e.CommandSource)).Parent.Parent));
            deviceList.Rows.RemoveAt(row.RowIndex);
            //ShowPostedDeviceList();
            gv_addDeviceView.DataSource = deviceList.DefaultView;
            this.gv_addDeviceView.DataKeyNames = new string[] { "id" };
            gv_addDeviceView.DataBind();
            DeviceData = deviceList;

        }
    }
    protected void FileSelect_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e)
    {
        e.CallbackData = SaveAsUploadedFile(e.UploadedFile);
    }
    string SaveAsUploadedFile(UploadedFile file)
    {
        string tempPath = Server.MapPath("~/Temp/");
        string fileName = Path.Combine(tempPath, file.FileName);
        file.SaveAs(fileName);

        devManagementFac dev = new devManagementFac();
        DataTable DeviceTable = dev.LoadDataAnalyze(fileName, tempPath);

        DeviceData = DeviceTable;

        return file.FileName;
    }
    protected void btn_GetTemplate_Click(object sender, EventArgs e)
    {
        devManagementFac dev = new devManagementFac();
        ExcelRenderNode result = dev.GenerateTemplateFile(Session["Category"].ToString(), Server.MapPath("~\\Temp\\"), Server.MapPath("~\\DeviceImgs\\"));

        GlobalClassNamespace.GlobalClass.DeleteDir(Server.MapPath("~\\Temp\\"));

        ExcelRender.RenderToBrowser(result.ms, Context, result.errMsg + ".zip");

        result.ms.Close();
        result.ms.Dispose();


    }
    protected void btn_preview_Click(object sender, EventArgs e)
    {
        ShowPostedDeviceList();
    }
}


public partial class Manager_Equipment_AddDeviceListFromFile : System.Web.UI.Page
{
    public bool CheckAssetID(DataRow deviceInfo)
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();

        if (deviceInfo["s_assetid"] != null)
        {
            return deviceManage.CheckAssetID(deviceInfo["s_assetid"].ToString());
        }
        return false;
    }

    public bool CheckCustomID(DataRow deviceInfo)
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();

        if (deviceInfo["d_customid"] != null)
        {
            return deviceManage.CheckCustomID(deviceInfo["d_customid"].ToString());
        }
        return false;
    }
}