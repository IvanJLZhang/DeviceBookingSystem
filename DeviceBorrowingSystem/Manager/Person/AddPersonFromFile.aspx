<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPersonFromFile.aspx.cs" Inherits="Manager_Person_AddPersonFromFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Add Person List From File</title>
    <script type="text/javascript" language="javascript" src="../JS/MainJavaScript.js"></script>
    <style type="text/css">
        html {
            font-family: Calibri;
            /*background-color:ActiveBorder;*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%--        <asp:UpdatePanel ID="upd_main" runat="server">
            <ContentTemplate>--%>
        <div align="center">
            <table>
                <tr>
                    <th colspan="2">
                        <h2>Add person list from file
                        </h2>
                    </th>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:FileUpload ID="FileSelect" runat="server" Height="21px" Width="333px" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_upload" runat="server" OnClick="btn_upload_Click" Text="Upload" />
                                </td>

                                <td>
                                    <asp:Button ID="btn_AddAll" runat="server" OnClick="btn_AddAll_Click" Text="Add All" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_GetTemplate" runat="server" Text="Get Template" OnClick="btn_GetTemplate_Click"></asp:Button>
                                </td>

                                <td>
                                    <asp:Button ID="Button1" Text="Cancel" runat="server" OnClick="Button1_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>

                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                        <asp:GridView ID="gv_PersonList" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="558px"
                            OnRowCommand="gv_PersonList_RowCommand">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="UserName" SortExpression="P_Name">
                                    <ItemStyle Wrap="false" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Label2" runat="server" Text='<%# Bind("P_Name") %>' CommandName="PersonDetail" CommandArgument='<%# Bind("P_ID")%>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="P_Location" HeaderText="Location" SortExpression="P_Location" />
                                <asp:BoundField DataField="P_Department" HeaderText="Department" SortExpression="P_Department" />
                                <asp:BoundField DataField="P_Email" HeaderText="Email" SortExpression="P_Email" />
                                <asp:BoundField DataField="P_ExNumber" HeaderText="ExeNumber" SortExpression="P_ExNumber" />
                                <asp:TemplateField HeaderText="Role" SortExpression="P_Role">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# GetRoleStr(int.Parse(DataBinder.Eval(Container.DataItem, "P_Role").ToString())) %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# GetRoleStr(int.Parse(DataBinder.Eval(Container.DataItem, "P_Role").ToString())) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_Detele" runat="server" CausesValidation="False" CommandName="DeleteItem" Text="DELETE" CommandArgument='<%# Bind("P_ID") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>

        </div>
        <%--            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gv_deviceView" />
                <asp:PostBackTrigger ControlID="btn_upload" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </form>
</body>
</html>
