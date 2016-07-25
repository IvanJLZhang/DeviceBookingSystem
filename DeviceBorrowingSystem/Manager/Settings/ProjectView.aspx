<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectView.aspx.cs" Inherits="Manager_Settings_ProjectView" %>

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
            <h2>Project Management</h2>
            <dx:ASPxGridView ID="gv_project_view" runat="server" AutoGenerateColumns="False" Theme="Moderno"
                OnRowDeleting="gv_project_view_RowDeleting" OnRowUpdating="gv_project_view_RowUpdating" OnInitNewRow="gv_project_view_InitNewRow" OnRowInserting="gv_project_view_RowInserting"
                OnCellEditorInitialize="gv_project_view_CellEditorInitialize" OnStartRowEditing="gv_project_view_StartRowEditing" OnCancelRowEditing="gv_project_view_CancelRowEditing">
                <Columns>
                    <dx:GridViewDataTextColumn VisibleIndex="1" Caption="Project Code" FieldName="PJ_Code">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Project" FieldName="PJ_Name">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn VisibleIndex="3" Caption="Cust Name" FieldName="Cust_Name">
                        <HeaderStyle CssClass="gridViewHeader" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Project Stage" VisibleIndex="4" FieldName="pj_stage">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <EditItemTemplate>
                            <span style="position: static; float: left">
                                <dx:ASPxTextBox ID="tb_projectButton" runat="server" Text='<%# Bind("pj_stage") %>' ReadOnly="true" Width="75px" Theme="Moderno"></dx:ASPxTextBox>
                            </span>
                            <dx:ASPxButton ID="btn_addps" runat="server" Width="28px" ToolTip="Edit Project Stage" CommandName="EditPS" CommandArgument='<%# Bind("PJ_Code") %>' OnCommand="btn_addps_Command">
                                <Image Url="../../Images/Edit.png"></Image>
                                <Border BorderWidth="0" />
                            </dx:ASPxButton>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn ButtonType="Image" ShowDeleteButton="True" ShowNewButton="true" ShowEditButton="true" VisibleIndex="5" Caption="Command" ShowClearFilterButton="True">
                        <HeaderStyle CssClass="gridViewHeader" />
                        <DeleteButton Image-Url="../../Images/Delete.png">
<Image Url="../../Images/Delete.png"></Image>
                        </DeleteButton>
                        <EditButton Image-Url="../../Images/Edit.png">
<Image Url="../../Images/Edit.png"></Image>
                        </EditButton>
                        <NewButton Image-Url="../../Images/addGrey.png">
<Image Url="../../Images/addGrey.png"></Image>
                        </NewButton>
                    </dx:GridViewCommandColumn>

                </Columns>
                <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
                <SettingsBehavior ConfirmDelete="true" AllowFocusedRow="true" />
                <SettingsPager PageSize="15"></SettingsPager>
                <SettingsText ConfirmDelete="Do you want to delete the project?" />
                <Styles CommandColumn-Spacing="3px">
                    <CommandColumn Spacing="3px"></CommandColumn>
                </Styles>
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
