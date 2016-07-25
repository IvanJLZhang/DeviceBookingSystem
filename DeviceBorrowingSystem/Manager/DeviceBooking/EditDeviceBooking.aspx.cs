using AjaxControlToolkit;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_DeviceBooking_EditDeviceBooking : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.tb_LoanerName.Attributes.Add("onclick", "ownerSelect();");
            ShowDeviceBooking();
        }

        if (ViewState["Create"] != null && (bool)ViewState["Create"])
        {
            CheckAddControl(this.tb_EndDate, this.ddl_EndTime, this.hf_endDateTime, this.hf_newTableID);
        }
    }

    private void ShowDeviceBooking()
    {
        //if (Request["id"] != null)
        //{
        //string deviceBookingID = Request["id"].ToString();
        string deviceBookingID = "BD201411260001";
        cl_DeviceBookingManage deviceBooking = new cl_DeviceBookingManage();
        DataTable deviceBookingInfo = deviceBooking.GetBookingInfoByID(deviceBookingID);
        if (deviceBookingInfo != null)
        {
            string loanerID = deviceBookingInfo.Rows[0]["Loaner_ID"].ToString();
            cl_PersonManage person = new cl_PersonManage();
            DataTable personInfo = person.GetPersonInfoByID(loanerID);
            if (personInfo != null)
            {
                this.tb_LoanerName.Text = personInfo.Rows[0]["P_Name"].ToString();
            }

            cl_DeviceManage device = new cl_DeviceManage();
            DataTable deviceInfo = device.GetDeviceDetailByID(deviceBookingInfo.Rows[0]["Device_ID"].ToString());
            if (deviceInfo != null)
            {
                this.lbtn_DeviceName.Text = deviceInfo.Rows[0]["Device_Name"].ToString();
                this.lbtn_DeviceName.Attributes.Add("DeviceID", deviceBookingInfo.Rows[0]["Device_ID"].ToString());
            }

            DateTime startDT = Convert.ToDateTime(deviceBookingInfo.Rows[0]["Loan_DateTime"]);
            this.lbl_startDT.Text = startDT.ToString("yyyy/MM/dd HH:mm");

            DateTime endDT = Convert.ToDateTime(deviceBookingInfo.Rows[0]["Plan_To_ReDateTime"]);
            this.tb_EndDate.Text = endDT.ToString("yyyy/MM/dd");

            ddl_EndTime.SelectedValue = endDT.ToString("HH:mm");
            this.hf_endDateTime.Value = endDT.ToString("yyyy/MM/dd HH:mm");

            this.ddl_Project.SelectedValue = deviceBookingInfo.Rows[0]["Project_ID"].ToString();
            this.ddl_TestCategory.SelectedValue = deviceBookingInfo.Rows[0]["TestCategory_ID"].ToString();

            this.tb_PjStage.Text = deviceBookingInfo.Rows[0]["PJ_Stage"].ToString();
            this.tb_Comment.Text = deviceBookingInfo.Rows[0]["Comment"].ToString();
        }


        //}
    }
    private void NewTable(string tableID, DateTime startDT, DateTime endDT)
    {
        Table newTable = new Table();
        newTable.ID = tableID;
        TableRow newRow = new TableRow();

        // 第一行
        TableHeaderCell headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "Start DateTime: ";
        newRow.Cells.Add(headerCell);

        TableCell cell = new TableCell();
        Label lbl = new Label();
        lbl.ToolTip = "Loan DateTime";
        lbl.Text = startDT.ToString("yyyy/MM/dd HH:mm");
        cell.Controls.Add(lbl);
        cell.HorizontalAlign = HorizontalAlign.Left;
        newRow.Cells.Add(cell);

        headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "End DateTime: ";
        newRow.Cells.Add(headerCell);

        cell = new TableCell();
        TextBox tb = new TextBox();
        tb.ID = "tb_endDate" + endDT.ToString("yyyyMMddHHmm");
        tb.Text = endDT.ToString("yyyy/MM/dd");
        tb_EndDate.AutoPostBack = true;
        tb.TextChanged += new EventHandler(test);
        cell.Controls.Add(tb);
        CalendarExtender calender = new CalendarExtender();
        calender.Format = "yyyy/MM/dd";
        calender.TargetControlID = tb.ID;
        cell.Controls.Add(calender);
        DropDownList ddl = new DropDownList();
        for (int index = 0; index != 24; index++)
        {
            string text = String.Format("{0:00}:00", index);
            ListItem item = new ListItem(text);
            ddl.Items.Add(item);
            string text1 = String.Format("{0:00}:30", index);
            item = new ListItem(text1);
            ddl.Items.Add(item);
        }
        ddl.SelectedValue = endDT.ToString("HH:mm");
        ddl.SelectedIndexChanged += new EventHandler(ddl_EndTime_SelectedIndexChanged);
        cell.Controls.Add(ddl);

        Button btn = new Button();
        btn.Text = "Clieck";
        btn.Click += new EventHandler(test);
        cell.Controls.Add(btn);

        HiddenField hf = new HiddenField();
        hf.ID = "hf_endDT" + endDT.ToString("yyyyMMddHHmm");
        cell.Controls.Add(hf);
        hf = new HiddenField();
        hf.ID = "hf_newTableID" + endDT.ToString("yyyyMMddHHmm");
        cell.Controls.Add(hf);


        newRow.Cells.Add(cell);
        newTable.Rows.Add(newRow);

        // 第二行
        newRow = new TableRow();
        headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "Project Name: ";
        newRow.Cells.Add(headerCell);

        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Left;
        ddl = new DropDownList();
        ddl.DataSourceID = this.SqlDataSource1.ID;
        ddl.DataTextField = "PJ_Name";
        ddl.DataValueField = "PJ_Code";
        cell.Controls.Add(ddl);
        newRow.Cells.Add(cell);

        headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "Cust Name: ";
        newRow.Cells.Add(headerCell);

        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Left;
        ddl = new DropDownList();
        ddl.DataSourceID = this.SqlDataSource1.ID;
        ddl.DataTextField = "Cust_Name";
        ddl.DataValueField = "PJ_Code";
        cell.Controls.Add(ddl);
        newRow.Cells.Add(cell);

        newTable.Rows.Add(newRow);

        // 第三行
        newRow = new TableRow();

        newRow = new TableRow();
        headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "Test Category: ";
        newRow.Cells.Add(headerCell);

        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Left;
        ddl = new DropDownList();
        ddl.DataSourceID = this.SqlDataSource3.ID;
        ddl.DataTextField = "Name";
        ddl.DataValueField = "ID";
        cell.Controls.Add(ddl);
        newRow.Cells.Add(cell);

        headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "Loaner Name: ";
        newRow.Cells.Add(headerCell);

        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Left;
        tb = new TextBox();
        tb.ID = "tb_LoanerName" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        tb.Attributes.Add("onclick", "var obj = window.showModalDialog('../Equipment/OwnerSelect.aspx', '', ''); if (obj != null) {var clientid = '" + tb.ClientID + "'; var tb = window.document.getElementById(clientid);tb.value = obj;}");
        cell.Controls.Add(tb);

        newRow.Cells.Add(cell);
        newTable.Rows.Add(newRow);

        // 第四行
        newRow = new TableRow();

        newRow = new TableRow();
        headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "Project Stage: ";
        newRow.Cells.Add(headerCell);

        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Left;
        tb = new TextBox();
        tb.ID = "tb_PjStage" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        cell.Controls.Add(tb);
        newRow.Cells.Add(cell);

        newTable.Rows.Add(newRow);
        // 第五行
        newRow = new TableRow();

        newRow = new TableRow();
        headerCell = new TableHeaderCell();
        headerCell.HorizontalAlign = HorizontalAlign.Left;
        headerCell.Text = "Comment:";
        newRow.Cells.Add(headerCell);

        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.ColumnSpan = 3;
        tb = new TextBox();
        tb.ID = "tb_Comment" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        tb.TextMode = TextBoxMode.MultiLine;
        tb.Width = this.tb_Comment.Width;
        tb.Height = this.tb_Comment.Height;
        cell.Controls.Add(tb);
        newRow.Cells.Add(cell);

        newTable.Rows.Add(newRow);

        // 画分隔符
        newRow = new TableRow();
        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Left;
        cell.ColumnSpan = 4;
        cell.Text = "<br/><hr/>";
        newRow.Cells.Add(cell);
        newTable.Rows.Add(newRow);
        this.ph_InsertCtrl.Controls.Add(newTable);
    }
    protected void lbtn_DeviceName_Click(object sender, EventArgs e)
    {
        string deviceId = lbtn_DeviceName.Attributes["DeviceID"];
        string scriptText = "var obj = window.showModalDialog('../Equipment/EquipmentDetail.aspx?id=" + deviceId + "&type=view', '', 'resizable = yes; dialogWidth = 750px'); if(obj != null && obj == 'OK') window.location.href = window.location.href;";
        Response.Write("<script>" + scriptText + "</script>");
    }
    public void tb_EndDate_TextChanged(object sender, EventArgs e)
    {
        TableCell cell = (TableCell)(((TextBox)sender).Parent);
        TextBox endDate_lbl = (TextBox)cell.Controls[0];
        DropDownList dl = (DropDownList)cell.Controls[2];
        HiddenField hf1 = (HiddenField)cell.Controls[3];
        HiddenField hf2 = (HiddenField)cell.Controls[4];

        if (ViewState["Create"] == null)
        {
            ViewState["Create"] = true;
            CheckAddControl(endDate_lbl, dl, hf1, hf2);
        }

    }
    void test(object sender, EventArgs e)
    {
        //if (ViewState["Create"] == null)
        //{
        //    ViewState["Create"] = true;
        //    CheckAddControl(this.tb_EndDate, this.ddl_EndTime, this.hf_endDateTime, this.hf_newTableID);
        //}
        Response.Write("<script>alert('Got it!!');</script>");
    }
    public void ddl_EndTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        //CheckAddControl();
    }
    private void CheckAddControl(TextBox endDate, DropDownList endTime, HiddenField h1, HiddenField h2)
    {
        string endDT = endDate.Text + " " + endTime.SelectedValue;
        DateTime startDT = DateTime.Parse(endDT).AddMinutes(30);
        DateTime endDateTime = DateTime.Parse(h1.Value);
        if (endDT.CompareTo(h1.Value) != 0)
        {
            string tableName = "tbl_" + startDT.ToString("yyyyMMddHHmm");
            NewTable(tableName, startDT, endDateTime);
            h2.Value = tableName;
        }
        //else
        //{
        //    if (this.hf_newTableID.Value != "")
        //    {
        //        Control table = this.ph_InsertCtrl.FindControl(this.hf_newTableID.Value);
        //        if (table != null)
        //        {
        //            this.ph_InsertCtrl.Controls.Remove(table);
        //        }
        //    }
        //}
    }

    private void UpdateTableData(string tableName, DateTime startDT, DateTime endDT)
    {
        //LinkButton lbtn = (LinkButton)ph_InsertCtrl.FindControl("LinkButton1");
        //lbtn.Text = "Got It!";
        //Table table = (Table)this.ph_InsertCtrl.Controls.(tableName);
        //Label lblStartDT = (Label)table.Rows[0].Cells[1].Controls[0];
        //lbl_startDT.Text = startDT.ToString("yyyy/MM/dd HH:mm");

        //TextBox tb_endDate = (TextBox)table.Rows[0].Cells[3].Controls[0];
        //tb_endDate.Text = endDT.ToString("yyyy/MM/dd");
        //DropDownList dl = (DropDownList)table.Rows[0].Cells[3].Controls[2];
        //dl.SelectedValue = endDT.ToString("HH:mm");
    }
}