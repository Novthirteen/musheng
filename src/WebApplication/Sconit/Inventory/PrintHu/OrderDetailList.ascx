<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderDetailList.ascx.cs"
    Inherits="Inventory_PrintHu_OrderDetailList" %>

<script type="text/javascript" language="javascript">
    function GVCheckClick() {
        if ($(".GVHeader input:checkbox").attr("checked") == true) {
            $(".GVRow input:checkbox").attr("checked", true);
            $(".GVAlternatingRow input:checkbox").attr("checked", true);
        }
        else {
            $(".GVRow input:checkbox").attr("checked", false);
            $(".GVAlternatingRow input:checkbox").attr("checked", false);
        }
    }
</script>

<fieldset>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <div onclick="GVCheckClick()">
                            <asp:CheckBox ID="CheckAll" runat="server" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                        <asp:CheckBox ID="CheckBoxGroup" name="CheckBoxGroup" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.Sequence}">
                    <ItemTemplate>
                        <asp:Label ID="lblSeq" runat="server" Text='<%# Bind("Sequence") %>' onmouseup="if(!readOnly)select();" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.Item.Code}">
                    <ItemTemplate>
                        <asp:TextBox ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' ReadOnly="true"
                            Width="100" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.Item.Description}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("Item.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.ItemBrand}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemBrand" runat="server" Text='<%# Bind("Item.ItemBrand.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.Uom}">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Bind("Uom.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.UnitCount}">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.HuLotSize}">
                    <ItemTemplate>
                        <asp:Label ID="lblHuLotSize" runat="server" Text='<%# Bind("HuLotSize","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.InventoryDate}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbManufactureDate" runat="server" onmouseup="if(!readOnly)select();"  onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" 
                            Width="80"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.SupplierLotNo}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbSupplierLotNo" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuSupplierLotNo") %>'
                            Width="50" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.OrderQty}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbOrderQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("OrderedQty","{0:0.########}") %>'
                            Width="50"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revOrderQty" runat="server" Display="Dynamic"
                            ControlToValidate="tbOrderQty" Enabled="false"></asp:RegularExpressionValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.SortLevel1}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbSortLevel1" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuSortLevel1") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.ColorLevel1}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbColorLevel1" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuColorLevel1") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.SortLevel2}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbSortLevel2" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuSortLevel2") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.OrderDetail.ColorLevel2}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbColorLevel2" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuColorLevel2") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="tablefooter">
        <asp:Literal ID="lblHuCopies" runat="server" Text="${Inventory.PrintHu.Item.Copies}:" />
        <asp:TextBox ID="tbCopies" Text="1" runat="server" onmouseup="if(!readOnly)select();"  Width="50"></asp:TextBox>
        <asp:RangeValidator ID="rvCopies" ControlToValidate="tbCopies"
             runat="server" Display="Dynamic" ErrorMessage="${Common.Validator.Valid.Number}"
             Type="Integer" MinimumValue="0" MaximumValue="100"  ValidationGroup="vgPrint" />
        <asp:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" OnClick="btnPrint_Click"
            CssClass="button2" ValidationGroup="vgPrint" />
    </div>
</fieldset>
