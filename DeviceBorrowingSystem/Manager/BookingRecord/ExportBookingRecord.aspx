<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportBookingRecord.aspx.cs" Inherits="Manager_BookingRecord_ExportBookingRecord" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="Stylesheet" type="text/css" href="../../CSS/Manager_ExportBookingRecord.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm_main" runat="server"></asp:ScriptManager>
        <div>
            <table border="1px" cellspacing="0px" style="width: 640px">
                <caption>
                    <h1>Record Report</h1>
                </caption>

                <tr>
                    <th align="left" style="width: 100px">Category: </th>
                    <td>
                        <cc1:ComboBox runat="server" ID="cb_category" AutoCompleteMode="Suggest" RenderMode="Block" Width="100px"
                            AutoPostBack="true" OnSelectedIndexChanged="cb_category_SelectedIndexChanged">
                            <asp:ListItem Value="1" Text="Device"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Equipment"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Chamber"></asp:ListItem>
                        </cc1:ComboBox>
                    </td>
                </tr>

            </table>
            <asp:Panel ID="p_filter" runat="server" Visible="false" Enabled="true">
                <table border="1px" cellspacing="0px" style="width: 640px">
                    <tr>
                        <th align="left" style="width: 100px">Device Filters: </th>
                        <td>
                            <font>Site:</font>
                            <cc1:ComboBox ID="cb_site" runat="server" DropDownStyle="DropDownList" Width="50px">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>WHC</asp:ListItem>
                                <asp:ListItem>WCH</asp:ListItem>
                                <asp:ListItem>WKS</asp:ListItem>
                            </cc1:ComboBox>
                            <font>Department:</font>
                            <cc1:ComboBox ID="cb_department" runat="server" Width="80px" DropDownStyle="DropDownList">
                            </cc1:ComboBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" style="width: 100px">Borrow Filters:</th>
                        <td>
                            <font>Project:</font>
                            <cc1:ComboBox ID="cb_project" runat="server" Width="80px" DropDownStyle="DropDownList"></cc1:ComboBox>
                            <font>Test Category:</font>
                            <cc1:ComboBox ID="cb_testcategory" runat="server" Width="100px" DropDownStyle="DropDownList"></cc1:ComboBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table border="1px" cellspacing="0px" style="width: 640px">
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="p_monthly" runat="server" Visible="false">
                            <asp:LinkButton ID="lbtn_exportToExcel_monthly" runat="server" Text="Monthly report" OnClick="lbtn_exportToExcel_monthly_Click" />
                            <img src="../../Images/excel-file.gif" />
                            <font>From: </font>
                            <asp:TextBox ID="tb_startDate_Month" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tb_startDate_Month"></cc1:CalendarExtender>
                            <font>To: </font>
                            <asp:TextBox ID="tb_endDate_Month" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="tb_endDate_Month"></cc1:CalendarExtender>
                            <br />
                            <br />
                        </asp:Panel>

                        <asp:Panel ID="p_yearly" runat="server" Visible="false">
                            <asp:LinkButton ID="lbtn_exportToExcel_yearly" runat="server" Text="Yearly report" OnClick="lbtn_exportToExcel_yearly_Click" />
                            <img src="../../Images/excel-file.gif" />
                            <cc1:ComboBox ID="cb_yearly" runat="server" Width="50px"></cc1:ComboBox>
                            <br />
                            <br />
                        </asp:Panel>

                        <asp:Panel ID="p_allbyrecord" runat="server" Visible="false">
                            <asp:LinkButton ID="lbtn_exportToExcel_ar" runat="server" Text="Export all record for borrowing data" OnClick="lbtn_exportToExcel_ar_Click" />
                            <img src="../../Images/excel-file.gif" />
                            <font>From: </font>
                            <asp:TextBox ID="tb_startDate_ar" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tb_startDate_ar"></cc1:CalendarExtender>
                            <font>To: </font>
                            <asp:TextBox ID="tb_endDate_ar" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="tb_endDate_ar"></cc1:CalendarExtender>
                            <br />
                            <br />
                        </asp:Panel>

                        <asp:Panel ID="p_allbydevice" runat="server" Visible="false">
                            <asp:LinkButton ID="lbtn_exportToExcel_ad" runat="server" Text="Export all device data" OnClick="lbtn_exportToExcel_ad_Click" />
                            <img src="../../Images/excel-file.gif" />
                            <font>From: </font>
                            <asp:TextBox ID="tb_startDate_ad" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="tb_startDate_ad"></cc1:CalendarExtender>
                            <font>To: </font>
                            <asp:TextBox ID="tb_endDate_ad" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="tb_endDate_ad"></cc1:CalendarExtender>
                            <br />
                            <br />
                        </asp:Panel>

                        <asp:Panel ID="p_chamber" runat="server" Visible="false">
                            <asp:LinkButton ID="lbtn_exportToExcel_chamber" runat="server" Text="Chamber dashboard" OnClick="lbtn_exportToExcel_chamber_Click" />
                            <img src="../../Images/excel-file.gif" />
                            <cc1:ComboBox ID="cb_charmberDashbord_year" runat="server" Width="50px"></cc1:ComboBox>
                            <br />
                            <br />
                        </asp:Panel>

                    </td>
                </tr>

            </table>

        </div>
    </form>
</body>
</html>
