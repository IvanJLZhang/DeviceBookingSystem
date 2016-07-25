using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DeviceImgs_UploadImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.imgFrame.Attributes["src"] = "DeviceImgView.aspx";
    }
    protected void ASPxUploadControl1_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e)
    {
        string fileName = Server.MapPath("./UploadImages/") + "1.png";
        e.UploadedFile.SaveAs(fileName, true);

        //this.imgFrame.Attributes["src"] = "DeviceImgView.aspx";
    }
}