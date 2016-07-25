using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeviceImgs_DeviceImgView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowImage();
    }
    void ShowImage() {
        //if (Request["imagepath"] != null)
        //{
        //    this.imgShow.ImageUrl = Request["imagepath"].ToString();
        //    if (Request["w"] != null)
        //    {
        //        this.imgShow.Width = Int32.Parse(Request["w"].ToString());
        //    }
        //    if (Request["h"] != null)
        //        this.imgShow.Height = Int32.Parse(Request["h"].ToString());
        //}
        this.imgShow.ImageUrl = "./UploadImages/1.png";
    }
}