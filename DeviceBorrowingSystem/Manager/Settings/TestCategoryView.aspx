<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestCategoryView.aspx.cs" Inherits="Manager_Settings_TestCategoryView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <h2>Test Category Management</h2>
            <dx:ASPxGridView ID="gv_purpose_view" runat="server" AutoGenerateColumns="False" Theme="Moderno"
                OnRowDeleting="gv_purpose_view_RowDeleting" OnRowUpdating="gv_purpose_view_RowUpdating" OnInitNewRow="gv_purpose_view_InitNewRow" OnRowInserting="gv_purpose_view_RowInserting">
                <Columns>
                    <dx:GridViewDataTextColumn VisibleIndex="1" Caption="ID" FieldName="ID">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <EditItemTemplate>
                            <dx:ASPxTextBox ID="lbl_id" runat="server" Text='<%# Bind("ID") %>' ReadOnly="true"></dx:ASPxTextBox>
                        </EditItemTemplate>
                        
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Test Category" FieldName="Name">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <EditItemTemplate>
                            <dx:ASPxTextBox ID="lbl_name" runat="server" Text='<%# Bind("Name") %>'></dx:ASPxTextBox>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn ButtonType="Image" ShowDeleteButton="True" ShowEditButton="true" ShowNewButton="true" VisibleIndex="4" Caption="Command" ShowClearFilterButton="True">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <DeleteButton Image-Url="../../Images/Delete.png">
                        </DeleteButton>
                        <EditButton Image-Url="../../Images/Edit.png"></EditButton>
                        <NewButton Image-Url="../../Images/addGrey.png"></NewButton>
                    </dx:GridViewCommandColumn>
                </Columns>
                <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
                <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
                <SettingsBehavior ConfirmDelete="true" AllowFocusedRow="true" />
                <SettingsPager PageSize="15"></SettingsPager>
                <SettingsText ConfirmDelete="Do you want to delete the test category?" />
                <Styles CommandColumn-Spacing="3px"></Styles>
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
