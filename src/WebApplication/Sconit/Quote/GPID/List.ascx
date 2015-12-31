<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Quote_GPID_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<fieldset>
    <div class="GridView" id="divGroup">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="ID"
            DefaultSortDirection="Descending">
            <Columns>
                <%--<asp:BoundField DataField="Id" HeaderText="${Quote.Tooling.TLNo}" SortExpression="Id" />--%>
                <asp:TemplateField HeaderText="${Quote.Tooling.ProjectNo}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ID")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.GPID.Cusomer}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CustomerCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.GPID.Desc}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Descr")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.GPID.StartDate}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "StartDate")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.GPID.Status}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Status")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
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