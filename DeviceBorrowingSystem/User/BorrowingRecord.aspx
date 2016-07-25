<%@ Page Title="" Language="C#" MasterPageFile="~/User/User_MasterPage.master" AutoEventWireup="true" CodeFile="BorrowingRecord.aspx.cs" Inherits="User_BorrowingRecord" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Borrow record</title>
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
    <asp:UpdatePanel ID="upd_Main" runat="server">
        <ContentTemplate>
            <div id="divBody" align="center">
                <table>
                    <tr>
                        <td>Device Category:
            <asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="1" Text="Device"></asp:ListItem>
                <asp:ListItem Value="2" Text="Equipment"></asp:ListItem>
                <asp:ListItem Value="3" Text="Chamber"></asp:ListItem>
            </asp:DropDownList>
                            &nbsp;&nbsp;
            Booking ID:
            <asp:TextBox ID="tb_bookingId" runat="server"></asp:TextBox>
                            &nbsp;&nbsp;
            <asp:Button ID="btn_search" runat="server" Text="Search" OnClick="btn_search_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btn_ExportExcel" runat="server" Text="Export to Excel" OnClick="btn_ExportExcel_Click" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                                OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="Booking_ID" OnPageIndexChanging="GridView1_PageIndexChanging" OnDataBinding="GridView1_DataBinding" OnDataBound="GridView1_DataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" runat="server" Checked="false" onclick="doSelect();" />ALL
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Booking ID" SortExpression="Booking_ID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtn_Approve" runat="server" Text='<%# Bind("Booking_ID") %>' CommandName="Approve" CommandArgument='<%# Bind("Booking_ID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Device" SortExpression="Device_Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtn_DeviceDetail" runat="server" Text='<%# Bind("Device_Name") %>' CommandName="DeviceDetail" CommandArgument='<%# Bind("Device_ID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="true" Width="300px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start DT" SortExpression="Loan_DateTime">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Loan_DateTime") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Loan_DateTime").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End DT" SortExpression="Plan_To_ReDateTime">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Plan_To_ReDateTime") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Plan_To_ReDateTime").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Return DT" SortExpression="Real_ReDateTime">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Real_ReDateTime") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Real_ReDateTime").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Status") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Status" runat="server" Text='<%# GetStatusStr(int.Parse(DataBinder.Eval(Container.DataItem, "Status").ToString())) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtn_Delete" runat="server" CausesValidation="False" CommandName="Delete" Text="DELETE" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Booking_ID").ToString() + "," + DataBinder.Eval(Container.DataItem, "Status").ToString() %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Loaner_ID, tbl_DeviceBooking.Device_ID, tbl_DeviceBooking.Project_ID, tbl_DeviceBooking.TestCategory_ID, tbl_DeviceBooking.PJ_Stage, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Real_ReDateTime, tbl_DeviceBooking.Status, tbl_DeviceBooking.Comment, tbl_DeviceBooking.Reviewer_ID, tbl_DeviceBooking.Date, tbl_DeviceBooking.Review_Comment, tbl_Person.P_Name, tbl_summary_dev_title.s_name AS Device_Name FROM tbl_DeviceBooking INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID INNER JOIN tbl_summary_dev_title ON tbl_DeviceBooking.Device_ID = tbl_summary_dev_title.s_id WHERE (tbl_Person.P_ID = @UserID) AND (tbl_summary_dev_title.s_category = @Category)" DeleteCommand="DELETE FROM tbl_DeviceBooking WHERE (Booking_ID = @Booking_ID)">
                                <DeleteParameters>
                                    <asp:Parameter Name="Booking_ID" />
                                </DeleteParameters>
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserID" SessionField="UserID" />
                                    <asp:ControlParameter ControlID="DropDownList1" Name="Category" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="Export to Excel" OnClick="btn_ExportExcel_Click" Visible="false" />
                        </td>
                    </tr>
                </table>
                <hr />
                <br />
                <asp:SqlDataSource ID="SqlDataSource4" runat="server"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

