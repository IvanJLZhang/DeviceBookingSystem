<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeviceDetail.aspx.cs" Inherits="Manager_Equipment_DeviceDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .tableStyle {
        }

            .tableStyle th {
                width: 100px;
                text-align: left;
            }
    </style>
    <script type="text/javascript">
        function showImage(s, e) {
            if (e.callbackData) {
                var img = document.getElementById("img_deviceImage");
                img.src = e.callbackData;
                pc_pickimage.Hide();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btn_Save" />
            </Triggers>
            <ContentTemplate>
                <div align="center">
                    <asp:Panel ID="pnl_DTitle" runat="server">
                        <table class="tableStyle">
                            <tr>
                                <td rowspan="2">
                                    <dx:ASPxImage ID="img_deviceImage" ClientInstanceName="ASPxImage1" runat="server" ShowLoadingImage="true" Height="160px" Width="150px"
                                        ImageUrl="~/Images/用户-002.gif">
                                    </dx:ASPxImage>
                                    <script type="text/javascript">
                                        function pickImage(s, e) {
                                            pc_pickimage.Show();
                                        }
                                    </script>
                                    <dx:ASPxButton ID="btn_changeImage" runat="server" Text="Pick an Image" Width="150px">
                                        <ClientSideEvents Click="function(s,e){pickImage(s,e);}" />
                                    </dx:ASPxButton>
                                </td>
                                <td align="left" valign="top">
                                    <dx:ASPxTextBox ID="tb_name" runat="server" Theme="Moderno" Width="250px">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo">
                                            <RequiredField IsRequired="true" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <dx:ASPxMemo ID="tb_description" runat="server" Height="100px" Width="520px" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxMemo>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>

                    <asp:Panel ID="pnl_baseInfo" runat="server">
                        <table class="tableStyle">
                            <tr>
                                <th>ID</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_id" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <th>Owner</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_owner" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Asset ID</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_assetid" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <th>Category</th>
                                <td>
                                    <dx:ASPxComboBox ID="cb_category" runat="server" Theme="Moderno"
                                         AutoPostBack="true" ReadOnly="true">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                        <Items>
                                            <dx:ListEditItem Text="Device" Value="1" />
                                            <dx:ListEditItem Text="Equipment" Value="2" />
                                            <dx:ListEditItem Text="Chamber" Value="3" />

                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Vender</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_vender" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <th>Cost</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_cost" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Status</th>
                                <td>
                                    <dx:ASPxComboBox ID="cb_status" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="pnl_deviceDetail" runat="server">
                        <table class="tableStyle">
                            <tr>
                                <th>Custom ID</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_CustomID" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Class</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_class" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <th>Interface</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_interface" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnl_equipmentDetail" runat="server">
                        <table class="tableStyle">
                            <tr>
                                <th>Lab</th>
                                <td>
                                    <dx:ASPxComboBox ID="cb_lab" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Test Time</th>
                                <td>
                                    <dx:ASPxTimeEdit ID="te_testTime" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTimeEdit>
                                </td>
                                <th>Working Hours</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_avg_hr" runat="server" Theme="Moderno"
                                        NullText="default is 24 hours/day">
                                        <NullTextStyle ForeColor="Red"></NullTextStyle>
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Loan Day</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_loanday" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnl_chamberDetail" runat="server">
                        <table class="tableStyle">
                            <tr>
                                <th>Lab</th>
                                <td>
                                    <dx:ASPxComboBox ID="cb_lab_c" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <th>Working Hours</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_avg_hr_c" runat="server" Theme="Moderno"
                                        NullText="default is 24 hours/day">
                                        <NullTextStyle ForeColor="Red"></NullTextStyle>
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                                <th>Loan Day</th>
                                <td>
                                    <dx:ASPxTextBox ID="tb_loanday_c" runat="server" Theme="Moderno">
                                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="DeviceInfo"></ValidationSettings>
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnl_command" runat="server">
                        <table>
                            <tr>
                                <td style="width: 40%">
                                    <dx:ASPxButton ID="btn_Save" runat="server" Text="Save" Theme="Moderno" Width="100px" OnClick="btn_Save_Click" ValidationGroup="Update">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_cancel" runat="server" Text="Cancel" Theme="Moderno" Width="100px">
                                        <ClientSideEvents Click="function(s,e){window.close();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnl_deviceTable" runat="server">
                        <table>
                            <tr>
                                <td colspan="2">
                                    <dx:ASPxGridView ID="gv_deviceTable" runat="server" AutoGenerateColumns="False" Theme="Moderno">
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption="Device" VisibleIndex="0">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Site" VisibleIndex="1">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Owner" VisibleIndex="2">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewCommandColumn ButtonType="Image" Caption="Command" ShowDeleteButton="True" VisibleIndex="3">
                                                <DeleteButton Image-Url="../../Images/Delete.png"></DeleteButton>
                                            </dx:GridViewCommandColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <dx:ASPxButton ID="btn_submit" runat="server" Text="Submit" Theme="Moderno" Width="100px" OnClick="btn_submit_Click"></dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_cancel1" runat="server" Text="Cancel" Theme="Moderno" Width="100px">
                                        <ClientSideEvents Click="function(s,e){window.close();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                    <dx:ASPxPopupControl ID="pc_pickimage" ClientInstanceName="pc_pickimage" runat="server" Theme="Moderno"
                        HeaderText="Pick an Image"
                        Modal="True"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <dx:ASPxUploadControl ID="upc_deviceImage" runat="server" UploadMode="Auto" Width="280px"
                                    ShowUploadButton="true" OnFileUploadComplete="upc_deviceImage_FileUploadComplete">
                                    <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.gif,.png"></ValidationSettings>

                                    <AdvancedModeSettings EnableMultiSelect="false" />

                                    <ClientSideEvents FileUploadComplete="function(s,e){showImage(s,e);}" />
                                </dx:ASPxUploadControl>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <dx:ASPxValidationSummary ID="ASPxValidationSummary1" runat="server"></dx:ASPxValidationSummary>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
