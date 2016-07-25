using BLL;
using GlobalClassNamespace;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Equipment_Calibration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //ShowCalibrationHistory();
            if (Request["type"] != null)
            {
                string type = Request["type"].ToString();
                if (type.CompareTo("view") == 0)
                {
                    this.ibtn_AddCalibration.Enabled = false;
                    this.ibtn_AddFloatingPrice.Enabled = false;
                }
            }
        }
    }
    private void ShowCalibrationHistory()
    {
        if (Request["id"] != null)
        {
            string deviceid = Request["id"].ToString();
            cl_Calibration calibrationManage = new cl_Calibration();
            DataTable calibrationHistory = calibrationManage.GetCalibrationHistoryByDeviceID(deviceid);
            if (calibrationHistory != null)
            {
                this.gv_CalibrationHistory.DataSource = calibrationHistory.DefaultView;
                this.gv_CalibrationHistory.DataKeyNames = new string[] { "C_ID" };
                this.gv_CalibrationHistory.DataBind();
            }
        }
    }

    protected void ibtn_AddCalibration_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            tbl_Calibration calibration = new tbl_Calibration();
            calibration.ID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            calibration.Calibration_Cost = Convert.ToDouble(this.tb_CalibrationCost.Text);
            calibration.Calibration_Date = DateTime.Parse(this.tb_Calibration.Text);
            calibration.Calibration_Duration = Convert.ToInt32(this.tb_caliDuration.Text);
            calibration.Reminding_day = Convert.ToInt32(this.tb_RemindingDays.Text);
            calibration.Device_ID = Request["id"].ToString();

            cl_Calibration calibrationManage = new cl_Calibration();
            if (calibrationManage.AddCalibrationRecord(calibration))
            {
                //ShowCalibrationHistory();
                this.gv_CalibrationHistory.DataBind();
                this.tb_Calibration.Text = "";
                this.tb_CalibrationCost.Text = "0";
                GlobalClass.PopMsg(this.Page, "Add Successfully!");
            }
            else
            {
                GlobalClass.PopMsg(this.Page, "Failed!!plz check data!");
            }
        }
        catch { }


    }
    protected void ibtn_AddFloatingPrice_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            tbl_FloatingPrice floatingPrice = new tbl_FloatingPrice();
            floatingPrice.Device_ID = Request["id"].ToString();
            floatingPrice.Inside_cost = Convert.ToDouble(this.tb_insidecost.Text);
            floatingPrice.Outside_cost = Convert.ToDouble(this.tb_outsideCost.Text);
            floatingPrice.Year = Convert.ToInt32(this.tb_Year.Text);
            floatingPrice.Note = this.tb_Note.Text;

            cl_FloatingPrice floatingPriceManage = new cl_FloatingPrice();
            if (floatingPriceManage.CheckRecordByYearDeviceID(floatingPrice.Year, floatingPrice.Device_ID))
            {
                if (floatingPriceManage.AddFloatingPrice(floatingPrice))
                {
                    this.gv_FloatingPrice.DataBind();
                    GlobalClass.PopMsg(this.Page, "Add Successfully!");
                }
                else
                {
                    GlobalClass.PopMsg(this.Page, "Failed!!plz check data!");
                }
            }
            else
            {
                this.tb_Year.Text = "";
                GlobalClass.PopMsg(this.Page, "Year data already exists, plz go to edit column.");
            }
        }
        catch
        { }
    }
    protected void gv_FloatingPrice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Request["type"] != null)
        {
            string type = Request["type"].ToString();
            if (type.CompareTo("view") == 0)
            {
                e.Row.Cells[4].Enabled = false;
            }
        }
    }
}