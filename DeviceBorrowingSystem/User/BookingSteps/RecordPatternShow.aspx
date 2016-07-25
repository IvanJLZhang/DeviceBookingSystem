<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecordPatternShow.aspx.cs" Inherits="User_BookingSteps_RecordPatternShow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <title></title>
    <style type="text/css">
        table tr th {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div align="left" style="width: 100%">
            <asp:Panel ID="pnl_filter" runat="server">
                <table>
                    <tr>
                        <th colspan="2">
                            <dx:ASPxTextBox ID="tb_deviceID" runat="server" NullText="Input Device ID/Name" Height="17px" Width="370px" OnTextChanged="tb_deviceID_TextChanged" AutoPostBack="true"
                                Theme="Moderno">
                            </dx:ASPxTextBox>
                        </th>
                        <td>
                            <dx:ASPxComboBox ID="cb_DeviceName" runat="server" Height="16px" Width="370px" NullText="Select a device" Theme="Moderno"
                                AutoPostBack="true" OnSelectedIndexChanged="cb_DeviceName_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <th>FROM</th>
                        <td>
                            <dx:ASPxDateEdit ID="de_from" runat="server" Theme="Moderno" AutoPostBack="true" OnDateChanged="DateChanged">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <th>TO</th>
                        <td>
                            <dx:ASPxDateEdit ID="de_to" runat="server" Theme="Moderno" AutoPostBack="true" OnDateChanged="DateChanged">
                            </dx:ASPxDateEdit>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnl_viewFrame" runat="server">
                <dx:ASPxGridView ID="gv_RecordView" runat="server" AutoGenerateColumns="False" Theme="Metropolis"
                    OnDataBound="gv_RecordView_DataBound"
                    OnInit="gv_RecordView_Init">
                    <Columns>
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
                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd-HH:mm">

                            </PropertiesDateEdit>
                            <HeaderStyle CssClass="gridViewHeader" />
                            
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataDateColumn Caption="TO" VisibleIndex="7" FieldName="Plan_To_ReDateTime">
                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd-HH:mm">
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
                                if (obj != null && obj == "OK") window.location.reload();
                            }

                            s.cpBookingId = null;
                            s.cpType = null;
                        }
                    }
                </script>
            </asp:Panel>

        </div>
    </form>
</body>
</html>
