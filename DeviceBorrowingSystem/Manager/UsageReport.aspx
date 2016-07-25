<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="UsageReport.aspx.cs" Inherits="Manager_UsageReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Summary Report
    </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function doSelect() {
            var dom = document.all;
            var el = event.srcElement;
            if (el.id.indexOf("chkHeader") >= 0 && el.tagName == "INPUT" && el.type.toLowerCase() == "checkbox") {
                var ischecked = false;
                if (el.checked)
                    ischecked = true;
                for (i = 0; i < dom.length; i++) {
                    if (dom[i].type == undefined) continue;
                    if (dom[i].id.indexOf("chkSelect") >= 0 && dom[i].tagName == "INPUT" && dom[i].type.toLowerCase() == "checkbox")
                        dom[i].checked = ischecked;
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="upd_main" runat="server">
        <ContentTemplate>
            <asp:Panel ID="p_Navigate" runat="server">
                <table style="width: 100%">
                    <tr>
                        <td>Category: 
            <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true" ViewStateMode="Enabled">
                <asp:ListItem Value="1" Text="Device"></asp:ListItem>
                <asp:ListItem Value="2" Text="Equipment"></asp:ListItem>
                <asp:ListItem Value="3" Text="Chamber"></asp:ListItem>
            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Panel ID="p_Opera" runat="server" HorizontalAlign="Right">
                                <table align="right">
                                    <tr>
                                        <td>
                                            <dx:ASPxTextBox ID="tb_searchText" runat="server" Width="170px" Height="20px" NullText="text search text"
                                                ToolTip="support search text:&#10;BookingID;&#10;Device Name;&#10;Owner ID/Name;&#10;Loaner ID/Name;&#10;Custom ID">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgbtn_Search" runat="server" ImageUrl="~/Images/search.png" Height="20px" Width="22px" ToolTip="Search" OnClick="btn_search_Click" />
                                            &nbsp;&nbsp;
                                            &nbsp;&nbsp;<asp:ImageButton ID="linkbutton" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton_Click" ToolTip="Add a single record" />
                                            <%--&nbsp;&nbsp;<asp:ImageButton ID="imgb_AddFromFile" runat="server" Height="20px" ImageUrl="~/Images/AddFromFile.png" Width="22px" OnClick="imgb_AddFromFile_Click" ToolTip="Add record list from file" />--%>
                                            <asp:ImageButton ID="btn_ExportExcel" runat="server" ImageUrl="~/Images/printer.png" Height="20px" Width="22px" ToolTip="Export to Excel" OnClick="btn_ExportExcel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <hr />
            <br />
            <div id="divBody" align="center">
                <asp:Panel ID="pnl_filter" runat="server" HorizontalAlign="Left">
                    <table>
                        <tr>
                            <td>Status:
                                <asp:DropDownList ID="ddl_status" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Text="ALL"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="NoReturn" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Return"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>Page Size: 
                            <asp:DropDownList ID="ddl_pagesize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_pagesize_SelectedIndexChanged" Width="100px">
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                            </asp:DropDownList>

                            </td>
                            <asp:Panel ID="pnl_deviceFilter" runat="server">
                                <td>Class:
                                    <asp:DropDownList ID="ddl_class" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>Interface:
                                    <asp:DropDownList ID="ddl_interface" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_status_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </asp:Panel>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnl_recordshow" runat="server" Height="600px" ScrollBars="Auto">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                        OnRowCommand="GridView1_RowCommand" OnDataBounding="GridView1_DataBounding" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging"
                        OnDataBound="GridView1_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Select" Visible="false">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkHeader" runat="server" Checked="false" onclick="doSelect();" />ALL
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" Checked="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Booking ID" SortExpression="Booking_ID" Visible="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_Approve" runat="server" Text='<%# Bind("Booking_ID") %>' CommandName="Approve" CommandArgument='<%# Bind("Booking_ID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Device Name" SortExpression="Device_Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtn_DeviceDetail" runat="server" Text='<%# Bind("Device_Name") %>' CommandName="DeviceDetail" CommandArgument='<%# Bind("Device_ID") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Wrap="true" Width="300px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Custom_ID" HeaderText="Custom_ID">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_customid" runat="server" Text='<%# Bind("Custom_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="P_Name" HeaderText="Loaner" SortExpression="P_Name" HeaderStyle-Width="100px">
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PJ_Name" HeaderText="Project" SortExpression="PJ_Name" HeaderStyle-Width="80px">
                                <HeaderStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="Test Category" SortExpression="Name" HeaderStyle-Width="120px">
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Start DT" SortExpression="Loan_DateTime">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Loan_DateTime") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Loan_DateTime").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End DT" SortExpression="Plan_To_ReDateTime">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Plan_To_ReDateTime") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Plan_To_ReDateTime").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Return DT" SortExpression="Real_ReDateTime">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Real_ReDateTime") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Real_ReDateTime").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemStyle Width="120px" />
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lbtn_Return" runat="server" CausesValidation="False" CommandName="Return" CommandArgument='<%# Bind("Booking_ID") %>' Text="RETURN"></asp:LinkButton>--%>
                                    <asp:ImageButton ID="lbtn_Return" runat="server" CausesValidation="false" CommandName="Return" CommandArgument='<%# Bind("Booking_ID") %>' ImageUrl="~/Images/return.png" Width="20px" Height="20px" ToolTip="return the device" />
                                    &nbsp;&nbsp;
                                            <asp:ImageButton ID="img_Modify" runat="server" ImageUrl="~/Images/Edit.png" Width="20px" Height="20px" CommandName="Modify" CommandArgument='<%# Bind("Booking_ID") %>'
                                                ToolTip="Modify Booking sheet" />
                                    &nbsp;&nbsp;
                                    <asp:ImageButton ID="ibtn_Delete" runat="server" CausesValidation="false" CommandName="DeleteRecord" CommandArgument='<%# Bind("Booking_ID") %>' ImageUrl="~/Images/Delete.png" ToolTip="delete the record" Width="20px" Height="20px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, tbl_DeviceBooking.Device_ID, tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.TestCategory_ID, tbl_DeviceBooking.PJ_Stage, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Real_ReDateTime, tbl_DeviceBooking.Status, tbl_DeviceBooking.Comment, tbl_DeviceBooking.Reviewer_ID, tbl_DeviceBooking.Date, tbl_DeviceBooking.Review_Comment, tbl_Project.PJ_Name, tbl_Person.P_Name, tbl_TestCategory.Name, tbl_Person_1.P_Name AS Reviewer_Name, tbl_summary_dev_title.s_name AS Device_Name FROM tbl_DeviceBooking INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code INNER JOIN tbl_TestCategory ON tbl_DeviceBooking.TestCategory_ID = tbl_TestCategory.ID INNER JOIN tbl_Person AS tbl_Person_1 ON tbl_DeviceBooking.Reviewer_ID = tbl_Person_1.P_ID INNER JOIN tbl_summary_dev_title ON tbl_DeviceBooking.Device_ID = tbl_summary_dev_title.s_id WHERE (tbl_DeviceBooking.Status &gt;= 2) AND (tbl_summary_dev_title.s_category = @s_category)">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="DropDownList1" Name="s_category" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
                <hr />
                <br />
                <asp:SqlDataSource ID="SqlDataSource4" runat="server"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgbtn_Search" />
            <%--<asp:PostBackTrigger ControlID="btn_ExportExcel" />--%>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

