<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step1.aspx.cs" Inherits="User_BookingSteps_Step1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <table width="100%" style="margin-top:100px">
        <tr>
            <td colspan="3">
                <dx:ASPxRadioButtonList ID="ASPxRadioButtonList1" runat="server" ValueType="System.String" Theme="Moderno"
                     RepeatDirection="Horizontal" style="margin-right: 16px" Width="600px" OnSelectedIndexChanged="ASPxRadioButtonList1_SelectedIndexChanged"
                     AutoPostBack="true" OnInit="ASPxRadioButtonList1_Init">
                    <Items>
                        <dx:ListEditItem Selected="true" Value="1" Text="Device" />
                         <dx:ListEditItem Selected="false" Value="2" Text="Equipment" />
                         <%--<dx:ListEditItem Selected="false" Value="3" Text="Chamber" />--%>
                    </Items>
                    <Paddings Padding="50px" />
                </dx:ASPxRadioButtonList>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
