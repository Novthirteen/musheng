<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PreviewDetail.ascx.cs"
    Inherits="MRP_ShiftPlan_Import_PreviewDetail" %>
<div class="GridView">
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="false" OnRowDataBound="GV_List_RowDataBound"
        ShowHeader="false">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblSequence" runat="server" />
                    <asp:HiddenField ID="hfFlowDetailId" runat="server" Value='<%# Eval("FlowDetail.Id")%>' />
                    <asp:HiddenField ID="hfBom" runat="server" Value='<%# Eval("Bom.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblItemDescription" runat="server" Text='<%# Eval("Item.Description")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("Uom.Code")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblOrderedQty" runat="server" Text='<%# Eval("OrderedQty","{0:0.########}")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="${Common.Business.Remark}" DataField="Remark" />
        </Columns>
    </asp:GridView>
    <div class="tablefooter">
        ${Common.Business.Total}:<asp:Label ID="lblTotal" runat="server" />
    </div>
</div>
