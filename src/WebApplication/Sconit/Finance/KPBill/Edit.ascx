<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Finance_Bill_Edit" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<fieldset>
    <legend>${MasterData.Bill.POBill}</legend>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblOrderId" runat="server" Text="${MasterData.Bill.BillNo}:" />
            </td>
            <td class="td02">
                <sc1:ReadonlyTextBox ID="tbOrderId" runat="server" CodeField="QAD_ORDER_ID" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCreateDate" runat="server" Text="${MasterData.Bill.CreateDate}:" />
            </td>
            <td class="td02">
                <sc1:ReadonlyTextBox ID="tbCreateDate" runat="server" CodeField="ORDER_PUB_DATE" />
            </td>
        </tr>
    </table>
    <div class="tablefooter">
        <asp:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" Width="59px"
            OnClick="btnPrint_Click" />
        <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" Width="59px"
            OnClick="btnBack_Click" />
    </div>
</fieldset>
<fieldset>
    <legend>${MasterData.Bill.BillDetail}</legend>
    <div class="GridView">
        <asp:GridView ID="Gv_List" runat="server" AllowPaging="False" DataKeyNames="item_seq_id"
            AllowSorting="False" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText=" ${MasterData.KPBill.PurchaseOrderId}" SortExpression="purchase_order_id">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "purchase_order_id")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.ItemCode}" SortExpression="part_code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "part_code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.KPBill.InComingOrderId}" SortExpression="incoming_order_id">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "incoming_order_id")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.SeqId}" SortExpression="seq_id">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "seq_id")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.InComingQty}" SortExpression="incoming_qty">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "incoming_qty", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.Price}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "price")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.Uom}" SortExpression="um">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "um")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.Price1}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "price1", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.Price2}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "price2", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.ItemDescription}" SortExpression="part_name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "part_name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.DeliverOrderId}" SortExpression="deliver_order_id">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "deliver_order_id")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.KPBill.IncomingDate}" SortExpression="incoming_date">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "incoming_date", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</fieldset>
