using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_Person_Charge_of_Department : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowDepartmentList();
        }
    }

    private void ShowDepartmentList()
    {
        cl_PersonManage personManage = new cl_PersonManage();
        DataTable dptList = personManage.GetDepartmentList(0);
        foreach (DataRow row1 in dptList.Rows)
        {
            if (row1["DptValue"].ToString().CompareTo("0") == 0)
            { dptList.Rows.Remove(row1); break; }
        }

        dptList.DefaultView.Sort = "DptValue";
        this.dl_DptList.DataSource = dptList.DefaultView;
        this.dl_DptList.DataKeyField = "DptValue";
        this.dl_DptList.DataBind();


        string chargeofDpt = String.Empty;
        if (Request["cdpt"] != null)
        {
            chargeofDpt = Request["cdpt"].ToString();
        }
        if (chargeofDpt != String.Empty)
        {
            if (chargeofDpt.CompareTo("0") == 0)
            {
                foreach (DataListItem item in this.dl_DptList.Items)
                {

                    CheckBox cb = (CheckBox)item.FindControl("cb_DptName");
                    cb.Checked = true;
                }
            }
            else
            {
                string[] chargeofdptArr = chargeofDpt.Split(',');
                foreach (var dpt in chargeofdptArr)
                {
                    foreach (DataListItem item in this.dl_DptList.Items)
                    {
                        if (dpt.CompareTo(dl_DptList.DataKeys[item.ItemIndex]) == 0)
                        {
                            CheckBox cb = (CheckBox)item.FindControl("cb_DptName");
                            cb.Checked = true;
                        }
                    }
                }
            }
        }
    }
    protected void cb_all_CheckedChanged(object sender, EventArgs e)
    {
        foreach (DataListItem item in this.dl_DptList.Items)
        {
            CheckBox cb = (CheckBox)item.FindControl("cb_DptName");
            cb.Checked = cb_all.Checked;
        }
    }
    protected void btn_admit_Click(object sender, EventArgs e)
    {
        string chargeofdpt = String.Empty;
        foreach (DataListItem item in this.dl_DptList.Items)
        {
            CheckBox cb = (CheckBox)item.FindControl("cb_DptName");
            if (cb.Checked)
            {
                chargeofdpt += cb.Text + ",";
            }
        }
        if (chargeofdpt.CompareTo(String.Empty) != 0)
            chargeofdpt = chargeofdpt.Remove(chargeofdpt.Length - 1);

        Response.Write("<script>window.returnValue = '" + chargeofdpt + "'; window.close();</script>");
    }
}