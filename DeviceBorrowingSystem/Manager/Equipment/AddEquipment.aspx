<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEquipment.aspx.cs" Inherits="Manager_AddEquipment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../CSS/MainStyleSheet.css" rel="Stylesheet" />
    <style type="text/css">
        #EquipmentImg {
            FILTER: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }

        .auto-style1 {
            width: 90px;
            text-align: left;
        }

        .auto-style2 {
            width: 185px;
            text-align: left;
        }
    </style>
    <script type="text/javascript" language="javascript" src="../JS/MainJavaScript.js"></script>
    <script type="text/javascript" language="javascript">
        function showDiv() {
            var divDL = document.getElementById("divDL");
            if (divDL.style.visibility == "hidden")
                divDL.style.visibility = "visible";
            else
                divDL.style.visibility = "hidden";
        }
    </script>
    <title>Add</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <div id="divBaseInfo">
                <table id="tableDeviceInfo">
                    <tr align="center">
                        <td align="left" style="width: 90px">
                            <asp:Label ID="Label1" runat="server" Text="Name"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tb_EquipmentName" runat="server" Width="200px"></asp:TextBox><span style="color: red">*</span>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="name can not be null" ControlToValidate="tb_EquipmentName"
                                ValidationGroup="AddEquipment" Text="*"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="left" style="width: 70px">
                            <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tb_Introduce" runat="server" TextMode="MultiLine" Height="69px" Width="450px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="left">
                            <asp:Label ID="Label3" runat="server" Text="Image"></asp:Label>
                        </td>
                    </tr>
                    <tr align="center">
                        <td></td>
                        <td align="left">
                            <asp:Image ID="equipmentImg" AlternateText="device image" runat="server" Width="120px" Height="140px" BorderStyle="Dotted" BorderWidth="1px" />
                            <br />
                            <asp:FileUpload ID="idFile" accept="image/*" runat="server" onchange="__doPostBack('LinkButton2', '')" Width="120px" ToolTip="Select a device picture." />
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <hr align="left" style="width: 560px" />
            <div id="divDetail" style="width: 550px">
                <table id="tbl_summary" border="1" cellspacing="0px">
                    <tr>
                        <td class="auto-style1">Asset_ID: </td>
                        <td class="auto-style2">
                            <asp:TextBox ID="tb_assetID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Owner Name: </td>
                        <td>
                            <asp:UpdatePanel ID="upd1" runat="server">
                                <ContentTemplate>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT P_ID, P_Name FROM tbl_Person WHERE (P_Role &gt;= 1)"></asp:SqlDataSource>
                                    <script language="javascript" type="text/javascript">
                                        function SetOwner() {
                                            var clientID = '<%= tb_ownername.ClientID %>';
                                            var tbcbd = window.document.getElementById(clientID);
                                            var retValue = window.showModalDialog('OwnerSelect.aspx', '', '');
                                            if (retValue != null) {
                                                tbcbd.value = retValue;
                                            }
                                        }
                                    </script>
                                    <input type="text" id="tb_ownername" onclick="SetOwner();" runat="server" readonly="readonly" /><span style="color: red">*</span>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Owner Name can not be null!" ControlToValidate="tb_ownername"
                                ValidationGroup="AddEquipment" Height="1px">*</asp:RequiredFieldValidator>
                        </td>
                        <td class="auto-style1">Site: </td>
                        <td class="auto-style2">
                            <asp:DropDownList ID="tb_location" runat="server" Height="20px" Width="67px">
                                <asp:ListItem Selected="True">WKS</asp:ListItem>
                                <asp:ListItem Selected="false">WHC</asp:ListItem>
                                <asp:ListItem>WCH</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Status: </td>
                        <td class="auto-style2">
                            <%--<asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>--%>
                            <asp:DropDownList ID="ddl_status" runat="server">
                                <asp:ListItem Selected="True" Value="1">Usable</asp:ListItem>
                                <asp:ListItem Value="2">Broken</asp:ListItem>
                                <asp:ListItem Value="3">Lost</asp:ListItem>
                                <asp:ListItem Value="4">Return to Custumer</asp:ListItem>
                                <asp:ListItem Value="5">NotForCirculation</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Vender: </td>
                        <td class="auto-style2">
                            <asp:TextBox ID="tb_vender" runat="server"></asp:TextBox>
                        </td>
                        <td class="auto-style1">Cost: </td>
                        <td class="auto-style2">
                            <asp:TextBox ID="tb_cost" runat="server"></asp:TextBox>
                        </td>
                    </tr>

                    <asp:Panel ID="pnl_device" runat="server" Visible="false">
                        <tr>
                            <td class="auto-style1">Custom_ID: </td>
                            <td class="auto-style2">
                                <asp:TextBox ID="tb_Custom_ID" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Class: </td>
                            <td>
                                <asp:TextBox ID="tb_deviceClass" runat="server" Visible="True"></asp:TextBox>
<%--                                <asp:DropDownList ID="ddl_class" runat="server" Visible="true">
                                    <asp:ListItem Selected="True" Text="NULL" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Outside" Value="Outside"></asp:ListItem>
                                    <asp:ListItem Text="Inside" Value="Inside"></asp:ListItem>
                                    <asp:ListItem Text="Chamber" Value="Chamber"></asp:ListItem>
                                    <asp:ListItem Text="Non-chamber" Value="Non-chamber"></asp:ListItem>
                                </asp:DropDownList>--%>
                            </td>
                            <td class="auto-style1">Interface: </td>
                            <td class="auto-style2">
                                <asp:TextBox ID="tb_interface" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </asp:Panel>
                    <asp:Panel ID="pnl_equipment_chamber" runat="server" Visible="false">
                        <tr>
                            <td class="auto-style1">Testing Time: </td>
                            <td>
                                <asp:TextBox ID="tb_testTime" runat="server" Width="70px">00:00</asp:TextBox>
                                <asp:Label ID="format" runat="server" Text="(e.g.=hh:mm)" ForeColor="Red"></asp:Label>
                            </td>
                            <td class="auto-style1">Working Hours: </td>
                            <td class="auto-style2">
                                <asp:TextBox ID="tb_avghr" runat="server" Width="70px">24</asp:TextBox><span style="color: red">hours/day</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Lab Location: </td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddl_labLocation" runat="server" Height="20px" Width="67px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </asp:Panel>
                </table>
            </div>
            <br />
            <br />
            <hr style="width: 560px;" align="left" />
            <div id="divOper">
                <table>
                    <tr>
                        <td style="width: 197px">
                            <asp:Button ID="btn_Save" runat="server" Text="ADD" OnClick="btn_Save_Click" Height="30px" Width="84px" ValidationGroup="AddEquipment" />
                        </td>
                        <td style="width: 217px" align="right">
                            <asp:Button ID="btn_Cancel" runat="server" Text="CANCEL" OnClick="btn_Cancel_Click" Height="30px" Width="84px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="AddEquipment" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
