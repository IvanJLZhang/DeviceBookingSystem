using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_BookingPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            ShowStepPage();
    }
    #region session value
    int STEP
    {
        get
        {
            if (Session["STEP"] == null)
                return 1;
            else
            {
                int step = Convert.ToInt32(Session["STEP"]);
                if (step <= 1)
                {
                    Session["STEP"] = 1;
                    return 1;
                }
                else
                    return step;
            }
        }
        set
        {
            Session["STEP"] = value;
        }
    }

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
            Session["Category"] = value;
        }
    }
    #endregion

    #region step list
    List<StepProperty> steps = new List<StepProperty>() { 
          new StepProperty(){stepName = "Step 1: Click Category", stepUrl = "./BookingSteps/Step1.aspx"}
        , new StepProperty(){stepName = "Step 2: Select Device", stepUrl = "../Manager/Device/deviceView.aspx"}
        , new StepProperty(){stepName = "Step 2: Select Equipment", stepUrl = "../Manager/Equipment/EquipmentViewEx.aspx"}
        , new StepProperty(){stepName = "Step 2: Select Chamber", stepUrl = "../Manager/Equipment/EquipmentViewEx.aspx"}
        , new StepProperty(){stepName = "Step 3: Fill Booking Date Time", stepUrl = "./BookingSteps/Step3.aspx"}
        , new StepProperty(){stepName = "Step 4: Fill Booking Detail Information", stepUrl = "./BookingSteps/Step4_FillDetailBookingInfo.aspx"}
    };
    #endregion
    void ShowStepPage()
    {
        // check Step
        if (this.STEP <= 1)
        {
            this.btn_back.Enabled = false;
        }
        else
            this.btn_back.Enabled = true;
        if (this.STEP >= 4)
        {
            this.btn_next.Enabled = false;
        }
        else
            this.btn_next.Enabled = true;
        switch (STEP)
        {
            case 1:
                this.lbl_stepName.Text = steps[0].stepName;
                devViewFrame.Attributes["src"] = steps[0].stepUrl;
                break;
            case 2:
                Session.Remove("SelectedIDList");
                switch (this.cat)
                {
                    case Category.Device:
                        this.lbl_stepName.Text = steps[1].stepName;
                        devViewFrame.Attributes["src"] = steps[1].stepUrl + "?type=0&device_status=1";
                        break;
                    case Category.Equipment:
                        this.lbl_stepName.Text = steps[2].stepName;
                        devViewFrame.Attributes["src"] = steps[2].stepUrl + "?type=0&device_status=1";

                        break;
                    case Category.Chamber:
                        this.lbl_stepName.Text = steps[3].stepName;
                        devViewFrame.Attributes["src"] = steps[3].stepUrl + "?type=0&device_status=1";
                        break;
                }
                break;
            case 3:
                if (Session["SelectedIDList"] == null)
                {
                    this.STEP = 2;
                    ShowStepPage();
                    break;
                }
                Session.Remove("NewBookingData");
                this.lbl_stepName.Text = steps[4].stepName;
                devViewFrame.Attributes["src"] = steps[4].stepUrl;
                break;
            case 4:
                if (Session["NewBookingData"] == null)
                {
                    this.STEP = 3;
                    ShowStepPage();
                    break;
                }
                this.lbl_stepName.Text = steps[5].stepName;
                devViewFrame.Attributes["src"] = steps[5].stepUrl;
                break;
        }


    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        this.STEP = this.STEP - 1;
        ShowStepPage();
    }
    protected void btn_next_Click(object sender, EventArgs e)
    {
        this.STEP = this.STEP + 1;
        ShowStepPage();
    }
}