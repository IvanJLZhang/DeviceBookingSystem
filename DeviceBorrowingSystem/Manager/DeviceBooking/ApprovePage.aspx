<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApprovePage.aspx.cs" Inherits="User_ApprovePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Approve/Reject</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div align="center">
            <h3>Booking Details:
            </h3>
            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" CellPadding="6" DataKeyNames="Booking_ID,Device_ID,PJ_Code,P_ID" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
                <EditRowStyle BackColor="#2461BF" />
                <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" Width="150px" HorizontalAlign="Left" />
                <RowStyle HorizontalAlign="Left" />
                <Fields>
                    <asp:BoundField DataField="Booking_ID" HeaderText="Booking ID" ReadOnly="True" SortExpression="Booking_ID" />
                    <asp:BoundField DataField="Device_Name" HeaderText="Device Name" SortExpression="Device_Name" />
                    <asp:BoundField DataField="P_Name" HeaderText="Loaner Name" SortExpression="P_Name" />
                    <asp:BoundField DataField="P_Department" HeaderText="Department" SortExpression="P_Department" />
                    <asp:BoundField DataField="P_ExNumber" HeaderText="ExNumber" SortExpression="P_ExNumber" />
                    <asp:BoundField DataField="PJ_Name" HeaderText="Project" SortExpression="PJ_Name" />
                    <asp:BoundField DataField="PJ_Stage" HeaderText="Project Stage" SortExpression="PJ_Stage" />
                    <asp:BoundField DataField="Name" HeaderText="Test Category" SortExpression="Name" />
                    <asp:BoundField DataField="Cust_Name" HeaderText="Cust Name" SortExpression="Cust_Name" />
                    <asp:TemplateField HeaderText="Start DT" SortExpression="Loan_DateTime">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Loan_DateTime") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Loan_DateTime") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Loan_DateTime").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="End DT" SortExpression="Plan_To_ReDateTime">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Plan_To_ReDateTime") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Plan_To_ReDateTime") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "Plan_To_ReDateTime").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Return DT" SortExpression="Return DT">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("[Return DT]") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("[Return DT]") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# GetTimeZoneDateTimeString(DataBinder.Eval(Container.DataItem, "[Return DT]").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reviewer Name">
                        <ItemTemplate>
                            <asp:Label ID="lbl_reviewName" runat="server" Text='<%# GetReviewerName(DataBinder.Eval(Container.DataItem, "Reviewer_ID").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                        <ItemTemplate>
                            <asp:TextBox ID="Label1" runat="server" Text='<%# Bind("Comment") %>' TextMode="MultiLine" ReadOnly="true" Width="560px" Height="95px"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Fields>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
            </asp:DetailsView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT tbl_DeviceBooking.Booking_ID, tbl_DeviceBooking.Device_ID, tbl_Person.P_Name, tbl_Person.P_Department, tbl_Person.P_ExNumber, tbl_Project.PJ_Name, tbl_Project.Cust_Name, tbl_DeviceBooking.Loan_DateTime, tbl_DeviceBooking.Plan_To_ReDateTime, tbl_DeviceBooking.Comment, tbl_Device.Device_Name, tbl_Device.Device_ID, tbl_Project.PJ_Code, tbl_Person.P_ID, tbl_TestCategory.Name, tbl_DeviceBooking.PJ_Stage, tbl_DeviceBooking.Reviewer_ID, tbl_DeviceBooking.Real_ReDateTime AS [Return DT] FROM tbl_DeviceBooking INNER JOIN tbl_Device ON tbl_DeviceBooking.Device_ID = tbl_Device.Device_ID INNER JOIN tbl_Person ON tbl_DeviceBooking.Loaner_ID = tbl_Person.P_ID INNER JOIN tbl_Project ON tbl_DeviceBooking.Project_ID = tbl_Project.PJ_Code INNER JOIN tbl_TestCategory ON tbl_DeviceBooking.TestCategory_ID = tbl_TestCategory.ID WHERE (tbl_DeviceBooking.Booking_ID = @Booking_ID)">
                <SelectParameters>
                    <asp:QueryStringParameter Name="Booking_ID" QueryStringField="bookingId" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <div align="center">
            <hr />
            <h3>Approve/Reject Reason:
            </h3>
            <asp:TextBox ID="tb_Reason" runat="server" TextMode="MultiLine" Width="725px" Height="85px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please fill the Approve/Reject reson!" ControlToValidate="tb_Reason" ValidationGroup="CommentCheck">*</asp:RequiredFieldValidator>
            <br />
            <hr />
            <table>
                <tr style="width: 400px">
                    <td style="width: 200px">
                        <asp:Button ID="btn_Approve" runat="server" Text="Approve" OnClick="btn_Approve_Click" ValidationGroup="CommentCheck" />
                    </td>
                    <td style="width: 200px">
                        <asp:Button ID="btn_Reject" runat="server" Text="Reject" OnClick="btn_Reject_Click" ValidationGroup="CommentCheck" />
                    </td>
                    <td style="width: 200px">
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" OnClick="btn_Cancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
