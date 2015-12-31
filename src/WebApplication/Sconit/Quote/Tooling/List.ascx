<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Quote_Tooling_List" %>

<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<fieldset>
    <div class="GridView" id="divGroup">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="TL_No"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="TL_No"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:BoundField DataField="TL_No" HeaderText="${Quote.Tooling.TLNo}" SortExpression="TL_No" />
                <asp:TemplateField HeaderText="${Quote.Tooling.TLName}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "TL_Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Tooling.TLSpec}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "TL_Spec")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Tooling.Customer}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Customer")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ApplyDate" HeaderText="${Quote.Tooling.ApplyDate}"
                    SortExpression="ApplyDate" />
                <asp:TemplateField HeaderText="${Quote.Tooling.ArriveDate}">
                    <ItemTemplate>
                        <asp:Label ID="lblWinTime" Text='<%# Bind("ArriveDate") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Tooling.Price}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Price")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Tooling.Number}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Number")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Tooling.CustomerBStatus}">
                    <ItemTemplate>
                        <%#(bool) DataBinder.Eval(Container.DataItem, "CustomerBStatus") == false?"未结算":"已结算"%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Tooling.SupplierInNo}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "SuppliersInNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Quote.Tooling.ProjectNo}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ProjectNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TL_No") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TL_No") %>'
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
