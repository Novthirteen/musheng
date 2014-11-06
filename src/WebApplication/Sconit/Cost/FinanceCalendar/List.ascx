<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Cost_FinanceCalendar_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            ShowSeqNo="true" AllowSorting="true">
            <Columns>
                <asp:BoundField DataField="FinanceYear" HeaderText="${Cost.FinanceCalendar.Year}" SortExpression="FinanceYear" />
                <asp:BoundField DataField="FinanceMonth" HeaderText="${Cost.FinanceCalendar.Month}"
                    SortExpression="FinanceMonth" />
                <asp:BoundField DataField="StartDate" HeaderText="${Common.Business.StartDate}" DataFormatString="{0:yyyy-MM-dd}" 
                    SortExpression="StartDate" />
                <asp:BoundField DataField="EndDate" HeaderText="${Common.Business.EndDate}" DataFormatString="{0:yyyy-MM-dd}" 
                    SortExpression="EndDate" />
                <asp:CheckBoxField DataField="IsClosed" HeaderText="${Cost.FinanceCalendar.IsClosed}"
                    SortExpression="IsClosed" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" />
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
