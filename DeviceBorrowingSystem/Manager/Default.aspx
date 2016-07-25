<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Manager_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/Manager_Home.css" rel="stylesheet" />
    <link href="../CSS/MainStyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .inline {
            display: inline;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div align="center">
        <asp:Panel ID="pnl_command" runat="server" HorizontalAlign="Left">
            <span style="float: right">
                <dx:ASPxButton ID="btn_allApprove" runat="server" Text="Approve" Theme="Moderno" CssClass="inline"></dx:ASPxButton>
                <dx:ASPxButton ID="btn_allReject" runat="server" Text="Reject" Theme="Moderno" CssClass="inline"></dx:ASPxButton>
            </span>
            <dx:ASPxComboBox ID="cb_cat" runat="server" CssClass="inline" ValueType="System.Int32" Theme="Moderno" AutoPostBack="true" OnSelectedIndexChanged="cb_cat_SelectedIndexChanged"
                Caption="Category">
            </dx:ASPxComboBox>

        </asp:Panel>
        <hr />
        <asp:Panel ID="pnl_RecordRequest" runat="server">
            <dx:ASPxGridView ID="gv_requestView" runat="server" Theme="Moderno" AutoGenerateColumns="False"
                Caption="New Borrow Request View"
                OnInit="gv_requestView_Init"
                OnDataBound="gv_requestView_DataBound"
                OnCustomButtonCallback="gv_requestView_CustomButtonCallback">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Booking_ID" VisibleIndex="2" FieldName="Booking_ID">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Custom_ID" VisibleIndex="3" FieldName="Custom_ID">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Device" VisibleIndex="4" FieldName="Device_Name">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Dpt." ToolTip="Department" VisibleIndex="5" FieldName="Owner_Dpt">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Loaner" VisibleIndex="6" FieldName="LoanerName">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Recurrence" VisibleIndex="7" FieldName="db_is_recurrence">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="From" VisibleIndex="8" FieldName="Loan_DateTime">
                        <PropertiesDateEdit DisplayFormatString="yyy/MM/dd HH:mm"></PropertiesDateEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn Caption="TO" VisibleIndex="9" FieldName="Plan_To_ReDateTime">
                        <PropertiesDateEdit DisplayFormatString="yyy/MM/dd HH:mm"></PropertiesDateEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTimeEditColumn Caption="Start" FieldName="db_start" VisibleIndex="10" ToolTip="appointment time-start">
                        <PropertiesTimeEdit DisplayFormatString="hh\:mm">
                        </PropertiesTimeEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTimeEditColumn>
                    <dx:GridViewDataTimeEditColumn Caption="End" FieldName="db_end" VisibleIndex="11" ToolTip="appointment time-end">
                        <PropertiesTimeEdit DisplayFormatString="hh\:mm">
                        </PropertiesTimeEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTimeEditColumn>
                    <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="12" Caption="Command">
                        <CellStyle CssClass="gridViewCommandButton"></CellStyle>
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="btn_edit">
                                <Image Url="~/Images/Edit.png" Height="20px" ToolTip="Edit" Width="20px">
                                </Image>
                            </dx:GridViewCommandColumnCustomButton>
                            <dx:GridViewCommandColumnCustomButton ID="btn_approve">
                                <Image Url="~/Images/Details.png" Height="20px" ToolTip="Approve/Reject" Width="20px">
                                    <SpriteProperties Left="3px" />
                                </Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dx:GridViewCommandColumn>

                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="1">
                    </dx:GridViewCommandColumn>
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
                        if (s.cpType == 'btn_approve') {
                            window.open('./DeviceBooking/ApprovePage.aspx?bookingId=' + intID, '', '');
                        }
                        else if (s.cpType == 'btn_edit' && s.cpViewType != null) {
                            var viewType = s.cpViewType;
                            var obj = window.open('./BookingRecord/RecordDetail.aspx?type=' + viewType + '&booking_id=' + intID, '', '');
                        }
                        else if (s.cpType == 'btn_add' && s.cpViewType != null) {
                            var viewType = s.cpViewType;
                            var obj = window.open('./Person/UserInfo.aspx?uid=' + intID + '&type=' + viewType);
                        }
                        s.cpBookingId = null;
                        s.cpType = null;
                    }
                }
            </script>
        </asp:Panel>
        <hr style="margin-top:10px; margin-bottom:10px" />
        <asp:Panel ID="pnl_UserRequest" runat="server">
            <dx:ASPxGridView ID="gv_newUserView" runat="server" Theme="Moderno" AutoGenerateColumns="False"
                Caption="New User Request View"
                OnInit="gv_newUserView_Init"
                OnDataBound="gv_newUserView_DataBound"
                OnCustomButtonCallback="gv_newUserView_CustomButtonCallback">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="UID" VisibleIndex="2" FieldName="P_ID">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Email" VisibleIndex="3" FieldName="P_Email">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Department" VisibleIndex="4" FieldName="P_Department">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="12" Caption="Command">
                        <CellStyle CssClass="gridViewCommandButton"></CellStyle>
                        <CustomButtons>
                            <dx:GridViewCommandColumnCustomButton ID="btn_add">
                                <Image Url="../Images/Details.png" Height="20px" ToolTip="Edit" Width="20px">
                                </Image>
                            </dx:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dx:GridViewCommandColumn>
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
        </asp:Panel>
    </div>
</asp:Content>

