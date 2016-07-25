<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddDeviceListFromFile.aspx.cs" Inherits="Manager_Equipment_AddDeviceListFromFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Add Device/Equipment List From File</title>
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
            <table width="80%">
                <tr>
                    <th colspan="2">
                        <h2>
                            <asp:Label ID="lbl_title" runat="server"></asp:Label>
                        </h2>
                    </th>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <script type="text/javascript">
                                        function Uploader_OnUploadStart() {
                                            btnUpload.SetEnabled(false);
                                        }

                                        function Uploader_OnFilesUploadComplete(args) {
                                            UpdateUploadButton();
                                            alert("Uploaded, now please click the Preview check button to check the data!");
                                        }
                                        function UpdateUploadButton() {
                                            btnUpload.SetEnabled(uploader.GetText(0) != "");
                                        }
                                    </script>
                                    <dx:ASPxUploadControl ID="FileSelect" runat="server" UploadMode="Auto" Width="400px" Height="20px"
                                        ClientInstanceName="uploader" ShowProgressPanel="true" ShowUploadButton="false"
                                        NullText="Select a device data file(.zip)"
                                        OnFileUploadComplete="FileSelect_FileUploadComplete">

                                        <ClientSideEvents
                                            FilesUploadComplete="function(s, e){ Uploader_OnFilesUploadComplete(e); }"
                                            FileUploadStart="function(s, e){ Uploader_OnUploadStart(); }"
                                            TextChanged="function(s, e){ UpdateUploadButton(); }" />

                                        <ValidationSettings AllowedFileExtensions=".zip"></ValidationSettings>
                                    </dx:ASPxUploadControl>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Upload" ClientInstanceName="btnUpload"
                                        ClientEnabled="False" Style="margin: 0 auto;">
                                        <ClientSideEvents Click="function(s, e) { uploader.Upload(); }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </td>
                    <td>
                        <table cellspacing="10px">
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btn_preview" runat="server" AutoPostBack="false" Text="Preview Check" OnClick="btn_preview_Click">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <script type="text/javascript">
                                    </script>
                                    <dx:ASPxButton ID="btn_AddAll" runat="server" OnClick="btn_AddAll_Click" Text="Add All">
                                        <ClientSideEvents Click="function(s, e){ e.processOnServer = confirm('Are sure to add the these devices?'); }" />
                                    </dx:ASPxButton>
                                </td>

                                <td>
                                    <dx:ASPxButton ID="btn_GetTemplate" runat="server" Text="Get Template" OnClick="btn_GetTemplate_Click"></dx:ASPxButton>
                                </td>
                                <td>
                                    <script type="text/javascript">
                                        function CloseWindow() {
                                            window.close();
                                        }
                                    </script>
                                    <dx:ASPxButton ID="btn_Cancel" runat="server" Text="Cancel" AutoPostBack="false" OnClick="Button1_Click">
                                    </dx:ASPxButton>
                                    <%--<asp:Button ID="Button1" Text="Cancel" runat="server" OnClick="Button1_Click" />--%></td>
                            </tr>
                        </table>


                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gv_deviceView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                            OnRowCommand="gv_deviceView_RowCommand" Caption="Need provide necessary data or parameter error">
                            <AlternatingRowStyle BackColor="White" />

                            <Columns>
                                <asp:TemplateField HeaderText="Device Name" SortExpression="Device_Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Label2" runat="server" Text='<%# Bind("s_name") %>' CommandName="DeviceDetailInfo" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Owner" SortExpression="P_Name">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("owner") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="s_assetid" HeaderText="Asset_ID" />
                                <asp:BoundField DataField="s_note" HeaderText="Description" />
                                <asp:TemplateField HeaderText="Error Comment">
                                    <ItemStyle ForeColor="Red" Width="150px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_error" runat="server" Text='<%# Bind("errnote") %>'>NA</asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_Detele" runat="server" CausesValidation="False" CommandName="DeleteItem" Text="DELETE" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
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
                        <br />
                        <asp:GridView ID="gv_addDeviceView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                            OnRowCommand="gv_addDeviceView_RowCommand" Caption="Can be added" CaptionAlign="Top">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Device Name" SortExpression="Device_Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Label2" runat="server" Text='<%# Bind("s_name") %>' CommandName="DeviceDetailInfo" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="300px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Owner" SortExpression="Owner">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("owner") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="s_assetid" HeaderText="Asset_ID" />
                                <asp:BoundField DataField="s_note" HeaderText="Description" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_Detele" runat="server" CausesValidation="False" CommandName="DeleteItem" Text="DELETE" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
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
