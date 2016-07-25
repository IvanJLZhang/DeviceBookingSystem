<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DatePicker.ascx.cs" Inherits="UserCtrl_DatePicker" %>
<div>
    <asp:TextBox ID="tb_Date" runat="server" Width="100px" Text='<%# DateTime.Now.ToString("yyyy/MM/dd") %>'></asp:TextBox>
    <asp:ImageButton ID="imgbtn_date" runat="server" ImageUrl="~/Images/calendar_month.png" OnClick="imgbtn_date_Click" />
    <div style="position: absolute">
        <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px"
             Visible="false" TitleFormat="MonthYear" OnPreRender="Calendar1_PreRender">
            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
            <NextPrevStyle VerticalAlign="Bottom" />
            <OtherMonthDayStyle ForeColor="#808080" />
            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
            <SelectorStyle BackColor="#CCCCCC" />
            <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
            <WeekendDayStyle BackColor="#FFFFCC" />
        </asp:Calendar>
    </div>
</div>

