<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddRecord.aspx.cs" Inherits="Manager_DeviceBooking_AddRecord" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <title>Add Record</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <table align="center">
                        <caption>
                            <h2>Add Record</h2>
                        </caption>
                        <tr>
                            <th colspan="2">
                                <dx:ASPxTextBox ID="tb_deviceID" runat="server" NullText="Input Device ID/Name" Height="17px" Width="370px" OnTextChanged="tb_deviceID_TextChanged" AutoPostBack="true"></dx:ASPxTextBox>
                            </th>
                            <td colspan="2">
                                <span style="float: left; position:static">
                                    <dx:ASPxComboBox ID="cb_DeviceName" runat="server" Height="16px" Width="370px" NullText="Select a device"></dx:ASPxComboBox>
                                </span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="You need select a device" ControlToValidate="cb_DeviceName" ValidationGroup="check">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr align="left">
                            <th>FROM: </th>
                            <td>
                                <%--<asp:Label ID="lbl_startDT" runat="server" ToolTip="Loan DateTime" Text='<%# DateTime.Parse( DataBinder.Eval(Container.DataItem, "Loan_DateTime").ToString()).ToString("yyyy/MM/dd HH:mm") %>'></asp:Label>--%>
                                <asp:TextBox ID="tb_startDT" runat="server" Text=''></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="You need set a Start Date!" ControlToValidate="tb_startDT" ValidationGroup="check">*</asp:RequiredFieldValidator>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tb_startDT" Format="yyyy/MM/dd"></cc1:CalendarExtender>
                                <asp:DropDownList ID="ddl_startTime" runat="server">
                                    <asp:ListItem>00:00</asp:ListItem>
                                    <asp:ListItem>00:30</asp:ListItem>
                                    <asp:ListItem>01:00</asp:ListItem>
                                    <asp:ListItem>01:30</asp:ListItem>
                                    <asp:ListItem>02:00</asp:ListItem>
                                    <asp:ListItem>02:30</asp:ListItem>
                                    <asp:ListItem>03:00</asp:ListItem>
                                    <asp:ListItem>03:30</asp:ListItem>
                                    <asp:ListItem>04:00</asp:ListItem>
                                    <asp:ListItem>04:30</asp:ListItem>
                                    <asp:ListItem>05:00</asp:ListItem>
                                    <asp:ListItem>06:30</asp:ListItem>
                                    <asp:ListItem>07:00</asp:ListItem>
                                    <asp:ListItem>07:30</asp:ListItem>
                                    <asp:ListItem>08:00</asp:ListItem>
                                    <asp:ListItem>08:30</asp:ListItem>
                                    <asp:ListItem>09:00</asp:ListItem>
                                    <asp:ListItem>09:30</asp:ListItem>
                                    <asp:ListItem>10:00</asp:ListItem>
                                    <asp:ListItem>10:30</asp:ListItem>
                                    <asp:ListItem>11:00</asp:ListItem>
                                    <asp:ListItem>11:30</asp:ListItem>
                                    <asp:ListItem>12:00</asp:ListItem>
                                    <asp:ListItem>12:30</asp:ListItem>
                                    <asp:ListItem>13:00</asp:ListItem>
                                    <asp:ListItem>13:30</asp:ListItem>
                                    <asp:ListItem>14:00</asp:ListItem>
                                    <asp:ListItem>14:30</asp:ListItem>
                                    <asp:ListItem>15:00</asp:ListItem>
                                    <asp:ListItem>15:30</asp:ListItem>
                                    <asp:ListItem>16:00</asp:ListItem>
                                    <asp:ListItem>16:30</asp:ListItem>
                                    <asp:ListItem>17:00</asp:ListItem>
                                    <asp:ListItem>17:30</asp:ListItem>
                                    <asp:ListItem>18:00</asp:ListItem>
                                    <asp:ListItem>18:30</asp:ListItem>
                                    <asp:ListItem>19:00</asp:ListItem>
                                    <asp:ListItem>19:30</asp:ListItem>
                                    <asp:ListItem>20:00</asp:ListItem>
                                    <asp:ListItem>20:30</asp:ListItem>
                                    <asp:ListItem>21:00</asp:ListItem>
                                    <asp:ListItem>21:30</asp:ListItem>
                                    <asp:ListItem>22:00</asp:ListItem>
                                    <asp:ListItem>22:30</asp:ListItem>
                                    <asp:ListItem>23:00</asp:ListItem>
                                    <asp:ListItem>23:30</asp:ListItem>
                                </asp:DropDownList>
                            </td>

                            <th>TO:</th>
                            <td>
                                <asp:TextBox ID="tb_EndDate" runat="server" ToolTip="Plan to return DateTime" AutoPostBack="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="You need set a end date!" ControlToValidate="tb_EndDate" ValidationGroup="check">*</asp:RequiredFieldValidator>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tb_EndDate" Format="yyyy/MM/dd"></cc1:CalendarExtender>
                                <%--<cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="btn_EndDate" WatermarkText="Plan to return datetime"></cc1:TextBoxWatermarkExtender>--%>
                                <asp:DropDownList ID="ddl_EndTime" runat="server">
                                    <asp:ListItem>00:00</asp:ListItem>
                                    <asp:ListItem>00:30</asp:ListItem>
                                    <asp:ListItem>01:00</asp:ListItem>
                                    <asp:ListItem>01:30</asp:ListItem>
                                    <asp:ListItem>02:00</asp:ListItem>
                                    <asp:ListItem>02:30</asp:ListItem>
                                    <asp:ListItem>03:00</asp:ListItem>
                                    <asp:ListItem>03:30</asp:ListItem>
                                    <asp:ListItem>04:00</asp:ListItem>
                                    <asp:ListItem>04:30</asp:ListItem>
                                    <asp:ListItem>05:00</asp:ListItem>
                                    <asp:ListItem>06:30</asp:ListItem>
                                    <asp:ListItem>07:00</asp:ListItem>
                                    <asp:ListItem>07:30</asp:ListItem>
                                    <asp:ListItem>08:00</asp:ListItem>
                                    <asp:ListItem>08:30</asp:ListItem>
                                    <asp:ListItem>09:00</asp:ListItem>
                                    <asp:ListItem>09:30</asp:ListItem>
                                    <asp:ListItem>10:00</asp:ListItem>
                                    <asp:ListItem>10:30</asp:ListItem>
                                    <asp:ListItem>11:00</asp:ListItem>
                                    <asp:ListItem>11:30</asp:ListItem>
                                    <asp:ListItem>12:00</asp:ListItem>
                                    <asp:ListItem>12:30</asp:ListItem>
                                    <asp:ListItem>13:00</asp:ListItem>
                                    <asp:ListItem>13:30</asp:ListItem>
                                    <asp:ListItem>14:00</asp:ListItem>
                                    <asp:ListItem>14:30</asp:ListItem>
                                    <asp:ListItem>15:00</asp:ListItem>
                                    <asp:ListItem>15:30</asp:ListItem>
                                    <asp:ListItem>16:00</asp:ListItem>
                                    <asp:ListItem>16:30</asp:ListItem>
                                    <asp:ListItem>17:00</asp:ListItem>
                                    <asp:ListItem>17:30</asp:ListItem>
                                    <asp:ListItem>18:00</asp:ListItem>
                                    <asp:ListItem>18:30</asp:ListItem>
                                    <asp:ListItem>19:00</asp:ListItem>
                                    <asp:ListItem>19:30</asp:ListItem>
                                    <asp:ListItem>20:00</asp:ListItem>
                                    <asp:ListItem>20:30</asp:ListItem>
                                    <asp:ListItem>21:00</asp:ListItem>
                                    <asp:ListItem>21:30</asp:ListItem>
                                    <asp:ListItem>22:00</asp:ListItem>
                                    <asp:ListItem>22:30</asp:ListItem>
                                    <asp:ListItem>23:00</asp:ListItem>
                                    <asp:ListItem>23:30</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="left">
                            <th>Project: </th>
                            <td>
                                <span style="float:left;position:static"><dx:ASPxComboBox ID="ddl_Project" runat="server" AutoPostBack="true" DataSourceID="SqlDataSource1" TextField="PJ_Name" ValueField="PJ_Code" OnSelectedIndexChanged="ddl_Project_SelectedIndexChanged"></dx:ASPxComboBox></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="You need select project!" ControlToValidate="ddl_Project" ValidationGroup="check">*</asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [PJ_Code], [PJ_Name], [Cust_Name] FROM [tbl_Project]"></asp:SqlDataSource>
                            </td>
                            <th>Cust Name: </th>
                            <td>
                                <span style="float: left; position:static"><dx:ASPxTextBox ID="tb_CustName" runat="server" Enabled="false"></dx:ASPxTextBox></span>
                                <%--                                <asp:DropDownList ID="ddl_CustName" runat="server" ToolTip="Cust Name" DataSourceID="SqlDataSource2" DataTextField="Cust_Name" DataValueField="PJ_Code"></asp:DropDownList>--%>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [Cust_Name], [PJ_Code] FROM [tbl_Project] WHERE ([PJ_Code] = @PJ_Code)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddl_Project" Name="PJ_Code" PropertyName="Value" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr align="left">
                            <th>Test Category: </th>
                            <td>
                                <span style="float: left; position:static"><dx:ASPxComboBox ID="cb_TestCategory" runat="server" DataSourceID="SqlDatasource3" TextField="Name" ValueField="ID"></dx:ASPxComboBox></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="You need select test category" ControlToValidate="cb_TestCategory" ValidationGroup="check">*</asp:RequiredFieldValidator>
                                <%-- <asp:DropDownList ID="ddl_TestCategory" runat="server" ToolTip="Test Category" DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="ID"></asp:DropDownList>--%>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [ID], [Name] FROM [tbl_TestCategory]"></asp:SqlDataSource>
                            </td>
                            <th>Project Stage: </th>
                            <td>
                                <asp:TextBox ID="tb_PjStage" runat="server" ToolTip="Project Stage" Text='<%# Bind("PJ_Stage") %>' ReadOnly="true"></asp:TextBox></td>
                        </tr>
                        <tr align="left">
                            <th>Loaner Department: </th>
                            <td>
                                <script type="text/javascript">
                                    function BeginCallBack(s, e) {
                                        var cb_Loaner = window.document.getElementsByName("cb_Loaner");
                                        cb_Loaner.value = "";
                                    }
                                </script>
                                <span style="float: left; position:static"><dx:ASPxComboBox ID="ddl_loanerDpt" runat="server" AutoPostBack="true" DataSourceID="SqlDataSource4" TextField="P_Department" ValueField="P_Department" OnCallback="ddl_loanerDpt_Callback" OnSelectedIndexChanged="ddl_loanerDpt_SelectedIndexChanged">
                                    <ClientSideEvents BeginCallback="function(s,e){BeginCallBack(s,e);}" />
                                </dx:ASPxComboBox>
                                    </span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="You need select a department first!" ControlToValidate="ddl_loanerDpt" ValidationGroup="check">*</asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT DISTINCT P_Department FROM tbl_Person WHERE (P_Department IS NOT NULL) AND (P_Department &lt;&gt; '') AND (P_Activate = '1') ORDER BY P_Department"></asp:SqlDataSource>
                            </td>
                            <th>Loaner</th>
                            <td>
                                <span style="float: left; position:static"><dx:ASPxComboBox ID="ddl_LoanerName" ClientInstanceName="cb_Loaner" runat="server" DataSourceID="SqlDataSource5" TextField="P_Name" ValueField="P_ID" SelectedIndex="-1"></dx:ASPxComboBox></span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="You need select loaner" ControlToValidate="ddl_LoanerName" ValidationGroup="check">*</asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [P_ID], [P_Name] FROM [tbl_Person] WHERE ([P_Department] = @P_Department)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddl_loanerDpt" Name="P_Department" PropertyName="Value" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr align="left">
                            <th>Set Status</th>
                            <td>
                                <dx:ASPxComboBox ID="cb_Status" runat="server">
                                    <Items>
                                        <%--<dx:ListEditItem Text="NEW_SUBMIT" Value="1" />--%>
                                        <dx:ListEditItem Text="APPROVE_NORETURN" Value="2" />
                                        <dx:ListEditItem Text="RETURN" Value="3" Selected="true" />
                                        <%--<dx:ListEditItem Text="REJECT" Value="-1" />--%>
                                    </Items>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr align="left">
                            <th>Comment: </th>
                            <td colspan="3">
                                <asp:TextBox ID="tb_Comment" runat="server" TextMode="MultiLine" Width="100%" Height="100px" Text='<%# Bind("Comment") %>'></asp:TextBox></td>
                        </tr>
                        <tr align="left">
                            <th>Reviewer Comment: </th>
                            <td colspan="3">
                                <asp:TextBox ID="tb_ReviewComment" runat="server" TextMode="MultiLine" Width="100%" Height="100px" Text='<%# Bind("Review_Comment") %>'></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th colspan="4">
                                <br />
                                <hr />
                            </th>
                        </tr>
                    </table>
                    <table align="center" style="width: 740px">
                        <tr>
                            <td>
                                <asp:Button ID="btn_new" runat="server" Text="New Record" OnClick="btn_new_Click" ValidationGroup="check"></asp:Button>

                            </td>
                            <td>
                                <asp:Button ID="btn_submit" runat="server" Text="Approve" OnClick="btn_submit_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="check" />
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxGridView ID="gv_records" runat="server" AutoGenerateColumns="False" OnRowDeleting="gv_records_RowDeleting">
                                    <Columns>
                                        <dx:GridViewDataDateColumn FieldName="Device Name" VisibleIndex="1" Caption="Device">
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataDateColumn VisibleIndex="4" Caption="FROM" FieldName="Loan_DateTime">
                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataDateColumn VisibleIndex="5" Caption="TO" FieldName="Plan_To_ReDateTime">
                                            <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                                            </PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewCommandColumn ShowDeleteButton="True" VisibleIndex="6" ButtonType="Image">
                                            <DeleteButton Image-Url="../../Images/Delete.png">
                                                <Image Url="../../Images/Delete.png">
                                                </Image>
                                            </DeleteButton>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataDateColumn Caption="Booking ID" FieldName="Booking_ID" VisibleIndex="0">
                                        </dx:GridViewDataDateColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
