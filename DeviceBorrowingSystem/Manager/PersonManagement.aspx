<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="PersonManagement.aspx.cs" Inherits="Manager_PersonManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Person Management</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upd_main" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="imgbtn_PrintPersonList" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="p_Navigate" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td>
                        </td>
                        <td align="right">
                            <asp:Panel ID="p_Opera" runat="server" HorizontalAlign="Right">
                            <asp:ImageButton ID="Button1" runat="server" Height="20px" ImageUrl="~/Images/add.png" ToolTip="Add a person" Width="22px" OnClick="Button1_Click" />
                                &nbsp;&nbsp;<asp:ImageButton ID="imgb_AddFromFile" runat="server" Height="20px" ImageUrl="~/Images/AddFromFile.png" Width="22px" OnClick="imgb_AddFromFile_Click" ToolTip="Add Person list from file" />
                                &nbsp;&nbsp;<asp:ImageButton ID="imgbtn_PrintPersonList" runat="server" Height="20px" ImageUrl="~/Images/printer.png" Width="22px" OnClick="imgbtn_PrintPersonList_Click" ToolTip="Print Person List" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div align="center">
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <iframe style="width: 100%; height: 800px" src="Person/PersonTableView.aspx" id="devViewFrame"></iframe>
                        </td>
                    </tr>
                </table>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

