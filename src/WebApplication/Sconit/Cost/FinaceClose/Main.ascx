<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Cost_FinanceClose_Main" %>
<fieldset>
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="FinanceYear" HeaderText="${Cost.FinanceCalendar.Year}"
                SortExpression="FinanceYear" />
            <asp:BoundField DataField="FinanceMonth" HeaderText="${Cost.FinanceCalendar.Month}"
                SortExpression="FinanceMonth" />
            <asp:BoundField DataField="StartDate" HeaderText="${Common.Business.StartDate}" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"
                SortExpression="StartDate" />
            <asp:TemplateField HeaderText="${Common.Business.EndDate}">
                <ItemTemplate>
                    <asp:TextBox ID="tbEndDate" runat="server" 
                        Text='<%# Bind("EndDate","{0:yyyy-MM-dd HH:mm:ss}") %>' ReadOnly="true" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.GridView.Action}">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnClose" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                        Text="${Common.Button.FinanceClose}" OnClick="lbtnClose_Click" OnClientClick="return confirm('是否确定月结此财政月?')">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>
