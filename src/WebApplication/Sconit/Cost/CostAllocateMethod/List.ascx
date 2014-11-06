<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Cost_CostAllocateMethod_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            ShowSeqNo="true" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="${Cost.CostAllocateMethod.CostGroup}" SortExpression="CostGroup.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CostGroup.Code") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateMethod.CostCenter}" SortExpression="Center.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CostCenter.Code") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateMethod.CostElement}" SortExpression="CostElement.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CostElement.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateMethod.DependCostElement}" SortExpression="DependCostElement.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DependCostElement.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateMethod.ExpenseElement}" SortExpression="ExpenseElement.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ExpenseElement.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.CostAllocateMethod.AllocateBy}" > 
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="ddlAllocateBy" runat="server" Code="AllocateBy" Value='<%# Bind("AllocateBy") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
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
