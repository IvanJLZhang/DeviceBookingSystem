<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="Setting.aspx.cs" Inherits="Manager_Setting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Setting</title>
    <style type="text/css">
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up_Mail" runat="server">
        <ContentTemplate>
            <div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

