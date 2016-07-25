<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="DevManagement.aspx.cs" Inherits="Manager_DevManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server" ID="up_conditionSearch">
        <Triggers>
            <asp:PostBackTrigger ControlID="imgbtn_PrintDeviceList" />
        </Triggers>
        <ContentTemplate>

            <asp:Panel ID="p_Navigate" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td>Category: 
                            <asp:DropDownList ID="ddl_category" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_category_SelectedIndexChanged">
                                <asp:ListItem Value="1">Device</asp:ListItem>
                                <asp:ListItem Value="2">Equipment</asp:ListItem>
                                <asp:ListItem Value="3">Chamber</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
<%--                            <asp:TextBox ID="tb_Search" runat="server" Width="200px" Height="20px"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="tb_Search" WatermarkText="Class\Interface\Name\Location"></cc1:TextBoxWatermarkExtender>
                            <asp:ImageButton ID="imgbtn_Search" runat="server" ImageUrl="~/Images/search.png" Height="20px" Width="22px" ToolTip="Search" OnClick="imgbtn_Search_Click" />--%>
                            &nbsp;&nbsp;<asp:ImageButton ID="linkbutton" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton_Click" ToolTip="Add a Device" />
                            &nbsp;&nbsp;<asp:ImageButton ID="imgb_AddFromFile" runat="server" Height="20px" ImageUrl="~/Images/AddFromFile.png" Width="22px" OnClick="imgb_AddFromFile_Click" ToolTip="Add Device list from file" />
                            &nbsp;&nbsp;<asp:ImageButton ID="imgbtn_PrintDeviceList" runat="server" Height="20px" ImageUrl="~/Images/printer.png" Width="22px" OnClick="imgbtn_PrintDeviceList_Click" ToolTip="Print Device List" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <hr />
    <iframe style="width: 100%; height: 800px" src="Device/deviceView.aspx" id="devViewFrame" runat="server"></iframe>
</asp:Content>

