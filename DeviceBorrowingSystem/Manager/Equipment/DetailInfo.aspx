<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetailInfo.aspx.cs" Inherits="Manager_Equipment_DetailInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .hideFile {
            display: none;
            /*visibility:hidden;*/
        }

        .auto-style1 {
            width: 115px;
            text-align: left;
        }

        .auto-style2 {
            width: 170px;
            text-align: left;
        }
    </style>
    <style type="text/css">
        #EquipmentImg {
            FILTER: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale);
        }

        .auto-style3 {
            width: 201px;
        }

        .auto-style4 {
            width: 220px;
        }

        .auto-style5 {
            width: 215px;
        }
    </style>
    <script type="text/javascript" src="../JS/MainJavaScript.js"></script>
    <script language="javascript" type="text/javascript">

        //function SelectImgFile() {
        //    document.getElementById("selectImg").click();
        //}

        function showDiv() {
            var divDL = document.getElementById("divDL");
            if (divDL.style.visibility == "hidden")
                divDL.style.visibility = "visible";
            else
                divDL.style.visibility = "hidden";
        }

        function PreviewImg(imgFile) {
            var img = document.getElementById("img_DeviceImg");
            img.src = "";
            var newPreview = document.getElementById("equipmentImg");
            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="width:650px">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:Panel ID="panel_BaseInfoView" runat="server" Height="100%">
                <table id="TablBaseInfo">
                    <tr>
                        <td align="center" style="width: 110px; height: 110px">
                            <asp:Image ID="equipmentImg" AlternateText="device image" runat="server" Width="100px" Height="80px" BorderStyle="Dotted" BorderWidth="1px" />
                            <br />
                            <asp:FileUpload ID="idFile" accept="image/*" runat="server" onchange="__doPostBack('LinkButton2', '')" Width="100px" ToolTip="Select a device picture." />
                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton>
                            <asp:HiddenField ID="PreImg" runat="server" />
                        </td>
                        <td style="vertical-align: top; text-align: left; width: 440px">
                            <asp:TextBox ID="tb_EquipmentName" runat="server" BorderStyle="Dotted" BorderWidth="1px" Width="254px"
                                Text="asdfasdf" Height="16px"></asp:TextBox>


                            <br />
                            <br />
                            <asp:TextBox ID="tb_Introduce" runat="server" BorderStyle="Solid" TextMode="MultiLine" Height="62px" Width="421px"
                                BorderWidth="1px"
                                Text="dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd"></asp:TextBox>


                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
                <br />
                <hr />
                <div id="divDetail">
                    <asp:Panel ID="pnl_summary" runat="server">
                        <table id="tbl_summary" border="1" cellspacing="0px">
                            <tr>
                                <td class="auto-style1">Device ID: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_deviceId" runat="server" ReadOnly="True"></asp:TextBox>
                                </td>
                                <td class="auto-style1">Asset_ID: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_assetID" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">Owner: </td>
                                <td class="auto-style2">
                                    <asp:UpdatePanel ID="upd1" runat="server">
                                        <ContentTemplate>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" SelectCommand="SELECT P_ID, P_Name FROM tbl_Person WHERE (P_Role &gt;= 1)"></asp:SqlDataSource>
                                            <script language="javascript" type="text/javascript">
                                                function SetOwner() {

                                                    var ownerdpt = '<%= GetPersonInfoByName() %>';


                                                    var clientID = '<%= tb_ownername.ClientID %>';
                                                    var tbcbd = this.document.getElementById(clientID);
                                                    var retValue = window.showModalDialog('OwnerSelect.aspx?dpt=' + ownerdpt, '', '');
                                                    if (retValue != null) {
                                                        tbcbd.value = retValue;
                                                    }
                                                }
                                            </script>
                                            <input type="text" id="tb_ownername" onclick="SetOwner();" runat="server" readonly="readonly" /><span style="color: red">*</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="auto-style1">Site: </td>
                                <td class="auto-style2">
                                    <asp:Label ID="lbl_site" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">Status: </td>
                                <td class="auto-style2">
                                    <asp:DropDownList ID="ddl_status" runat="server">
                                        <asp:ListItem Selected="True" Value="1">Usable</asp:ListItem>
                                        <asp:ListItem Value="2">Broken</asp:ListItem>
                                        <asp:ListItem Value="3">Lost</asp:ListItem>
                                        <asp:ListItem Value="4">Return to Custumer</asp:ListItem>
                                        <asp:ListItem Value="5">NotForCirculation</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td></td>

                            </tr>
                            <tr>
                                <td class="auto-style1">Vender: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_vender" runat="server" Height="16px"></asp:TextBox>
                                </td>
                                <td class="auto-style1">Cost: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_cost" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:Panel ID="pnl_device" runat="server" Visible="false">
                        <table id="tbl_device" border="1" cellspacing="0px">
                            <tr>
                                <td class="auto-style1">Custom_ID: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_Custom_ID" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">Class: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_Class" runat="server" Visible="True"></asp:TextBox>
<%--                                    <asp:DropDownList ID="ddl_class" runat="server" Visible="True">
                                        <asp:ListItem Selected="True" Text="NULL"></asp:ListItem>
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
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnl_equipment_chamber" runat="server" Visible="false">
                        <table id="tbl_equipment_chamber" border="1" cellspacing="0px">
                            <tr>
                                <td class="auto-style1">Testing Time: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_testTime" runat="server">00:00</asp:TextBox>
                                </td>
                                <td class="auto-style1">Working Hours: </td>
                                <td class="auto-style2">
                                    <asp:TextBox ID="tb_avghr" runat="server">24</asp:TextBox>
                                    <span style="color: red">hours/day</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="auto-style1">Lab Location: </td>
                                <td class="auto-style2">
                                    <asp:DropDownList ID="ddl_labLocation" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <br />
                <hr align="left"/>
                <div style="width: 550px; text-align: center">
                    <asp:Button ID="Button1" runat="server" Text="UPDATE" Width="75px" OnClick="Button1_Click" />


                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <input id="Button3" type="button" value="CANCEL" style="width: 75px" onclick="window.close();" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
