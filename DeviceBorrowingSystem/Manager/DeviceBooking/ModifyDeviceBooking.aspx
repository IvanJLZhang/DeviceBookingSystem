<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModifyDeviceBooking.aspx.cs" Inherits="Manager_DeviceBooking_ModifyDeviceBooking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title></title>
    <script type="text/javascript">
        function x() {
            var dl = document.getElementById('dl_FillBorrowingInfo');
            alert(dl.children.length);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <table>
                <tr>
                    <th colspan="2">Borrowing Information: </th>
                </tr>
                <tr>
                    <td align="left" style="width: 50%">
                        <asp:LinkButton ID="lbtn_DeviceName" runat="server" OnClick="lbtn_DeviceName_Click">aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa</asp:LinkButton></td>
                    <td align="right" style="width: 50%">
                        <asp:Button ID="btn_Approve" runat="server" Text="Approve" OnClick="btn_Approve_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" OnClientClick="javascript: window.close();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:DataList ID="dl_FillBorrowingInfo" runat="server" RepeatDirection="Horizontal" RepeatColumns="1"
                            OnItemCreated="dl_FillBorrowingInfo_ItemCreated">
                            <ItemTemplate>
                                <table>
                                    <tr align="left">
                                        <th>Start DateTime: </th>
                                        <td>
                                            <%--<asp:Label ID="lbl_startDT" runat="server" ToolTip="Loan DateTime" Text='<%# DateTime.Parse( DataBinder.Eval(Container.DataItem, "Loan_DateTime").ToString()).ToString("yyyy/MM/dd HH:mm") %>'></asp:Label>--%>
                                            <asp:TextBox ID="tb_startDT" runat="server" Text='<%# DateTime.Parse( DataBinder.Eval(Container.DataItem, "Loan_DateTime").ToString()).ToString("yyyy/MM/dd") %>'></asp:TextBox>
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

                                        <th>End Date Time: </th>
                                        <td>
                                            <asp:TextBox ID="tb_EndDate" runat="server" ToolTip="Plan to return DateTime" AutoPostBack="true" OnTextChanged="tb_EndDate_TextChanged" Text='<%# DateTime.Parse( DataBinder.Eval(Container.DataItem, "Plan_To_ReDateTime").ToString()).ToString("yyyy/MM/dd") %>'></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tb_EndDate" Format="yyyy/MM/dd"></cc1:CalendarExtender>
                                            <%--<cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="btn_EndDate" WatermarkText="Plan to return datetime"></cc1:TextBoxWatermarkExtender>--%>
                                            <asp:DropDownList ID="ddl_EndTime" runat="server" OnSelectedIndexChanged="ddl_EndTime_SelectedIndexChanged" AutoPostBack="true">
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
                                            <asp:ImageButton ID="ibtn_addTimeDuration" runat="server" ImageUrl="~/Images/addGrey.png" />
                                            <asp:HiddenField ID="hf_endDateTime" runat="server" Value='<%# DateTime.Parse( DataBinder.Eval(Container.DataItem, "Plan_To_ReDateTime").ToString()).ToString("yyyy/MM/dd HH:mm") %>' />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <th>Project Name: </th>
                                        <td>
                                            <asp:DropDownList ID="ddl_Project" runat="server" AutoPostBack="True" ToolTip="Project" DataSourceID="SqlDataSource1" DataTextField="PJ_Name" DataValueField="PJ_Code"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [PJ_Code], [PJ_Name], [Cust_Name] FROM [tbl_Project]"></asp:SqlDataSource>
                                        </td>
                                        <th>Cust Name: </th>
                                        <td>
                                            <asp:DropDownList ID="ddl_CustName" runat="server" ToolTip="Cust Name" DataSourceID="SqlDataSource2" DataTextField="Cust_Name" DataValueField="PJ_Code"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [Cust_Name], [PJ_Code] FROM [tbl_Project] WHERE ([PJ_Code] = @PJ_Code)">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddl_Project" Name="PJ_Code" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <th>Test Category: </th>
                                        <td>
                                            <asp:DropDownList ID="ddl_TestCategory" runat="server" ToolTip="Test Category" DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [ID], [Name] FROM [tbl_TestCategory]"></asp:SqlDataSource>
                                        </td>
                                        <th>Project Stage: </th>
                                        <td>
                                            <asp:TextBox ID="tb_PjStage" runat="server" ToolTip="Project Stage" Text='<%# Bind("PJ_Stage") %>'></asp:TextBox></td>
                                    </tr>
                                    <tr align="left">
                                        <th>Loaner Department: </th>
                                        <td>
                                            <asp:DropDownList ID="ddl_loanerDpt" runat="server" AutoPostBack="true" DataSourceID="SqlDataSource4" DataTextField="P_Department" DataValueField="P_Department"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT DISTINCT P_Department FROM tbl_Person WHERE (P_Department IS NOT NULL) AND (P_Department &lt;&gt; '') AND (P_Activate = '1') ORDER BY P_Department"></asp:SqlDataSource>
                                        </td>
                                        <th>Loaner Name</th>
                                        <td>
                                            <asp:DropDownList ID="ddl_LoanerName" runat="server" DataSourceID="SqlDataSource5" DataTextField="P_Name" DataValueField="P_ID"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [P_ID], [P_Name] FROM [tbl_Person] WHERE ([P_Department] = @P_Department)">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddl_loanerDpt" Name="P_Department" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
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
                                            <br />
                                        </th>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hf_endDateTime" runat="server" />
        </div>
    </form>
</body>
</html>
