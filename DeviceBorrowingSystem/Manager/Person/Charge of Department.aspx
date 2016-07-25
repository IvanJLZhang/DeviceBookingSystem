<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Charge of Department.aspx.cs" Inherits="Manager_Person_Charge_of_Department" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <base target="_self" />
    <style type="text/css">
        #Button1 {
            height: 25px;
            width: 66px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
            <asp:DataList ID="dl_DptList" runat="server" CellPadding="4" ForeColor="#333333" RepeatDirection="Horizontal" RepeatColumns="3" CaptionAlign="Top" Caption="Department List"
                 >
            <AlternatingItemStyle BackColor="White" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <ItemStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:CheckBox ID="cb_DptName" Text='<%# Bind("DptName") %>' runat="server" AutoPostBack="true" />
            </ItemTemplate>
            
            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            
        </asp:DataList>
        <asp:CheckBox ID="cb_all" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_all_CheckedChanged" />
        <br />
        <asp:Button ID="btn_admit" runat="server" Text="Admit" Height="25px" Width="65px" OnClick="btn_admit_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <input id="Button1" type="button" value="Cancel" onclick="window.close();" />
        </div>
    </form>
</body>
</html>
