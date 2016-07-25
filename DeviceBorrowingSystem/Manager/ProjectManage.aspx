<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Manager_MasterPage.master" AutoEventWireup="true" CodeFile="ProjectManage.aspx.cs" Inherits="Manager_ProjectManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Project/Test Category Management</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="up_main" runat="server">
        <ContentTemplate>
            <asp:Panel ID="p_Navigate" runat="server">
                <table style="width:100%">
                    <tr>
                        <td>Manage Item: 
                            <asp:DropDownList ID="ddl_Type" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged">
                                <asp:ListItem>Project</asp:ListItem>
                                <asp:ListItem>Test Category</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:ImageButton ID="ImageButton4" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton1_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <hr />
            <br />
            <asp:Panel ID="panel_ProjectManagement" runat="server" Visible="false">
                <table align="center">
                    <%--                    <tr>
                        <td align="center">
                            <h3>Project Management
                            </h3>
                        </td>
                        <td align="right" style="text-transform: uppercase">
                            <asp:ImageButton ID="ImageButton4" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton1_Click" />
                            <asp:LinkButton ID="linkbutton4" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton1_Click">Add Project</asp:LinkButton>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GridView3" runat="server" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="PJ_Code" DataSourceID="SqlDataSource1" EnableModelValidation="True" ForeColor="#333333" GridLines="None" Width="558px" AllowPaging="True">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="PJ_Code" HeaderText="Project Code" ReadOnly="True" />
                                    <asp:BoundField DataField="PJ_Name" HeaderText="Project Name" SortExpression="PJ_Name" />
                                    <asp:BoundField DataField="Cust_Name" HeaderText="Cust Name" SortExpression="Cust_Name" />
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" DeleteCommand="DELETE FROM tbl_Project WHERE (PJ_Code = @PJ_Code)" InsertCommand="INSERT INTO tbl_Project(PJ_Code, PJ_Name, Cust_Name, Date) VALUES (@PJ_Code, @PJ_Name, @Cust_Name, GETDATE())" SelectCommand="SELECT tbl_Project.* FROM tbl_Project" UpdateCommand="UPDATE tbl_Project SET PJ_Name = @PJ_Name, Cust_Name = @Cust_Name, Date = GETDATE() WHERE (PJ_Code = @PJ_Code)">
                                <DeleteParameters>
                                    <asp:Parameter Name="PJ_Code" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="PJ_Code" />
                                    <asp:Parameter Name="PJ_Name" />
                                    <asp:Parameter Name="Cust_Name" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="PJ_Name" />
                                    <asp:Parameter Name="Cust_Name" />
                                    <asp:Parameter Name="PJ_Code" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
<%--                    <tr>
                        <td></td>
                        <td align="right" style="text-transform: uppercase">
                            <asp:ImageButton ID="ImageButton5" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton1_Click" />
                            <asp:LinkButton ID="linkbutton5" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton1_Click">Add Project</asp:LinkButton>
                        </td>
                    </tr>--%>
                </table>
            </asp:Panel>

            <asp:Panel ID="panel_TestCatManagement" runat="server" Visible="false">
                <table align="center">
<%--                    <tr>
                        <td align="center">
                            <h3>Test Category Management
                            </h3>
                        </td>
                        <td align="right" style="text-transform: uppercase">
                            <asp:ImageButton ID="ImageButton6" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton2_Click" />
                            <asp:LinkButton ID="linkbutton6" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton2_Click">Add Test_Category</asp:LinkButton>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GridView4" runat="server" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4"
                                DataKeyNames="ID" DataSourceID="SqlDataSource2" EnableModelValidation="True" ForeColor="#333333" GridLines="None" Width="558px" AllowPaging="True">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="Test_TypeID" SortExpression="ID" ReadOnly="true" />
                                    <asp:BoundField DataField="Name" HeaderText="TestCategory_Name" SortExpression="Name" />
                                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" DeleteCommand="DELETE FROM tbl_TestCategory WHERE (ID = @ID)" InsertCommand="INSERT INTO tbl_TestCategory(ID, Name, Date) VALUES (@ID, @Name, GETDATE())" SelectCommand="SELECT tbl_TestCategory.* FROM tbl_TestCategory" UpdateCommand="UPDATE tbl_TestCategory SET Name = @Name, Date = GETDATE() WHERE (ID = @ID)">
                                <DeleteParameters>
                                    <asp:Parameter Name="ID" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="ID" />
                                    <asp:Parameter Name="Name" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="Name" />
                                    <asp:Parameter Name="ID" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
<%--                    <tr>
                        <td></td>
                        <td align="right" style="text-transform: uppercase">
                            <asp:ImageButton ID="ImageButton7" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton2_Click" />
                            <asp:LinkButton ID="linkbutton7" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton2_Click">Add Test_Category</asp:LinkButton>
                        </td>
                    </tr>--%>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<div id="divBody" style="width: 100%; height: 420px" align="center">
        <table>
            <tr>
                <td style="vertical-align:top">
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <h3>Project Management
                                    </h3>
                                </td>
                                <td align="right" style="text-transform: uppercase">
                                    <asp:ImageButton ID="Button1" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton1_Click" />
                                    <asp:LinkButton ID="linkbutton" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton1_Click">Add Project</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"style="border-right:solid; border-right-style: groove; border-right-width: thin; border-right-color: #000000;">
                                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="PJ_Code" DataSourceID="SqlDataSource1" EnableModelValidation="True" ForeColor="#333333" GridLines="None" Width="558px" AllowPaging="True">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="PJ_Code" HeaderText="Project Code" ReadOnly="True" />
                                            <asp:BoundField DataField="PJ_Name" HeaderText="Project Name" SortExpression="PJ_Name" />
                                            <asp:BoundField DataField="Cust_Name" HeaderText="Cust Name" SortExpression="Cust_Name" />
                                            <asp:CommandField ShowEditButton="True" />
                                            <asp:CommandField ShowDeleteButton="True" />
                                        </Columns>
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" DeleteCommand="DELETE FROM tbl_Project WHERE (PJ_Code = @PJ_Code)" InsertCommand="INSERT INTO tbl_Project(PJ_Code, PJ_Name, Cust_Name, Date) VALUES (@PJ_Code, @PJ_Name, @Cust_Name, GETDATE())" SelectCommand="SELECT tbl_Project.* FROM tbl_Project" UpdateCommand="UPDATE tbl_Project SET PJ_Name = @PJ_Name, Cust_Name = @Cust_Name, Date = GETDATE() WHERE (PJ_Code = @PJ_Code)">
                                        <DeleteParameters>
                                            <asp:Parameter Name="PJ_Code" />
                                        </DeleteParameters>
                                        <InsertParameters>
                                            <asp:Parameter Name="PJ_Code" />
                                            <asp:Parameter Name="PJ_Name" />
                                            <asp:Parameter Name="Cust_Name" />
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="PJ_Name" />
                                            <asp:Parameter Name="Cust_Name" />
                                            <asp:Parameter Name="PJ_Code" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="right" style="text-transform: uppercase">
                                    <asp:ImageButton ID="ImageButton1" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton1_Click" />
                                    <asp:LinkButton ID="linkbutton1" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton1_Click">Add Project</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="vertical-align:top">
                    <div>
                        <table>
                            <tr>
                                <td align="center">
                                    <h3>Test Category Management
                                    </h3>
                                </td>
                                <td align="right" style="text-transform: uppercase">
                                    <asp:ImageButton ID="ImageButton2" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton2_Click" />
                                    <asp:LinkButton ID="linkbutton2" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton2_Click">Add Test_Category</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-left:solid; border-left-style: groove; border-left-width: thin; border-left-color: #000000;">
                                    <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4"
                                          DataKeyNames="ID" DataSourceID="SqlDataSource2" EnableModelValidation="True" ForeColor="#333333" GridLines="None" Width="558px" AllowPaging="True">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderText="Test_TypeID" SortExpression="ID" ReadOnly="true" />
                                            <asp:BoundField DataField="Name" HeaderText="TestCategory_Name" SortExpression="Name" />
                                            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                                        </Columns>
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:connStr %>" DeleteCommand="DELETE FROM tbl_TestCategory WHERE (ID = @ID)" InsertCommand="INSERT INTO tbl_TestCategory(ID, Name, Date) VALUES (@ID, @Name, GETDATE())" SelectCommand="SELECT tbl_TestCategory.* FROM tbl_TestCategory" UpdateCommand="UPDATE tbl_TestCategory SET Name = @Name, Date = GETDATE() WHERE (ID = @ID)">
                                        <DeleteParameters>
                                            <asp:Parameter Name="ID" />
                                        </DeleteParameters>
                                        <InsertParameters>
                                            <asp:Parameter Name="ID" />
                                            <asp:Parameter Name="Name" />
                                        </InsertParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="Name" />
                                            <asp:Parameter Name="ID" />
                                        </UpdateParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="right" style="text-transform: uppercase">
                                    <asp:ImageButton ID="ImageButton3" runat="server" Height="20px" ImageUrl="~/Images/add.png" Width="22px" OnClick="linkbutton2_Click" />
                                    <asp:LinkButton ID="linkbutton3" runat="server" Font-Underline="False" ForeColor="Black" OnClick="linkbutton2_Click">Add Test_Category</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>


    </div>--%>
</asp:Content>

