<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProjectStageView.aspx.cs" Inherits="Manager_Settings_ProjectStageView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <table style="margin-top: 20px">
                <tr>
                    <td>
                        <dx:ASPxLabel ID="lbl_pj_code" runat="server" Text="asdfadf" Theme="iOS"></dx:ASPxLabel>

                    </td>
                    <td>
                        <dx:ASPxLabel ID="lb_pj_namel" runat="server" Text="asdasdf" Theme="iOS"></dx:ASPxLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h3>Project Stage</h3>
                        <dx:ASPxGridView ID="gv_project_stage_view" runat="server" AutoGenerateColumns="False" Theme="Moderno"
                            OnRowDeleting="gv_project_stage_RowDeleting" OnRowUpdating="gv_project_stage_RowUpdating" OnInitNewRow="gv_project_stage_InitNewRow" OnRowInserting="gv_project_stage_RowInserting"
                            OnRowInserted="gv_project_stage_view_RowInserted">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Project Stage" VisibleIndex="4" FieldName="ps_stage">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn VisibleIndex="5" Caption="START" FieldName="ps_from">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                    <PropertiesDateEdit AllowUserInput="false" DisplayFormatString="yyyy/MM/dd"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn VisibleIndex="6" Caption="END" FieldName="ps_to">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                    <PropertiesDateEdit AllowUserInput="false" DisplayFormatString="yyyy/MM/dd"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewCommandColumn ButtonType="Image" ShowNewButton="true" ShowDeleteButton="True" ShowEditButton="true" VisibleIndex="9" Caption="Command" ShowClearFilterButton="True">
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
                                <dx:GridViewDataComboBoxColumn Caption="Status" VisibleIndex="8" FieldName="ps_status">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                    <PropertiesComboBox>
                                        <Items>
                                            <dx:ListEditItem Text="ACTIVATE" Value="1" />
                                            <dx:ListEditItem Text="NO_ACTIVATE" Value="0" />
                                        </Items>
                                    </PropertiesComboBox>
                                    <EditItemTemplate></EditItemTemplate>
                                    <EditFormSettings Caption=" " />
                                </dx:GridViewDataComboBoxColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                            <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
                            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
                            <SettingsBehavior ConfirmDelete="true" AllowFocusedRow="true" />
                            <SettingsPager PageSize="15"></SettingsPager>
                            <SettingsText ConfirmDelete="Do you want to delete the project?" />
                            <Styles CommandColumn-Spacing="3px">
                                <CommandColumn Spacing="3px"></CommandColumn>
                            </Styles>
                        </dx:ASPxGridView>
                    </td>
                </tr>
                <%--                <tr>
                    <td colspan="2">
                        <h3>Histrory or Not Activate yet </h3>
                        <dx:ASPxGridView ID="gv_pj_notActivate" runat="server" AutoGenerateColumns="False" Theme="Moderno"
                            OnRowDeleting="gv_project_stage_RowDeleting" OnRowUpdating="gv_project_stage_RowUpdating" OnInitNewRow="gv_project_stage_InitNewRow" OnRowInserting="gv_project_stage_RowInserting">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Project Stage" VisibleIndex="4" FieldName="ps_stage">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn VisibleIndex="5" Caption="START" FieldName="ps_from">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                    <PropertiesDateEdit AllowUserInput="false" DisplayFormatString="yyyy/MM/dd"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn VisibleIndex="6" Caption="END" FieldName="ps_to">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                    <PropertiesDateEdit AllowUserInput="false" DisplayFormatString="yyyy/MM/dd"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewCommandColumn ButtonType="Image" ShowNewButton="true" ShowDeleteButton="True" ShowEditButton="true" VisibleIndex="7" Caption="Command" ShowClearFilterButton="True">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                    <DeleteButton Image-Url="../../Images/Delete.png">
                                    </DeleteButton>
                                    <EditButton Image-Url="../../Images/Edit.png">
                                    </EditButton>
                                    <NewButton Image-Url="../../Images/addGrey.png"></NewButton>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataComboBoxColumn Caption="Status" VisibleIndex="6" FieldName="ps_status">
                                    <HeaderStyle CssClass="gridViewHeader" />
                                    <PropertiesComboBox>
                                        <Items>
                                            <dx:ListEditItem Text="ACTIVATE" Value="1" />
                                            <dx:ListEditItem Text="NO_ACTIVATE" Value="0" />
                                        </Items>
                                    </PropertiesComboBox>
                                </dx:GridViewDataComboBoxColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                            <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
                            <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True" />
                            <SettingsBehavior ConfirmDelete="true" AllowFocusedRow="true" />
                            <SettingsPager PageSize="15"></SettingsPager>
                            <SettingsText ConfirmDelete="Do you want to delete the project?" />
                            <Styles CommandColumn-Spacing="3px">
                                <CommandColumn Spacing="3px"></CommandColumn>
                            </Styles>
                        </dx:ASPxGridView>
                    </td>
                </tr>--%>
            </table>

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
