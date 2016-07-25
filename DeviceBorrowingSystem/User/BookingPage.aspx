<%@ Page Title="" Language="C#" MasterPageFile="~/User/User_MasterPage.master" AutoEventWireup="true" CodeFile="BookingPage.aspx.cs" Inherits="User_BookingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_next"/>
            <asp:PostBackTrigger ControlID="btn_back"/>
        </Triggers>
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td style="width: 10%">
                        <dx:ASPxButton ID="btn_back" runat="server" Text="<BACK" Theme="Moderno" OnClick="btn_back_Click"></dx:ASPxButton>
                    </td>
                    <td align="center">
                        <dx:ASPxLabel ID="lbl_stepName" runat="server" Text="Step 1--Select Device/Equipment" Theme="Moderno"
                            Font-Bold="true" Font-Size="Large">
                        </dx:ASPxLabel>
                    </td>
                    <td style="width: 10%">
                        <dx:ASPxButton ID="btn_next" runat="server" Text="NEXT>" Theme="Moderno" OnClick="btn_next_Click"></dx:ASPxButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">

                        <iframe style="width: 100%; height: 800px" id="devViewFrame" runat="server"></iframe>

                    </td>
                </tr>
                <tr>
                    <td style="width: 10%"></td>
                    <td></td>
                    <td style="width: 10%"></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

