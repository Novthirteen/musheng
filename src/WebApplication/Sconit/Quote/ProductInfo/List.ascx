<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Quote_ProductInfo_List" %>

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
                <%--<asp:BoundField DataField="Id" HeaderText="${Quote.Tooling.TLNo}" SortExpression="Id" />--%>
                <asp:TemplateField HeaderText="${Quote.Tooling.ProjectNo}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ProjectId")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.ProductInfo.CustomerName}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CustomerName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.ProductInfo.ProductName}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ProductName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.ProductInfo.ProductNo}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ProductNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="VersionNo" HeaderText="${Quote.ProductInfo.VersionNo}" />
                <asp:TemplateField HeaderText="${Quote.ProductInfo.DeliveryAdd}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DeliveryAdd")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.ProductInfo.LogisticsCost}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "LogisticsCost")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.ProductInfo.PackCost}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PackCost")%>
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
