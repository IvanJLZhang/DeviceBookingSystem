<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReturnDevice.aspx.cs" Inherits="Manager_OpenPage_ReturnDevice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Return Device</title>
    <style type="text/css">
        html {
            font-family: Calibri;
            /*background-color:ActiveBorder;*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <table>
                <tr>
                    <th colspan="2">
                        <h2 style="text-transform: uppercase">Return Device</h2>
                        <br />
                    </th>
                </tr>
                <tr>
                    <th align="left">Return Time: </th>
                    <td>
                        <asp:Label ID="lbl_ReturnTime" runat="server" Text='<%# DateTime.Now.ToString("yyyy-MM-dd HH:mm") %>' ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left">Check Status: </th>
                    <td>
                        <asp:DropDownList ID="ddl_DeviceStatus" runat="server">
                            <asp:ListItem Selected="True" Text="Usable" Value="1"></asp:ListItem>
                            <asp:ListItem Selected="false" Text="Broken" Value="2"></asp:ListItem>
                            <asp:ListItem Selected="false" Text="Lost" Value="3"></asp:ListItem>
                            <asp:ListItem Selected="false" Value="4">Return to Custumer</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btn_Return" runat="server" Text="Return" OnClick="btn_Return_Click" />
                    </td>
                    <td>
                        <input type="button" id="btn_Cancel" value="Cancel" onclick="window.event.returnValue = 'Hello'; window.close();" />
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
