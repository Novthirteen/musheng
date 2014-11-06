<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FlowDetailList.ascx.cs"
    Inherits="Inventory_PrintHu_FlowDetailList" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>


<script language="javascript" type="text/javascript">
    function GenerateFlowDetail(obj) {
        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "tbItemCode_suggest".length);
        if ($(obj).val() != "") {
            Sys.Net.WebServiceProxy.invoke('Webservice/FlowMgrWS.asmx', 'GenerateFlowDetailProxy', false,
                { "flowCode": "<%=FlowCode%>", "itemCode": $(obj).val(), "partyFromCode": "<%=PartyFromCode%>", "partyToCode": "<%=PartyToCode%>",
                    "moduleType": "<%=ModuleType%>", "changeRef": true, "startTime": "2000-1-1"
                },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbItemDescription').attr('value', result.ItemDescription);
                $('#' + parentId + 'tbRefItemCode_suggest').attr('value', result.ItemReferenceCode);
                $('#' + parentId + 'tbUom_suggest').attr('value', result.UomCode);
                $('#' + parentId + 'tbUnitCount').attr('value', result.UnitCount);
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
        }
    }
    function GenerateFlowDetailProxyByReferenceItem(obj) {
        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "tbRefItemCode_suggest".length);
        if ($(obj).val() != "") {
            Sys.Net.WebServiceProxy.invoke('Webservice/FlowMgrWS.asmx', 'GenerateFlowDetailProxyByReferenceItem', false,
                { "flowCode": "<%=FlowCode%>", "refItemCode": $(obj).val(), "partyFromCode": "<%=PartyFromCode%>", "partyToCode": "<%=PartyToCode%>",
                    "moduleType": "<%=ModuleType%>", "changeRef": false, "startTime": "2000-1-1"
                },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbItemCode_suggest').attr('value', result.ItemCode);
                $('#' + parentId + 'tbItemDescription').attr('value', result.ItemDescription);
                $('#' + parentId + 'tbUom_suggest').attr('value', result.UomCode);
                $('#' + parentId + 'tbUnitCount').attr('value', result.UnitCount);
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
        }
    }
</script>

<fieldset>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.Sequence}">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                        <asp:Label ID="lblSeq" runat="server" Text='<%# Bind("Sequence") %>' onmouseup="if(!readOnly)select();" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.Item.Code}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.Item.Description}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("Item.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.ItemBrand}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemBrand" runat="server" Text='<%# Bind("Item.ItemBrand.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.Uom}">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Bind("Uom.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.UnitCount}">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.HuLotSize}">
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
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.SupplierLotNo}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbSupplierLotNo" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuSupplierLotNo") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.OrderQty}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbOrderQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("OrderedQty","{0:0.########}") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.SortLevel1}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbSortLevel1" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuSortLevel1") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.ColorLevel1}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbColorLevel1" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuColorLevel1") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.SortLevel2}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbSortLevel2" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("HuSortLevel2") %>'
                            Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Inventory.PrintHu.FlowDetail.ColorLevel2}">
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
