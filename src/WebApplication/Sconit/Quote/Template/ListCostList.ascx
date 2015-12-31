<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListCostList.ascx.cs" Inherits="Quote_Template_ListCostList" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView" id="divGroup">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="Id"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:TemplateField HeaderText="${Quote.Template.CostList}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Template.CostCategory}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CCId.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Template.Number}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Number")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Template.Unit}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Unit")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Template.Price}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Price")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
