<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="SystemSettings.aspx.cs" Inherits="Manager_SystemSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe style="width: 100%; height: 800px" src="./Settings/ProjectView.aspx" id="devViewFrame" runat="server"></iframe>
</asp:Content>

