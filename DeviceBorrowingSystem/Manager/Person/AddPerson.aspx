<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPerson.aspx.cs" Inherits="Manager_AddPerson" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <title>Person Detail</title>
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
        <div align="center" style="margin-top: 20px">
            <table cellpadding="5px" class="table">
                <caption>
                    ADD Person
                </caption>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="UID" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <table style="border: none" cellpadding="0">
                            <tr>
                                <td>
                                    <dx:ASPxTextBox ID="tb_uid" runat="server" Text="" Theme="Moderno" Width="210px" ReadOnly="true">
                                    </dx:ASPxTextBox>
                                </td>
                                <td style="vertical-align: bottom">
                                    <asp:ImageButton ID="ibtn_check" runat="server" ImageUrl="~/Images/check_OK.png" Width="30px" Height="30px" ToolTip="check the uid?" Visible="true" OnClick="ibtn_check_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel9" runat="server" Text="EMAIL" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <dx:ASPxTextBox ID="tb_email" runat="server" Text="" Theme="Moderno" Width="250px" ReadOnly="false">
                            <ValidationSettings CausesValidation="true" ValidationGroup="Update">
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
                        <dx:ASPxTextBox ID="cb_dpt" runat="server" Text="" Theme="Moderno" Width="250px">
                            <ValidationSettings CausesValidation="true" ValidationGroup="Update">
                                <RequiredField IsRequired="true" ErrorText="department can not be null!" />
                            </ValidationSettings>
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel5" runat="server" Text="SITE" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <dx:ASPxComboBox ID="cb_site" runat="server" Theme="Moderno" Width="250px">
                            <ValidationSettings CausesValidation="true" ValidationGroup="Update">
                                <RequiredField IsRequired="true" ErrorText="site can not be null!" />
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel8" runat="server" Text="CHARGE of DPT." Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <script type="text/javascript">
                            function selectChargeOfDpt(s, e) {
                                var retValue = window.showModalDialog('./Charge of Department.aspx', '', '');
                                if (retValue != null) {
                                    cb_chargeDpt.SetText(retValue);
                                }
                            }
                        </script>
                        <dx:ASPxTextBox ID="cb_chargeDpt" ClientInstanceName="cb_chargeDpt" runat="server" Text="" Theme="Moderno" Width="250px" ReadOnly="true">
                            <ClientSideEvents GotFocus="function(s,e){selectChargeOfDpt(s,e);}" />
                        </dx:ASPxTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel11" runat="server" Text="ROLE" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <dx:ASPxComboBox ID="cb_role" runat="server" Theme="Moderno" Width="250px">
                            <Items>
                                <dx:ListEditItem Text="USER" Value="0" />
                                <dx:ListEditItem Text="REVIEWER" Value="10" />
                                <dx:ListEditItem Text="ADMIN" Value="20" />
                            </Items>

                            <ValidationSettings CausesValidation="true" ValidationGroup="Update">
                                <RequiredField IsRequired="true" ErrorText="role can not be null!" />
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="ACTIVATE" Theme="Moderno"></dx:ASPxLabel>
                    </td>
                    <td class="tds">
                        <dx:ASPxCheckBox ID="chb_ativate" runat="server" Theme="Moderno"></dx:ASPxCheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 40%">
                                    <dx:ASPxButton ID="btn_Save" runat="server" Text="NEW" Theme="Moderno" Width="100px" OnClick="btn_Save_Click" ValidationGroup="Update">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_cancel" runat="server" Text="Cancel" Theme="Moderno" Width="100px">
                                        <ClientSideEvents Click="function(s,e){window.close();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="pnl_AddUserTable" runat="server" Visible="false">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2" style="text-align: center">
                                                    <dx:ASPxGridView ID="gv_addedUser" runat="server" AutoGenerateColumns="False" OnRowDeleting="gv_addedUser_RowDeleting" Theme="Moderno" EnableTheming="True">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="UID" VisibleIndex="0" FieldName="P_ID">
                                                                <HeaderStyle CssClass="gridViewHeader" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="EAMIL" VisibleIndex="1" FieldName="P_Email">
                                                                <HeaderStyle CssClass="gridViewHeader" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="True" ButtonType="Image">
                                                                <HeaderStyle CssClass="gridViewHeader" />
                                                                <DeleteButton Image-Url="../../Images/Delete.png">
                                                                    <Image Url="../../Images/Delete.png">
                                                                    </Image>
                                                                </DeleteButton>
                                                            </dx:GridViewCommandColumn>
                                                        </Columns>
                                                        <SettingsDataSecurity AllowEdit="False" AllowInsert="False" />
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 40%">
                                                    <dx:ASPxButton ID="btn_submit" runat="server" Text="Submit" Theme="Moderno" Width="100px" OnClick="btn_submit_Click"></dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btn_cancel1" runat="server" Text="Cancel" Theme="Moderno" Width="100px">
                                                        <ClientSideEvents Click="function(s,e){window.close();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </asp:Panel>
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
