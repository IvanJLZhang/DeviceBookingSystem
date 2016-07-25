<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deviceView.aspx.cs" Inherits="Manager_Device_deviceView" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../CSS/Manager_Home.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd_main" runat="server">
            <ContentTemplate>
                <div>
                    <dx:ASPxGridView ID="agv_deviceTable" runat="server" AutoGenerateColumns="False" EnableTheming="True" Theme="Moderno"
                        OnRowDeleting="agv_deviceTable_RowDeleting" OnRowCommand="agv_deviceTable_RowCommand"
                        OnInit="agv_deviceTable_Init" OnDataBound="agv_deviceTable_DataBound"
                        OnHtmlRowCreated="agv_deviceTable_HtmlRowCreated"
                        OnCustomCallback="agv_deviceTable_CustomCallback"
                        OnCustomButtonCallback="agv_deviceTable_CustomButtonCallback">
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="true"></SettingsBehavior>
                        <Settings ShowFilterBar="Visible" ShowFilterRow="True" />
                        <SettingsPager PageSize="15"></SettingsPager>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="Device" VisibleIndex="1">
                                <Settings AllowAutoFilter="True" AllowSort="True" />
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Custom ID" VisibleIndex="2">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Owner" VisibleIndex="3">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Department" VisibleIndex="4">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Class" VisibleIndex="5">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Interface" VisibleIndex="6">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Site" VisibleIndex="7">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn Caption="Status" FieldName="s_status" VisibleIndex="8">
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
                            <dx:GridViewDataTextColumn FieldName="id" VisibleIndex="0" Caption="Select">
                                <DataItemTemplate>
                                    <input type="checkbox" id="group1" runat="server" />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn FieldName="BorrowStatus" VisibleIndex="10">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Text="OK" Value="1" />
                                        <dx:ListEditItem Text="BORROW_OUT" Value="0" />
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
                        </Columns>

                        <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                        <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
                        <SettingsText ConfirmDelete="Do you want to delete the device?" />
                        <Styles>
                            <CommandColumn Spacing="3px">
                            </CommandColumn>
                        </Styles>
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
                                    window.open('../Equipment/DeviceDetail.aspx?device_id=' + intID + "&type=" + s.cpViewType, '', '');
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
