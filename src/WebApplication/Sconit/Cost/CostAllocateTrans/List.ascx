<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Cost_CostAllocateTransaction_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            ShowSeqNo="true" AllowSorting="true" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Cost.CostAllocateTransaction.CostCenter}" SortExpression="Center.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CostCenter.Code") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateTransaction.CostElement}" SortExpression="CostElement.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CostElement.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateTransaction.DependCostElement}" SortExpression="DependCostElement.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DependCostElement.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateTransaction.ExpenseElement}" SortExpression="ExpenseElement.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ExpenseElement.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateTransaction.AllocateBy}" > 
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="ddlAllocateBy" runat="server" Code="AllocateBy" Value='<%# Bind("AllocateBy") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="${Cost.CostAllocateTransaction.EffDate}" SortExpression="EffectiveDate">
                    <ItemTemplate>
                        <asp:Label ID="lblEffDate" runat="server" Text='<%# Bind("EffectiveDate","{0:yyyy-MM-dd}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="${Cost.CostAllocateTransaction.Amount}">
                    <ItemTemplate>
                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click" Visible="false" />
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
