<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FillBorrowingInfo.aspx.cs" Inherits="User_FillBorrowingInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="Stylesheet" type="text/css" href="../CSS/User_FillBorrowingInfo.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Fill Borrowing Information</title>
    <script language="javascript" type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <h3>Step3--Fill Borrowing Information
            </h3>
            <h3>
                <asp:Label ID="Label1" runat="server" Text="You will borrow device(s) below:" ForeColor="Red"></asp:Label>
            </h3>
            <hr />
            <div id="div_borrowingItems">
                <asp:UpdatePanel ID="upd2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                            OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:BoundField HeaderText="Device Name" ItemStyle-Width="300px" DataField="Device_Name">
                                    <ItemStyle Width="350px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Start DateTime" ItemStyle-Width="200px" DataField="StartDateTime" DataFormatString="{0:yyyy/MM/dd/ HH:mm}">
                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="End DateTime" ItemStyle-Width="200px" DataField="EndDateTime" DataFormatString="{0:yyyy/MM/dd/ HH:mm}">
                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemStyle Width="50px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" CommandArgument='<%# Bind("id") %>' Text="DELETE"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT tbl_Device.Device_Name, tbl_DeviceBooking.* FROM tbl_Device INNER JOIN tbl_DeviceBooking ON tbl_Device.Device_ID = tbl_DeviceBooking.Device_ID"></asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <hr />
            </div>

            <div id="div_LoanerInformation">
                <h3>Loaner Information:
                </h3>
                <asp:UpdatePanel ID="upd1" runat="server">
                    <ContentTemplate>
                        <table cellpadding="10px" border="1">
                            <tr>
                                <td style="width: 110px;">ID:
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox ID="tb_Loanerid" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 110px;">Name:
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox ID="tb_Loanername" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="">
                                <td>Department:
                                </td>
                                <td>
                                    <asp:TextBox ID="tb_dpt" runat="server" Width="200px"></asp:TextBox>
                                </td>
                                <td>Eamil:
                                </td>
                                <td>
                                    <asp:TextBox ID="tb_email" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>ExNumber:
                                </td>
                                <td>
                                    <asp:TextBox ID="tb_exnumber" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Project:
                                </td>
                                <td>
                                    <%--                                    <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="PJ_Name" DataValueField="PJ_Code" Width="142px"
                                        AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" OnDataBound="DropDownList1_DataBound" Style="margin-bottom: 0px" AppendDataBoundItems="true">
                                        <asp:ListItem Selected="True" Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>--%>
                                    <dx:ASPxComboBox ID="DropDownList1" runat="server" ValueType="System.String" DataSourceID="SqlDataSource1" TextField="PJ_Name" ValueField="PJ_Code"
                                        AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" OnDataBound="DropDownList1_DataBound">
                                    </dx:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="project can not be null" ValidationGroup="FillBorrowInfo" ControlToValidate="DropDownList1">*</asp:RequiredFieldValidator>

                                </td>
                                <td>Cust Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="tb_custname" runat="server" ReadOnly="true" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Test Category:
                                </td>
                                <td>
                                    <%--                                    <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="SqlDataSource3" DataTextField="Name" DataValueField="ID" Width="142px" 
                                        AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" OnDataBound="DropDownList1_DataBound" Style="margin-bottom: 0px"
                                         AppendDataBoundItems="true">
                                        <asp:ListItem Selected="True" Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>--%>

                                    <dx:ASPxComboBox ID="DropDownList2" runat="server" ValueType="System.String" DataSourceID="SqlDataSource3" TextField="Name" ValueField="ID" HorizontalAlign="Left">
                                    </dx:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="rfv_testcategory" ControlToValidate="DropDownList2" ValidationGroup="FillBorrowInfo" runat="server" ErrorMessage="Test category can not be null">*</asp:RequiredFieldValidator>

                                </td>
                                <td>Project Stage:</td>
                                <td>
                                    <asp:TextBox ID="tb_pjStage" runat="server" Width="200px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Comment: 
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="tb_Comment" runat="server" TextMode="MultiLine" Height="95px" Width="560px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv_comment" runat="server" ControlToValidate="tb_Comment" ErrorMessage="Borrowing comment can not be null" ValidationGroup="FillBorrowInfo">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [PJ_Name], [PJ_Code], [Cust_Name] FROM [tbl_Project]"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT [ID], [Name] FROM [tbl_TestCategory]"></asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <hr />
            </div>

            <div id="div_command" class="divCommand">
                <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" ValidationGroup="FillBorrowInfo" />
                <asp:Button ID="btn_cancel" runat="server" Text="Cancel" OnClick="btn_cancel_Click" />

                <asp:ValidationSummary ID="ValidationSummary1" ShowMessageBox="true" ShowSummary="false" runat="server" ValidationGroup="FillBorrowInfo" />
            </div>
        </div>
    </form>
</body>
</html>
