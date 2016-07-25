<%@ Page Title="" Language="C#" MasterPageFile="~/User/User_MasterPage.master" AutoEventWireup="true" CodeFile="BorrowingPage.aspx.cs" Inherits="User_BorrowingPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="~/UserCtrl/DatePicker.ascx" TagPrefix="uc1" TagName="DatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Borrowing Device/Equipment
    </title>
    <link rel="Stylesheet" type="text/css" href="../CSS/User_BorrowingPage.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function doSelect() {
            var dom = document.all;
            var el = event.srcElement;
            if (el.id.indexOf("chkHeader") >= 0 && el.tagName == "INPUT" && el.type.toLowerCase() == "checkbox") {
                var ischecked = false;
                if (el.checked)
                    ischecked = true;
                for (i = 0; i < dom.length; i++) {
                    if (dom[i].type == undefined) continue;
                    if (dom[i].id.indexOf("chkSelect") >= 0 && dom[i].tagName == "INPUT" && dom[i].type.toLowerCase() == "checkbox")
                        dom[i].checked = ischecked;
                }
            }
        }
    </script>
    <asp:UpdatePanel ID="up_conditionSearch" runat="server">
        <ContentTemplate>
            <div id="div_step1">
                <h3 style="text-align: left;">Step 1--Select Device/Equipment:
                </h3>
                <div id="div_navigate">
                    <span>Category: 
                            <asp:DropDownList ID="ddl_category" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_category_SelectedIndexChanged">
                                <asp:ListItem Value="1" Text="Device"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Equipment"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Chamber"></asp:ListItem>
                            </asp:DropDownList>
                    </span>
                    <span>Site:
                            <asp:DropDownList ID="ddl_location" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_location_SelectedIndexChanged" Width="100px"></asp:DropDownList>
                    </span>
                    <span>Department:
                            <asp:DropDownList ID="ddl_Department" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Department_SelectedIndexChanged" AppendDataBoundItems="False">
                                <asp:ListItem Selected="True" Text="ALL" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                    </span>
                </div>
                <asp:Panel ID="panel_DeviceShow" runat="server" Width="100%" Visible="false">
                    <div id="div_filter">
                        <span style="">Class: 
                            <asp:DropDownList ID="ddl_class" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_class_SelectedIndexChanged" Width="100px">
                            </asp:DropDownList>
                        </span>
                        <span style="">Interface: 
                            <asp:DropDownList ID="ddl_Interface" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_Interface_SelectedIndexChanged" Width="100px"></asp:DropDownList>
                            &nbsp;&nbsp;
                            Page Size: 
                            <asp:DropDownList ID="ddl_pagesize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_pagesize_SelectedIndexChanged" Width="100px">
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                            </asp:DropDownList>
                        </span>
                        <span style="float: right">
                            <asp:Table ID="tbl_search" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <dx:ASPxTextBox ID="tb_Search" runat="server" Width="170px" NullText="input search text"
                                            ToolTip="support search text:&#10;Device Name;&#10;Owner Name/ID;&#10;Class;&#10;Interface;&#10;Custom_ID">
                                        </dx:ASPxTextBox>
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:ImageButton ID="imgbtn_Search" runat="server" ImageUrl="~/Images/search.png" Height="20px" Width="20px" ToolTip="Search" OnClick="imgbtn_Search_Click" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>

                            <%--<asp:TextBox ID="tb_Search" runat="server" Width="150px" Height="20px"></asp:TextBox>--%>

                        </span>
                    </div>
                    <div align="center">
                        <asp:Panel ID="pnl_deviceShow" runat="server" Height="500px" ScrollBars="Auto">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical"
                                AllowPaging="True" OnRowCommand="GridView1_RowCommand" OnDataBinding="GridView1_DataBinding" OnRowDataBound="GridView1_RowDataBound" OnPageIndexChanging="GridView1_PageIndexChanging">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkHeader" runat="server" Checked="false" onclick="doSelect();" />ALL
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Device Name" SortExpression="Device_Name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="Label2" runat="server" Text='<%# Bind("[Device Name]") %>' CommandName="DeviceDetailInfo" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="300px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Custom_ID" HeaderText="Custom_ID">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Owner Name" SortExpression="P_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("[Owner Name]") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Class" SortExpression="Device_Class">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Class") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interface" SortExpression="Device_interface">
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Interface") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Site" SortExpression="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("Site") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemStyle Width="50px" />
                                        <ItemTemplate>
                                            <asp:Panel ID="pnl_bgsetting" runat="server" Width="80px">
                                                <asp:Label ID="lbl_borrowStatus" runat="server" Text='<%# GetBorrowStatus(DataBinder.Eval(Container.DataItem, "borrow_status")) %>'></asp:Label>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </asp:Panel>
                <asp:Panel ID="panel_EquipShow" runat="server" Width="100%" Visible="false">
                    <table style="width: 500px" align="center">
                        <tr>
                            <td align="left">
                                <asp:DataList ID="dl_EquipmentInfo" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" CellPadding="4"
                                    OnItemCommand="DataList1_ItemCommand" BorderStyle="None" BorderWidth="1px" GridLines="Both"
                                    OnDataBinding="dl_EquipmentInfo_DataBinding" OnItemDataBound="dl_EquipmentInfo_ItemDataBound">
                                    <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                    <ItemTemplate>
                                        <div>
                                            <table width="500px" style="max-height: 400px">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="CheckBox1" runat="server" BorderStyle="Solid" BorderWidth="1px" />
                                                    </td>
                                                    <td rowspan="2">
                                                        <asp:Image ID="ImageButton2" runat="server" Height="100px" Width="150px" ImageUrl='<%# Bind("ImageUrl") %>' />
                                                    </td>
                                                    <td style="vertical-align: top; text-align: left">
                                                        <asp:LinkButton ID="TextBox1" runat="server" Text='<%# Bind("[Device Name]") %>' Height="40px"
                                                            CommandName="EquipmentDetail" CommandArgument='<%# Bind("id") %>'></asp:LinkButton>
                                                        <br />
                                                        <br />
                                                        <asp:Label ID="TextBox2" runat="server" TextMode="MultiLine" Height="72px" Width="220px" BorderStyle="None" Text='<%# Bind("Note") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ItemTemplate>
                                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                    <table align="right">
                        <tr>
                            <td style="width: 579px; text-align: right; font-size: 9pt;" colspan="2" align="right">
                                <td style="width: 579px; text-align: right; font-size: 9pt;" colspan="2" align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Current："></asp:Label>
                                    [
                                <asp:Label ID="labPage" runat="server" Text="1"></asp:Label>
                                    &nbsp;]&nbsp;&nbsp;
                                <asp:Label ID="Label3" runat="server" Text="All："></asp:Label>
                                    [
                                <asp:Label ID="labBackPage" runat="server"></asp:Label>
                                    &nbsp;]&nbsp;&nbsp;<asp:LinkButton ID="lnkbtnOne" runat="server" Font-Underline="False"
                                        OnClick="lnkbtnOne_Click" ToolTip="First Page" BorderStyle="Solid" BorderWidth="1px">|<</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkbtnUp" runat="server" Font-Underline="False"
                                OnClick="lnkbtnUp_Click" ToolTip="Pre Page" BorderStyle="Solid" BorderWidth="1px" Width="35px">< Pre</asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkbtnNext" runat="server" Font-Underline="False"
                                OnClick="lnkbtnNext_Click" ToolTip="Next Page" BorderStyle="Solid" BorderWidth="1px" Width="35px">Next ></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkbtnBack" runat="server" Font-Underline="False"
                                    OnClick="lnkbtnBack_Click" ToolTip="Last Page" BorderStyle="Solid" BorderWidth="1px">>|</asp:LinkButton>&nbsp;&nbsp;</td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <hr />
            </div>
            <div id="div_step2">
                <h3 style="text-align: left;">Step 2--Pick Date&Time:
                </h3>
                <hr />
                <div id="div_pickDate">
                    <span><font style="font-weight: bold">DATE:</font>FROM:                           
                        <asp:TextBox runat="server" ID="tb_StartDate" Text='<%# DateTime.Now.ToString("yyyy/MM/dd") %>'></asp:TextBox>
                        <img src="../Images/calendar_month.png" runat="server" id="imgCal" />
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tb_StartDate" PopupButtonID="imgCal"></cc1:CalendarExtender>
                    </span>
                    <span>TO:
                        <asp:TextBox runat="server" ID="tb_endDate" Text='<%# DateTime.Now.ToString("yyyy/MM/dd") %>'></asp:TextBox>
                        <img src="../Images/calendar_month.png" runat="server" id="img1" />
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tb_endDate" PopupButtonID="img1"></cc1:CalendarExtender>
                    </span>
                    <asp:Button ID="btn_Serach" runat="server" Text="Search" OnClick="btn_Serach_Click" />
                </div>
                <asp:Panel ID="panel_selTimeEquip" runat="server" Visible="false">
                    <div id="divSelTime" align="center">
                        <asp:DataList ID="dl_Seltime" runat="server" RepeatDirection="Horizontal" RepeatColumns="1"
                            OnItemDataBound="dl_Seltime_ItemDataBound" OnDataBinding="dl_Seltime_DataBinding"
                            OnItemCommand="dl_Seltime_ItemCommand">
                            <ItemTemplate>
                                <div id="div_itemTemplate">
                                    <div id="div_deviceName" class="div_deviceName">
                                        <asp:LinkButton ID="lnkBtn_DeviceName" runat="server" Text='<%# Bind("Device_Name") %>' CommandName="EquipmentDetail" CommandArgument='<%# Bind("Device_ID") %>'></asp:LinkButton>
                                    </div>
                                    <div id="div_filterOrCommand">
                                        <span style="float: left; margin-left: 20px;">TimeZone: 
                                            <asp:DropDownList ID="ddl_TimeZone" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_TimeZone_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="WHC">WHC(GMT+8)</asp:ListItem>
                                                <asp:ListItem Value="WKS">WKS(GMT+8)</asp:ListItem>
                                                <asp:ListItem Value="WCH">WCH(GMT-6)</asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                        <span style="float: right; margin-right: 50px;">
                                            <asp:ImageButton ID="ibtn_ADD" runat="server" ImageUrl="~/Images/addGrey.png" CommandName="Add" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Device_ID").ToString() + "," + DataBinder.Eval(Container.DataItem, "Device_Name").ToString() %>' ToolTip="add time duration" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtn_Clear" runat="server" ImageUrl="~/Images/erase.png" CommandName="Clear" ToolTip="clear selection" />
                                        </span>
                                    </div>
                                    <div id="div_dateTime">
                                        <table border="1">
                                            <tr align="left" style="font-size: small">
                                                <th align="left" style="width: 85px; font-size: large">Date\Time
                                                </th>
                                                <td style="width: 32px">00:00</td>
                                                <td style="width: 32px">01:00</td>
                                                <td style="width: 32px">02:00</td>
                                                <td style="width: 32px">03:00</td>
                                                <td style="width: 32px">04:00</td>
                                                <td style="width: 32px">05:00</td>
                                                <td style="width: 32px">06:00</td>
                                                <td style="width: 32px">07:00</td>
                                                <td style="width: 32px">08:00</td>
                                                <td style="width: 32px">09:00</td>
                                                <td style="width: 32px">10:00</td>
                                                <td style="width: 32px">11:00</td>
                                                <td style="width: 32px">12:00</td>
                                                <td style="width: 32px">13:00</td>
                                                <td style="width: 32px">14:00</td>
                                                <td style="width: 32px">15:00</td>
                                                <td style="width: 32px">16:00</td>
                                                <td style="width: 32px">17:00</td>
                                                <td style="width: 32px">18:00</td>
                                                <td style="width: 32px">19:00</td>
                                                <td style="width: 32px">20:00</td>
                                                <td style="width: 32px">21:00</td>
                                                <td style="width: 32px">22:00</td>
                                                <td style="width: 32px">23:00</td>
                                            </tr>
                                            <tr>
                                                <td colspan="25" align="left" style="text-wrap: avoid">
                                                    <asp:DataList ID="dl_SubSelTime" runat="server" RepeatDirection="Vertical" OnItemCommand="dl_SubSelTime_ItemCommand">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Date" runat="server" Width="85px" Font-Bold="true" Font-Size="Large" Text='<%# DateTime.Parse((DataBinder.Eval(Container.DataItem, "DateTime")).ToString()).ToString("yyyy/MM/dd") %>'></asp:Label>

                                                            <asp:LinkButton ID="lb0_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb0_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb1_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb1_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb2_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb2_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb3_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb3_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb4_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb4_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb5_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb5_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb6_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb6_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb7_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb7_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb8_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb8_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb9_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb9_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb10_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb10_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb11_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb11_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb12_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb12_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb13_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb13_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb14_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb14_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb15_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb15_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb16_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb16_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb17_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb17_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb18_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb18_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb19_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb19_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb20_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb20_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb21_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb21_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb22_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb22_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb23_0" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                            <asp:LinkButton ID="lb23_5" runat="server" Font-Underline="false" BackColor="AliceBlue" Width="15px" CommandName="SelTime" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>

                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="div_hiddenFiend">
                                        <asp:HiddenField ID="hf_SelTime" runat="server" Value='~' />
                                        <asp:HiddenField ID="hf_SelTimeZone" runat="server" />
                                    </div>
                                    <br />
                                    <div id="pickTimeList">
                                        <asp:GridView ID="gv_pickTimeList" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                            OnRowCommand="gv_pickTimeList_RowCommand" HorizontalAlign="Left">
                                            <Columns>
                                                <asp:BoundField HeaderText="Start DateTime" ItemStyle-Width="200px" DataField="StartDateTime" DataFormatString="{0:yyyy/MM/dd/ HH:mm}">
                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="End DateTime" ItemStyle-Width="200px" DataField="EndDateTime" DataFormatString="{0:yyyy/MM/dd/ HH:mm}">
                                                    <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemStyle Width="50px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtn_Delete" runat="server" CausesValidation="False" CommandName="DeleteRow" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "," + DataBinder.Eval(Container.DataItem, "Device_ID").ToString() %>' Text="DELETE"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </asp:Panel>
                <asp:Panel ID="panel_SelDateDevice" runat="server" Visible="false">
                    <asp:DataList ID="DeviceDataSel" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" HorizontalAlign="Center"
                        OnItemDataBound="DeviceDataSel_ItemDataBound" OnItemCommand="dl_Seltime_ItemCommand">
                        <ItemTemplate>
                            <table align="center">
                                <tr>
                                    <th colspan="8">
                                        <asp:LinkButton ID="lnkBtn_DeviceName" runat="server" Text='<%# Bind("Device_Name") %>' CommandName="EquipmentDetail" CommandArgument='<%# Bind("Device_ID") %>'></asp:LinkButton>
                                    </th>
                                </tr>
                                <tr>
                                    <td align="left" colspan="7">
                                        <asp:DropDownList ID="ddl_TimeZone_Device" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_TimeZone_Device_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="WHC">WHC(GMT+8)</asp:ListItem>
                                            <asp:ListItem Value="WKS">WKS(GMT+8)</asp:ListItem>
                                            <asp:ListItem Value="WCH">WCH(GMT-6)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:ImageButton ID="ibtn_Add" runat="server" CommandName="Add" ImageUrl="~/Images/addGrey.png" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Device_ID").ToString() + "," + DataBinder.Eval(Container.DataItem, "Device_Name").ToString() %>' ToolTip="add time duration" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="lbtn_clear" runat="server" CommandName="Clear" ImageUrl="~/Images/erase.png" ToolTip="clear selection"></asp:ImageButton>
                                    </td>
                                </tr>
                                <tr style="text-transform: uppercase">
                                    <th style="width: 165px">Date/Week</th>
                                    <th style="width: 80px;">Sunday</th>
                                    <th style="width: 80px">Monday</th>
                                    <th style="width: 80px">Tuesday</th>
                                    <th style="width: 80px">Wednesday</th>
                                    <th style="width: 80px">THURSDAY</th>
                                    <th style="width: 80px">Friday</th>
                                    <th style="width: 80px">Satday</th>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <asp:DataList ID="dl_date" runat="server" RepeatDirection="Vertical" OnItemCommand="dl_date_ItemCommand">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_date" runat="server" Width="165px" Text='<%# Bind("week") %>'></asp:Label>
                                                <asp:LinkButton ID="lbtn_day0" runat="server" Font-Underline="false" Width="80px" Enabled="false" BackColor="Gray" CommandName="SelDay" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>
                                                <asp:LinkButton ID="lbtn_day1" runat="server" Font-Underline="false" Width="80px" Enabled="false" BackColor="Gray" CommandName="SelDay" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>
                                                <asp:LinkButton ID="lbtn_day2" runat="server" Font-Underline="false" Width="80px" Enabled="false" BackColor="Gray" CommandName="SelDay" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>
                                                <asp:LinkButton ID="lbtn_day3" runat="server" Font-Underline="false" Width="80px" Enabled="false" BackColor="Gray" CommandName="SelDay" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>
                                                <asp:LinkButton ID="lbtn_day4" runat="server" Font-Underline="false" Width="80px" Enabled="false" BackColor="Gray" CommandName="SelDay" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>
                                                <asp:LinkButton ID="lbtn_day5" runat="server" Font-Underline="false" Width="80px" Enabled="false" BackColor="Gray" CommandName="SelDay" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>
                                                <asp:LinkButton ID="lbtn_day6" runat="server" Font-Underline="false" Width="80px" Enabled="false" BackColor="Gray" CommandName="SelDay" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemIndex").ToString() + "," + Container.ItemIndex + "," + DataBinder.Eval(Container.DataItem, "DeviceID").ToString() %>'>&nbsp;</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:DataList>
                                        <asp:HiddenField ID="hf_daySel" runat="server" />
                                        <asp:HiddenField ID="hf_SelTimeZone" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <div id="pickTimeList">
                                            <asp:GridView ID="gv_pickTimeList_Device" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                                OnRowCommand="gv_pickTimeList_Device_RowCommand" HorizontalAlign="Left">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Start DateTime" ItemStyle-Width="200px" DataField="StartDateTime" DataFormatString="{0:yyyy/MM/dd/ HH:mm}">
                                                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="End DateTime" ItemStyle-Width="200px" DataField="EndDateTime" DataFormatString="{0:yyyy/MM/dd/ HH:mm}">
                                                        <ItemStyle Width="150px" HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemStyle Width="50px" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtn_Delete" runat="server" CausesValidation="False" CommandName="DeleteRow" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id").ToString() + "," + DataBinder.Eval(Container.DataItem, "Device_ID").ToString() %>' Text="DELETE"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <hr />
                        </ItemTemplate>
                    </asp:DataList>
                </asp:Panel>
                <hr />
            </div>
            <div id="div_command">
                <asp:Button ID="btn_Submit" runat="server" Text="Next" OnClick="btn_Submit_Click" Enabled="false" CssClass="btn_Submit" />
                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" OnClick="btn_Cancel_Click" Enabled="false" CssClass="btn_Cancel" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

