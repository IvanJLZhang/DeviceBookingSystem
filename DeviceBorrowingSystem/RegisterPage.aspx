<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterPage.aspx.cs" Inherits="RegisterPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>User Register
    </title>
    <link href="CSS/Manager_Home.css" rel="stylesheet" />
    <link href="CSS/MainStyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .table {
            border: dashed;
            border-width: thin;
            position: relative;
            /*margin-top: 10px;*/
            /*width: 50%;*/
        }

        table tr {
            /*border: dashed;
                border-width: thin;*/
        }

            table tr td {
                text-align: right;
                vertical-align: text-bottom;
                font-size: large;
            }

        table caption {
            font-size: x-large;
            font-style: italic;
            font-weight: bold;
        }

        .tds {
            text-align: left;
            width: 60%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center" style="margin-top: 200px">
            <table cellpadding="5px" class="table">
                <caption>
                    New An User
                </caption>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="UID" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <dx:ASPxTextBox ID="tb_uid" runat="server" Text="K1207A49" Theme="Moderno" Width="250px" ReadOnly="false">
                            <ValidationSettings ValidationGroup="NEW">
                                <RequiredField IsRequired="true" ErrorText="please input valid UID" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="EMAIL" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <dx:ASPxTextBox ID="tb_email" runat="server" Text="Ivan_JL_Zhang@wistron.com" Theme="Moderno" Width="250px" ReadOnly="false">
                            <ValidationSettings CausesValidation="true" ValidationGroup="NEW">
                                <RequiredField IsRequired="true" ErrorText="email can not be null!" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel7" runat="server" Text="DEPARTMENT" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <dx:ASPxTextBox ID="tb_dpt" runat="server" Text="2RWK30" Theme="Moderno" Width="250px">
                            <ValidationSettings CausesValidation="true" ValidationGroup="NEW">
                                <RequiredField IsRequired="true" ErrorText="department can not be null!" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 40%">
                                    <dx:ASPxButton ID="btn_Save" runat="server" Text="Submit" Theme="Moderno" Width="100px" OnClick="btn_Save_Click" ValidationGroup="NEW">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_cancel" runat="server" Text="Cancel" Theme="Moderno" Width="100px">
                                        <ClientSideEvents Click="function(s,e){window.close();}" />
                                    </dx:ASPxButton>
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
