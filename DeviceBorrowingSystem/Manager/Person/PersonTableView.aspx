<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonTableView.aspx.cs" Inherits="Manager_Person_PersonTableView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../../CSS/MainStyleSheet.css" rel="stylesheet" />
    <link href="../../CSS/Manager_Home.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div align="center">
                    <dx:ASPxGridView ID="gv_PersonTable" runat="server" AutoGenerateColumns="False" Theme="Moderno" Caption="Person Table View"
                        OnRowDeleting="gv_PersonTable_RowDeleting" OnCustomButtonCallback="gv_PersonTable_CustomButtonCallback"
                        OnInit="gv_PersonTable_Init">
                        <Columns>
                            <dx:GridViewDataTextColumn Caption="UID" VisibleIndex="0" FieldName="P_ID">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="User Name" VisibleIndex="1" FieldName="P_Name">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn Caption="Site" VisibleIndex="2" FieldName="P_Location">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Value="WKS" Text="WKS" />
                                        <dx:ListEditItem Value="WHC" Text="WHC" />
                                        <dx:ListEditItem Value="WCH" Text="WCH" />
                                        <dx:ListEditItem Value="WHQ" Text="WHQ" />
                                        <dx:ListEditItem Value="WIH" Text="WIH" />
                                        <dx:ListEditItem Value="WZS" Text="WZS" />
                                        <dx:ListEditItem Value="WPH" Text="WPH" />
                                        <dx:ListEditItem Value="WMX" Text="WMX" />
                                        <dx:ListEditItem Value="WCZ" Text="WCZ" />
                                    </Items>
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataTextColumn Caption="Department" VisibleIndex="3" FieldName="P_Department">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Email" VisibleIndex="4" FieldName="P_Email">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="ExNumber" VisibleIndex="5" FieldName="P_ExNumber">
                                <HeaderStyle CssClass="gridViewHeader" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataComboBoxColumn Caption="Role" VisibleIndex="6" FieldName="P_Role">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Text="User" Value="0" />
                                        <dx:ListEditItem Text="Reviewer" Value="10" />
                                        <dx:ListEditItem Text="Leader" Value="11" />
                                        <dx:ListEditItem Text="Admin" Value="20" />
                                    </Items>
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataComboBoxColumn Caption="Activate" VisibleIndex="7" FieldName="P_Activate">
                                <HeaderStyle CssClass="gridViewHeader" />
                                <PropertiesComboBox>
                                    <Items>
                                        <dx:ListEditItem Text="TRUE" Value="True" />
                                        <dx:ListEditItem Text="FALSE" Value="False" />
                                    </Items>
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewCommandColumn Caption="Command" VisibleIndex="8" ShowDeleteButton="true" ButtonType="Image"
                                ShowClearFilterButton="true">
                                <CustomButtons>
                                    <dx:GridViewCommandColumnCustomButton ID="btn_edit">
                                        <Image Url="~/Images/Edit.png">
                                        </Image>

                                    </dx:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                                <HeaderStyle CssClass="gridViewHeader" />
                                <DeleteButton Image-Url="../../Images/Delete.png">
                                    <Image Url="../../Images/Delete.png">
                                    </Image>
                                </DeleteButton>
                            </dx:GridViewCommandColumn>
                        </Columns>
                        <ClientSideEvents EndCallback="function(s,e){EndCallBack(s,e);}" />
                        <Settings ShowFilterRow="true" ShowFilterBar="Auto" />
                        <SettingsBehavior ConfirmDelete="true" AllowFocusedRow="true" />
                        <SettingsPager PageSize="15"></SettingsPager>
                        <SettingsText ConfirmDelete="Do you want to delete this person?" />
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

                            if (s.cpType != null && s.cpType != "") {
                                if (s.cpType == "btn_edit") {
                                    var uid = s.cpID;
                                    window.open("./UserInfo.aspx?type=1&uid=" + uid + "", "_blank");
                                    s.cpType = s.cpID = "";
                                }
                            }
                        }
                    </script>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
