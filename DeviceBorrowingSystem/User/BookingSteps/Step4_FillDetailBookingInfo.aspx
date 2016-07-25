<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step4_FillDetailBookingInfo.aspx.cs" Inherits="User_BookingSteps_Step4_FillDetailBookingInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <style type="text/css">
        .div {
            margin-top: 20px;
        }

        table {
            /*margin-top:20px;*/
        }

            table tr {
                /*margin-bottom: 20px;*/
            }

                table tr th {
                    text-align: left;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="div">
            <table align="center" style="width: 80%">
                <tr>
                    <td colspan="4">
                        <iframe style="width: 100%;" id="recordView" runat="server" src="../RecordSimpleView.aspx"></iframe>
                    </td>
                </tr>
                <tr>
                    <th>Project</th>
                    <td colspan="3" align="left">
                        <dx:ASPxComboBox ID="cb_project" runat="server" ValueType="System.String" Theme="Moderno"
                            AutoPostBack="true" OnSelectedIndexChanged="cb_project_SelectedIndexChanged">
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Date value can not be null." ValidationGroup="validation"></ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <th>Cust Name</th>
                    <td align="left">
                        <dx:ASPxTextBox ID="tb_custName" runat="server" Theme="Moderno" ReadOnly="true"></dx:ASPxTextBox>
                    </td>

                    <th>Project Stage</th>
                    <td align="left">
                        <dx:ASPxTextBox ID="tb_pjStage" runat="server" Theme="Moderno" ReadOnly="true"></dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <th>Test Category</th>
                    <td colspan="3" align="left">
                        <dx:ASPxComboBox ID="cb_testCategory" runat="server" ValueType="System.String" Theme="Moderno">
                            <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Test Category can not be null." ValidationGroup="validation"></ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <th>Comment</th>
                    <td colspan="3" align="left">
                        <dx:ASPxMemo ID="mo_comment" runat="server" Height="80px" Width="725px" Theme="Moderno"></dx:ASPxMemo>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr style="height: 50px;">
                    <td colspan="2">
                        <dx:ASPxButton ID="btn_submit" runat="server" Text="SUBMIT" Theme="Moderno" Width="100px" OnClick="btn_submit_Click"></dx:ASPxButton>
                    </td>
                    <td colspan="2" align="right">
                        <dx:ASPxButton ID="btn_cancel" runat="server" Text="CANCEL" Theme="Moderno" Width="100px" OnClick="btn_cancel_Click">
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
