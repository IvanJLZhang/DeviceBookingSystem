<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerSelect.aspx.cs" Inherits="Manager_Equipment_OwnerSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title></title>
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
            <div style="width: 450px; border:solid thin 1px">
                <h2>Select Person
                </h2>
                <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
                    Department:
            <asp:DropDownList ID="ddl_department" runat="server" AppendDataBoundItems="false"
                OnSelectedIndexChanged="ddl_department_SelectedIndexChanged" AutoPostBack="true">
                <%--<asp:ListItem Selected="True" Value="0" Text="--ALL--"></asp:ListItem>--%>
            </asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT DISTINCT P_Department FROM tbl_Person WHERE (P_Role &gt;= @Role) ORDER BY P_Department">
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="Role" QueryStringField="maxrole" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <br />
                </asp:Panel>
                <asp:DataList ID="dl_UserList" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CellPadding="4" ForeColor="#333333"
                    OnItemCommand="dl_UserList_ItemCommand">
                    <AlternatingItemStyle BackColor="White" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <ItemStyle BackColor="#E3EAEB" />
                    <ItemTemplate>
                        <table>
                            <tr align="left">
                                <td align="left" style="width: 100px">
                                    <asp:LinkButton ID="lbl_UserName" runat="server" Text='<%# Bind("UserName") %>' ToolTip='<%# Bind("UserID") %>'
                                        CommandName="SelectUser" CommandArgument='<%# Bind("UserName") %>'></asp:LinkButton>
                                </td>
                            </tr>
                        </table>

                    </ItemTemplate>
                    <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                </asp:DataList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT P_Name AS UserName, P_ID AS UserID FROM tbl_Person WHERE (P_Department = @Department) Where @Department != '0'">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddl_department" Name="Department" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>

            </div>


        </div>
    </form>
</body>
</html>
