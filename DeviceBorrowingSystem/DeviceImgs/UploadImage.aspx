<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadImage.aspx.cs" Inherits="DeviceImgs_UploadImage" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxUploadControl ID="ASPxUploadControl1" runat="server" UploadMode="Auto" Width="280px"
             ShowUploadButton="true" ShowProgressPanel="true" NullText="Click here to select a image file..." OnFileUploadComplete="ASPxUploadControl1_FileUploadComplete">
            <BrowseButton Text="Select a Image"></BrowseButton>
            <ValidationSettings MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png"></ValidationSettings>
        </dx:ASPxUploadControl>
        <%--<asp:Image ID="img" runat="server" AlternateText="imgshow" />--%>
        <iframe id="imgFrame" runat="server"></iframe>
        <%--<asp:LinkButton ID="upload" runat="server" OnClick="upload_Click">Upload</asp:LinkButton>--%>
    </div>
    </form>
</body>
</html>
