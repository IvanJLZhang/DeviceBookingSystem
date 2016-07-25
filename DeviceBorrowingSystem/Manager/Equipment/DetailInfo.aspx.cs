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

public partial class Manager_Equipment_DetailInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["id"] == null || Session["Category"] == null)
                Response.Write("<script>window.open('', '_self', ''); window.close();</script>");
            if (Request["type"] != null)
            {
                string type = Request["type"].ToString();
                if (type.CompareTo("view") == 0)
                {
                    this.Button1.Enabled = false;
                }
            }
            SetLabLocationXml();
            ShowEquipmentDetail();
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
        this.ddl_labLocation.DataBind();
    }
    private void ShowEquipmentDetail()
    {
        if (Request["id"] != null)
        {
            string id = Request["id"].ToString().Trim();
            DataTable deviceInfo = new DataTable();
            if (Session["DeviceData"] != null)
            {// && Request["pagetype"] != null && Request["pagetype"].ToString().CompareTo("addview") == 0
                deviceInfo = (DataTable)Session["DeviceData"];
            }
            else
            {
                cl_DeviceManage device = new cl_DeviceManage();
                deviceInfo = device.GetDeviceDetailByID(id);
            }
            ShowDeviceDetailData(deviceInfo, id);
        }
    }
    private void ShowDeviceDetailData(DataTable deviceInfo, string id)
    {
        if (deviceInfo != null)
        {
            for (int index = 0; index != deviceInfo.Rows.Count; index++)
            {
                string ide = deviceInfo.Rows[index]["id"].ToString();
                if (id.CompareTo(deviceInfo.Rows[index]["id"].ToString().Trim()) == 0)
                {
                    this.tb_deviceId.Text = deviceInfo.Rows[index]["id"].ToString().Trim();
                    this.equipmentImg.ImageUrl = deviceInfo.Rows[index]["s_image_url"].ToString().Trim();
                    this.tb_EquipmentName.Text = deviceInfo.Rows[index]["s_name"].ToString().Trim();
                    this.tb_Introduce.Text = deviceInfo.Rows[index]["s_note"].ToString().Trim();

                    this.tb_assetID.Text = deviceInfo.Rows[index]["s_assetid"].ToString().Trim();

                    this.tb_ownername.Value = deviceInfo.Rows[index]["owner"].ToString().Trim();
                    this.lbl_site.Text = deviceInfo.Rows[index]["site"].ToString();
                    //this.ddl_location.SelectedValue = deviceInfo.Rows[index]["site"].ToString().Trim();
                    this.tb_vender.Text = deviceInfo.Rows[index]["s_vender"].ToString().Trim();
                    this.tb_cost.Text = deviceInfo.Rows[index]["s_cost"].ToString().Trim();
                    //this.tb_provider.Text = deviceInfo.Rows[index]["Device_Provider"].ToString().Trim();
                    this.ddl_status.SelectedValue = deviceInfo.Rows[index]["s_status"].ToString().Trim();
                    int category = (int)deviceInfo.Rows[index]["s_category"];
                    switch (category)
                    {
                        case 1:
                            this.pnl_device.Visible = true;
                            if (deviceInfo.Rows[index]["d_customid"] != null)
                                this.tb_Custom_ID.Text = deviceInfo.Rows[index]["d_customid"].ToString().Trim();
                            
                            this.tb_Class.Text = deviceInfo.Rows[index]["d_class"].ToString().Trim();
                           
                            this.tb_interface.Text = deviceInfo.Rows[index]["d_interface"].ToString().Trim();
                            break;
                        case 2:
                            this.pnl_equipment_chamber.Visible = true;
                            if (deviceInfo.Rows[index]["e_testing_time"].ToString().Trim() != String.Empty)
                            {
                                this.tb_testTime.Text = deviceInfo.Rows[index]["e_testing_time"].ToString().Trim();
                            }

                            if (deviceInfo.Rows[index]["e_lab_location"].ToString().Trim().CompareTo(String.Empty) != 0)
                                this.ddl_labLocation.SelectedValue = deviceInfo.Rows[index]["e_lab_location"].ToString().Trim();

                            if (deviceInfo.Rows[index]["e_avg_hr"].ToString().Trim() == String.Empty)
                                this.tb_avghr.Text = "24";
                            else
                                this.tb_avghr.Text = deviceInfo.Rows[index]["e_avg_hr"].ToString().Trim();
                            break;
                        case 3:
                            this.pnl_equipment_chamber.Visible = true;
                            if (deviceInfo.Rows[index]["c_lab_location"].ToString().Trim().CompareTo(String.Empty) != 0)
                                this.ddl_labLocation.SelectedValue = deviceInfo.Rows[index]["c_lab_location"].ToString().Trim();
                            this.tb_avghr.Visible = true;
                            if (deviceInfo.Rows[index]["c_avg_hr"].ToString().Trim() == String.Empty)
                                this.tb_avghr.Text = "24";
                            else
                                this.tb_avghr.Text = deviceInfo.Rows[index]["c_avg_hr"].ToString().Trim();
                            break;
                    }
                    break;
                }
            }

        }
    }
    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string imgUrl = SavePic();
        if (Session["DeviceData"] != null)
        {
            #region session中取
            DataTable deviceInfo = (DataTable)Session["DeviceData"];
            string deviceId = Request["id"].ToString().Trim();
            foreach (DataRow row in deviceInfo.Rows)
            {
                if (deviceId.CompareTo(row["id"]) == 0)
                {
                    row["s_image_url"] = this.equipmentImg.ImageUrl;
                    row["s_name"] = this.tb_EquipmentName.Text;
                    row["s_note"] = this.tb_Introduce.Text;

                    row["s_assetid"] = this.tb_assetID.Text;
                    row["s_vender"] = this.tb_vender.Text;
                    row["s_cost"] = this.tb_cost.Text;
                    row["s_status"] = this.ddl_status.SelectedValue;
                    row["s_note"] = this.tb_Introduce.Text;

                    row["owner"] = this.tb_ownername.Value;
                    cl_PersonManage personManage = new cl_PersonManage();
                    string p_id = personManage.GetPersonIDByName(this.tb_ownername.Value);
                    row["s_ownerid"] = p_id;

                    int category = (int)row["s_category"];
                    switch (category)
                    {
                        case 1:
                            row["d_customid"] = this.tb_Custom_ID.Text;
                            row["d_class"] = this.tb_Class.Text.Trim();
                            row["d_interface"] = this.tb_interface.Text;
                            break;
                        case 2:
                            row["e_testing_time"] = this.tb_testTime.Text;
                            if (this.tb_avghr.Text.Trim() == String.Empty)
                                row["e_avg_hr"] = 24;
                            else
                                row["e_avg_hr"] = this.tb_avghr.Text;

                            row["e_lab_location"] = this.ddl_labLocation.SelectedValue;
                            break;
                        case 3:
                            if (this.tb_avghr.Text.Trim() == String.Empty)
                                row["c_avg_hr"] = 24;
                            else
                                row["c_avg_hr"] = this.tb_avghr.Text;

                            row["c_lab_location"] = this.ddl_labLocation.SelectedValue;
                            break;
                    }
                    Response.Write("<script>alert('update data OK！');window.returnValue = 'OK';window.close();</script>");
                }
            }
            #endregion
        }
        else
        {
            cl_PersonManage personManage = new cl_PersonManage();
            string p_id = personManage.GetPersonIDByName(this.tb_ownername.Value);
            if (p_id == null)
            {
                GlobalClass.PopMsg(this.Page, personManage.errMsg);
                return;
            }
            int category = Convert.ToInt32(Session["Category"]);
            tbl_summary_dev_title summary = new tbl_summary_dev_title();
            tbl_device_detail device = null;
            tbl_equipment_detail equipment = null;
            tbl_chamber_detail chamber = null;
            string id = Request["id"].ToString().Trim();
            summary.s_id = id;
            summary.s_assetid = this.tb_assetID.Text.Trim();
            summary.s_category = category;
            summary.s_cost = float.Parse(this.tb_cost.Text.Trim()); ;
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
                    device.d_class = this.tb_Class.Text.Trim();
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

            if (deviceManage.UpdateDeviceInfo(summary, device, equipment, chamber))
            {
                Response.Write("<script>alert('update data OK！');window.returnValue = 'OK';window.close();</script>");
            }
            else
            {
                Response.Write("<script>alert('" + deviceManage.errMsg + "');window.returnValue = 'OK';window.close();</script>");
            }
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Write("<script>opener.location.href = opener.location.href;window.close();</script>");
    }
    protected void dl_userNameList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("Select") == 0)
        {
            this.tb_ownername.Value = e.CommandArgument.ToString();
        }
    }

    private string SavePic()
    {
        string deviceImage = this.equipmentImg.ImageUrl;
        if (!File.Exists(Server.MapPath(deviceImage)))
            deviceImage = String.Empty;
        if (deviceImage.CompareTo(String.Empty) == 0)
            return String.Empty;
        string fileType = Path.GetExtension(deviceImage);// 文件名扩展
        if (fileType.CompareTo(String.Empty) == 0)
            return String.Empty;
        string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + fileType;// 以免上传相同文件名冲突

        newFileName = "~/DeviceImgs/" + newFileName;
        if (!File.Exists(newFileName))
            File.Copy(Server.MapPath(deviceImage), Server.MapPath(newFileName), true);

        {// 删除Temp文件夹和设备原图片文件的内容
            string[] files = Directory.GetFiles(Server.MapPath("~/Temp/"));
            for (int index = 0; index != files.Length; index++)
            {
                File.Delete(files[index]);
            }

            if (File.Exists(Server.MapPath(this.PreImg.Value)))
            {
                File.Delete(Server.MapPath(this.PreImg.Value));
            }
        }
        return newFileName;
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

            this.PreImg.Value = this.equipmentImg.ImageUrl;
            this.equipmentImg.ImageUrl = path;
        }
    }



    public string GetPersonInfoByName()
    {
        cl_PersonManage personMana = new cl_PersonManage();
        DataTable person = personMana.GetPersonInfoByName(this.tb_ownername.Value);
        if (person != null)
            return person.Rows[0]["P_Department"].ToString();
        else
            return String.Empty;
    }
}