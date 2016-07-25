<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FillReason.aspx.cs" Inherits="Manager_DeviceBooking_FillReason" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../CSS/MainStyleSheet.css" rel="Stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <h2>Approve/Reject Reason</h2>
            <asp:TextBox ID="tb_Reason" runat="server" TextMode="MultiLine" Height="71px" Width="402px" ValidationGroup="Reason"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_Reason" ErrorMessage="Approve/Reject can not NULL." ValidationGroup="Reason">*</asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="btn_ApproveOrReject" runat="server" Text="Approve" Width="100px" OnClick="btn_ApproveOrReject_Click" ValidationGroup="Reason" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="Cancel" onclick="javascript: window.close();" style="width:100px" /><br />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true" ShowMessageBox="false" ValidationGroup="Reason" />
        </div>
    </form>
</body>
</html>
