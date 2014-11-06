<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Cost_StandardCost_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            ShowSeqNo="true" AllowSorting="true">
            <Columns>
                <asp:TemplateField HeaderText="${Cost.StandardCost.Item}" SortExpression="Item">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Item") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.StandardCost.CostElement}" SortExpression="CostElement.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CostElement.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Cost.StandardCost.CostGroup}" SortExpression="CostGroup.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CostGroup.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Cost" HeaderText="${Cost.StandardCost.Cost}" DataFormatString="{0:0.########}" />
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
