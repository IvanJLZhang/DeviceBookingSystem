using BLL;
using GlobalClassNamespace;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_BorrowingPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.tb_StartDate.Text = DateTime.Now.ToString("M/d/yyyy");
            this.tb_endDate.Text = DateTime.Now.ToString("M/d/yyyy");
            if (Session["Category"] != null)
            {
                this.ddl_category.SelectedValue = Session["Category"].ToString();
            }
            showdeviceInfo();
        }
    }

    void showdeviceInfo(){
        if (ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            //this.Title = "Device Manage";
            this.panel_DeviceShow.Visible = true;
            this.panel_EquipShow.Visible = false;

            this.panel_selTimeEquip.Visible = false;
            this.panel_SelDateDevice.Visible = false;
            this.btn_Submit.Enabled = false;
            this.btn_Cancel.Enabled = false;

            InitALLDDLData();
            SetGridViewData();
        }
        else if (ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
        {
            //this.Title = "Equipment Manage";
            this.panel_EquipShow.Visible = true;
            this.panel_DeviceShow.Visible = false;

            this.panel_selTimeEquip.Visible = false;
            this.panel_SelDateDevice.Visible = false;
            this.btn_Submit.Enabled = false;
            this.btn_Cancel.Enabled = false;
            InitDDLLocationData();
            InitDDLDepartmentData();
            PagedDataBind();
        }
    
    }
    protected void ddl_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.SelectedItems != null)
        {
            this.SelectedItems.Clear();
            this.SelectedItems = null;
        }
        this.Session["Category"] = this.ddl_category.SelectedValue;
        Response.Redirect(Request.Url.ToString()); 
        //this.p_Navigate.Enabled = false;

    }
    private bool changed = false;
    protected List<string> SelectedItems
    {
        get { return ViewState["selecteditems"] != null ? (List<string>)ViewState["selecteditems"] : null; }
        set { ViewState["selecteditems"] = value; }
    }
    private void GetSelectedItem()
    {
        List<string> selecteditems = null;
        if (this.SelectedItems == null)
            selecteditems = new List<string>();
        else
            selecteditems = this.SelectedItems;

        // 获取选中的记录
        if (ddl_category.SelectedValue.CompareTo("1") == 0)
        {// Device
            for (int index = 0; index != this.GridView1.Rows.Count; index++)
            {
                System.Web.UI.WebControls.CheckBox cbx = (System.Web.UI.WebControls.CheckBox)this.GridView1.Rows[index].FindControl("chkSelect");
                string id = this.GridView1.DataKeys[index].Value.ToString();

                if (selecteditems.Contains(id) && !cbx.Checked)
                {
                    selecteditems.Remove(id);
                }
                if (!selecteditems.Contains(id) && cbx.Checked)
                {
                    selecteditems.Add(id);
                }
            }

            this.SelectedItems = selecteditems;
        }
        else if (ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
        {// Equipment
            for (int index = 0; index != this.dl_EquipmentInfo.Items.Count; index++)
            {
                CheckBox cbx = (CheckBox)this.dl_EquipmentInfo.Items[index].FindControl("CheckBox1");
                string id = this.dl_EquipmentInfo.DataKeys[index].ToString();

                if (selecteditems.Contains(id) && !cbx.Checked)
                {
                    selecteditems.Remove(id);
                }
                if (!selecteditems.Contains(id) && cbx.Checked)
                {
                    selecteditems.Add(id);
                }
            }

            this.SelectedItems = selecteditems;
        }

    }

    private DataTable PickDateTimeList
    {
        get
        {
            if (Session["PickDateTimeList"] == null)
            {
                return null;
            }
            else
                return Session["PickDateTimeList"] as DataTable;
        }
        set
        {
            Session["PickDateTimeList"] = value;
        }
    }
    #region Device Show
    private void InitALLDDLData()
    {
        InitDDLLocationData();
        InitDDLDepartmentData();
        InitDDLClassData();
        InitDDLInterfaceData();
    }
    private void InitDDLLocationData()
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable result = null;
        // Set this.ddl_Location
        result = null;
        result = deviceManage.GetLocationListByCa(this.ddl_category.SelectedValue);
        if (result == null)
        {
            GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            return;
        }
        this.ddl_location.DataSource = result.DefaultView;
        this.ddl_location.DataTextField = "Text";
        this.ddl_location.DataValueField = "Value";
        this.ddl_location.DataBind();
    }
    private void InitDDLDepartmentData()
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable result = null;
        // Set Department
        result = null;
        result = deviceManage.GetDepartmentListByCaLo(this.ddl_category.SelectedValue, this.ddl_location.SelectedValue);
        if (result == null)
        {
            GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            return;
        }
        this.ddl_Department.DataSource = result.DefaultView;
        this.ddl_Department.DataTextField = "Text";
        this.ddl_Department.DataValueField = "Value";
        this.ddl_Department.DataBind();
    }
    private void InitDDLClassData()
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable result = null;
        // Set this.ddl_class
        result = null;
        result = deviceManage.GetClassListByCaDeLo(this.ddl_category.SelectedValue, this.ddl_Department.SelectedValue, this.ddl_location.SelectedValue);
        if (result == null)
        {
            //GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            string scriptText = "alert('" + deviceManage.errMsg + "')";
            ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
            return;
        }
        this.ddl_class.DataSource = result.DefaultView;
        this.ddl_class.DataTextField = "Text";
        this.ddl_class.DataValueField = "Value";
        this.ddl_class.DataBind();
    }
    private void InitDDLInterfaceData()
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable result = null;
        // Set Interface
        result = null;
        result = deviceManage.GetIntercaceListByCaDeClLo(this.ddl_category.SelectedValue, this.ddl_Department.SelectedValue, this.ddl_class.SelectedValue, this.ddl_location.SelectedValue);
        if (result == null)
        {
            GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            return;
        }
        this.ddl_Interface.DataSource = result.DefaultView;
        this.ddl_Interface.DataTextField = "Text";
        this.ddl_Interface.DataValueField = "Value";
        this.ddl_Interface.DataBind();
    }
    protected void ddl_location_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitDDLDepartmentData();
        if (ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            InitDDLClassData();
            InitDDLInterfaceData();
            SetGridViewData();
        }
        else if (ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue == "3")
        {
            PagedDataBind();
        }
    }
    protected void ddl_Department_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            InitDDLClassData();
            InitDDLInterfaceData();
            SetGridViewData();
        }
        else if (ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue == "3")
        {
            PagedDataBind();
        }
    }
    protected void ddl_class_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitDDLInterfaceData();
        SetGridViewData();
    }

    protected void ddl_Interface_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetGridViewData();
    }

    private void SetGridViewData()
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();

        DataTable result = deviceManage.GetDeviceNameList(this.ddl_category.SelectedValue, this.ddl_class.SelectedValue, this.ddl_Interface.SelectedValue, this.ddl_location.SelectedValue, this.ddl_Department.SelectedValue);
        if (result == null)
        {
            //GlobalClass.PopMsg(this.Page, deviceManage.errMsg);
            string scriptText = "alert('" + deviceManage.errMsg + "')";
            ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
            return;
        }

        DataTable showResult = result.Clone();
        foreach (DataRow device in result.Rows) {
            int status = Convert.ToInt32(device["Status"]);
            if (status <= 1) {
                DataRow newRow = showResult.NewRow();
                foreach (DataColumn col in result.Columns) {
                    newRow[col.Caption] = device[col.Caption];
                }
                showResult.Rows.Add(newRow);
            }
        }
        this.GridView1.DataSource = showResult;
        this.GridView1.DataKeyNames = new string[] { "id" };
        this.GridView1.PageSize = Convert.ToInt32(this.ddl_pagesize.SelectedValue);
        this.GridView1.DataBind();


    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("DeviceDetailInfo") == 0)
        {
            string scriptText = "var obj = window.showModalDialog('../Manager/Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            //string scriptText = "window.open('../Manager/Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'width=580px, height=500px, top=100px, left=100px, scrollbars=yes');";
            ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>window.open('EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'width=580px, height=500px, top=100px, left=100px, scrollbars=yes'); </script>");
        }
    }
    protected void GridView1_DataBinding(object sender, EventArgs e)
    {
        GetSelectedItem();
        changed = true;
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1 && this.SelectedItems != null)
        {
            string id = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
            System.Web.UI.WebControls.CheckBox cbx = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("chkSelect");
            if (this.SelectedItems.Contains(id))
            {
                cbx.Checked = true;
            }
            else
                cbx.Checked = false;
        }
        if (e.Row.RowIndex > -1)
        {
            Panel pnl_bgsetting = (Panel)e.Row.FindControl("pnl_bgsetting");
            Label lbl_borrowStatus = (Label)e.Row.FindControl("lbl_borrowStatus");
            if (lbl_borrowStatus.Text == "OK")
            {
                pnl_bgsetting.BackColor = Color.Green;
            }
            else
            {
                pnl_bgsetting.BackColor = Color.Gray;

            }
        }

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GetSelectedItem();
        this.GridView1.PageIndex = e.NewPageIndex;
        SetGridViewData();
    }
    protected void imgbtn_Search_Click(object sender, ImageClickEventArgs e)
    {
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable result = deviceManage.SearchDevice(this.ddl_category.SelectedValue, this.tb_Search.Text.Trim());

        if (result == null)
        {
            string scriptText = "alert('" + deviceManage.errMsg + "')";
            ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
            return;
        }

        this.GridView1.DataSource = result;
        this.GridView1.DataKeyNames = new string[] { "id" };
        this.GridView1.DataBind();

        this.tb_Search.Text = "";
    }

    #endregion

    #region Equipment Show
    private void PagedDataBind()
    {
        int curpage = Convert.ToInt32(this.labPage.Text);
        PagedDataSource ps = new PagedDataSource();
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable ds = deviceManage.GetDeviceNameList(this.ddl_category.SelectedValue, "0", "0", this.ddl_location.SelectedValue, this.ddl_Department.SelectedValue);
        if (ds == null)
        {
            log4net.ILog log = log4net.LogManager.GetLogger("logerror");
            log.Error("Add Project:" + deviceManage.errMsg);
            return;
        }
        ps.DataSource = ds.DefaultView;
        ps.AllowPaging = true;
        ps.PageSize = 4;
        ps.CurrentPageIndex = curpage - 1;
        this.lnkbtnOne.Enabled = true;
        this.lnkbtnBack.Enabled = true;
        this.lnkbtnNext.Enabled = true;
        this.lnkbtnUp.Enabled = true;

        if (curpage == 1)
        {
            this.lnkbtnOne.Enabled = false;
            this.lnkbtnUp.Enabled = false;
        }
        if (curpage == ps.PageCount)
        {
            this.lnkbtnNext.Enabled = false;
            this.lnkbtnBack.Enabled = false;
        }

        this.labBackPage.Text = Convert.ToString(ps.PageCount);

        this.dl_EquipmentInfo.DataSource = ps;
        this.dl_EquipmentInfo.DataKeyField = "id";
        this.dl_EquipmentInfo.DataBind();
    }
    protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("EquipmentDetail") == 0)
        {
            string scriptText = "var obj = window.showModalDialog('../Manager/Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            //Response.Write("<script>window.open('EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'width=580px, height=500px, top=100px, left=100px'); </script>");
            //string scriptText = "window.open('../Manager/Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "&type=view', '', 'width=580px, height=500px, top=100px, left=100px, scrollbars=yes');";
            ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
        }
    }
    protected void dl_EquipmentInfo_DataBinding(object sender, EventArgs e)
    {
        GetSelectedItem();
        changed = true;
    }
    protected void dl_EquipmentInfo_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemIndex > -1 && this.SelectedItems != null)
        {
            string id = dl_EquipmentInfo.DataKeys[e.Item.ItemIndex].ToString();
            if (this.SelectedItems.Contains(id))
            {
                CheckBox cbx = (CheckBox)e.Item.FindControl("CheckBox1");
                cbx.Checked = true;
            }
        }
    }

    /// <summary>
    /// 第一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnOne_Click(object sender, EventArgs e)
    {
        this.labPage.Text = "1";
        this.PagedDataBind();
    }
    /// <summary>
    /// 上一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnUp_Click(object sender, EventArgs e)
    {
        this.labPage.Text = Convert.ToString(Convert.ToInt32(this.labPage.Text) - 1);
        this.PagedDataBind();
    }
    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        this.labPage.Text = Convert.ToString(Convert.ToInt32(this.labPage.Text) + 1);
        this.PagedDataBind();
    }
    /// <summary>
    /// 最后一页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        this.labPage.Text = labBackPage.Text;
        this.PagedDataBind();
    }
    #endregion


    protected void btn_Serach_Click(object sender, EventArgs e)
    {
        DateTime startDate = DateTime.Parse(this.tb_StartDate.Text);
        DateTime endDate = DateTime.Parse(this.tb_endDate.Text);
        if (startDate > endDate)
        {
            PopMsg("Start date can not after End date, plz select write date!");
            return;
        }

        GetSelectedItem();
        if (this.SelectedItems == null || this.SelectedItems.Count <= 0)
        {
            PopMsg("Please Select device or equipment first.");
            return;
        }
        if (this.ddl_category.SelectedValue.CompareTo("2") == 0 || this.ddl_category.SelectedValue == "3")
        {
            this.panel_selTimeEquip.Visible = true;
            cl_DeviceManage deviceManage = new cl_DeviceManage();
            DataTable tblDevice = deviceManage.GetDeviceDetailByIDs(this.SelectedItems);

            this.dl_Seltime.DataSource = tblDevice;
            this.dl_Seltime.DataKeyField = "s_id";
            this.dl_Seltime.DataBind();

            string personSite = "";
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                cl_PersonManage person = new cl_PersonManage();
                DataTable personInfo = person.GetPersonInfoByID(userID);
                if (personInfo != null)
                {
                    personSite = personInfo.Rows[0]["P_Location"].ToString();
                }
            }
            double clientTimeZone = 0.0;
            if (personSite.CompareTo("WKS") == 0 || personSite.CompareTo("WHC") == 0)
            {
                clientTimeZone = 8.0;
            }
            else
            {
                clientTimeZone = -6.0;
            }
            CheckDateTime(clientTimeZone, clientTimeZone - 8);
        }
        else if (this.ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            this.panel_SelDateDevice.Visible = true;
            //this.lbl_dayDuration.Text = startDate.ToString("yyyy/MM/dd") + "~" + endDate.ToString("yyyy/MM/dd");
            //dl_dateBindData(startDate, endDate);
            cl_DeviceManage deviceManage = new cl_DeviceManage();
            DataTable tblDevice = deviceManage.GetDeviceDetailByIDs(this.SelectedItems);

            this.DeviceDataSel.DataSource = tblDevice;
            this.DeviceDataSel.DataKeyField = "s_id";
            this.DeviceDataSel.DataBind();


            string personSite = "";
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                cl_PersonManage person = new cl_PersonManage();
                DataTable personInfo = person.GetPersonInfoByID(userID);
                if (personInfo != null)
                {
                    personSite = personInfo.Rows[0]["P_Location"].ToString();
                }
            }
            double clientTimeZone = 0.0;
            if (personSite.CompareTo("WKS") == 0 || personSite.CompareTo("WHC") == 0)
            {
                clientTimeZone = 8.0;
            }
            else
            {
                clientTimeZone = -6.0;
            }
            CheckDateTime(clientTimeZone, 0);
        }


        this.btn_Submit.Enabled = true;
        this.btn_Cancel.Enabled = true;
        this.PickDateTimeList = null;
    }

    protected void dl_Seltime_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        // 绑定第二层DataList数据
        DateTime startDate = DateTime.Parse(this.tb_StartDate.Text);
        DateTime endDate = DateTime.Parse(this.tb_endDate.Text);

        DataTable data = new DataTable();
        data.Columns.Add(new DataColumn("DateTime", typeof(System.Data.SqlTypes.SqlDateTime)));
        data.Columns.Add(new DataColumn("ItemIndex", typeof(System.Data.SqlTypes.SqlString)));
        data.Columns.Add(new DataColumn("DeviceID", typeof(System.Data.SqlTypes.SqlString)));
        for (; startDate.CompareTo(endDate) <= 0; startDate = startDate.AddDays(1))
        {
            DataRow row = data.NewRow();
            row["DateTime"] = startDate;
            row["ItemIndex"] = e.Item.ItemIndex.ToString();
            row["DeviceID"] = dl_Seltime.DataKeys[e.Item.ItemIndex].ToString();
            data.Rows.Add(row);
        }
        DropDownList ddl_TimeZone = (DropDownList)e.Item.FindControl("ddl_TimeZone");
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(userID);
            if (personInfo != null)
            {
                string site = personInfo.Rows[0]["P_Location"].ToString();
                cl_DeviceManage device = new cl_DeviceManage();
                DataTable deviceInfo = device.GetDeviceDetailByID(dl_Seltime.DataKeys[e.Item.ItemIndex].ToString());
                if (deviceInfo != null)
                {
                    string deviceSite = deviceInfo.Rows[0]["site"].ToString();
                    if (site.CompareTo(deviceSite) == 0)
                    {
                        if (site.CompareTo("WKS") == 0 || site.CompareTo("WHC") == 0)
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT+8", site));
                            ddl_TimeZone.SelectedValue = site;
                        }
                        else
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT-6", site));
                            ddl_TimeZone.SelectedValue = site;
                        }
                    }
                    else
                    {
                        if (site.CompareTo("WKS") == 0 || site.CompareTo("WHC") == 0)
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT+8", site));
                            if (deviceSite.CompareTo("WKS") == 0 || deviceSite.CompareTo("WHC") == 0)
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT+8", deviceSite));
                            }
                            else
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT-6", deviceSite));
                            }
                            ddl_TimeZone.SelectedValue = site;
                        }
                        else
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT-6", site));
                            if (deviceSite.CompareTo("WKS") == 0 || deviceSite.CompareTo("WHC") == 0)
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT+8", deviceSite));
                            }
                            else
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT-6", deviceSite));
                            }
                            ddl_TimeZone.SelectedValue = site;
                        }
                    }
                }
                HiddenField hf_SelTimeZone = (HiddenField)e.Item.FindControl("hf_SelTimeZone");
                if (site.CompareTo("WKS") == 0 || site.CompareTo("WHC") == 0)
                {
                    hf_SelTimeZone.Value = (8).ToString();
                }
                else
                {
                    hf_SelTimeZone.Value = (-6).ToString();
                }
                //ddl_TimeZone.SelectedValue = site;
            }
        }
        DataList dl = (DataList)e.Item.FindControl("dl_SubSelTime");
        dl.DataSource = data.DefaultView;
        dl.DataKeyField = "DateTime";
        dl.DataBind();
    }
    protected void dl_Seltime_DataBinding(object sender, EventArgs e)
    {

    }
    protected void dl_Seltime_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.CompareTo("EquipmentDetail") == 0)
        {
            //Response.Write("<script>window.open('EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'width=580px, height=500px, top=100px, left=100px'); </script>");
            string scriptText = "window.open('../Manager/Equipment/EquipmentDetail.aspx?id=" + e.CommandArgument.ToString() + "', '', 'width=580px, height=500px, top=10px, left=100px, scrollbars=yes');";
            ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
        }

        #region 清空选项
        if (e.CommandName == "Clear")
        {
            if (this.ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
            {
                DataList dl_SubSelTime = (DataList)e.Item.FindControl("dl_SubSelTime");
                HiddenField hf = (HiddenField)e.Item.FindControl("hf_SelTime");
                hf.Value = "~";
                ClearSel_Equip(dl_SubSelTime);
                //for (int index = 0; index != dl_SubSelTime.Items.Count; index++)
                //{// 清空之前的选项
                //    DataListItem line = dl_SubSelTime.Items[index];
                //    for (int indey = 0; indey != 24; indey++)
                //    {
                //        LinkButton lbtn = (LinkButton)line.FindControl("lb" + indey + "_0");
                //        if (lbtn != null)
                //        {
                //            lbtn.BackColor = System.Drawing.Color.AliceBlue;
                //        }
                //        lbtn = (LinkButton)line.FindControl("lb" + indey + "_5");
                //        if (lbtn != null)
                //        {
                //            lbtn.BackColor = System.Drawing.Color.AliceBlue;
                //        }
                //    }
                //}
            }
            else if (this.ddl_category.SelectedValue.CompareTo("1") == 0)
            {
                DataList dl_SubSelTime = (DataList)e.Item.FindControl("dl_date");
                HiddenField hf = (HiddenField)e.Item.FindControl("hf_daySel");
                hf.Value = "~";
                ClearSel_Device(dl_SubSelTime);
                //for (int index = 0; index != dl_SubSelTime.Items.Count; index++)
                //{// 清空之前的选项
                //    DataListItem line = dl_SubSelTime.Items[index];
                //    for (int indey = 0; indey != 7; indey++)
                //    {
                //        LinkButton lbtn = (LinkButton)line.FindControl("lbtn_day" + indey);
                //        if (lbtn != null && lbtn.Enabled)
                //        {
                //            lbtn.BackColor = System.Drawing.Color.AliceBlue;
                //        }
                //    }
                //}
            }
        }
        #endregion
        if (e.CommandName.CompareTo("Add") == 0)
        {
            string device_id = e.CommandArgument.ToString().Split(',')[0];
            string device_name = e.CommandArgument.ToString().Split(',')[1];
            if (this.ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
            {
                DataList dl_SubSelTime = (DataList)e.Item.FindControl("dl_SubSelTime");
                HiddenField hf = (HiddenField)e.Item.FindControl("hf_SelTime");
                GridView gv_pickTimeList = e.Item.FindControl("gv_pickTimeList") as GridView;
                string startDateTime = hf.Value.Split('~')[0];
                string endDateTime = startDateTime;
                if (hf.Value.Split('~')[1].CompareTo(String.Empty) != 0)
                    endDateTime = hf.Value.Split('~')[1];
                if (startDateTime.CompareTo(String.Empty) == 0)
                    return;
                InitPickDataTimeList();
                DataRow row = PickDateTimeList.NewRow();
                row["Device_ID"] = device_id;
                row["Device_Name"] = device_name;
                row["StartDateTime"] = DateTime.Parse(startDateTime);

                DateTime enddatetime = DateTime.Parse(endDateTime);
                enddatetime = enddatetime.AddMinutes(30);

                row["EndDateTime"] = enddatetime;
                PickDateTimeList.Rows.Add(row.ItemArray);
                DataRow[] rows = this.PickDateTimeList.Select("Device_ID=" + device_id);
                DataTable table = this.PickDateTimeList.Clone();
                foreach (DataRow o in rows)
                {
                    table.ImportRow(o);
                }
                gv_pickTimeList.DataSource = table.DefaultView;
                gv_pickTimeList.DataKeyNames = new string[] { "id" };
                gv_pickTimeList.DataBind();
            }
            else if (this.ddl_category.SelectedValue.CompareTo("1") == 0)
            {
                DataList dl_date = (DataList)e.Item.FindControl("dl_date");
                HiddenField hf = (HiddenField)e.Item.FindControl("hf_daySel");
                GridView gv_pickTimeList_Device = e.Item.FindControl("gv_pickTimeList_Device") as GridView;

                string startDateTime = hf.Value.Split('~')[0];
                string endDateTime = startDateTime;
                if (hf.Value.Split('~')[1].CompareTo(String.Empty) != 0)
                    endDateTime = hf.Value.Split('~')[1];
                if (startDateTime.CompareTo(String.Empty) == 0)
                    return;
                InitPickDataTimeList();
                DataRow row = PickDateTimeList.NewRow();
                row["Device_ID"] = device_id;
                row["Device_Name"] = device_name;



                DateTime enddatetime = DateTime.Parse(endDateTime);
                enddatetime = enddatetime.AddDays(1);


                row["StartDateTime"] = DateTime.Parse(startDateTime);
                row["EndDateTime"] = enddatetime;


                PickDateTimeList.Rows.Add(row.ItemArray);
                DataRow[] rows = this.PickDateTimeList.Select("Device_ID=" + device_id);
                DataTable table = this.PickDateTimeList.Clone();
                foreach (DataRow o in rows)
                {
                    table.ImportRow(o);
                }
                gv_pickTimeList_Device.DataSource = table.DefaultView;
                gv_pickTimeList_Device.DataKeyNames = new string[] { "id" };
                gv_pickTimeList_Device.DataBind();
            }
        }
    }

    private void InitPickDataTimeList()
    {
        if (PickDateTimeList == null)
        {
            PickDateTimeList = new DataTable();
            PickDateTimeList.Columns.Add(new DataColumn("id", typeof(int)));
            PickDateTimeList.PrimaryKey = new DataColumn[] { PickDateTimeList.Columns["id"] };
            PickDateTimeList.Columns["id"].AutoIncrement = true;
            PickDateTimeList.Columns["id"].AutoIncrementSeed = 1;
            PickDateTimeList.Columns["id"].AutoIncrementStep = 1;
            PickDateTimeList.Columns.Add(new DataColumn("Device_ID", typeof(String)));
            PickDateTimeList.Columns.Add(new DataColumn("Device_Name", typeof(String)));
            PickDateTimeList.Columns.Add(new DataColumn("StartDateTime", typeof(DateTime)));
            PickDateTimeList.Columns.Add(new DataColumn("EndDateTime", typeof(DateTime)));
        }
    }
    protected void dl_SubSelTime_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "SelTime")
        {
            string[] argv = e.CommandArgument.ToString().Split(',');
            LinkButton lbtn = (LinkButton)e.CommandSource;
            if (lbtn.Enabled && lbtn.Attributes["bookingId"] != null)
            {
                string bookingId = lbtn.Attributes["bookingId"].ToString();
                string scriptText = "window.open('../Manager/DeviceBooking/ApprovePage_View.aspx?bookingId=" + bookingId + "', '', 'width=760px, top=10px, left=100px' );";
                ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
            }
            else
            {
                string personSite = "";
                if (Session["UserID"] != null)
                {
                    string userID = Session["UserID"].ToString();
                    cl_PersonManage person = new cl_PersonManage();
                    DataTable personInfo = person.GetPersonInfoByID(userID);
                    if (personInfo != null)
                    {
                        personSite = personInfo.Rows[0]["P_Location"].ToString();
                    }
                }
                if (personSite != String.Empty)
                {
                    DropDownList ddl = (DropDownList)this.dl_Seltime.Items[int.Parse(argv[0])].FindControl("ddl_TimeZone");
                    ddl.SelectedValue = personSite;
                    ddl_TimeZone_SelectedIndexChanged(ddl, null);
                    //ddl.SelectedIndexChanged += new EventHandler(ddl_TimeZone_SelectedIndexChanged);
                }
                HiddenField hf = (HiddenField)this.dl_Seltime.Items[int.Parse(argv[0])].FindControl("hf_SelTime");

                HiddenField hf_timeZone = (HiddenField)this.dl_Seltime.Items[int.Parse(argv[0])].FindControl("hf_SelTimeZone");

                DataList dl_SubSelTime = (DataList)this.dl_Seltime.Items[int.Parse(argv[0])].FindControl("dl_SubSelTime");
                string date = DateTime.Parse(dl_SubSelTime.DataKeys[e.Item.ItemIndex].ToString()).ToString("yyyy/MM/dd");
                string selDateTime = date + " " + GetTimeStr(lbtn.ID);
                SetHFData(hf, DateTime.Parse(selDateTime), argv);
                //PopMsg(hf.Value);


                double showTimeZone = Convert.ToDouble(hf_timeZone.Value);
                double timeZone = showTimeZone - GetClientTimeZone();
                //SelTimeDuration_Ex(dl_SubSelTime, hf, timeZone);
                SelTimeDuration(dl_SubSelTime, hf);
            }
        }
    }
    private void SelTimeDuration(DataList dl_SubSelTime, HiddenField hf)
    {
        if (this.ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
        {
            #region Equiupment
            if (hf.Value.Split('~')[0].CompareTo(String.Empty) == 0)
                return;
            DateTime startDateTime = DateTime.Parse(hf.Value.Split('~')[0]);
            DateTime endDateTime = startDateTime;
            if (hf.Value.Split('~')[1].CompareTo(String.Empty) != 0)
            {
                endDateTime = DateTime.Parse(hf.Value.Split('~')[1]);
            }
            ClearSel_Equip(dl_SubSelTime);
            for (DateTime dateTimeIndex = startDateTime; dateTimeIndex <= endDateTime; dateTimeIndex = dateTimeIndex.AddMinutes(30))
            {
                DateTime date = new DateTime(dateTimeIndex.Year, dateTimeIndex.Month, dateTimeIndex.Day);
                int result = Contains(dl_SubSelTime, date.ToString());
                if (result > -1)
                {
                    DataListItem line = dl_SubSelTime.Items[result];
                    LinkButton lbtn = (LinkButton)line.FindControl(GetId(dateTimeIndex));
                    if (lbtn != null && lbtn.BackColor == System.Drawing.Color.Red)
                    {
                        ClearSel_Equip(dl_SubSelTime);
                        break;
                    }
                    if (lbtn != null)
                    {
                        lbtn.BackColor = System.Drawing.Color.Blue;
                    }
                }
            }
            #endregion
        }
        else if (this.ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            #region Device
            if (hf.Value.Split('~')[0].CompareTo(String.Empty) == 0)
                return;
            DateTime startDateTime = DateTime.Parse(hf.Value.Split('~')[0]);
            DateTime endDateTime = startDateTime;
            if (hf.Value.Split('~')[1].CompareTo(String.Empty) != 0)
            {
                endDateTime = DateTime.Parse(hf.Value.Split('~')[1]);
            }
            //for (int index = 0; index != dl_SubSelTime.Items.Count; index++)
            //{// 清空之前的选项
            //    DataListItem line = dl_SubSelTime.Items[index];
            //    for (int indey = 0; indey != 7; indey++)
            //    {
            //        LinkButton lbtn = (LinkButton)line.FindControl("lbtn_day" + indey);
            //        if (lbtn != null && lbtn.Enabled)
            //        {
            //            lbtn.BackColor = System.Drawing.Color.AliceBlue;
            //        }
            //    }
            //}
            ClearSel_Device(dl_SubSelTime);

            for (DateTime dateTimeIndex = startDateTime; dateTimeIndex <= endDateTime; dateTimeIndex = dateTimeIndex.AddDays(1))
            {
                DateTime date = new DateTime(dateTimeIndex.Year, dateTimeIndex.Month, dateTimeIndex.Day);
                int result = Contains(dl_SubSelTime, date);
                if (result > -1)
                {
                    DataListItem line = dl_SubSelTime.Items[result];
                    LinkButton lbtn = (LinkButton)line.FindControl(GetId_Device(dateTimeIndex));

                    if (lbtn != null && lbtn.BackColor == System.Drawing.Color.Red)
                    {
                        ClearSel_Device(dl_SubSelTime);
                        break;
                    }

                    if (lbtn != null && lbtn.Enabled)
                    {
                        lbtn.BackColor = System.Drawing.Color.Blue;
                    }
                }
            }
            #endregion
        }
    }
    private void SelTimeDuration_Ex(DataList dl_SubSelTime, HiddenField hf, double timeZone)
    {
        if (this.ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
        {
            #region Equiupment
            if (hf.Value.Split('~')[0].CompareTo(String.Empty) == 0)
                return;
            DateTime startDateTime = DateTime.Parse(hf.Value.Split('~')[0]);
            DateTime endDateTime = startDateTime;
            if (hf.Value.Split('~')[1].CompareTo(String.Empty) != 0)
            {
                endDateTime = DateTime.Parse(hf.Value.Split('~')[1]);
            }
            ClearSel_Equip(dl_SubSelTime);

            startDateTime = startDateTime.AddHours(timeZone);
            endDateTime = endDateTime.AddHours(timeZone);

            for (DateTime dateTimeIndex = startDateTime; dateTimeIndex <= endDateTime; dateTimeIndex = dateTimeIndex.AddMinutes(30))
            {
                DateTime date = new DateTime(dateTimeIndex.Year, dateTimeIndex.Month, dateTimeIndex.Day);
                int result = Contains(dl_SubSelTime, date.ToString());
                if (result > -1)
                {
                    DataListItem line = dl_SubSelTime.Items[result];
                    LinkButton lbtn = (LinkButton)line.FindControl(GetId(dateTimeIndex));
                    if (lbtn != null && lbtn.BackColor == System.Drawing.Color.Red)
                    {
                        ClearSel_Equip(dl_SubSelTime);
                        break;
                    }
                    if (lbtn != null)
                    {
                        lbtn.BackColor = System.Drawing.Color.Blue;
                    }
                }
            }
            #endregion
        }
        else if (this.ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            #region Device
            if (hf.Value.Split('~')[0].CompareTo(String.Empty) == 0)
                return;
            DateTime startDateTime = DateTime.Parse(hf.Value.Split('~')[0]);
            DateTime endDateTime = startDateTime;
            if (hf.Value.Split('~')[1].CompareTo(String.Empty) != 0)
            {
                endDateTime = DateTime.Parse(hf.Value.Split('~')[1]);
            }
            //for (int index = 0; index != dl_SubSelTime.Items.Count; index++)
            //{// 清空之前的选项
            //    DataListItem line = dl_SubSelTime.Items[index];
            //    for (int indey = 0; indey != 7; indey++)
            //    {
            //        LinkButton lbtn = (LinkButton)line.FindControl("lbtn_day" + indey);
            //        if (lbtn != null && lbtn.Enabled)
            //        {
            //            lbtn.BackColor = System.Drawing.Color.AliceBlue;
            //        }
            //    }
            //}
            ClearSel_Device(dl_SubSelTime);

            startDateTime = startDateTime.AddHours(timeZone);
            endDateTime = endDateTime.AddHours(timeZone);

            for (DateTime dateTimeIndex = startDateTime; dateTimeIndex <= endDateTime; dateTimeIndex = dateTimeIndex.AddDays(1))
            {
                DateTime date = new DateTime(dateTimeIndex.Year, dateTimeIndex.Month, dateTimeIndex.Day);
                int result = Contains(dl_SubSelTime, date);
                if (result > -1)
                {
                    DataListItem line = dl_SubSelTime.Items[result];
                    LinkButton lbtn = (LinkButton)line.FindControl(GetId_Device(dateTimeIndex));

                    if (lbtn != null && lbtn.BackColor == System.Drawing.Color.Red)
                    {
                        ClearSel_Device(dl_SubSelTime);
                        break;
                    }

                    if (lbtn != null && lbtn.Enabled)
                    {
                        lbtn.BackColor = System.Drawing.Color.Blue;
                    }
                }
            }
            #endregion
        }
    }
    private void ClearSel_Device(DataList dataList)
    {
        for (int index = 0; index != dataList.Items.Count; index++)
        {// 清空之前的选项
            DataListItem line = dataList.Items[index];
            for (int indey = 0; indey != 7; indey++)
            {
                LinkButton lbtn = (LinkButton)line.FindControl("lbtn_day" + indey);
                if (lbtn != null && lbtn.Enabled && lbtn.BackColor != System.Drawing.Color.Red)
                {
                    lbtn.BackColor = System.Drawing.Color.AliceBlue;
                }
            }
        }
    }
    private void ClearSel_Equip(DataList dataList)
    {
        for (int index = 0; index != dataList.Items.Count; index++)
        {// 清空之前的选项
            DataListItem line = dataList.Items[index];
            for (int indey = 0; indey != 24; indey++)
            {
                LinkButton lbtn = (LinkButton)line.FindControl("lb" + indey + "_0");
                if (lbtn != null && lbtn.BackColor != System.Drawing.Color.Red)
                {
                    lbtn.BackColor = System.Drawing.Color.AliceBlue;
                }
                lbtn = (LinkButton)line.FindControl("lb" + indey + "_5");
                if (lbtn != null && lbtn.BackColor != System.Drawing.Color.Red)
                {
                    lbtn.BackColor = System.Drawing.Color.AliceBlue;
                }
            }
        }
    }
    private int Contains(DataList dl_SubSelTime, DateTime date)
    {
        for (int index = 0; index != dl_SubSelTime.DataKeys.Count; index++)
        {
            DateTime dataKey = DateTime.Parse(dl_SubSelTime.DataKeys[index].ToString());
            if (date >= dataKey && date < dataKey.AddDays(7))
            {
                return index;
            }
        }
        return -1;
    }
    private int Contains(DataList dl_SubSelTime, string date)
    {
        for (int index = 0; index != dl_SubSelTime.DataKeys.Count; index++)
        {
            string dataKey = dl_SubSelTime.DataKeys[index].ToString();
            if (date.ToString().CompareTo(dataKey) == 0)
            {
                return index;
            }
        }
        return -1;
    }
    private void SetHFData(HiddenField hf, DateTime dt, string[] argv)
    {
        string[] dateTimeStr = hf.Value.Split('~');
        if (dateTimeStr[0].CompareTo(String.Empty) == 0
            || (dateTimeStr[0].CompareTo(String.Empty) != 0 && dateTimeStr[1].CompareTo(String.Empty) != 0))
        {// []~[] OR [dateTime1]~[dateTime2]
            hf.Value = dt.ToString() + "~";
        }
        else if (dateTimeStr[1].CompareTo(String.Empty) == 0)
        {// dateTime~[]
            DateTime startDate = DateTime.Parse(dateTimeStr[0]);
            if (dt >= startDate)
            {
                hf.Value += dt.ToString();
            }
            else
            {
                hf.Value = dt.ToString() + "~" + startDate.ToString();
            }
        }
    }
    private string GetTimeStr(string lbtnId)
    {
        //lb12_0
        lbtnId = lbtnId.Replace("lb", "");
        string[] time = lbtnId.Split('_');
        DateTime ts = new DateTime(1, 1, 1, int.Parse(time[0]), int.Parse(time[1]) * 6, 0);
        return ts.ToString("HH:mm");
    }

    private string GetId(DateTime timeStr)
    {
        string lbtnId = "lb";
        lbtnId += timeStr.Hour + "_";
        lbtnId += timeStr.Minute / 6;

        return lbtnId;
    }
    private string GetId_Device(DateTime timeStr)
    {
        string lbtnId = "lbtn_day";
        lbtnId += GetDayIndexOfweek(timeStr);
        return lbtnId;
    }


    protected void dl_date_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "SelDay")
        {
            string[] argv = e.CommandArgument.ToString().Split(',');
            LinkButton lbtn = (LinkButton)e.CommandSource;

            if (lbtn.Enabled && lbtn.Attributes["bookingId"] != null)
            {
                string bookingId = lbtn.Attributes["bookingId"].ToString();
                string scriptText = "window.open('../Manager/DeviceBooking/ApprovePage_View.aspx?bookingId=" + bookingId + "', '', 'width=760px, top=10px, left=100px' );";
                ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
            }
            else
            {
                string personSite = "";
                if (Session["UserID"] != null)
                {
                    string userID = Session["UserID"].ToString();
                    cl_PersonManage person = new cl_PersonManage();
                    DataTable personInfo = person.GetPersonInfoByID(userID);
                    if (personInfo != null)
                    {
                        personSite = personInfo.Rows[0]["P_Location"].ToString();
                    }
                }
                if (personSite != String.Empty)
                {
                    DropDownList ddl = (DropDownList)this.DeviceDataSel.Items[int.Parse(argv[0])].FindControl("ddl_TimeZone_Device");
                    ddl.SelectedValue = personSite;
                    ddl_TimeZone_Device_SelectedIndexChanged(ddl, null);
                    //ddl.SelectedIndexChanged += new EventHandler(ddl_TimeZone_SelectedIndexChanged);
                }


                HiddenField hf = (HiddenField)this.DeviceDataSel.Items[int.Parse(argv[0])].FindControl("hf_daySel");

                HiddenField hf_timeZone = (HiddenField)this.DeviceDataSel.Items[int.Parse(argv[0])].FindControl("hf_SelTimeZone");

                DataList dl_date = (DataList)this.DeviceDataSel.Items[int.Parse(argv[0])].FindControl("dl_date");
                DateTime date = DateTime.Parse(dl_date.DataKeys[e.Item.ItemIndex].ToString());
                //PopMsg(e.CommandArgument.ToString() + ":" + date.AddDays(GetDayIndex(lbtn.ID)).ToString("yyyy/MM/dd"));
                DateTime selDateTime = date.AddDays(GetDayIndex(lbtn.ID));
                SetHFData(hf, selDateTime, argv);
                //PopMsg(hf.Value);

                double showTimeZone = Convert.ToDouble(hf_timeZone.Value);
                double timeZone = showTimeZone - GetClientTimeZone();
                //SelTimeDuration_Ex(dl_date, hf, timeZone);
                SelTimeDuration(dl_date, hf);
            }
        }
    }

    protected void DeviceDataSel_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        DropDownList ddl_TimeZone = (DropDownList)e.Item.FindControl("ddl_TimeZone_Device");
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(userID);
            if (personInfo != null)
            {
                string site = personInfo.Rows[0]["P_Location"].ToString();
                cl_DeviceManage device = new cl_DeviceManage();
                DataTable deviceInfo = device.GetDeviceDetailByID(DeviceDataSel.DataKeys[e.Item.ItemIndex].ToString());
                if (deviceInfo != null)
                {
                    string deviceSite = deviceInfo.Rows[0]["site"].ToString();
                    if (site.CompareTo(deviceSite) == 0)
                    {
                        if (site.CompareTo("WKS") == 0 || site.CompareTo("WHC") == 0)
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT+8", site));
                            ddl_TimeZone.SelectedValue = site;
                        }
                        else
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT-6", site));
                            ddl_TimeZone.SelectedValue = site;
                        }
                    }
                    else
                    {
                        if (site.CompareTo("WKS") == 0 || site.CompareTo("WHC") == 0)
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT+8", site));
                            if (deviceSite.CompareTo("WKS") == 0 || deviceSite.CompareTo("WHC") == 0)
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT+8", deviceSite));
                            }
                            else
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT-6", deviceSite));
                            }
                            ddl_TimeZone.SelectedValue = site;
                        }
                        else
                        {
                            ddl_TimeZone.Items.Clear();
                            ddl_TimeZone.Items.Add(new ListItem(site + "-GMT-6", site));
                            if (deviceSite.CompareTo("WKS") == 0 || deviceSite.CompareTo("WHC") == 0)
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT+8", deviceSite));
                            }
                            else
                            {
                                ddl_TimeZone.Items.Add(new ListItem(deviceSite + "-GMT-6", deviceSite));
                            }
                            ddl_TimeZone.SelectedValue = site;
                        }
                    }
                }

                HiddenField hf_SelTimeZone = (HiddenField)e.Item.FindControl("hf_SelTimeZone");
                if (site.CompareTo("WKS") == 0 || site.CompareTo("WHC") == 0)
                {
                    hf_SelTimeZone.Value = (8).ToString();
                }
                else
                {
                    hf_SelTimeZone.Value = (-6).ToString();
                }
                //ddl_TimeZone.SelectedValue = site;
            }
        }
        // 绑定第二层DataList数据
        DateTime startDate = DateTime.Parse(this.tb_StartDate.Text);
        DateTime endDate = DateTime.Parse(this.tb_endDate.Text);

        DataList dl_date = (DataList)e.Item.FindControl("dl_date");
        string itemIndex = e.Item.ItemIndex.ToString();
        string device_id = DeviceDataSel.DataKeys[e.Item.ItemIndex].ToString();
        dl_dateBindData(dl_date, startDate, endDate, itemIndex, device_id);
    }
    private void dl_dateBindData(DataList dl_date, DateTime startDate, DateTime endDate, string itemIndex, string device_id)
    {
        DateTime SundayStart = GetSundayDate(startDate);
        DateTime SatdayEnd = GetSatdayDate(endDate);

        DataTable table = new DataTable("dayPick");
        DataColumn column = new DataColumn();
        DataRow row;


        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "week";
        table.Columns.Add(column);
        column = new DataColumn();
        column.DataType = System.Type.GetType("System.String");
        column.ColumnName = "day";
        table.Columns.Add(column);
        table.Columns.Add(new DataColumn("ItemIndex", typeof(System.Data.SqlTypes.SqlString)));
        table.Columns.Add(new DataColumn("DeviceID", typeof(System.Data.SqlTypes.SqlString)));
        for (DateTime dayIndex = SundayStart; dayIndex <= SatdayEnd; dayIndex = dayIndex.AddDays(7))
        {
            row = table.NewRow();
            row["week"] = dayIndex.ToString("yyyy/MM/dd") + "~" + dayIndex.AddDays(6).ToString("yyyy/MM/dd");
            table.Rows.Add(row);
            row["day"] = dayIndex.ToString("yyyy/MM/dd");
            row["ItemIndex"] = itemIndex;
            row["DeviceID"] = device_id;
            //this.dl_date.
        }

        dl_date.DataSource = table;
        dl_date.DataKeyField = "day";
        dl_date.DataBind();

        for (DateTime dayIndex = startDate; dayIndex <= endDate; dayIndex = dayIndex.AddDays(1))
        {
            int dayIndexOfweek = GetDayIndexOfweek(dayIndex);
            for (int index = 0; index != dl_date.Items.Count; index++)
            {
                DataListItem line = dl_date.Items[index];
                DateTime date = DateTime.Parse(dl_date.DataKeys[index].ToString());
                if (dayIndex >= date && dayIndex < date.AddDays(7))
                {
                    LinkButton lbtn = (LinkButton)line.FindControl(GetLbtnId(dayIndexOfweek));
                    if (!lbtn.Enabled && lbtn.BackColor == System.Drawing.Color.Gray)
                    {
                        lbtn.Enabled = true;
                        lbtn.BackColor = System.Drawing.Color.AliceBlue;
                    }
                }
            }
        }
    }
    private string GetLbtnId(int dayIndexOfWeek)
    {
        return "lbtn_day" + dayIndexOfWeek;
    }
    private int GetDayIndexOfweek(DateTime date)
    {// Begin with sunday
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                return 0;
            case DayOfWeek.Monday:
                return 1;
            case DayOfWeek.Tuesday:
                return 2;
            case DayOfWeek.Wednesday:
                return 3;
            case DayOfWeek.Thursday:
                return 4;
            case DayOfWeek.Friday:
                return 5;
            case DayOfWeek.Saturday:
                return 6;
            default:
                return 0;
        }
    }
    private DateTime GetSundayDate(DateTime date)
    {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                return date;
            case DayOfWeek.Monday:
                return date.AddDays(-1);
            case DayOfWeek.Tuesday:
                return date.AddDays(-2);
            case DayOfWeek.Wednesday:
                return date.AddDays(-3);
            case DayOfWeek.Thursday:
                return date.AddDays(-4);
            case DayOfWeek.Friday:
                return date.AddDays(-5);
            case DayOfWeek.Saturday:
                return date.AddDays(-6);
            default:
                return date;
        }
    }
    private DateTime GetSatdayDate(DateTime date)
    {
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                return date.AddDays(6);
            case DayOfWeek.Monday:
                return date.AddDays(5);
            case DayOfWeek.Tuesday:
                return date.AddDays(4);
            case DayOfWeek.Wednesday:
                return date.AddDays(3);
            case DayOfWeek.Thursday:
                return date.AddDays(2);
            case DayOfWeek.Friday:
                return date.AddDays(1);
            case DayOfWeek.Saturday:
                return date.AddDays(0);
            default:
                return date;
        }
    }
    private int GetDayIndex(string lbtnId)
    {
        lbtnId = lbtnId.Replace("lbtn_day", "");
        return int.Parse(lbtnId);
    }


    private void CheckDateTime(double clientTimeZone, double showTimeZone)
    {
        //// Check时间进度，禁用已经过时的选项块
        //for (int index = 0; index != this.dl_Seltime.Items.Count; index++)
        //{
        //    DataList dataList = (DataList)this.dl_Seltime.Items[index].FindControl("dl_SubSelTime");
        //    for (int indey = 0; indey != dataList.Items.Count; indey++)
        //    {
        //        DateTime date = DateTime.Parse(dataList.DataKeys[indey].ToString());
        //        if (date.ToString("yyyy/MM/dd").CompareTo(DateTime.Now.ToString("yyyy/MM/dd")) == 0)
        //        {// 当天
        //            DataListItem item = dataList.Items[indey];
        //            Forbidden(ref item, DateTime.Now.Hour, DateTime.Now.Minute);
        //        }
        //        else
        //        {
        //            if (date.CompareTo(DateTime.Now) < 0)
        //            {// 当天以前全部禁用
        //                DataListItem item = dataList.Items[indey];
        //                Forbidden(ref item, 0, 0);
        //            }
        //        }
        //    }
        //}

        // 根据Device_ID将已经借用的纪录显示出来
        // 需要转换到服务器所在时区
        DateTime startDate = DateTime.Parse(this.tb_StartDate.Text).AddHours(8 - clientTimeZone);
        DateTime endDate = DateTime.Parse(this.tb_endDate.Text).AddDays(1).AddHours(8 - clientTimeZone);
        if (this.ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
        {
            for (int index = 0; index != this.dl_Seltime.Items.Count; index++)
            {
                string device_id = this.dl_Seltime.DataKeys[index].ToString().Trim();
                DataList dataList = (DataList)this.dl_Seltime.Items[index].FindControl("dl_SubSelTime");
                cl_DeviceBookingManage deviceBookingManagement = new cl_DeviceBookingManage();
                //startDate = startDate.AddHours(8);
                //endDate = endDate.AddHours(20);
                DataTableCollection tables = deviceBookingManagement.GetBookingItemsByIDandSDT(device_id, startDate, endDate);
                if (tables != null && tables.Count > 0)
                {
                    DataTable table = tables[0];
                    for (int indey = 0; indey != table.Rows.Count; indey++)
                    {
                        DateTime loan_DT = DateTime.Parse(table.Rows[indey]["Loan_DateTime"].ToString());
                        DateTime pRet_DT = DateTime.Parse(table.Rows[indey]["Plan_To_ReDateTime"].ToString());
                        // 需要再转换为客户端选择在时区
                        loan_DT = loan_DT.AddHours(showTimeZone);
                        pRet_DT = pRet_DT.AddHours(showTimeZone);
                        bool started = true;
                        string loanerId = table.Rows[indey]["Loaner_ID"].ToString();
                        string bookingId = table.Rows[indey]["Booking_ID"].ToString();
                        cl_PersonManage personManage = new cl_PersonManage();
                        string userName = personManage.GetPersonInfoByID(loanerId).Rows[0]["P_Name"].ToString();
                        for (DateTime timeIndex = loan_DT; timeIndex < pRet_DT; timeIndex = timeIndex.AddHours(0.5))
                        {
                            MakeBooked(dataList, timeIndex, ref started, "", bookingId);
                        }
                    }
                }
            }
        }
        else if (this.ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            for (int index = 0; index != this.DeviceDataSel.Items.Count; index++)
            {
                string device_id = this.DeviceDataSel.DataKeys[index].ToString().Trim();
                DataList dataList = (DataList)this.DeviceDataSel.Items[index].FindControl("dl_date");
                cl_DeviceBookingManage deviceBookingManagement = new cl_DeviceBookingManage();
                //startDate = startDate.AddHours(8);
                //endDate = endDate.AddHours(20);
                DataTableCollection tables = deviceBookingManagement.GetBookingItemsByIDandSDT(device_id, startDate, endDate);
                if (tables != null && tables.Count > 0)
                {
                    DataTable table = tables[0];
                    for (int indey = 0; indey != table.Rows.Count; indey++)
                    {
                        DateTime loan_DT = DateTime.Parse(table.Rows[indey]["Loan_DateTime"].ToString());
                        DateTime pRet_DT = DateTime.Parse(table.Rows[indey]["Plan_To_ReDateTime"].ToString());
                        bool started = true;
                        string loanerId = table.Rows[indey]["Loaner_ID"].ToString();
                        string bookingId = table.Rows[indey]["Booking_ID"].ToString();
                        cl_PersonManage personManage = new cl_PersonManage();
                        string userName = personManage.GetPersonInfoByID(loanerId).Rows[0]["P_Name"].ToString();
                        for (DateTime timeIndex = loan_DT; timeIndex <= pRet_DT; timeIndex = timeIndex.AddDays(1))
                        {
                            MakeBooked(dataList, timeIndex, ref started, userName, bookingId);
                        }
                    }
                }
            }
        }
    }
    private void MakeBooked(DataList dataList, DateTime time, ref bool started, string loanerId, string bookingId)
    {
        if (ddl_category.SelectedValue.CompareTo("2") == 0 || ddl_category.SelectedValue.CompareTo("3") == 0)
        {
            for (int index = 0; index != dataList.Items.Count; index++)
            {
                DateTime date = DateTime.Parse(dataList.DataKeys[index].ToString());
                if (date.ToString("yyyy/MM/dd").CompareTo(time.ToString("yyyy/MM/dd")) == 0)
                {
                    string lbtnId = "lb" + time.Hour + "_" + time.Minute / 6;
                    LinkButton lbtn = (LinkButton)dataList.Items[index].FindControl(lbtnId);
                    lbtn.Enabled = true;
                    lbtn.BackColor = System.Drawing.Color.Red;
                    if (started)
                    {
                        lbtn.Text = loanerId;
                        started = false;
                    }
                    lbtn.Attributes.Add("bookingId", bookingId);
                    break;
                }
            }
        }
        else if (ddl_category.SelectedValue.CompareTo("1") == 0)
        {
            for (int index = 0; index != dataList.Items.Count; index++)
            {
                DateTime date = DateTime.Parse(dataList.DataKeys[index].ToString());
                if (time >= date && time < date.AddDays(7))
                {
                    string lbtnId = GetId_Device(time);
                    LinkButton lbtn = (LinkButton)dataList.Items[index].FindControl(lbtnId);
                    lbtn.Enabled = true;
                    lbtn.BackColor = System.Drawing.Color.Red;
                    if (started)
                    {
                        lbtn.Text = loanerId;
                        started = false;
                    }
                    lbtn.Attributes.Add("bookingId", bookingId);
                    break;
                }
            }
        }
    }
    private void PopMsg(string msg)
    {
        if (msg == null || msg.CompareTo(String.Empty) == 0)
            return;
        msg = msg.Replace("\r", "\\r");
        msg = msg.Replace("\n", "\\n");
        string scriptText = "alert('" + msg + "')";
        System.Web.UI.ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string scriptText = "window.open('FillBorrowingInfo.aspx?deviceClass=" + this.ddl_category.SelectedValue + "', '', 'width=730px, scrollbars=yes, top=100px, left=100px');";
        ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
    }
    protected void btn_Cancel_Click(object sender, EventArgs e)
    {

        //Response.Write("<script>window.location.href = window.location.href;</script>");
        string scriptText = "window.location.href = window.location.href;";
        System.Web.UI.ScriptManager.RegisterStartupScript(this.up_conditionSearch, this.GetType(), "click", scriptText, true);
    }


    private string GetBookingID(string device_class)
    {
        cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();

        string serialNumber = deviceBookingManage.GetMaxBookingID(device_class);
        if (serialNumber != "0" && serialNumber != String.Empty)
        {
            string IDType = serialNumber.Substring(0, 2);
            string headDate = serialNumber.Substring(2, 8);
            int lastNumber = int.Parse(serialNumber.Substring(10));

            if (headDate == DateTime.Now.ToString("yyyyMMdd"))
            {
                lastNumber++;
                return IDType + headDate + lastNumber.ToString("0000");
            }
        }
        return device_class + DateTime.Now.ToString("yyyyMMdd") + "0001";
    }


    protected void ddl_TimeZone_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl_TimeZone = (DropDownList)sender;
        DataListItem dl = (DataListItem)(ddl_TimeZone.Parent);
        string personSite = "";
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(userID);
            if (personInfo != null)
            {
                personSite = personInfo.Rows[0]["P_Location"].ToString();
            }
        }
        double clientTimeZone = 0.0;
        if (personSite.CompareTo("WKS") == 0 || personSite.CompareTo("WHC") == 0)
        {
            clientTimeZone = 8.0;
        }
        else
        {
            clientTimeZone = -6.0;
        }
        double timeZone = 0.0;
        double selectTimeZone = 0.0;
        HiddenField hf_SelTimeZone = (HiddenField)dl.FindControl("hf_SelTimeZone");
        if (ddl_TimeZone.SelectedValue.CompareTo("WKS") == 0 || ddl_TimeZone.SelectedValue.CompareTo("WHC") == 0)
        {
            //double timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
            //HiddenField hf_TimeZone = (HiddenField)Master.FindControl("hf_TimeZone");
            //if (hf_TimeZone.Value.CompareTo(String.Empty) != 0)
            //{
            //    double clientTimeZone = Convert.ToDouble(hf_TimeZone.Value);
            //    timeZone = 8 - clientTimeZone;
            //}
            timeZone = 8 - clientTimeZone;
            hf_SelTimeZone.Value = (8).ToString();
            selectTimeZone = 8;
        }
        else
        {
            //HiddenField hf_TimeZone = (HiddenField)Master.FindControl("hf_TimeZone");
            //if (hf_TimeZone.Value.CompareTo(String.Empty) != 0)
            //{
            //    double clientTimeZone = Convert.ToDouble(hf_TimeZone.Value);
            //    timeZone = -6 - clientTimeZone;
            //}
            timeZone = -6 - clientTimeZone;
            hf_SelTimeZone.Value = (-6).ToString();
            selectTimeZone = -6;
        }
        // 绑定第二层DataList数据
        DateTime startDate = DateTime.Parse(this.tb_StartDate.Text).AddHours(timeZone);
        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        DateTime endDate = DateTime.Parse(this.tb_endDate.Text).AddHours(timeZone);
        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);


        DataTable data = new DataTable();
        data.Columns.Add(new DataColumn("DateTime", typeof(System.Data.SqlTypes.SqlDateTime)));
        data.Columns.Add(new DataColumn("ItemIndex", typeof(System.Data.SqlTypes.SqlString)));
        data.Columns.Add(new DataColumn("DeviceID", typeof(System.Data.SqlTypes.SqlString)));
        for (; startDate.CompareTo(endDate) <= 0; startDate = startDate.AddDays(1))
        {
            DataRow row = data.NewRow();
            row["DateTime"] = startDate;
            row["ItemIndex"] = dl.ItemIndex.ToString();
            row["DeviceID"] = dl_Seltime.DataKeys[dl.ItemIndex].ToString();
            data.Rows.Add(row);
        }
        DataList dlSub = (DataList)dl.FindControl("dl_SubSelTime");
        dlSub.DataSource = data.DefaultView;
        dlSub.DataKeyField = "DateTime";
        dlSub.DataBind();

        CheckDateTime(selectTimeZone, selectTimeZone - 8);


        HiddenField hf = (HiddenField)dl.FindControl("hf_SelTime");
        SelTimeDuration_Ex(dlSub, hf, timeZone);
    }
    protected void ddl_TimeZone_Device_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl_TimeZone = (DropDownList)sender;
        DataListItem dl = (DataListItem)(ddl_TimeZone.Parent);
        string personSite = "";
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(userID);
            if (personInfo != null)
            {
                personSite = personInfo.Rows[0]["P_Location"].ToString();
            }
        }
        double clientTimeZone = 0.0;
        if (personSite.CompareTo("WKS") == 0 || personSite.CompareTo("WHC") == 0)
        {
            clientTimeZone = 8.0;
        }
        else
        {
            clientTimeZone = -6.0;
        }
        double timeZone = 0.0;
        HiddenField hf_SelTimeZone = (HiddenField)dl.FindControl("hf_SelTimeZone");
        double selectTimeZone = 0.0;
        if (ddl_TimeZone.SelectedValue.CompareTo("WKS") == 0 || ddl_TimeZone.SelectedValue.CompareTo("WHC") == 0)
        {
            //double timezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
            //HiddenField hf_TimeZone = (HiddenField)Master.FindControl("hf_TimeZone");
            //if (hf_TimeZone.Value.CompareTo(String.Empty) != 0)
            //{
            //    double clientTimeZone = Convert.ToDouble(hf_TimeZone.Value);
            //    timeZone = 8 - clientTimeZone;
            //}
            timeZone = 8 - clientTimeZone;
            hf_SelTimeZone.Value = (8).ToString();
            selectTimeZone = 8;
        }
        else
        {
            //HiddenField hf_TimeZone = (HiddenField)Master.FindControl("hf_TimeZone");
            //if (hf_TimeZone.Value.CompareTo(String.Empty) != 0)
            //{
            //    double clientTimeZone = Convert.ToDouble(hf_TimeZone.Value);
            //    timeZone = -6 - clientTimeZone;
            //}
            timeZone = -6 - clientTimeZone;
            hf_SelTimeZone.Value = (-6).ToString();
            selectTimeZone = -6;
        }

        // 绑定第二层DataList数据
        DateTime startDate = DateTime.Parse(this.tb_StartDate.Text).AddHours(timeZone);
        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
        DateTime endDate = DateTime.Parse(this.tb_endDate.Text).AddHours(timeZone);
        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);

        DataList dl_date = (DataList)dl.FindControl("dl_date");
        string itemIndex = dl.ItemIndex.ToString();
        string device_id = DeviceDataSel.DataKeys[dl.ItemIndex].ToString();
        dl_dateBindData(dl_date, startDate, endDate, itemIndex, device_id);

        CheckDateTime(selectTimeZone, selectTimeZone - 8);

        HiddenField hf = (HiddenField)dl.FindControl("hf_daySel");
        SelTimeDuration_Ex(dl_date, hf, timeZone);
    }
    private double GetClientTimeZone()
    {
        string personSite = "";
        if (Session["UserID"] != null)
        {
            string userID = Session["UserID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(userID);
            if (personInfo != null)
            {
                personSite = personInfo.Rows[0]["P_Location"].ToString();
            }
        }
        double clientTimeZone = 0.0;
        if (personSite.CompareTo("WKS") == 0 || personSite.CompareTo("WHC") == 0)
        {
            clientTimeZone = 8.0;
        }
        else
        {
            clientTimeZone = -6.0;
        }

        return clientTimeZone;
    }

    protected void gv_pickTimeList_Device_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lbtn_delete = e.CommandSource as LinkButton;
        GridView gv_pickTimeList = lbtn_delete.Parent.Parent.Parent.Parent as GridView;
        if (e.CommandName.CompareTo("DeleteRow") == 0)
        {
            string id = e.CommandArgument.ToString().Split(',')[0];
            string device_id = e.CommandArgument.ToString().Split(',')[1];
            foreach (DataRow row in this.PickDateTimeList.Rows)
            {
                if (id.CompareTo(row["id"].ToString()) == 0)
                {
                    this.PickDateTimeList.Rows.Remove(row);
                    break;
                }
            }
            DataRow[] rows = this.PickDateTimeList.Select("Device_ID=" + device_id);
            DataTable table = this.PickDateTimeList.Clone();
            foreach (DataRow o in rows)
            {
                table.ImportRow(o);
            }
            gv_pickTimeList.DataSource = table.DefaultView;
            gv_pickTimeList.DataKeyNames = new string[] { "id" };
            gv_pickTimeList.DataBind();
        }
    }

    protected void gv_pickTimeList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lbtn_delete = e.CommandSource as LinkButton;
        GridView gv_pickTimeList = lbtn_delete.Parent.Parent.Parent.Parent as GridView;
        if (e.CommandName.CompareTo("DeleteRow") == 0)
        {
            string id = e.CommandArgument.ToString().Split(',')[0];
            string device_id = e.CommandArgument.ToString().Split(',')[1];
            foreach (DataRow row in this.PickDateTimeList.Rows)
            {
                if (id.CompareTo(row["id"].ToString()) == 0)
                {
                    this.PickDateTimeList.Rows.Remove(row);
                    break;
                }
            }
            DataRow[] rows = this.PickDateTimeList.Select("Device_ID=" + device_id);
            DataTable table = this.PickDateTimeList.Clone();
            foreach (DataRow o in rows)
            {
                table.ImportRow(o);
            }
            gv_pickTimeList.DataSource = table.DefaultView;
            gv_pickTimeList.DataKeyNames = new string[] { "id" };
            gv_pickTimeList.DataBind();
        }
    }
    protected void ddl_pagesize_SelectedIndexChanged(object sender, EventArgs e)
    {
        showdeviceInfo();
    }



    public string GetBorrowStatus(object borrow_status) {
        return Enum.GetName(typeof(BorrowStatus), borrow_status);
    }
}