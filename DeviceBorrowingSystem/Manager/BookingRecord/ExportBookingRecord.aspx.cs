using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_BookingRecord_ExportBookingRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.cb_category.SelectedValue = Session["Category"].ToString();
            InitGUIData();
        }
    }

    void InitGUIData()
    {
        //this.cb_chamber.Items.Clear();
        this.cb_department.Items.Clear();
        this.cb_project.Items.Clear();
        this.cb_site.Items.Clear();
        this.cb_testcategory.Items.Clear();
        this.cb_yearly.Items.Clear();
        ExportBookingRecordFactory factory = new ExportBookingRecordFactory(this.cb_category.SelectedItem.Text.Trim());
        // init location list
        var result = factory.GetLocationList();
        if (result != null)
        {
            this.cb_site.Items.Add("");
            foreach (DataRow row in result[0].Rows)
            {
                this.cb_site.Items.Add(row["site"].ToString());
            }
        }
        // init department list
        result = factory.GetDepartmentList();
        if (result != null)
        {
            this.cb_department.Items.Add("");
            foreach (DataRow row in result[0].Rows)
            {
                this.cb_department.Items.Add(row["department"].ToString());
            }
        }

        // init project list
        result = factory.GetProjectList();
        if (result != null)
        {
            this.cb_project.Items.Add("");
            foreach (DataRow row in result[0].Rows)
            {
                this.cb_project.Items.Add(new ListItem(row["pname"].ToString(), row["pid"].ToString()));
            }
        }
        // init test category list
        result = factory.GetTestCategoryList();
        if (result != null)
        {
            this.cb_testcategory.Items.Add("");
            foreach (DataRow row in result[0].Rows)
            {
                this.cb_testcategory.Items.Add(new ListItem(row["tcname"].ToString(), row["tcid"].ToString()));
            }
        }

        // init year
        result = factory.GetYearList(2);
        if (result != null)
        {
            this.cb_yearly.DataSource = result[0];
            this.cb_yearly.DataTextField = "year";
            this.cb_yearly.DataValueField = "year";
            this.cb_yearly.DataBind();

        }
        // init chamber year
        result = factory.GetYearList(3);
        if (result != null)
        {
            this.cb_charmberDashbord_year.DataSource = result[0];
            this.cb_charmberDashbord_year.DataTextField = "year";
            this.cb_charmberDashbord_year.DataValueField = "year";
            this.cb_charmberDashbord_year.DataBind();

        }
        string category = this.cb_category.SelectedValue;
        if (category == "1")
        {
            this.p_allbydevice.Visible = true;
            this.p_allbyrecord.Visible = true;

            this.p_monthly.Visible = false;
            this.p_yearly.Visible = false;
            this.p_chamber.Visible = false;
        }
        else if (category == "2")
        {
            this.p_allbydevice.Visible = false;
            this.p_allbyrecord.Visible = false;
            this.p_chamber.Visible = false;

            this.p_monthly.Visible = true;
            this.p_yearly.Visible = true;
        }
        else if (category == "3")
        {
            this.p_allbydevice.Visible = false;
            this.p_allbyrecord.Visible = false;
            this.p_monthly.Visible = false;
            this.p_yearly.Visible = false;

            this.p_chamber.Visible = true;
        }

    }
    DeviceFilters GetFilters()
    {
        DeviceFilters filter = new DeviceFilters();
        filter.Category = this.cb_category.SelectedValue;
        filter.Site = this.cb_site.SelectedValue;
        filter.Department = this.cb_department.SelectedValue;
        //filter.Chamber = this.cb_chamber.SelectedValue;
        filter.Project_ID = this.cb_project.SelectedValue;
        filter.TestCategory_ID = this.cb_testcategory.SelectedValue;
        return filter;
    }
    protected void cb_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Category"] = cb_category.SelectedValue;
        InitGUIData();
    }
    /// <summary>
    /// Export all record for borrowing data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtn_exportToExcel_ar_Click(object sender, EventArgs e)
    {
        DeviceFilters filter = GetFilters();
        if (this.tb_startDate_ar.Text != "" && this.tb_endDate_ar.Text != "")
        {
            filter.StartDate = DateTime.Parse(this.tb_startDate_ar.Text);
            filter.EndDate = DateTime.Parse(this.tb_endDate_ar.Text);
            if (filter.StartDate > filter.EndDate)
            {
                GlobalClassNamespace.GlobalClass.PopMsg(this.Page, "Start Date can not be later than End Date!");
                return;
            }
        }

        ExportBookingRecordFactory factory = new ExportBookingRecordFactory();
        var result = factory.GetAllRecordByRecord(this.tb_startDate_ar.Text == "" ? null : (DateTime?)Convert.ToDateTime(this.tb_startDate_ar.Text), this.tb_endDate_ar.Text == "" ? null : (DateTime?)Convert.ToDateTime(this.tb_endDate_ar.Text));
        if (result.ms != null)
        {
            ExcelRender.RenderToBrowser(result.ms, Context, "Device_Borrowing_Record_From_" + filter.StartDate.ToString("yyyyMMdd") + "_To_" + filter.EndDate.ToString("yyyyMMdd") + ".xls");
        }
        else
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, result.errMsg);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtn_exportToExcel_ad_Click(object sender, EventArgs e)
    {
        DeviceFilters filter = GetFilters();
        if (this.tb_startDate_ad.Text == "" || this.tb_endDate_ad.Text == "")
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, "Start Date or End Date must be setted!");
            return;
        }
        filter.StartDate = DateTime.Parse(this.tb_startDate_ad.Text);
        filter.EndDate = DateTime.Parse(this.tb_endDate_ad.Text);

        if (filter.StartDate > filter.EndDate)
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, "Start Date can not be later than End Date!");
            return;
        }

        ExportBookingRecordFactory factory = new ExportBookingRecordFactory();
        var result = factory.GetAllRecordByDevice(filter.StartDate, filter.EndDate);
        if (result.ms != null)
        {
            ExcelRender.RenderToBrowser(result.ms, Context, "Equipment_Borrowing_Record_From_" + filter.StartDate.ToString("yyyyMMdd") + "_To_" + filter.EndDate.ToString("yyyyMMdd") + ".xls");
        }
        else
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, result.errMsg);
        }
    }
    /// <summary>
    /// chamber报表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtn_exportToExcel_chamber_Click(object sender, EventArgs e)
    {
        DeviceFilters filter = GetFilters();
        int year = int.Parse(this.cb_yearly.SelectedValue);

        ExportBookingRecordFactory factory = new ExportBookingRecordFactory();
        var result = factory.GetChamberDashboard(year);
        if (result.ms != null)
        {
            ExcelRender.RenderToBrowser(result.ms, Context, "Equipment_Year_Report_" + year + ".xls");
        }
        else
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, result.errMsg);
        }
    }
    // 产生年度报表
    protected void lbtn_exportToExcel_yearly_Click(object sender, EventArgs e)
    {
        DeviceFilters filter = GetFilters();
        int year = int.Parse(this.cb_yearly.SelectedValue);

        ExportBookingRecordFactory factory = new ExportBookingRecordFactory();
        var result = factory.GetYearlyReport(year);
        if (result.ms != null)
        {
            ExcelRender.RenderToBrowser(result.ms, Context, "Equipment_Year_Report_" + year + ".xls");
        }
        else
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, result.errMsg);
        }
    }
    // 产生月度报表
    protected void lbtn_exportToExcel_monthly_Click(object sender, EventArgs e)
    {
        DeviceFilters filter = GetFilters();
        if (this.tb_startDate_Month.Text == "" || this.tb_endDate_Month.Text == "")
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, "Start Date or End Date must be setted!");
            return;
        }
        filter.StartDate = DateTime.Parse(this.tb_startDate_Month.Text);
        filter.EndDate = DateTime.Parse(this.tb_endDate_Month.Text);

        if (filter.StartDate > filter.EndDate)
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, "Start Date can not be later than End Date!");
            return;
        }
        string tempPath = Server.MapPath("~/Temp/");
        ExportBookingRecordFactory factory = new ExportBookingRecordFactory();
        var result = factory.GetmonthlyReport(filter.StartDate, filter.EndDate, tempPath);
        if (result.ms != null)
        {
            ExcelRender.RenderToBrowser(result.ms, Context, "Equipment_Borrowing_Record_From_" + filter.StartDate.ToString("yyyyMMdd") + "_To_" + filter.EndDate.ToString("yyyyMMdd") + ".xls");
        }
        else
        {
            GlobalClassNamespace.GlobalClass.PopMsg(this.Page, result.errMsg);
        }
    }
}