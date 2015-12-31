<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PriceList.ascx.cs" Inherits="Quote_Item_PriceList" %>

<script type="text/javascript">
    function txtPurchasePriceChange(obj) {
        var price = $(obj).val();

        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "txtPurchasePrice".length);

        var num = $('#' + parentId + 'txtSingleNum').val();
        $('#' + parentId + 'txtPrice').attr('value', num * price);
    }
</script>

<fieldset>
    <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                OnRowDataBound="GV_List_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="${MasterData.ItemMap.Item}">
                <ItemTemplate>
                    <asp:TextBox ID="txtItemCode" runat="server" Text='<%# Bind("ItemCode") %>' Enabled="false"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.SingleNum}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblSingleNum" runat="server" Text='<%# Bind("SingleNum") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtSingleNum" runat="server" Width="50" Text='<%# Bind("SingleNum") %>' Enabled="false"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.PurchasePrice}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblPurchasePrice" runat="server" Text='<%# Bind("PurchasePrice") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtPurchasePrice1" runat="server" Width="80" Enabled="false"></asp:TextBox>
                    <asp:TextBox ID="txtPurchasePrice" runat="server" Width="80" Text='<%# Bind("PurchasePrice") %>' ></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.ItemQuote.Price}">
                <ItemTemplate>
                    <%--<asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>--%>
                    <asp:TextBox ID="txtPrice" runat="server" Width="80"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</fieldset>