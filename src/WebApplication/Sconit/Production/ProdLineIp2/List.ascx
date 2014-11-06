<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Production_ProdLineIp2_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="Detail.ascx" TagName="detail" TagPrefix="uc" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" DefaultSortExpression="ProdLine" DefaultSortDirection="Ascending">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.ProductionLine}" SortExpression="ProdLine">
                    <ItemTemplate>
                        <asp:Label ID="lblProdLine" runat="server" Text='<%# Eval("ProdLine")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Production.ProdLineIp2.ProdLineFact}" SortExpression="ProdLineFact">
                    <ItemTemplate>
                        <asp:Label ID="lblProdLineFact" runat="server" Text='<%# Eval("ProdLineFact")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.OrderNo}" SortExpression="OrderNo">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.FG}" SortExpression="FG">
                    <ItemTemplate>
                        <asp:Label ID="lblFG" runat="server" Text='<%# Eval("FG")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Position}" SortExpression="Position">
                    <ItemTemplate>
                        <asp:Label ID="lblPosition" runat="server" Text='<%# Eval("Position")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Item}" SortExpression="Item">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.LotNo}" SortExpression="Hu.LotNo">
                    <ItemTemplate>
                        <asp:Label ID="lblHuLotNo" runat="server" Text='<%# Eval("Hu.LotNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.HuId}">
                    <ItemTemplate>
                        <asp:Label ID="lblHuNo" runat="server" Text='<%# Eval("Hu.HuId")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.FeedType}" SortExpression="Type">
                    <ItemTemplate>
                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.CreateDate}" SortExpression="CreateDate">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateDate" runat="server" Text='<%# Eval("CreateDate")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.CreateUser}" SortExpression="CreateUser">
                    <ItemTemplate>
                        <asp:Label ID="lblCreateUser" runat="server" Text='<%# Eval("CreateUser")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# Eval("Id")%>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
<uc:detail ID="ucdetail" runat="server" Visible="false" />