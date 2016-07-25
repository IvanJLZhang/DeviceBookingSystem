<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomizeRecord.aspx.cs" Inherits="Manager_BookingRecord_CustomizeRecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="Stylesheet" type="text/css" href="../../CSS/Manager_ExportBookingRecord.css" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm_main" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd_main" runat="server">
            <Triggers>
               <asp:PostBackTrigger ControlID="dl_Command" />
            </Triggers>
            <ContentTemplate>
                <div>
                    <table width="80%">
                        <tr>
                            <td colspan="2">Customize Filter
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" Text="Device Category" CheckState="Checked" Enabled="false" Theme="Moderno"></dx:ASPxCheckBox>
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="cb_category" runat="server" SelectedIndex="0" AutoPostBack="true" OnSelectedIndexChanged="cb_category_SelectedIndexChanged" Theme="Moderno">
                                    <Items>
                                        <dx:ListEditItem Selected="True" Text="Device" Value="1" />
                                        <dx:ListEditItem Text="Equipment" Value="2" />
                                        <dx:ListEditItem Text="Chamber" Value="3" />
                                    </Items>
                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="category can not be null"></ValidationSettings>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxCheckBox ID="chb_device" ClientInstanceName="chb_device" runat="server" Text="Device Filters" AutoPostBack="true" OnCheckedChanged="chb_CheckedChanged" Theme="Moderno"></dx:ASPxCheckBox>
                            </td>
                            <td>
                                <span style="float: left; margin-right: 20px">
                                    <dx:ASPxComboBox ID="cb_site" ClientInstanceName="cb_site" runat="server" ValueType="System.String" NullText="SITE" Enabled="false" Theme="Moderno"></dx:ASPxComboBox>
                                </span>
                                <dx:ASPxComboBox ID="cb_dpt" ClientInstanceName="cb_dpt" runat="server" ValueType="System.String" NullText="DEPARTMENT" Enabled="false" Theme="Moderno"></dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxCheckBox ID="chb_record" runat="server" Text="Borrow Filters" AutoPostBack="true" OnCheckedChanged="chb_CheckedChanged" Theme="Moderno"></dx:ASPxCheckBox>
                            </td>
                            <td>
                                <span style="float: left; margin-right: 20px">
                                    <dx:ASPxComboBox ID="cb_project" runat="server" ValueType="System.String" NullText="PROJECT" Enabled="false" Theme="Moderno"></dx:ASPxComboBox>
                                </span>
                                <dx:ASPxComboBox ID="cb_test_category" runat="server" ValueType="System.String" NullText="TEST CATEGORY" Enabled="false" Theme="Moderno"></dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dx:ASPxCheckBox ID="chb_duration" runat="server" Text="Duration Filters" AutoPostBack="true" OnCheckedChanged="chb_CheckedChanged" Theme="Moderno"></dx:ASPxCheckBox>
                            </td>
                            <td>
                                <span style="float: left; margin-right: 20px">
                                    <dx:ASPxComboBox ID="cb_year" runat="server" ValueType="System.String" NullText="YEAR" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="cb_year_SelectedIndexChanged" Theme="Moderno"></dx:ASPxComboBox>
                                </span>

                                <span style="float: left; margin-right: 20px">
                                    <dx:ASPxDateEdit ID="de_Start" runat="server" NullText="FROM" Enabled="false" Theme="Moderno">
                                    </dx:ASPxDateEdit>
                                </span>

                                <dx:ASPxDateEdit ID="de_End" runat="server" NullText="TO" Enabled="false" Theme="Moderno">
                                </dx:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span style="float: left; margin-right: 20px">
                                    <dx:ASPxButton ID="btn_apply" runat="server" Text="APPLY" OnClick="btn_apply_Click" Theme="Moderno"></dx:ASPxButton>
                                </span>
                                <dx:ASPxButton ID="btn_Clear" runat="server" Text="CLEAR" OnClick="btn_Clear_Click" Theme="Moderno"></dx:ASPxButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span>Operation</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:DataList ID="dl_Command" runat="server" RepeatColumns="1" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                                     OnItemCommand="dl_Command_ItemCommand">
                                    <AlternatingItemStyle BackColor="#CCCCCC" />
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtn_command" runat="server" Text='<%# Bind("CommandText") %>' CommandName='<%# Bind("CommandName") %>'></asp:LinkButton>
                                        <asp:ImageButton ID="ibtn_command" runat="server" ImageUrl="~/Images/excel-file.gif" CommandName='<%# Bind("CommandName") %>' ToolTip='<%# Bind("CommandText") %>' />
                                    </ItemTemplate>
                                    <SelectedItemStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                </asp:DataList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxGridView ID="gv_RecordView" runat="server" Caption="Record Preview" AutoGenerateColumns="False" Theme="Metropolis" Visible="false"
                                    OnCustomButtonCallback="gv_RecordView_CustomButtonCallback"
                                    OnCustomButtonInitialize="gv_RecordView_CustomButtonInitialize">
                                    <Columns>
                                        <%--                    <dx:GridViewDataTextColumn Caption="Booking ID" VisibleIndex="0" FieldName="Booking_ID">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>--%>
                                        <dx:GridViewDataTextColumn Caption="Device" VisibleIndex="1" FieldName="Device_Name">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Loaner" VisibleIndex="3" FieldName="LoanerName">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Custom ID" VisibleIndex="5" FieldName="Custom_ID">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Project" VisibleIndex="6" FieldName="PJ_Name">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Test Category" VisibleIndex="7" FieldName="TestCategory">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataComboBoxColumn Caption="Status" VisibleIndex="15" FieldName="Status">
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
                                        <dx:GridViewDataDateColumn Caption="From" VisibleIndex="9" FieldName="Loan_DateTime">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataDateColumn Caption="TO" VisibleIndex="12" FieldName="Plan_To_ReDateTime">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataDateColumn Caption="Return" VisibleIndex="14" FieldName="Real_ReDateTime">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Caption="Device Dpt." ToolTip="Owner Department" VisibleIndex="2" FieldName="Owner_Dpt">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Loaner Dpt." ToolTip="Loaner Department" VisibleIndex="4" FieldName="Loaner_Dpt">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                        </dx:GridViewDataTextColumn>
                                        <%--                                        <dx:GridViewCommandColumn VisibleIndex="0" ShowDeleteButton="true" ButtonType="Image" Caption="Command">
                                            <HeaderStyle CssClass="gridViewHeader" />
                                            <DeleteButton Image-Url="../../Images/Delete.png" Image-Width="15px" Image-Height="15px"></DeleteButton>
                                            <CustomButtons>
                                                <dx:GridViewCommandColumnCustomButton ID="btn_view" Image-ToolTip="view record detail" Image-Url="../../Images/application_windows_grow.png" Image-Width="15px" Image-Height="15px"></dx:GridViewCommandColumnCustomButton>
                                                <dx:GridViewCommandColumnCustomButton ID="btn_return" Image-ToolTip="set status to return" Image-Url="../../Images/return.png" Image-Width="15px" Image-Height="15px"></dx:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
                                        </dx:GridViewCommandColumn>--%>
                                    </Columns>
                                    <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                                    <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
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
                                                window.open('../DeviceBooking/ApprovePage_View.aspx?bookingId=' + intID, '', 'width=760px, scrollbars=yes, left=100px');
                                            }
                                            else if (s.cpType == 'btn_edit') {
                                                var obj = window.showModalDialog('../DeviceBooking/ModifyDeviceBooking.aspx?id=' + intID, '', 'dialogWidth=750px; resizable=yes');
                                                if (obj != null && obj == 'OK') { window.location.href = window.location.href; }
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
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
