<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link type="text/css" rel="Stylesheet" href="CSS/style.css" />
    <title>Device Borrowing System</title>
    <script>
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="login">
                <h1 style="font-size: x-large">Login to the System</h1>
                <p>
                    <dx:ASPxTextBox ID="tb_UserName" runat="server" Width="278px" Height="30px" NullText="UID"></dx:ASPxTextBox>
                </p>

                <p>
                    <dx:ASPxTextBox ID="tb_Pwd" runat="server" Width="278px" Height="30px" NullText="PASSWORD" Password="true"></dx:ASPxTextBox>
                </p>
                <%--                <p class="remember_me">
                        <asp:CheckBox ID="remember_me" runat="server" Text="Remember me on this computer" />
                </p>--%>
                <panel class="submit" style="margin-top:20px">
                    <table width="100%" style="margin-top:20px">
                        <tr>
                            <td align="left">
                                <dx:ASPxButton ID="btn_Login" runat="server" Text="Login" CssClass="submit_button" OnClick="btn_Login_Click"></dx:ASPxButton>
                            </td>
                            <td align="right">
                                <dx:ASPxHyperLink ID="ff" runat="server" Text="Register" Theme="Moderno" Cursor="pointer">
                                    <ClientSideEvents Click="function(s,e){window.open('RegisterPage.aspx');}" />
                                </dx:ASPxHyperLink>
                            </td>
                        </tr>
                    </table>
                </panel>
            </div>
        </div>
    </form>
</body>
</html>
