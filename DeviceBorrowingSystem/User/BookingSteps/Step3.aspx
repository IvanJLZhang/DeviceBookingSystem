<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step3.aspx.cs" Inherits="User_BookingSteps_Step3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <style type="text/css">
        .panelStyle {
            border: solid;
            border-width: 1px;
            margin-bottom: 10px;
        }

        .tableStyle {
            margin-bottom: 10px;
            margin-left: 20px;
        }

        table tr th {
            text-align: left;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td valign="top" style="width:300px">
                                <asp:Panel ID="pnl_deviceList" runat="server" CssClass="panelStyle">
                                    <dx:ASPxRadioButtonList ID="rbtnl_deviceList" runat="server" ValueType="System.String" Theme="Moderno"
                                         AutoPostBack="true" OnSelectedIndexChanged="DateChanged"></dx:ASPxRadioButtonList>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="pnl_recurrence" CssClass="panelStyle">
                                    <dx:ASPxCheckBox ID="chb_recurrence" runat="server" Text="Recurrence Pattern" Font-Bold="true" Font-Size="Medium"
                                        OnCheckedChanged="chb_recurrence_CheckedChanged" AutoPostBack="true" Theme="Moderno">
                                    </dx:ASPxCheckBox>
                                    <table style="margin-left: 20px; margin-top: 10px">
                                        <tr>
                                            <td>
                                                <dx:ASPxRadioButton ID="rb_daily" runat="server" Text="Daily" GroupName="Recurrence" Checked="true" Theme="Moderno"
                                                    AutoPostBack="true" OnCheckedChanged="rb_daily_weekly_CheckedChanged">
                                                </dx:ASPxRadioButton>
                                            </td>
                                            <td>
                                                <dx:ASPxLabel runat="server" ID="lbl_everyDay" Text="Every day" Theme="Moderno"></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dx:ASPxRadioButton ID="rb_weekly" runat="server" Text="Weekly" GroupName="Recurrence" Theme="Moderno"
                                                    OnCheckedChanged="rb_daily_weekly_CheckedChanged" AutoPostBack="true">
                                                </dx:ASPxRadioButton>
                                            </td>
                                            <td>
                                                <dx:ASPxCheckBoxList ID="chbl_weekly" runat="server" ValueType="System.String" RepeatDirection="Horizontal" RepeatColumns="2"
                                                    Theme="Moderno">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="You need select at least one week day." ValidationGroup="validation"></ValidationSettings>
                                                    <Items>
                                                        <dx:ListEditItem Text="Mon." Value="1" />
                                                        <dx:ListEditItem Text="Tue." Value="2" />
                                                        <dx:ListEditItem Text="Wed." Value="3" />
                                                        <dx:ListEditItem Text="Thu." Value="4" />
                                                        <dx:ListEditItem Text="Fri." Value="5" />
                                                        <dx:ListEditItem Text="Sat." Value="6" />
                                                        <dx:ListEditItem Text="Sun." Value="7" />
                                                    </Items>
                                                </dx:ASPxCheckBoxList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel runat="server" ID="pnl_rangeOfdate" CssClass="panelStyle" Visible="false">
                                    <h3>Range of Date</h3>
                                    <table class="tableStyle">
                                        <tr>
                                            <th>FROM</th>
                                            <td>
                                                <dx:ASPxDateEdit ID="de_from" runat="server" Theme="Moderno" AutoPostBack="true"
                                                     OnDateChanged="DateChanged">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Date value can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>TO</th>
                                            <td>
                                                <dx:ASPxDateEdit ID="de_to" runat="server" Theme="Moderno" AutoPostBack="true"
                                                     OnDateChanged="DateChanged">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="TO_Date value can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                                <asp:Panel runat="server" ID="pnl_appointmentTime" CssClass="panelStyle" Visible="false">
                                    <h3>Appointment Time</h3>
                                    <table class="tableStyle">
                                        <tr>
                                            <th>START</th>
                                            <td>
                                                <dx:ASPxComboBox ID="cb_start" runat="server" Theme="Moderno">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="START_Time can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>END</th>
                                            <td>
                                                <dx:ASPxComboBox ID="cb_end" runat="server" Theme="Moderno">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="END_Time can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <dx:ASPxCheckBox ID="chb_allDayevent" runat="server" Checked="false" Text="Allday event" Theme="Moderno" OnCheckedChanged="chb_allDayevent_CheckedChanged"
                                                    AutoPostBack="true">
                                                </dx:ASPxCheckBox>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                                <asp:Panel ID="pnl_appointmentDatetime" runat="server" CssClass="panelStyle">
                                    <h3>Appointment DateTime</h3>
                                    <table class="tableStyle">
                                        <tr>
                                            <th>FROM</th>
                                            <td>
                                                <dx:ASPxDateEdit ID="de_from2" runat="server" Theme="Moderno" AutoPostBack="true"
                                                     OnDateChanged="DateChanged">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Date can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <dx:ASPxComboBox ID="cb_start2" runat="server" Theme="Moderno">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Time can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>TO</th>
                                            <td>
                                                <dx:ASPxDateEdit ID="de_to2" runat="server" Theme="Moderno" AutoPostBack="true"
                                                     OnDateChanged="DateChanged">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="TO_Date can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <dx:ASPxComboBox ID="cb_end2" runat="server" Theme="Moderno">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="TO_Time can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                            </td>
                            <td valign="top" align="left">
                                <iframe style="width: 100%;" id="devViewFrame" runat="server" height="650px"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="pnl_newCommand" runat="server" CssClass="panelStyle">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <dx:ASPxButton ID="btn_new" runat="server" Text="NEW" Width="100px" ValidationGroup="validation"
                                                    OnClick="btn_new_Click">
                                                </dx:ASPxButton>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="btn_cancel" runat="server" Text="CANCEL" Width="100px"></dx:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <iframe style="width: 100%;" id="recordView" runat="server" src="../RecordSimpleView.aspx"></iframe>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
