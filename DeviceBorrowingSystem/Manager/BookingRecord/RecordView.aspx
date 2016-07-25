<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecordView.aspx.cs" Inherits="Manager_BookingRecord_RecordView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd_main" runat="server">
            <ContentTemplate>
                <div align="center">
                    <dx:ASPxGridView ID="gv_RecordView" runat="server" AutoGenerateColumns="False" Theme="Metropolis"
                        OnRowDeleting="gv_RecordView_RowDeleting"
                        OnCustomButtonCallback="gv_RecordView_CustomButtonCallback"
                        OnDataBound="gv_RecordView_DataBound"
                        OnInit="gv_RecordView_Init">
                        <Columns>
                            <%--<dx:GridViewDataTextColumn Caption="Booking ID" VisibleIndex="0" FieldName="Booking_ID">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>--%>
                            <dx:GridViewCommandColumn VisibleIndex="0" ShowDeleteButton="true" ButtonType="Image" Caption="Command"
                                ShowClearFilterButton="true">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <DeleteButton Image-Url="../../Images/Delete.png" Image-Width="15px" Image-Height="15px">
                                    <Image Height="15px" Url="../../Images/Delete.png" Width="15px">
                                    </Image>
                                </DeleteButton>
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btn_return" Image-ToolTip="set status to return" Image-Url="../../Images/return.png" Image-Width="15px" Image-Height="15px">
                                        <Image Height="15px" ToolTip="set status to return" Url="../../Images/return.png" Width="15px">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                    <dx:GridViewCommandColumnCustomButton ID="btn_edit" Image-ToolTip="edit the record" Image-Url="../../Images/Edit.png" Image-Height="15px" Image-Width="15px"></dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewCommandColumn VisibleIndex="1" ButtonType="Image" Caption="Detail">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btn_view" Image-ToolTip="view record detail" Image-Url="../../Images/application_windows_grow.png" Image-Width="15px" Image-Height="15px">
                                        <Image Height="15px" ToolTip="view record detail" Url="../../Images/application_windows_grow.png" Width="15px">
                                        </Image>
                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="Device" VisibleIndex="2" FieldName="Device_Name">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Custom ID" VisibleIndex="3" FieldName="Custom_ID">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn Caption="Status" VisibleIndex="4" FieldName="Status">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Text="REJECT" Value="-1" />
                                        <dx:ListEditItem Text="NEW_NOSUBMIT" Value="0" />
                                        <dx:ListEditItem Text="NEW_SUBMIT" Value="1" />
                                        <dx:ListEditItem Text="APPROVE_NORETURN" Value="2" />
                                        <dx:ListEditItem Text="RETURN" Value="3" />
                                    </Items>
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataTextColumn Caption="Recurrence" ToolTip="Is Recurrence" VisibleIndex="5" FieldName="db_is_recurrence">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn Caption="From" VisibleIndex="6" FieldName="Loan_DateTime">
                                <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                </PropertiesDateEdit>
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn Caption="TO" VisibleIndex="7" FieldName="Plan_To_ReDateTime">
                                <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                </PropertiesDateEdit>
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTimeEditColumn Caption="Start" FieldName="db_start" VisibleIndex="8" ToolTip="appointment time-start">
                                <PropertiesTimeEdit DisplayFormatString="hh\:mm">
                                </PropertiesTimeEdit>
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTimeEditColumn>
                            <dx:GridViewDataTimeEditColumn Caption="End" FieldName="db_end" VisibleIndex="9" ToolTip="appointment time-end">
                                <PropertiesTimeEdit DisplayFormatString="hh\:mm">
                                </PropertiesTimeEdit>
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTimeEditColumn>
                            <dx:GridViewDataDateColumn Caption="Return" VisibleIndex="10" FieldName="Real_ReDateTime">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                </PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>

                            <dx:GridViewDataTextColumn Caption="Project" VisibleIndex="11" FieldName="PJ_Name">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Test Category" VisibleIndex="12" FieldName="TestCategory">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Loaner" VisibleIndex="13" FieldName="LoanerName">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Device Dpt." ToolTip="Owner Department" VisibleIndex="14" FieldName="Owner_Dpt">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Loaner Dpt." ToolTip="Loaner Department" VisibleIndex="15" FieldName="Loaner_Dpt">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                        <Settings ShowFilterRow="true" ShowFilterBar="Visible" />
                        <SettingsBehavior ConfirmDelete="true" AllowFocusedRow="true" />
                        <SettingsPager PageSize="15"></SettingsPager>
                        <SettingsText ConfirmDelete="Do you want to delete the record?" />
                        <Styles CommandColumn-Spacing="3px">
                            <CommandColumn Spacing="3px"></CommandColumn>
                        </Styles>
                    </dx:ASPxGridView>
                    <script type="text/javascript">
                        function EndCallBack(s, e) {
                            if (s.cpAlertMsg != "" && s.cpAlertMsg != null) {
                                alert(s.cpAlertMsg);
                                s.cpAlertMsg = null;
                            }
                            if (s.cpBookingId != null && s.cpType != null) {
                                var intID = s.cpBookingId;
                                if (s.cpType == 'btn_view') {
                                    window.open('../DeviceBooking/ApprovePage_View.aspx?bookingId=' + intID, '', '');
                                }
                                else if (s.cpType == 'btn_edit' && s.cpViewType != null) {
                                    var viewType = s.cpViewType;
                                    var obj = window.open('./RecordDetail.aspx?type=' + viewType + '&booking_id=' + intID, '', '');
                                }
                                else if (s.cpType == "btn_return") {
                                    var obj = window.showModalDialog("../OpenPage/ReturnDevice.aspx?bookingid=" + intID, '', 'dialogHeight = 300px;');
                                    if (obj != null && obj == "OK")
                                        window.location.reload();
                                }

                                s.cpBookingId = null;
                                s.cpType = null;
                            }
                        }
                    </script>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
