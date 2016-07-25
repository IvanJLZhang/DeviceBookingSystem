<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddProject.aspx.cs" Inherits="Manager_AddProject" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Add
    </title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <table>
                <tr>
                    <th colspan="3">ADD Project
                        <br />
                        <br />
                    </th>
                </tr>
                <tr>
                    <td>Project Code：
                    </td>
                    <td>
                        <asp:TextBox ID="tb_PJCode" runat="server"></asp:TextBox><span style="color:red">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_PJCode" ErrorMessage="project code 不能为空" ValidationGroup="AddProject">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Project Name：
                    </td>
                    <td>
                        <asp:TextBox ID="tb_pjname" runat="server"></asp:TextBox><span style="color:red">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tb_pjname" ErrorMessage="project name 不能为空" ValidationGroup="AddProject">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Cust name：
                    </td>
                    <td>
                        <asp:TextBox ID="tb_custname" runat="server"></asp:TextBox><span style="color:red">*</span>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="ref_UName" runat="server" ControlToValidate="tb_custname" ErrorMessage="cust name不能为空" ValidationGroup="AddProject">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <hr align="left" style="width: 400px" />
                        <asp:Button ID="btn_add" runat="server" Text="ADD" Width="85px" Height="30px" OnClick="btn_add_Click" ValidationGroup="AddProject"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btn_calcel" runat="server" Text="CANCEL" Width="85px" Height="30px" OnClick="btn_calcel_Click" />
                        <br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="AddProject" />
                    </td>
                </tr>
                <tr>
                    <td>

                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <asp:Label ID="lbl_errMsg" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
