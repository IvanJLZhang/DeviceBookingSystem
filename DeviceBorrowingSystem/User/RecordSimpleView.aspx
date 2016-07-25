<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecordSimpleView.aspx.cs" Inherits="User_RecordSampleView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../CSS/Manager_Home.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center" style="margin-top: 10px">
            <dx:ASPxGridView ID="gv_recordView" runat="server" Theme="Moderno" AutoGenerateColumns="False" OnInit="gv_recordView_Init"
                 OnRowDeleting="gv_recordView_RowDeleting">
                <Columns>
                    <dx:GridViewDataTextColumn VisibleIndex="0" Caption="d_id" FieldName="s_id" Visible="false">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <CellStyle Wrap="True"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="1" Caption="Device" FieldName="s_name">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <CellStyle Wrap="True"></CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Recurrence" FieldName="db_is_recurrence">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn VisibleIndex="11" ShowDeleteButton="true" ButtonType="Image">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <DeleteButton Image-Url="../../Images/Delete.png">
<Image Url="../../Images/Delete.png"></Image>
                        </DeleteButton>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataDateColumn Caption="From" FieldName="Loan_DateTime" VisibleIndex="4">
                        <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                        </PropertiesDateEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn Caption="To" FieldName="Plan_To_ReDateTime" VisibleIndex="6">
                        <PropertiesDateEdit DisplayFormatString="yyyy/MM/dd HH:mm">
                        </PropertiesDateEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTimeEditColumn Caption="Start" FieldName="db_start" VisibleIndex="8">
                        <PropertiesTimeEdit DisplayFormatString="hh:mm">
                        </PropertiesTimeEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTimeEditColumn>
                    <dx:GridViewDataTimeEditColumn Caption="End" FieldName="db_end" VisibleIndex="10">
                        <PropertiesTimeEdit DisplayFormatString="hh:mm">
                        </PropertiesTimeEdit>
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTimeEditColumn>
                </Columns>
                <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="true"></SettingsBehavior>
                <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                <SettingsDataSecurity AllowDelete="True" />
                <SettingsText ConfirmDelete="Do you want to delete the reocrd?" />
            </dx:ASPxGridView>
            <script type="text/javascript">
                function EndCallBack(s, e) {
                    if (s.cpAlertMsg != "" && s.cpAlertMsg != null) {
                        alert(s.cpAlertMsg);
                        s.cpAlertMsg = null;
                    }
                }
            </script>
        </div>
    </form>
</body>
</html>
