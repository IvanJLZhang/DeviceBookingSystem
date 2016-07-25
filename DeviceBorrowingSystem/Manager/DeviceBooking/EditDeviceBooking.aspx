<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditDeviceBooking.aspx.cs" Inherits="Manager_DeviceBooking_EditDeviceBooking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Modify Device Booking
    </title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div align="center">
            <table>
                <tr>
                    <th>Borrowing Information: </th>
                </tr>
                <tr>
                    <td align="left">
                        <asp:LinkButton ID="lbtn_DeviceName" runat="server" OnClick="lbtn_DeviceName_Click">aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa</asp:LinkButton></td>
                </tr>
            </table>
            <hr />
            <asp:Table ID="tbl_BorrowingInfo" runat="server">
                <asp:TableRow runat="server">
                    <asp:TableHeaderCell HorizontalAlign="Left">
                        Start DateTime: 
                    </asp:TableHeaderCell>
                    <asp:TableCell runat="server" HorizontalAlign="Left">
                        <asp:Label ID="lbl_startDT" runat="server" ToolTip="Loan DateTime">2015/02/13 10:00</asp:Label>
                    </asp:TableCell>
                    <asp:TableHeaderCell HorizontalAlign="Left">End Date Time: </asp:TableHeaderCell>
                    <asp:TableCell>
                        <asp:TextBox ID="tb_EndDate" runat="server" ToolTip="Plan to return DateTime" AutoPostBack="true" OnTextChanged="tb_EndDate_TextChanged"></asp:TextBox>
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
                        <asp:HiddenField ID="hf_endDateTime" runat="server" />
                        <asp:HiddenField ID="hf_newTableID" runat="server" Value="" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell HorizontalAlign="Left">Project Name: </asp:TableHeaderCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:DropDownList ID="ddl_Project" runat="server" AutoPostBack="true" ToolTip="Project" DataSourceID="SqlDataSource1" DataTextField="PJ_Name" DataValueField="PJ_Code"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableHeaderCell HorizontalAlign="Left">Cust Name: </asp:TableHeaderCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:DropDownList ID="ddl_CustName" runat="server" ToolTip="Cust Name" DataSourceID="SqlDataSource2" DataTextField="Cust_Name" DataValueField="PJ_Code"></asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell HorizontalAlign="Left">Test Category: </asp:TableHeaderCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:DropDownList ID="ddl_TestCategory" runat="server" ToolTip="Test Category" DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="ID"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableHeaderCell HorizontalAlign="Left">Loaner Name: </asp:TableHeaderCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:TextBox ID="tb_LoanerName" runat="server" ReadOnly="true" ToolTip="Select Loaner Name"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell HorizontalAlign="Left">Project Stage: </asp:TableHeaderCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:TextBox ID="tb_PjStage" runat="server" ToolTip="Project Stage"></asp:TextBox>
                        <%--<cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="tb_PjStage" WatermarkText="Type Project Stage"></cc1:TextBoxWatermarkExtender>--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell HorizontalAlign="Left">Comment: </asp:TableHeaderCell>
                    <asp:TableCell ColumnSpan="3" HorizontalAlign="Left">
                        <asp:TextBox ID="tb_Comment" runat="server" TextMode="MultiLine" Width="100%" Height="100px"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4"><br /><hr /></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [ID], [Name] FROM [tbl_TestCategory]"></asp:SqlDataSource>
            <script type="text/javascript">
                function ownerSelect() {
                    var obj = window.showModalDialog('../Equipment/OwnerSelect.aspx', '', '');
                    if (obj != null) {
                        var clientid = '<%= tb_LoanerName.ClientID%>';
                        var tb = window.document.getElementById(clientid);
                        tb.value = obj;
                    }
                }
            </script>

            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [PJ_Code], [Cust_Name] FROM [tbl_Project] WHERE ([PJ_Code] = @PJ_Code)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="ddl_Project" Name="PJ_Code" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [PJ_Code], [PJ_Name], [Cust_Name] FROM [tbl_Project] ORDER BY [PJ_Name]"></asp:SqlDataSource>
            <asp:PlaceHolder ID="ph_InsertCtrl" runat="server" >
                <%--<asp:LinkButton ID="LinkButton1" runat="server">Test</asp:LinkButton>--%>
            </asp:PlaceHolder>
            <asp:Table ID="HolderTable" runat="server"></asp:Table>
        </div>
    </form>
</body>
</html>
