using BLL;
using GlobalClassNamespace;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public partial class Manager_AddEquipment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["cat"] == null)
        {
            //Response.Write("<script>window.open('', '_self', ''); window.close();</script>");
            return;
        }
        if (!IsPostBack)
        {
            int cat = Convert.ToInt32(Session["Category"]);
            if (cat == 1)
            {
                this.pnl_device.Visible = true;

            }
            else {
                this.pnl_equipment_chamber.Visible = true;
                SetLabLocationXml();
            }
        }
    }

    private void SetLabLocationXml()
    {
        WebParams webParams = new WebParams();
        webParams.xmlDocPathName = Server.MapPath("~/ConfigData/WebParams.xml");

        XmlNodeList labNameList = webParams.GetNodeValueList("Lab_Location", "Lab_Name");
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("LabName", typeof(System.String)));
        DataRow row = null;
        foreach (XmlNode node in labNameList)
        {
            row = dataTable.NewRow();
            row["LabName"] = node.InnerText;
            dataTable.Rows.Add(row);
        }

        this.ddl_labLocation.DataSource = dataTable.DefaultView;
        this.ddl_labLocation.DataTextField = "LabName";
        this.ddl_labLocation.DataValueField = "LabName";
        this.ddl_labLocation.SelectedValue = "NULL";
        this.ddl_labLocation.DataBind();
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        string imgUrl = SavePic();

        cl_PersonManage personManage = new cl_PersonManage();
        string p_id = personManage.GetPersonIDByName(this.tb_ownername.Value.Trim());
        if (p_id == null)
        {
            GlobalClass.PopMsg(this.Page, personManage.errMsg);
            return;
        }
        int category = Convert.ToInt32(Session["Category"]);//.ToString());
        if (category == 0)
        {
            return;
        }
        tbl_summary_dev_title summary = new tbl_summary_dev_title();
        tbl_device_detail device = null;
        tbl_equipment_detail equipment = null;
        tbl_chamber_detail chamber = null;

        string id = GetDeviceID(category);
        summary.s_id = id;
        summary.s_assetid = this.tb_assetID.Text.Trim();
        summary.s_category = category;

        string cost = this.tb_cost.Text.Trim();
        try
        {
            summary.s_cost = float.Parse(this.tb_cost.Text.Trim()); ;
        }
        catch {
            summary.s_cost = 0.0;
        }
        
        summary.s_image_url = imgUrl;
        summary.s_name = this.tb_EquipmentName.Text.Trim(); ;
        summary.s_note = this.tb_Introduce.Text.Trim();
        summary.s_ownerid = p_id;
        summary.s_status = Convert.ToInt32(this.ddl_status.SelectedValue);
        summary.s_vender = this.tb_vender.Text.Trim();

        switch (category)
        {
            case 1:
                device = new tbl_device_detail();
                device.d_class = this.tb_deviceClass.Text.Trim();
                device.d_customid = this.tb_Custom_ID.Text.Trim();
                device.d_id = id;
                device.d_interface = this.tb_interface.Text.Trim();
                break;
            case 2:
                equipment = new tbl_equipment_detail();
                if (this.tb_avghr.Text.Trim() == String.Empty)
                    equipment.e_avg_hr = 24;
                else
                    equipment.e_avg_hr = Int32.Parse(this.tb_avghr.Text.Trim());
                equipment.e_id = id;
                equipment.e_lab_location = this.ddl_labLocation.SelectedValue.Trim();
                equipment.e_loan_day = 0;
                try
                {
                    equipment.e_testing_time = TimeSpan.Parse(this.tb_testTime.Text.Trim());
                }
                catch
                {
                    equipment.e_testing_time = TimeSpan.Zero;
                }
                break;
            case 3:
                chamber = new tbl_chamber_detail();
                chamber.c_id = id;
                if (this.tb_avghr.Text.Trim() == String.Empty)
                    chamber.c_avg_hr = 24;
                else
                    chamber.c_avg_hr = Int32.Parse(this.tb_avghr.Text.Trim());
                chamber.c_lab_location = this.ddl_labLocation.SelectedValue.Trim();
                chamber.c_loan_day = 0;
                break;
            default:
                break;
        }
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        if (deviceManage.AddDevice(summary, device, equipment, chamber))
        {
            Response.Write("<script>window.returnValue = 'OK';window.close();</script>");
        }
        else
        {
            GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            log.Error("Add Equipment:" + personManage.errMsg);
        }
    }

    string GetDeviceID(int category) {
        string id = String.Empty;
        switch (category) { 
        
            case 1:
                id += "DT";
                break;
            case 2:
                id += "ET";
                break;
            case 3:
                id += "CT";
                break;
        }

        id += DateTime.Now.ToString("yyyyMMddss");
        return id;
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {
        {// 删除Temp文件夹中的内容
            string[] files = Directory.GetFiles(Server.MapPath("~/Temp"));
            for (int index = 0; index != files.Length; index++)
            {
                File.Delete(files[index]);
            }
        }
        Response.Write("<script>window.close();</script>");
    }
    private string SavePic()
    {
        string deviceImage = this.equipmentImg.ImageUrl;
        if (deviceImage.CompareTo(String.Empty) == 0)
            return String.Empty;
        string fileType = Path.GetExtension(deviceImage);// 文件名扩展
        if (fileType.CompareTo(String.Empty) == 0)
            return String.Empty;
        string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileType;// 以免上传相同文件名冲突

        newFileName = "~/DeviceImgs/" + newFileName;
        File.Copy(Server.MapPath(deviceImage), Server.MapPath(newFileName), true);

        {// 删除Temp文件夹中的内容
            string[] files = Directory.GetFiles(Server.MapPath("~/Temp/"));
            for (int index = 0; index != files.Length; index++)
            {
                File.Delete(files[index]);
            }
        }
        return newFileName;
    }
    protected void tb_ownername_TextChanged(object sender, EventArgs e)
    {
        //this.dl_userNameList.Visible = !this.dl_userNameList.Visible;
    }
    protected void userNameList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("Select") == 0)
        {
            this.tb_ownername.Value = e.CommandArgument.ToString();
        }
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        ImageShow();
    }

    public void ImageShow()
    {
        if (idFile.PostedFile != null && idFile.PostedFile.ContentLength > 0)
        {
            string ext = System.IO.Path.GetExtension(idFile.PostedFile.FileName).ToLower();
            if (ext != ".jpg" && ext != ".jepg" && ext != ".bmp" && ext != ".gif" && ext != ".png")
            {
                return;
            }
            string filename = "Image_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
            string path = "~/Temp/" + filename;
            idFile.PostedFile.SaveAs(Server.MapPath(path));

            this.equipmentImg.ImageUrl = path;
        }
    }
}