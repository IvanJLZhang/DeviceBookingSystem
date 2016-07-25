<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EquipmentView.aspx.cs" Inherits="Manager_Equipment_EquipmentView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="up_conditionSearch" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Panel ID="p_filter" runat="server">
                        Site:
                            <asp:DropDownList ID="ddl_location" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_location_SelectedIndexChanged" Width="100px"></asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                            Department:
                            <asp:DropDownList ID="ddl_Department" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Department_SelectedIndexChanged" AppendDataBoundItems="False">
                                <asp:ListItem Selected="True" Text="ALL" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                    </asp:Panel>
                    <asp:Panel ID="panel_EquipShow" runat="server" Width="100%" Visible="true">
                        <table align="center">
                            <tr>
                                <td align="left">
                                    <asp:DataList ID="DataList1" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" CellPadding="4"
                                        OnItemCommand="DataList1_ItemCommand" BorderStyle="None" BorderWidth="1px" GridLines="Both"
                                        OnItemDataBound="DataList1_ItemDataBound">
                                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                        <%--<ItemStyle BackColor="White" ForeColor="#330099" />--%>
                                        <ItemTemplate>
                                            <div>
                                                <table width="500px" style="max-height: 400px">
                                                    <tr>
                                                        <td>
                                                            <asp:Image ID="ImageButton2" runat="server" Height="100px" Width="150px" ImageUrl='<%# Bind("ImageUrl") %>' />
                                                        </td>
                                                        <td style="vertical-align: top; text-align: left">
                                                            <asp:LinkButton ID="TextBox1" runat="server" Text='<%# Bind("[Device Name]") %>' Height="40px"
                                                                CommandName="EquipmentDetail" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="TextBox2" runat="server" TextMode="MultiLine" Height="72px" Width="223px" BorderStyle="None" Text='<%# Bind("Note") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="EquipmentDetail" CommandArgument='<%# Bind("id") %>'>Detail</asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton ID="lbtn_Delete" runat="server" CommandName="DeleteItem" CommandArgument='<%# Bind("id") %>'>Delete</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ItemTemplate>
                                        <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                    </asp:DataList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 579px; text-align: right; font-size: 9pt;" align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Current："></asp:Label>
                                    [
                                <asp:Label ID="labPage" runat="server" Text="1"></asp:Label>
                                    &nbsp;]&nbsp;&nbsp;
                                <asp:Label ID="Label3" runat="server" Text="All："></asp:Label>
                                    [
                                <asp:Label ID="labBackPage" runat="server"></asp:Label>
                                    &nbsp;]&nbsp;&nbsp;<asp:LinkButton ID="lnkbtnOne" runat="server" Font-Underline="False"
                                        OnClick="lnkbtnOne_Click" ToolTip="First Page" BorderStyle="Solid" BorderWidth="1px">|<</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkbtnUp" runat="server" Font-Underline="False"
                                OnClick="lnkbtnUp_Click" ToolTip="Pre Page" BorderStyle="Solid" BorderWidth="1px" Width="35px"><  Pre</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkbtnNext" runat="server" Font-Underline="False"
                                OnClick="lnkbtnNext_Click" ToolTip="Next Page" BorderStyle="Solid" BorderWidth="1px" Width="35px">Next ></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkbtnBack" runat="server" Font-Underline="False"
                                    OnClick="lnkbtnBack_Click" ToolTip="Last Page" BorderStyle="Solid" BorderWidth="1px">>|</asp:LinkButton>&nbsp;&nbsp;</td>
                            </tr>
                            <%--                    <tr>
                        <td></td>
                        <td align="right">
                            <asp:ImageButton ID="ImageButton3" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="ImageButton2_Click" />
                        </td>
                    </tr>--%>
                        </table>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
