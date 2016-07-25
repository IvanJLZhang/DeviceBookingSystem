using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Default : System.Web.UI.Page
{
    #region properties
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
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitControlStateAndData();
        }
    }
    protected void gv_requestView_Init(object sender, EventArgs e)
    {
        ShowNewBookingRecords();
    }
    protected void gv_requestView_DataBound(object sender, EventArgs e)
    {
        if (cat != Category.Device)
        {
            this.gv_requestView.Columns["Custom_ID"].Visible = false;
        }
    }
    protected void gv_requestView_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var intID = Convert.ToString(gv_requestView.GetRowValues(gv_requestView.FocusedRowIndex, "Booking_ID"));
        var view = sender as ASPxGridView;

        view.JSProperties["cpType"] = e.ButtonID;
        view.JSProperties["cpBookingId"] = intID;
        view.JSProperties["cpViewType"] = (Int32)ViewType.EDIT;
    }
    protected void cb_cat_SelectedIndexChanged(object sender, EventArgs e)
    {
        cat = (Category)Convert.ToInt32(this.cb_cat.Value);
        Response.Redirect(Request.Url.ToString());
    }

    protected void gv_newUserView_Init(object sender, EventArgs e)
    {
        if (pnl_UserRequest.Visible)
            ShowNewUserRequest();
    }
    protected void gv_newUserView_DataBound(object sender, EventArgs e)
    {

    }
    protected void gv_newUserView_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var intID = Convert.ToString(gv_newUserView.GetRowValues(gv_newUserView.FocusedRowIndex, "P_ID"));
        var view = sender as ASPxGridView;

        view.JSProperties["cpType"] = e.ButtonID;
        view.JSProperties["cpBookingId"] = intID;
        view.JSProperties["cpViewType"] = (Int32)ViewType.ADD;
    }
    #region private methods
    void InitControlStateAndData()
    {
        this.cb_cat.Items.Clear();
        this.cb_cat.Items.Add(Category.Device.ToString(), (Int32)Category.Device);
        this.cb_cat.Items.Add(Category.Equipment.ToString(), (Int32)Category.Equipment);
        this.cb_cat.Items.Add(Category.Chamber.ToString(), (Int32)Category.Chamber);
        this.cb_cat.Value = (Int32)cat;

        this.btn_allApprove.Visible = false;
        this.btn_allReject.Visible = false;


        //DataTable newUserRequest = personManageFac.GetNewPersonTable(this.LoginUserInfo);
        if (Session["Role"] != null)
        {
            UserRole role = (UserRole)Convert.ToInt32(Session["Role"]);
            switch (role)
            {
                case UserRole.ADMIN:
                case UserRole.REVIEWER:
                case UserRole.LEADER:
                    this.pnl_UserRequest.Visible = true;
                    break;
                case UserRole.USER:
                    this.pnl_UserRequest.Visible = false;
                    break;
            }
        }
        else
        {
            this.pnl_UserRequest.Visible = false;
        }
    }
    /// <summary>
    /// 显示新申请订单
    /// </summary>
    public void ShowNewBookingRecords()
    {
        DataTable dptList = GetChargeOfDptTable();
        List<RecordStatus> statusList = new List<RecordStatus>();
        statusList.Add(RecordStatus.NEW_SUBMIT);
        DataTable records = RecordManagment.GetRecords(this.cat, statusList, dptList);
        if (records != null)
        {
            this.gv_requestView.DataSource = records;
            this.gv_requestView.KeyFieldName = "Booking_ID";
            this.gv_requestView.DataBind();
        }
    }
    /// <summary>
    /// 获取用户的charge of department
    /// </summary>
    /// <returns></returns>
    DataTable GetChargeOfDptTable()
    {
        if (LoginUserInfo != null)
            return personManageFac.GetChargeOfDptList(this.LoginUserInfo);
        else
            return null;
    }
    /// <summary>
    /// 显示新用户申请单
    /// </summary>
    public void ShowNewUserRequest()
    {
        DataTable newUserRequest = personManageFac.GetNewPersonTable(this.LoginUserInfo);
        if (newUserRequest != null)
        {
            this.gv_newUserView.DataSource = newUserRequest.DefaultView;
            this.gv_newUserView.KeyFieldName = "P_ID";
            this.gv_newUserView.DataBind();
        }
    }
    #endregion


}