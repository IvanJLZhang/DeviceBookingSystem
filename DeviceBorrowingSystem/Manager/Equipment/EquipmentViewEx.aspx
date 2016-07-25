<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EquipmentViewEx.aspx.cs" Inherits="Manager_Equipment_EquipmentViewEx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd_main" runat="server">
            <ContentTemplate>
                <div>
                    <dx:ASPxGridView ID="agv_deviceTable" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Moderno"
                        OnRowDeleting="agv_deviceTable_RowDeleting" OnRowCommand="agv_deviceTable_RowCommand"
                         OnInit="agv_deviceTable_Init" OnDataBound="agv_deviceTable_DataBound" OnCustomButtonCallback="agv_deviceTable_CustomButtonCallback"
                         OnHtmlRowCreated="agv_deviceTable_HtmlRowCreated"
                         OnCustomCallback="agv_deviceTable_CustomCallback">

                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="true"></SettingsBehavior>

                        <Settings ShowFilterBar="Visible" ShowFilterRow="True" />
                        <SettingsPager PageSize="6"></SettingsPager>
                        <Columns>
                            <dx:GridViewDataImageColumn Caption=" " VisibleIndex="2" FieldName="ImageUrl">
                                <PropertiesImage ImageWidth="200px" ImageHeight="150px">
                                    
                                </PropertiesImage>
                            </dx:GridViewDataImageColumn>
                            <dx:GridViewDataTextColumn FieldName="Device Name" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Owner Name" VisibleIndex="6">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Department" VisibleIndex="7">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Site" VisibleIndex="10">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn Caption="Status" FieldName="s_status" VisibleIndex="11">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Text="Usable" Value="1" />
                                        <dx:ListEditItem Text="Broken" Value="2" />
                                        <dx:ListEditItem Text="Lost" Value="3" />
                                        <dx:ListEditItem Text="ReturnToCustomer" Value="4" />
                                        <dx:ListEditItem Text="NotForCirculation" Value="5" />
                                    </Items>
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewBandColumn VisibleIndex="14" Caption="Command">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <Columns>
                                    <dx:GridViewCommandColumn ButtonType="Image" ShowDeleteButton="True" Caption="Delete" VisibleIndex="0" ShowClearFilterButton="True" Visible="true">
                                        <HeaderStyle CssClass="gridViewHeader" />
                                        <DeleteButton Image-Url="../../Images/Delete.png">
                                            <Image Url="../../Images/Delete.png"></Image>
                                        </DeleteButton>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewCommandColumn ButtonType="Image" Caption="Detail" VisibleIndex="1" ShowClearFilterButton="True" Visible="true">
                                        <HeaderStyle CssClass="gridViewHeader" />
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="btn_view" Image-Url="../../Images/Details.png" Image-ToolTip="View details">
                                                <Image ToolTip="View details" Url="../../Images/Details.png">
                                                </Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>
                                </Columns>
                            </dx:GridViewBandColumn>
                            <dx:GridViewDataTextColumn FieldName="id" VisibleIndex="0" Caption="Select">
                                <DataItemTemplate>
                                    <input type="checkbox" id="group1" runat="server" />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="5" FieldName="Note" Width="250px">
                                <HeaderStyle CssClass="gridViewHeader" />
                                
                                <CellStyle Wrap="True"></CellStyle>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                        <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
                        <SettingsText ConfirmDelete="Do you want to delete the device?" />
                    </dx:ASPxGridView>

                    <script type="text/javascript">
                        function EndCallBack(s, e) {
                            if (s.cpAlertMsg != "" && s.cpAlertMsg != null) {
                                alert(s.cpAlertMsg);
                                s.cpAlertMsg = null;
                            }

                            if (s.cpDeviceId != null && s.cpCommand != null && s.cpViewType != null) {
                                var intID = s.cpDeviceId;
                                if (s.cpCommand == 'btn_view') {
                                    window.open('./DeviceDetail.aspx?device_id=' + intID + "&type=" + s.cpViewType, '', '');
                                }
                                s.cpDeviceId = null;
                                s.cpCommand = null;
                                s.cpViewType = null;
                            }
                        }
                    </script>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
