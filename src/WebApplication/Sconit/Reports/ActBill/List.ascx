<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Reports_ActBill_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:gridview id="GV_List" runat="server" autogeneratecolumns="False" datakeynames="Id"
            allowmulticolumnsorting="false" autoloadstyle="false" seqno="0" seqtext="No."
            showseqno="true" allowsorting="True" allowpaging="True" pagerid="gp" width="100%"
            cellmaxlength="10" typename="com.Sconit.Web.CriteriaMgrProxy" selectmethod="FindAll"
            selectcountmethod="FindCount" defaultsortexpression="BillAddress.Party.Name"
            defaultsortdirection="Ascending">
            <Columns>
                <asp:TemplateField SortExpression="BillAddress.Party.Name">
                    <ItemTemplate>
                        <asp:Label ID="lblPartyName" runat="server" Text='<%# Eval("BillAddress.Party.Name")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.OrderNo}" SortExpression="OrderNo">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ReceiptNo}" SortExpression="ReceiptNo">
                    <ItemTemplate>
                        <asp:Label ID="lblReceiptNo" runat="server" Text='<%# Eval("ReceiptNo")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ExternalReceiptNo}" SortExpression="ExternalReceiptNo">
                    <ItemTemplate>
                        <asp:Label ID="lblExternalReceiptNo" runat="server" Text='<%# Eval("ExternalReceiptNo")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ItemCode}" SortExpression="Item.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.ItemDescription}" SortExpression="Item.Desc1">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("Item.Description")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.Uom}" SortExpression="Uom.Code">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Uom.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.UnitCount}">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Eval("UnitCount", "{0:0.###}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="${MasterData.ActingBill.UnitPrice}">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("UnitPrice", "{0:0.########}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.ActingBillQty}" >
                    <ItemTemplate>
                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty", "{0:0.###}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="${MasterData.ActingBill.ActingAmount}">
                    <ItemTemplate>
                        <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount", "{0:0.########}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Reports.ActBill.EffectiveDate}" SortExpression="EffectiveDate">
                    <ItemTemplate>
                        <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Eval("EffectiveDate")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:gridview>
        <cc1:gridpager id="gp" runat="server" gridviewid="GV_List" pagesize="10">
        </cc1:gridpager>
    </div>
    <table class="mtable" id="tabTotalAmount" runat="server" visible="false">
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblTotalAmount" runat="server" Text="${Reports.ActBill.TotalAmount}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbTotalAmount" runat="server" Visible="true" onfocus="this.blur();"
                    Width="150px" />
            </td>
        </tr>
    </table>
</fieldset>
