using BLL;
using DevExpress.Web.ASPxEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_BookingRecord_CustomizeRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitFilters();
            ShowOperation();
        }
        else
        {
            btn_apply_Click(null, null);
        }
    }

    DataTable LoginUserInfo
    {
        get
        {
            if (Session["LoginUserInfo"] == null)
                return null;
            else
                return (DataTable)Session["LoginUserInfo"];
        }
    }
    DataTable ChargeOfDptList
    {
        get
        {
            if (ViewState["ChargeOfDptList"] == null)
                return null;
            return (DataTable)ViewState["ChargeOfDptList"];
        }
        set
        {
            ViewState["ChargeOfDptList"] = value;
        }
    }

    protected void cb_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowOperation();
    }
    void ShowOperation()
    {
        Category category = (Category)Convert.ToInt32(this.cb_category.SelectedItem.Value);

        DataTable commandList = new DataTable();
        commandList.Columns.Add("CommandText", typeof(String));
        commandList.Columns.Add("CommandName", typeof(String));
        DataRow newRow = commandList.NewRow();

        newRow["CommandText"] = "Export filter record";
        newRow["CommandName"] = "filter_record";
        commandList.Rows.Add(newRow.ItemArray);
        switch (category)
        {
            case Category.Device:
                newRow["CommandText"] = "Export all record group by borrow record";
                newRow["CommandName"] = "all_record_br";
                commandList.Rows.Add(newRow.ItemArray);

                newRow["CommandText"] = "Export all record group by device";
                newRow["CommandName"] = "all_record_d";
                commandList.Rows.Add(newRow.ItemArray);
                break;
            case Category.Equipment:
                newRow["CommandText"] = "Export monthly record";
                newRow["CommandName"] = "monthly";
                commandList.Rows.Add(newRow.ItemArray);
                //newRow["CommandText"] = "Export yearly report";
                //newRow["CommandName"] = "yearly";
                //commandList.Rows.Add(newRow.ItemArray);
                newRow["CommandText"] = "Export global equipment status report";
                newRow["CommandName"] = "global";
                commandList.Rows.Add(newRow.ItemArray);
                break;
            case Category.Chamber:
                commandList.Rows.Add(newRow.ItemArray);
                newRow["CommandText"] = "Export chamber dashboard report";
                newRow["CommandName"] = "dashboard";
                commandList.Rows.Add(newRow.ItemArray);
                break;
        }

        this.dl_Command.DataSource = commandList.DefaultView;
        this.dl_Command.DataBind();
    }
    #region
    void InitFilters()
    {
        if (LoginUserInfo != null)
        {
            cl_PersonManage personManage = new cl_PersonManage();
            DataTable dptList = personManage.GetChargeOfDptListByUserInfo(LoginUserInfo);
            ChargeOfDptList = dptList;
            this.cb_dpt.DataSource = dptList.DefaultView;
            this.cb_dpt.TextField = "DptName";
            this.cb_dpt.ValueField = "DptValue";
            this.cb_dpt.DataBind();
            this.cb_dpt.Value = null;
        }

        this.cb_project.DataSource = settingsHandler.GetProjectTable();
        this.cb_project.TextField = "PJ_Name";
        this.cb_project.ValueField = "PJ_Code";
        this.cb_project.DataBind();
        this.cb_project.Value = null;

        this.cb_test_category.DataSource = settingsHandler.GetPurposeTable();
        this.cb_test_category.TextField = "Name";
        this.cb_test_category.ValueField = "ID";
        this.cb_test_category.DataBind();
        this.cb_test_category.Value = null;

        this.cb_site.DataSource = settingsHandler.GetSiteList();
        this.cb_site.TextField = "site_addr";
        this.cb_site.ValueField = "id";
        this.cb_site.DataBind();
        this.cb_site.Value = null;

        ExportBookingRecordFactory yearFac = new ExportBookingRecordFactory();
        this.cb_year.DataSource = yearFac.GetYearList(Convert.ToInt32(this.cb_category.SelectedItem.Value))[0];
        this.cb_year.ValueField = "year";
        this.cb_year.TextField = "year";
        this.cb_year.DataBind();
        this.cb_year.Value = null;

        this.de_Start.Text = null;
        this.de_End.Text = null;

    }
    #endregion
    protected void gv_RecordView_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
    {

    }
    protected void gv_RecordView_CustomButtonInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonEventArgs e)
    {

    }
    protected void chb_CheckedChanged(object sender, EventArgs e)
    {
        this.cb_dpt.Enabled = this.cb_site.Enabled = this.chb_device.Checked;
        this.cb_project.Enabled = this.cb_test_category.Enabled = this.chb_record.Checked;
        this.cb_year.Enabled = this.de_Start.Enabled = this.de_End.Enabled = this.chb_duration.Checked;
    }
    protected void btn_apply_Click(object sender, EventArgs e)
    {
        RecordFilter filter = GetFilter();
        DataTable records = RecordManagment.GetRecords(filter);
        if (records != null)
        {
            this.gv_RecordView.Visible = true;
            this.gv_RecordView.DataSource = records.DefaultView;
            this.gv_RecordView.KeyFieldName = "Booking_ID";
            this.gv_RecordView.DataBind();
        }
    }

    private RecordFilter GetFilter()
    {
        RecordFilter filter = new RecordFilter();
        // get filters
        Category category = (Category)Convert.ToInt32(this.cb_category.Value);
        filter.category = category;

        filter.deviceFilter = this.chb_device.Checked;
        filter.d_site = this.cb_site.SelectedItem != null ? this.cb_site.SelectedItem.Text.ToString() : string.Empty;
        filter.d_department = this.cb_dpt.SelectedItem != null ? this.cb_dpt.SelectedItem.Value.ToString() : string.Empty;

        filter.recordFilter = this.chb_record.Checked;
        filter.r_project = this.cb_project.SelectedItem != null ? this.cb_project.SelectedItem.Value.ToString() : string.Empty;
        filter.r_purpose = this.cb_test_category.SelectedItem != null ? this.cb_test_category.SelectedItem.Value.ToString() : string.Empty;

        filter.durationFilter = this.chb_duration.Checked;
        filter.df_year = this.cb_year.Value != null ? Convert.ToInt32(this.cb_year.Value) : 0;
        filter.df_start = this.de_Start.Date;
        filter.df_end = this.de_End.Date;

        return filter;
    }
    protected void btn_Clear_Click(object sender, EventArgs e)
    {
        this.chb_device.Checked = false;
        this.chb_record.Checked = false;
        this.chb_duration.Checked = false;
        chb_CheckedChanged(null, null);
        InitFilters();

        this.gv_RecordView.Visible = false;
    }
    protected void cb_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (this.cb_year.SelectedItem != null)
        //{
        //    int year = Convert.ToInt32(this.cb_year.SelectedItem.Value);
        //    if (year != DateTime.Now.Year)
        //    {
        //        this.de_Start.Date = new DateTime(year, 1, 1);
        //        this.de_End.Date = new DateTime(year, 1, 1);
        //    }
        //}
    }
    protected void dl_Command_ItemCommand(object source, DataListCommandEventArgs e)
    {
        LinkButton lbtn = e.CommandSource as LinkButton;
        var filter = GetFilter();
        ExcelRenderNode result = new ExcelRenderNode();
        string tempPath = Server.MapPath("~/Temp/");
        if (e.CommandName == "all_record_br")
        {
            result = ExportBookingRecordFactory.GetAllRecordByRecord(filter);
        }

        else if (e.CommandName == "all_record_d")
        {
            result = ExportBookingRecordFactory.GetAllRecordByDevice(filter);
        }

        else if (e.CommandName == "monthly")
        {
            result = ExportBookingRecordFactory.GetmonthlyReport(filter, tempPath);
        }

        else if (e.CommandName == "yearly")
        {
            ExportBookingRecordFactory recordMana = new ExportBookingRecordFactory();
            result = recordMana.GetYearlyReport(filter);
        }
        else if (e.CommandName == "filter_record")
        {
            //ExportBookingRecordFactory recordMana = new ExportBookingRecordFactory();
            result = ExportBookingRecordFactory.GetFilterReport(filter);
        }
        else if (e.CommandName == "global")
        {
            ExportBookingRecordFactory recordMana = new ExportBookingRecordFactory();
            if (filter.df_year == 0) filter.df_year = 2016;
            result = recordMana.GetGlobleReport(filter);
        }
        else if (e.CommandName == "dashboard")
        {
            ExportBookingRecordFactory recordMana = new ExportBookingRecordFactory();
            result = recordMana.GetYearlyReport(filter);
        }
        if (result.ms != null)
        {
            string title = lbtn.Text.Replace("Export ", "");
            title = title.Replace(" ", "_");
            title += "_" + filter.df_year + "_" + filter.df_start.ToString("yyyyMMdd") + "_" + filter.df_end.ToString("yyyyMMdd") + ".xls";
            title = filter.category.ToString() + "_" + title;
            ExcelRender.RenderToBrowser(result.ms, Context, title);
        }
        else
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, result.errMsg);
        }
    }
}