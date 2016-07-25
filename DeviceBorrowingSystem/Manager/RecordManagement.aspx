<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="RecordManagement.aspx.cs" Inherits="Manager_RecordManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upd_main" runat="server">
        <ContentTemplate>
            <asp:Panel ID="p_Navigate" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td>Category: 
            <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" ViewStateMode="Enabled">
                <asp:ListItem Value="1" Text="Device"></asp:ListItem>
                <asp:ListItem Value="2" Text="Equipment"></asp:ListItem>
                <asp:ListItem Value="3" Text="Chamber"></asp:ListItem>
            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Panel ID="p_Opera" runat="server" HorizontalAlign="Right">
                                <table align="right">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="linkbutton" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton_Click" ToolTip="Add a single record" />
                                            <%--&nbsp;&nbsp;<asp:ImageButton ID="imgb_AddFromFile" runat="server" Height="20px" ImageUrl="~/Images/AddFromFile.png" Width="22px" OnClick="imgb_AddFromFile_Click" ToolTip="Add record list from file" />--%>
                                            <asp:ImageButton ID="btn_ExportExcel" runat="server" ImageUrl="~/Images/printer.png" Height="20px" Width="22px" ToolTip="Export to Excel" OnClick="btn_ExportExcel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td colspan="2">
                <iframe style="width: 100%; height: 800px" runat="server" src="./BookingRecord/RecordView.aspx?type=1" id="devViewFrame"></iframe>
            </td>
        </tr>
    </table>
</asp:Content>

