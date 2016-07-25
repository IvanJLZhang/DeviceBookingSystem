<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecordDetail.aspx.cs" Inherits="Manager_BookingRecord_RecordDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style3 {
        }

        table th {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd_main" runat="server">
            <ContentTemplate>
                <div>
                    <table align="center">
                        <tr>
                            <td colspan="2" align="center">
                                <dx:ASPxLabel ID="tb_deviceName" runat="server" Text="testtest" Theme="Moderno"
                                    Font-Size="Larger">
                                </dx:ASPxLabel>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="border-width:1px;border:thin">
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
                                                <dx:ASPxCheckBoxList ID="chbl_weekly" runat="server" ValueType="System.String" RepeatDirection="Horizontal" RepeatColumns="3"
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

                            </td>
                            <td style="border-width:1px;border:thin">
                                <asp:Panel runat="server" ID="pnl_rangeOfdate" CssClass="panelStyle" Visible="false">
                                    <h3>Range of Date</h3>
                                    <table class="tableStyle">
                                        <tr>
                                            <th>FROM</th>
                                            <td>
                                                <dx:ASPxDateEdit ID="de_from" runat="server" Theme="Moderno">
                                                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Date value can not be null." ValidationGroup="validation"></ValidationSettings>
                                                </dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>TO</th>
                                            <td>
                                                <dx:ASPxDateEdit ID="de_to" runat="server" Theme="Moderno">
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
                                                <dx:ASPxDateEdit ID="de_from2" runat="server" Theme="Moderno">
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
                                                <dx:ASPxDateEdit ID="de_to2" runat="server" Theme="Moderno">
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
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table align="center">
                                    <tr>
                                        <th class="auto-style3">Status</th>
                                        <td align="left">
                                            <dx:ASPxComboBox ID="cb_status" runat="server" ValueType="System.Int32" Theme="Moderno"
                                                AutoPostBack="true" OnSelectedIndexChanged="cb_project_SelectedIndexChanged">
                                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Date value can not be null." ValidationGroup="validation"></ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <th class="auto-style3">Loaner</th>
                                        <td align="left">
                                            <dx:ASPxTextBox ID="tb_loaner" runat="server" Theme="Moderno">
                                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Date value can not be null." ValidationGroup="validation"></ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="auto-style3">Project</th>
                                        <td colspan="3" align="left">
                                            <dx:ASPxComboBox ID="cb_project" runat="server" ValueType="System.String" Theme="Moderno"
                                                AutoPostBack="true" OnSelectedIndexChanged="cb_project_SelectedIndexChanged">
                                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="FROM_Date value can not be null." ValidationGroup="validation"></ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="auto-style3">Cust Name</th>
                                        <td align="left">
                                            <dx:ASPxTextBox ID="tb_custName" runat="server" Theme="Moderno" ReadOnly="true"></dx:ASPxTextBox>
                                        </td>

                                        <th>Project Stage</th>
                                        <td align="left">
                                            <dx:ASPxTextBox ID="tb_pjStage" runat="server" Theme="Moderno" ReadOnly="true"></dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="auto-style3">Test Category</th>
                                        <td colspan="3" align="left">
                                            <dx:ASPxComboBox ID="cb_testCategory" runat="server" ValueType="System.String" Theme="Moderno">
                                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Test Category can not be null." ValidationGroup="validation"></ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="auto-style3">Comment</th>
                                        <td colspan="3" align="left">
                                            <dx:ASPxMemo ID="mo_comment" runat="server" Height="80px" Width="725px" Theme="Moderno"></dx:ASPxMemo>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="auto-style3">Review Comment</th>
                                        <td colspan="3" align="left">
                                            <dx:ASPxMemo ID="mo_reviewComment" runat="server" Height="80px" Width="725px" Theme="Moderno"></dx:ASPxMemo>
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
                                                <ClientSideEvents Click="function(s,e){window.close();}" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
