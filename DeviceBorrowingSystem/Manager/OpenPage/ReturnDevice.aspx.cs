using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Manager_OpenPage_ReturnDevice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["bookingid"] != null)
            {
                string booking_id = Request["bookingid"].ToString();
                if (RecordManagment.CheckStatus(booking_id) != 2)
                {
                    Response.Write("<script>window.close();</script>");
                }
            }
            this.lbl_ReturnTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
    protected void btn_Return_Click(object sender, EventArgs e)
    {
        if (Request["bookingid"] != null)
        {
            string booking_id = Request["bookingid"].ToString();
            cl_DeviceBookingManage deviceBookingManage = new cl_DeviceBookingManage();
            bool result = deviceBookingManage.ReturnDeviceByID(booking_id, this.ddl_DeviceStatus.SelectedValue);
            if (result)
            {
                Response.Write("<script>window.returnValue = 'OK';window.close();</script>");
            }
        }
    }
    protected void cancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();</script>");
    }
}