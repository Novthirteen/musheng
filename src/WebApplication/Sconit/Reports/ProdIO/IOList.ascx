<%@ Control Language="C#" AutoEventWireup="true" CodeFile="IOList.ascx.cs" Inherits="Reports_ProdIO_IOList" %>
<div class="GridView">
    <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
        ShowHeader="false" OnRowDataBound="GV_List_RowDataBound">
        <Columns>
            <asp:TemplateField ItemStyle-Width="20%">
                <ItemTemplate>
                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="50%">
                <ItemTemplate>
                    <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("Item.Description")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="10%">
                <ItemTemplate>
                    <asp:Label ID="lblUom" runat="server" Text='<%# Eval("Item.Uom.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="15%">
                <ItemTemplate>
                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("AccumQty","{0:F2}")%>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
