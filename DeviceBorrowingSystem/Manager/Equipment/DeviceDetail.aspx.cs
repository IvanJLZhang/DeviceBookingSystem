using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Manager_Equipment_DeviceDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitCtrlPropertyBindData();
            InitCtrlState();
            ShowDeviceDetail();
        }
    }
    #region propertys
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
    /// <summary>
    /// device info table
    /// </summary>
    DataTable _deviceInfo
    {
        get
        {
            if (Session["DeviceInfo"] == null)
                return null;
            else return (DataTable)Session["DeviceInfo"];
        }
        set
        {
            Session["DeviceInfo"] = value;
        }
    }
    #endregion
    #region Methods
    /// <summary>
    /// init control state
    /// </summary>
    void InitCtrlState()
    {
        ViewType viewType = GetViewType();
        this.cb_category.Value = this.cat;
        switch (cat)
        {
            case Category.Device:
                this.pnl_deviceDetail.Visible = true;
                this.pnl_equipmentDetail.Visible = false;
                this.pnl_chamberDetail.Visible = false;
                break;
            case Category.Equipment:
                this.pnl_deviceDetail.Visible = false;
                this.pnl_equipmentDetail.Visible = true;
                this.pnl_chamberDetail.Visible = false;
                break;
            case Category.Chamber:
                this.pnl_deviceDetail.Visible = false;
                this.pnl_equipmentDetail.Visible = false;
                this.pnl_chamberDetail.Visible = true;
                break;
            default:
                break;
        }
        // id is readonly always
        this.tb_id.ReadOnly = true;
        this.btn_Save.Text = viewType.ToString();
        switch (viewType)
        {
            case ViewType.EDIT:
                this.btn_Save.Enabled = true;
                btn_changeImage.Enabled = true;
                pnl_deviceTable.Visible = false;
                this.cb_category.SelectedIndexChanged -= cb_category_SelectedIndexChanged;

                break;
            case ViewType.VIEW:
                this.btn_Save.Enabled = false;
                btn_changeImage.Enabled = false;
                pnl_deviceTable.Visible = false;
                this.cb_category.SelectedIndexChanged -= cb_category_SelectedIndexChanged;
                break;
            case ViewType.ADD:
                // id generate automatically
                this.tb_id.Text = this.cat.ToString().Substring(0, 1) + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                pnl_deviceTable.Visible = true;

                this.btn_Save.Enabled = true;
                btn_changeImage.Enabled = true;

                this.cb_category.SelectedIndexChanged += cb_category_SelectedIndexChanged;
                break;
            case ViewType.DETELE:
            default:
                break;
        }
    }

    void InitCtrlPropertyBindData()
    {
        this.cb_category.Items.Clear();
        this.cb_category.Items.Add(Category.Device.ToString(), (Int32)Category.Device);
        this.cb_category.Items.Add(Category.Equipment.ToString(), (Int32)Category.Equipment);
        this.cb_category.Items.Add(Category.Chamber.ToString(), (Int32)Category.Chamber);

        this.cb_status.Items.Clear();
        this.cb_status.Items.Add(Status.Usable.ToString(), (Int32)Status.Usable);
        this.cb_status.Items.Add(Status.Broken.ToString(), (Int32)Status.Broken);
        this.cb_status.Items.Add(Status.Lost.ToString(), (Int32)Status.Lost);
        this.cb_status.Items.Add(Status.NotForCirculation.ToString(), (Int32)Status.NotForCirculation);
        this.cb_status.Items.Add(Status.ReturnToCustomer.ToString(), (Int32)Status.ReturnToCustomer);

        this.cb_lab.Items.Clear();
        this.cb_lab_c.Items.Clear();

        var lab = GetLabLocationXml();
        this.cb_lab.DataSource = lab.DefaultView;
        this.cb_lab.TextField = "LabName";
        this.cb_lab.ValueField = "LabName";
        this.cb_lab.DataBind();

        this.cb_lab_c.DataSource = lab.DefaultView;
        this.cb_lab_c.TextField = "LabName";
        this.cb_lab_c.ValueField = "LabName";
        this.cb_lab_c.DataBind();
    }

    /// <summary>
    /// show device detatil information by device id
    /// </summary>
    void ShowDeviceDetail()
    {
        ViewType viewType = GetViewType();
        if (Request["device_id"] != null && viewType != ViewType.ADD)
        {
            string id = Request["device_id"].ToString();
            DataRow deviceInfo = devManagementFac.GetDeviceInfo(id, this.cat);
            if (deviceInfo != null)
            {
                DataTable deviceTable = deviceInfo.Table.Clone();
                deviceTable.Rows.Add(deviceInfo.ItemArray);
                this._deviceInfo = deviceTable;

                this.tb_name.Text = deviceInfo["s_name"].ToString();
                this.tb_description.Text = deviceInfo["s_note"].ToString();
                if (File.Exists(deviceInfo["s_image_url"].ToString()))
                {
                    this.img_deviceImage.ImageUrl = deviceInfo["s_image_url"].ToString();
                }
                this.tb_id.Text = deviceInfo["s_id"].ToString();
                this.tb_owner.Text = deviceInfo["OwnerName"].ToString();
                this.tb_assetid.Text = deviceInfo["s_assetid"].ToString();
                this.tb_vender.Text = deviceInfo["s_vender"].ToString();
                this.tb_cost.Text = deviceInfo["s_cost"].ToString();
                this.cb_status.Value = deviceInfo["s_status"].ToString();
                switch (cat)
                {
                    case Category.Device:
                        this.tb_CustomID.Text = deviceInfo["d_customid"].ToString();
                        this.tb_class.Text = deviceInfo["d_class"].ToString();
                        this.tb_interface.Text = deviceInfo["d_interface"].ToString();
                        break;
                    case Category.Equipment:
                        this.te_testTime.Value = deviceInfo["e_testing_time"];
                        this.tb_avg_hr.Text = deviceInfo["e_avg_hr"].ToString();
                        this.tb_loanday.Text = deviceInfo["e_loan_day"].ToString();
                        this.cb_lab.Value = deviceInfo["e_lab_location"].ToString();

                        break;
                    case Category.Chamber:
                        this.tb_avg_hr_c.Text = deviceInfo["c_avg_hr"].ToString();
                        this.tb_loanday_c.Text = deviceInfo["c_loan_day"].ToString();
                        this.cb_lab_c.Value = deviceInfo["c_lab_location"].ToString();
                        break;
                }
            }
        }
        else
        {// 如果viewType 为Add 的话， 需要初始化Table
            switch (viewType)
            {

                case ViewType.ADD:
                    {
                        this._deviceInfo = devManagementFac.GetSummaryTableStruct(this.cat);
                        this.tb_id.Text = "";
                    }
                    break;
            }
        }
    }

    protected void cb_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.cat = (Category)Convert.ToInt32(this.cb_category.Value);
        InitCtrlPropertyBindData();
        InitCtrlState();
        ShowDeviceDetail();
    }
    protected void upc_deviceImage_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e)
    {
        string ext = System.IO.Path.GetExtension(e.UploadedFile.FileName).ToLower();
        string filename = "Image_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
        string path = "../../Temp/" + filename;
        e.UploadedFile.SaveAs(Server.MapPath(path));
        e.CallbackData = path;
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        ViewType viewType = GetViewType();
        switch (viewType)
        {
            case ViewType.EDIT:
                UpdateDeviceInfo();
                break;
        }
    }

    void UpdateDeviceInfo()
    {
        if (this._deviceInfo == null)
            return;
        DataRow deviceInfo = this._deviceInfo.Rows[0];
        //deviceInfo["s_id"] = "";
        //deviceInfo["s_ownerid"] = "";
        deviceInfo["s_name"] = this.tb_name.Text;
        deviceInfo["s_assetid"] = this.tb_assetid.Text;
        //deviceInfo["s_category"] = this;
        deviceInfo["s_vender"] = this.tb_vender.Text;
        deviceInfo["s_cost"] = this.tb_cost.Text;
        deviceInfo["s_status"] = this.cb_status.Value;
        deviceInfo["s_image_url"] = this.img_deviceImage.ImageUrl;
        deviceInfo["s_note"] = this.tb_description.Text;
        switch (cat)
        {
            case Category.Device:
                {
                    deviceInfo["d_customid"] = this.tb_CustomID.Text;
                    deviceInfo["d_class"] = this.tb_class.Text;
                    deviceInfo["d_interface"] = this.tb_interface.Text;
                }
                break;
            case Category.Equipment:
                {
                    deviceInfo["e_testing_time"] = this.te_testTime.Value;
                    deviceInfo["e_avg_hr"] = this.tb_avg_hr.Text;
                    deviceInfo["e_loan_day"] = this.tb_loanday.Text;
                    deviceInfo["e_lab_location"] = this.cb_lab.Value;
                }
                break;
            case Category.Chamber:
                {
                    deviceInfo["e_avg_hr"] = this.tb_avg_hr_c.Text;
                    deviceInfo["e_loan_day"] = this.tb_loanday_c.Text;
                    deviceInfo["e_lab_location"] = this.cb_lab_c.Value;
                }
                break;
        }

        int ret = devManagementFac.UpdateDeviceInfo(deviceInfo);
        if (ret >= 1)
        {
            Response.Write("<script>alert('update data OK！');window.returnValue = 'OK';opener.location.reload();window.close();</script>");
        }
        else
        {
            Response.Write("<script>alert('something is wrong!');</script>");
        }


    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {

    }

    ViewType GetViewType()
    {
        // check role
        ViewType viewType = ViewType.VIEW;// default is view
        if (Session["Role"] != null)
        {
            UserRole role = (UserRole)Convert.ToInt32(Session["Role"]);
            switch (role)
            {
                case UserRole.USER:// if the role is user, view type is view only.
                    viewType = ViewType.VIEW;
                    break;
                case UserRole.REVIEWER:
                case UserRole.LEADER:

                case UserRole.ADMIN:
                    if (Request["type"] != null)
                    {
                        viewType = (ViewType)Convert.ToInt32(Request["type"]);
                    }
                    break;
                default:
                    break;
            }
        }
        return viewType;
    }

    private DataTable GetLabLocationXml()
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

        return dataTable;
    }

    #endregion
}