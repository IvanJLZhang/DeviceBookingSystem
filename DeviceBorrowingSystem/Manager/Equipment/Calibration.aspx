<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calibration.aspx.cs" Inherits="Manager_Equipment_Calibration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <table>
                <tr>
                    <th colspan="2" align="left">Calibration History</th>
                </tr>
                <tr>
                    <th style="width: 150px"></th>
                    <td align="left">
                        <asp:GridView ID="gv_CalibrationHistory" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" DataSourceID="SqlDataSource5"
                            DataKeyNames="C_ID">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Calibration_Date" DataFormatString="{0:yyyy-MM-dd}" HeaderText="Calibration_Date" />
                                <asp:BoundField DataField="Calibration_Cost" HeaderText="Calibration_Cost" />
                                <asp:BoundField DataField="Create_Date" DataFormatString="{0:yyyy-MM-dd}" HeaderText="Create_Date" />
                                <asp:CommandField CancelText="CANCEL" DeleteText="DELETE" EditText="EDIT" ShowDeleteButton="True" ShowEditButton="True" UpdateText="UPDATE" />
                            </Columns>

                            <EditRowStyle BackColor="#2461BF" />

                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />

                            <RowStyle BackColor="#EFF3FB" />

                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                            <SortedAscendingCellStyle BackColor="#F5F7FB" />

                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />

                            <SortedDescendingCellStyle BackColor="#E9EBEF" />

                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>


                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" DeleteCommand="DELETE FROM [tbl_Calibration] WHERE [C_ID] = @C_ID" InsertCommand="INSERT INTO [tbl_Calibration] ([Device_ID], [C_ID], [Calibration_Date], [Calibration_Cost], [Create_Date]) VALUES (@Device_ID, @C_ID, @Calibration_Date, @Calibration_Cost, @Create_Date)" SelectCommand="SELECT [Device_ID], [C_ID], [Calibration_Date], [Calibration_Cost], [Create_Date] FROM [tbl_Calibration] WHERE ([Device_ID] = @Device_ID)" UpdateCommand="UPDATE [tbl_Calibration] SET [Calibration_Date] = @Calibration_Date, [Calibration_Cost] = @Calibration_Cost, [Create_Date] = GetDate() WHERE [C_ID] = @C_ID">
                            <DeleteParameters>
                                <asp:Parameter Name="C_ID" Type="String" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Device_ID" Type="String" />
                                <asp:Parameter Name="C_ID" Type="String" />
                                <asp:Parameter Name="Calibration_Date" Type="DateTime" />
                                <asp:Parameter Name="Calibration_Cost" Type="Double" />
                                <asp:Parameter Name="Create_Date" Type="DateTime" />
                            </InsertParameters>
                            <SelectParameters>
                                <asp:QueryStringParameter Name="Device_ID" QueryStringField="id" Type="String" />
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Calibration_Date" Type="DateTime" />
                                <asp:Parameter Name="Calibration_Cost" Type="Double" />
                                <asp:Parameter Name="C_ID" Type="String" />
                            </UpdateParameters>
                        </asp:SqlDataSource>


                        <hr />

                    </td>
                </tr>
                <tr>
                    <td style="width: 150px"></td>
                    <td>
                        <table align="left">
                            <tr>
                                <th align="left" class="auto-style3">Calibration Date:
                                </th>
                                <td align="left" class="auto-style5">
                                    <asp:TextBox ID="tb_Calibration" runat="server"></asp:TextBox>*
                                                <cc1:calendarextender id="CalendarExtender1" runat="server" targetcontrolid="tb_Calibration" enabled="True"></cc1:calendarextender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Calibration Date can not be null" ControlToValidate="tb_Calibration" ValidationGroup="AddCalibration">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="auto-style3">Calibration Cost:
                                </th>
                                <td align="left" class="auto-style5">
                                    <asp:TextBox ID="tb_CalibrationCost" runat="server">0</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="auto-style3">Reminding days:
                                </th>
                                <td align="left" class="auto-style5">
                                    <asp:TextBox ID="tb_RemindingDays" runat="server">1</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="auto-style3">Calibration Duration:
                                </th>
                                <td align="left" class="auto-style5">
                                    <asp:TextBox ID="tb_caliDuration" runat="server">3</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td align="right" class="auto-style5">
                                    <asp:ImageButton ID="ibtn_AddCalibration" runat="server" ImageUrl="~/Images/add.png" Width="22px" Height="20px" ToolTip="Add calibration Record" OnClick="ibtn_AddCalibration_Click" ValidationGroup="AddCalibration" />
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddCalibration" ShowSummary="False" ShowMessageBox="True" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th colspan="2" align="left">Floating Price</th>
                </tr>
                <tr>
                    <td style="width: 150px"></td>
                    <td align="left">
                        <asp:GridView ID="gv_FloatingPrice" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="id" DataSourceID="SqlDataSource3" ForeColor="#333333" GridLines="None"
                            OnRowDataBound="gv_FloatingPrice_RowDataBound">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" />
                                <asp:BoundField DataField="Inside_cost" HeaderText="Inside_cost" SortExpression="Inside_cost" />
                                <asp:BoundField DataField="Outside_cost" HeaderText="Outside_cost" SortExpression="Outside_cost" />
                                <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" />
                                <asp:CommandField CancelText="CANCEL" DeleteText="DELETE" EditText="EDIT" ShowDeleteButton="True" ShowEditButton="True" UpdateText="UPDATE" />
                            </Columns>

                            <EditRowStyle BackColor="#2461BF" />

                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />

                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />

                            <RowStyle BackColor="#EFF3FB" />

                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                            <SortedAscendingCellStyle BackColor="#F5F7FB" />

                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />

                            <SortedDescendingCellStyle BackColor="#E9EBEF" />

                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT * FROM [tbl_FloatingPrice] WHERE ([Device_ID] = @Device_ID) ORDER BY [Year]" UpdateCommand="UPDATE tbl_FloatingPrice SET Year = @Year, Device_ID = @Device_ID, Inside_cost = @Inside_cost, Note = @Note, Outside_cost = @Outside_cost" DeleteCommand="DELETE FROM tbl_FloatingPrice WHERE (id = @ID)">
                            <DeleteParameters>
                                <asp:Parameter Name="ID" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:QueryStringParameter Name="Device_ID" QueryStringField="id" Type="String" />
                            </SelectParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Year" />
                                <asp:Parameter Name="Device_ID" />
                                <asp:Parameter Name="Inside_cost" />
                                <asp:Parameter Name="Note" />
                                <asp:Parameter Name="Outside_cost" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <table align="left">
                            <tr align="left">
                                <th align="left">Year: 
                                </th>
                                <td>
                                    <asp:TextBox ID="tb_Year" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <th align="left">Inside Cost: 
                                </th>
                                <td>
                                    <asp:TextBox ID="tb_insidecost" runat="server">0</asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <th align="left">Outside Cost: 
                                </th>
                                <td>
                                    <asp:TextBox ID="tb_outsideCost" runat="server">0</asp:TextBox>
                                </td>
                            </tr>
                            <tr align="left">
                                <th align="left">Note: 
                                </th>
                                <td>
                                    <asp:TextBox ID="tb_Note" runat="server" TextMode="MultiLine" Width="215px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style3"></td>
                                <td align="right">
                                    <asp:ImageButton ID="ibtn_AddFloatingPrice" runat="server" ImageUrl="~/Images/add.png" Width="22px" Height="20px" ToolTip="Add calibration Record" OnClick="ibtn_AddFloatingPrice_Click" ValidationGroup="AddFloatingPrice" />
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddFloatingPrice" ShowSummary="False" ShowMessageBox="True" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
