using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Equipment_EquipmentViewEx : System.Web.UI.Page
{

    #region session value
    List<string> SelectedIDList
    {
        get
        {
            if (Session["SelectedIDList"] == null)
                return new List<string>();
            return (List<string>)Session["SelectedIDList"];
        }
        set
        {
            Session["SelectedIDList"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //ShowDataTable();
    }
    protected void agv_deviceTable_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        devManagementFac factory = new devManagementFac();
        string id = e.Keys["id"].ToString();
        agv_deviceTable.JSProperties.Remove("cpAlertMsg");//先清空
        if (factory.DeleteDevice(id))
        {
            agv_deviceTable.JSProperties["cpAlertMsg"] = "Deleted the device:" + id;
        }
        else
        {
            agv_deviceTable.JSProperties["cpAlertMsg"] = "Can not delete the device:" + id;
        }
        ShowDataTable();
    }
    protected void agv_deviceTable_RowCommand(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewRowCommandEventArgs e)
    {
        if (e.CommandArgs.CommandName.CompareTo("DeviceDetail") == 0)
        {
            string deviceId = e.CommandArgs.CommandArgument.ToString();
            //string path = Server.MapPath("~/Manager/Equipment/EquipmentDetail.aspx");
            string scriptText = "var obj = window.showModalDialog('./EquipmentDetail.aspx?id=" + deviceId + "&type=edit', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
            //string scriptText = "window.open('./Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'width=580px, height=500px, top=100px, left=100px'); ";
            ScriptManager.RegisterStartupScript(this.upd_main, this.GetType(), "click", scriptText, true);
            //Response.Write("<script>var obj = window.showModalDialog('../Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;</script>");
        }
    }

    void ShowDataTable()
    {

        List<Status> deviceStatus = new List<Status>();


        if (Request["device_status"] == null)
        { // default is 1,2,3,4,5
            deviceStatus.Add(Status.Usable);
            deviceStatus.Add(Status.Broken);
            deviceStatus.Add(Status.Lost);
            deviceStatus.Add(Status.NotForCirculation);
            deviceStatus.Add(Status.ReturnToCustomer);
        }
        else
        {
            string[] status = Request["device_status"].ToString().Split(',');
            foreach (string s in status)
            {
                deviceStatus.Add((Status)Convert.ToInt32(s));
            }
        }
        devManagementFac factory = new devManagementFac();
        DataTable deviceTable = factory.GetEquipmentTable(deviceStatus);
        {
            deviceTable.Columns.Add("Selected", typeof(Boolean));
            foreach (DataRow row in deviceTable.Rows)
            {
                string id = row["id"].ToString();
                if (this.SelectedIDList.Contains(id))
                {
                    row["Selected"] = true;
                }
                else
                {
                    row["Selected"] = false;
                }
            }
        }
        this.agv_deviceTable.DataSource = deviceTable.DefaultView;

        this.agv_deviceTable.KeyFieldName = "id";
        this.agv_deviceTable.DataBind();

        this.agv_deviceTable.FilterEnabled = true;
        this.agv_deviceTable.SettingsDataSecurity.AllowDelete = true;



    }
    protected void btn_check_CheckedChanged(object sender, EventArgs e)
    {
        ASPxCheckBox check = sender as ASPxCheckBox;
        string id = check.Text;
        if (check.Checked)
        {
            if (!SelectedIDList.Contains(id.ToString()))
            {
                var sel = SelectedIDList;
                sel.Add(id);
                SelectedIDList = sel;
            }
        }
        else
        {
            if (SelectedIDList.Contains(id.ToString()))
            {
                var sel = SelectedIDList;
                sel.Remove(id);
                SelectedIDList = sel;
            }
        }
    }
    protected void agv_deviceTable_Init(object sender, EventArgs e)
    {
        ShowDataTable();
    }
    protected void agv_deviceTable_DataBound(object sender, EventArgs e)
    {
        ViewType viewType = GetViewType();
        switch (viewType)
        {
            case ViewType.VIEW:
                foreach (GridViewColumn column in this.agv_deviceTable.Columns)
                {
                    if (column.Caption == "Command")
                    {
                        GridViewBandColumn bandColumn = column as GridViewBandColumn;
                        foreach (GridViewColumn command in bandColumn.Columns)
                        {
                            if (command.Caption == "Delete")
                                command.Visible = false;
                        }
                    }
                }
                break;
            case ViewType.EDIT:
            case ViewType.ADD:
            case ViewType.DETELE:
            case ViewType.NULL:
                foreach (GridViewColumn column in this.agv_deviceTable.Columns)
                {
                    if (column.VisibleIndex == 0)
                    {
                        column.Visible = false;
                    }
                }
                break;
        }
    }
    protected void agv_deviceTable_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var intID = Convert.ToString(agv_deviceTable.GetRowValues(agv_deviceTable.FocusedRowIndex, "id"));
        var view = sender as ASPxGridView;
        view.JSProperties["cpCommand"] = e.ButtonID;
        view.JSProperties["cpDeviceId"] = intID;

        ViewType viewType = ViewType.VIEW;
        if (Request["type"] == null)
        {// default is view
            viewType = ViewType.VIEW;
        }
        else
        {
            viewType = (ViewType)Convert.ToInt32(Request["type"]);
        }

        view.JSProperties["cpViewType"] = (Int32)viewType;
    }
    protected void agv_deviceTable_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        ViewType viewType = ViewType.VIEW;
        if (Request["type"] == null)
        {// default is view
            viewType = ViewType.VIEW;
        }
        else
        {
            viewType = (ViewType)Convert.ToInt32(Request["type"]);
        }
        if (viewType == ViewType.VIEW)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                GridViewDataColumn col = ((ASPxGridView)sender).Columns["id"] as GridViewDataColumn;
                System.Web.UI.HtmlControls.HtmlInputCheckBox btn = ((ASPxGridView)sender).FindRowCellTemplateControl(e.VisibleIndex, col, "group1") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                btn.Checked = ((ASPxGridView)sender).Selection.IsRowSelected(e.VisibleIndex);
                btn.Attributes.Add("onclick", "agv_deviceTable.PerformCallback(" + e.VisibleIndex.ToString() + "+';'+this.checked)");
            }
        }
    }
    protected void agv_deviceTable_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string[] pars = e.Parameters.Split(';');
        int index = Convert.ToInt32(pars[0]);
        bool isSelected = Convert.ToBoolean(pars[1]);

        if (isSelected)
        {// 增加
            ((ASPxGridView)sender).Selection.SelectRow(index);
            string id = agv_deviceTable.GetRowValues(index, "id").ToString();

            if (!SelectedIDList.Contains(id.ToString()))
            {
                var sel = SelectedIDList;
                sel.Add(id);
                SelectedIDList = sel;
            }
        }
        else
        {// 删除
            ((ASPxGridView)sender).Selection.SelectRow(index);
            string id = agv_deviceTable.GetRowValues(index, "id").ToString();

            if (SelectedIDList.Contains(id.ToString()))
            {
                var sel = SelectedIDList;
                sel.Remove(id);
                SelectedIDList = sel;
            }
        }
    }


    #region private methods
    /// <summary>
    /// Get view type
    /// </summary>
    /// <returns></returns>
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
    #endregion
}