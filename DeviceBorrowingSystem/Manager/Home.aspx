<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Manager_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="Stylesheet" type="text/css" href="../CSS/Manager_Home.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upd_main" runat="server">
        <ContentTemplate>

    <asp:Panel ID="p_Navigate" runat="server">
        <table style="width: 100%">
            <tr>
                <td align="left">Category: 
                            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                <asp:ListItem Text="Device" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Equipment" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Chamber" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp; Department:
                            <asp:DropDownList ID="ddl_dpt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_dpt_SelectedIndexChanged">
                            </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Panel ID="p_Opera" runat="server" HorizontalAlign="Right">
                        Booking ID:
                    <asp:TextBox ID="tb_bookingId" runat="server" Width="150px" Height="20px"></asp:TextBox>
                        <asp:ImageButton ID="imgbtn_Search" runat="server" ImageUrl="~/Images/search.png" Height="20px" Width="22px" ToolTip="Search" OnClick="btn_search_Click" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <hr />
    <br />
    <div style="width: 100%; height: 520px; text-align: center" align="center">
        <div id="divAdditionQuery" align="center">
            <div class="ApproveOrRejectCommand">
                <asp:Button ID="btn_Approved" runat="server" Text="Approve" OnClick="btn_Approved_Click" CssClass="buttonStyle" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_Reject" runat="server" Text="Reject" OnClick="btn_Reject_Click" CssClass="buttonStyle" />
            </div>
            <div style="margin-top: 10px">
                <script language="javascript" type="text/javascript">
                    function goToApprovePage(s, e) {
                        if (s.cpBookingId != null && s.cpType != null) {
                            var intID = s.cpBookingId;
                            if (s.cpType == 'btn_approve') {
                                window.open('./DeviceBooking/ApprovePage.aspx?bookingId=' + intID, '', 'width=760px, scrollbars=yes, left=100px');
                            }
                            else if (s.cpType == 'btn_edit') {
                                var obj = window.showModalDialog('./DeviceBooking/ModifyDeviceBooking.aspx?id=' + intID, '', 'dialogWidth=750px; resizable=yes');
                                if (obj != null && obj == 'OK') { window.location.href = window.location.href; }
                            }
                        }
                    }
                </script>
                <dx:ASPxGridView ID="gv_requestView" runat="server" AutoGenerateColumns="False" OnRowCommand="gv_requestView_RowCommand"
                    OnCustomButtonCallback="gv_requestView_CustomButtonCallback"
                    ClientSideEvents-EndCallback="function(s, e){goToApprovePage(s, e);}"
                    OnDataBound="GridView1_DataBound" Theme="Moderno">
                    <Settings ShowFilterBar="Visible" />
                    <SettingsBehavior AllowFocusedRow="true" />
                    <Columns>
                        <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="True" VisibleIndex="0">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataHyperLinkColumn Caption="Booking ID" VisibleIndex="1" FieldName="Booking_ID">
                            <HeaderStyle CssClass="gridViewHeader" />
                            <DataItemTemplate>
                                <asp:LinkButton ID="lbtn_Approve" runat="server" Text='<%# Bind("Booking_ID") %>' CommandName="Approve" CommandArgument='<%# Bind("Booking_ID") %>'></asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataHyperLinkColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Custom ID" FieldName="Custom_ID">
                            <HeaderStyle CssClass="gridViewHeader" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataHyperLinkColumn Caption="Device" VisibleIndex="3" FieldName="Device_Name" Width="300px">
                            <HeaderStyle CssClass="gridViewHeader" />
                            <DataItemTemplate>
                                <asp:LinkButton ID="lbtn_DeviceDetail" runat="server" Text='<%# Bind("Device_Name") %>' CommandName="DeviceDetail" CommandArgument='<%# Bind("Device_ID") %>'></asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataHyperLinkColumn>

                        <dx:GridViewDataHyperLinkColumn Caption="Loaner" VisibleIndex="4" FieldName="Loaner_Name">
                            <HeaderStyle CssClass="gridViewHeader" />
                            <DataItemTemplate>
                                <asp:LinkButton ID="lbtn_DeviceDetail" runat="server" Text='<%# Bind("Loaner_Name") %>' CommandName="UserDetail" CommandArgument='<%# Bind("Loaner_ID") %>'></asp:LinkButton>
                            </DataItemTemplate>
                        </dx:GridViewDataHyperLinkColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="5" Caption="Start" FieldName="Loan_DateTime">
                            <HeaderStyle CssClass="gridViewHeader" />
                            <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd HH:mm"></PropertiesTextEdit>
                            <Settings AllowSort="True" AllowAutoFilter="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn VisibleIndex="6" Caption="End" FieldName="Plan_To_ReDateTime">
                            <HeaderStyle CssClass="gridViewHeader" />
                            <PropertiesTextEdit DisplayFormatString="yyyy/MM/dd HH:mm"></PropertiesTextEdit>
                            <Settings AllowSort="True" AllowAutoFilter="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewCommandColumn ButtonType="Image" VisibleIndex="7" Caption="Command">
                            <CellStyle CssClass="gridViewCommandButton"></CellStyle>
                            <CustomButtons>
                                <dx:GridViewCommandColumnCustomButton ID="btn_edit">
                                    <Image Url="~/Images/Edit.png" Height="20px" ToolTip="Edit" Width="20px">
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                                <dx:GridViewCommandColumnCustomButton ID="btn_approve">
                                    <Image Url="~/Images/application_windows_grow.png" Height="20px" ToolTip="Approve/Reject" Width="20px">
                                        <SpriteProperties Left="3px" />
                                    </Image>
                                </dx:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                        </dx:GridViewCommandColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" />
                    <Styles>
                        <CommandColumn Spacing="2px"></CommandColumn>
                    </Styles>
                </dx:ASPxGridView>
            </div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, tbl_DeviceBooking.Device_ID, tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Comment, tbl_Person.P_Name, tbl_Person.P_ID, tbl_Device.Device_Name, tbl_Project.PJ_Name FROM tbl_DeviceBooking INNER JOIN tbl_Device ON tbl_DeviceBooking.Device_ID = tbl_Device.Device_ID INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code WHERE (tbl_DeviceBooking.Status = 1) AND (tbl_Device.Device_category = @Category)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList1" Name="Category" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

