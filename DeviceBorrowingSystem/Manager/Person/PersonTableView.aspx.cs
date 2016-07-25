using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Person_PersonTableView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void gv_PersonTable_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        e.Cancel = true;
        string id = e.Keys["P_ID"].ToString();
        UserRole oper_role = (UserRole)Convert.ToInt32(Session["Role"]);
        string oper_id = Session["UserID"].ToString();
        int result = personManageFac.DeletePersonByIDByRole(id, oper_role, oper_id);
        if (result > 0)
        {
            gv_PersonTable.JSProperties["cpAlertMsg"] = "Deleted user:" + id;
            gv_PersonTable_Init(null, null);
        }
        else
        {
            gv_PersonTable.JSProperties["cpAlertMsg"] = "Can not delete user:" + id;
        }


    }

    void ShowPersonTable()
    {
        if (Session["LoginUserInfo"] != null)
        {
            DataTable loginInfo = (DataTable)Session["LoginUserInfo"];
            var personList = personManageFac.GetPersonTable(loginInfo);

            if (personList != null)
            {
                this.gv_PersonTable.DataSource = personList;
                this.gv_PersonTable.KeyFieldName = "P_ID";
                this.gv_PersonTable.DataBind();
            }
        }
    }
    protected void gv_PersonTable_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        var intID = Convert.ToString(gv_PersonTable.GetRowValues(gv_PersonTable.FocusedRowIndex, "P_ID"));
        var view = sender as ASPxGridView;
        view.JSProperties["cpType"] = e.ButtonID;
        view.JSProperties["cpID"] = intID;
    }
    protected void gv_PersonTable_Init(object sender, EventArgs e)
    {
        ShowPersonTable();
    }
}