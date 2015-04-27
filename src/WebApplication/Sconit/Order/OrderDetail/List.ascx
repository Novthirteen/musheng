﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Order_OrderDetail_List" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="OrderTracer.ascx" TagName="OrderTracer" TagPrefix="uc2" %>

<script language="javascript" type="text/javascript" src="Js/calcamount.js"></script>

<script language="javascript" type="text/javascript">
    function discountChanged(obj, isBlank) {
        CalCulateRowAmount(obj, "tbDiscount", "BaseOnDiscount", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', isBlank);
    }
    function discountRateChanged(obj, isBlank) {
        CalCulateRowAmount(obj, "tbDiscountRate", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', isBlank);
    }
    function calcPrice(obj, isBlank) {
        CalCulateRowAmount(obj, "tbOrderQty", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', isBlank);
    }
    function GenerateFlowDetail(obj) {
        var objId = $(obj).attr("id");
        var parentId = objId.substring(0, objId.length - "tbItemCode_suggest".length);
        if ($(obj).val() != "") {
            Sys.Net.WebServiceProxy.invoke('Webservice/FlowMgrWS.asmx', 'GenerateFlowDetailProxy', false,
                { "flowCode": "<%=FlowCode%>", "itemCode": $(obj).val(), "partyFromCode": "<%=PartyFromCode%>", "partyToCode": "<%=PartyToCode%>",
                    "moduleType": "<%=ModuleType%>", "changeRef": true, "startTime": "<%=StartTime%>"
                },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbItemDescription').attr('value', result.ItemDescription);
                $('#' + parentId + 'tbRefItemCode_suggest').attr('value', result.ItemReferenceCode);
                $('#' + parentId + 'tbUom_suggest').attr('value', result.UomCode);
                $('#' + parentId + 'tbUnitCount').attr('value', result.UnitCount);
                $('#' + parentId + 'tbHuLotSize').attr('value', result.HuLotSize);
                $('#' + parentId + 'tbUnitPrice').attr('value', result.UnitPrice);
                $('#' + parentId + 'hfUnitPrice').attr('value', result.UnitPrice);
                $('#' + parentId + 'hfPriceListCode').attr('value', result.PriceListCode);
                $('#' + parentId + 'hfPriceListDetailId').attr('value', result.PriceListDetailId);
                $('#' + parentId + 'hfFlowDetailId').attr('value', result.Id);
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
                    "moduleType": "<%=ModuleType%>", "changeRef": false, "startTime": "<%=StartTime%>"
                },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbItemCode_suggest').attr('value', result.ItemCode);
                $('#' + parentId + 'tbItemDescription').attr('value', result.ItemDescription);
                $('#' + parentId + 'tbUom_suggest').attr('value', result.UomCode);
                $('#' + parentId + 'tbUnitCount').attr('value', result.UnitCount);
                $('#' + parentId + 'tbHuLotSize').attr('value', result.HuLotSize);
                $('#' + parentId + 'tbUnitPrice').attr('value', result.UnitPrice);
                $('#' + parentId + 'hfUnitPrice').attr('value', result.UnitPrice);
                $('#' + parentId + 'hfPriceListCode').attr('value', result.PriceListCode);
                $('#' + parentId + 'hfPriceListDetailId').attr('value', result.PriceListDetailId);
                $('#' + parentId + 'hfFlowDetailId').attr('value', result.Id);
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
        }
    }

    function orderDiscountChanged(obj) {
        CalCulateTotalAmount("BaseOnDiscount", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', 0);
    }

    function orderDiscountRateChanged(obj) {
        CalCulateTotalAmount("BaseOnDiscountRate", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', 0);
    }

    function GetUnitPriceByUom(obj) {
        if ($(obj).val() != "") {
            var objId = $(obj).attr("id");
            var parentId = objId.substring(0, objId.length - "tbUom_suggest".length);
            var priceListCodeId = "#" + parentId + "hfPriceListCode";
            var itemCodeId = "#" + parentId + "tbItemCode_suggest";
            var orderQtyId = "#" + parentId + "tbOrderQty";
            var priceListCode;
            if ($(priceListCodeId).val() == undefined) {
                priceListCode = "";
            } else {
                priceListCode = $(priceListCodeId).val();
            }
            Sys.Net.WebServiceProxy.invoke('Webservice/FlowMgrWS.asmx', 'GetUnitPriceByUom', false,
                { "priceListCode": priceListCode, "itemCode": $(itemCodeId).val(), "startTime": "<%=StartTime%>", "currencyCode": "<%=currencyCode%>",
                    "uomCode": $(obj).val()
                },
            function OnSucceeded(result, eventArgs) {
                $('#' + parentId + 'tbUnitPrice').attr('value', result);
            },
            function OnFailed(error) {
                alert(error.get_message());
            }
           );
            CalCulateRowAmount(obj, "tbUom_suggest", "BaseOnDiscountRate", "tbUnitPrice", "tbOrderQty", "tbDiscount", "tbDiscountRate", "tbPrice", '#<%= tbOrderDiscount.ClientID %>', '#<%= tbOrderDiscountRate.ClientID %>', '#<%= tbOrderDetailPrice.ClientID %>', '#<%= tbOrderPrice.ClientID %>', true);
        }
    }

    $(function () {
        //alert($(window).height());
        //alert($(document).height());
        //$(window).height();  //是获取当前也就是浏览器所能看到的页面的那部分的高度。
        //$(document).height();  //是获取整个页面的高度
        $(window).scrollTop($(document).height());
    });
</script>

<fieldset runat="server" id="fdDetail">
    <legend>${MasterData.Order.OrderDetail}</legend>
    <div>
        <asp:CustomValidator ID="cvShipQty" runat="server" />
        <asp:CustomValidator ID="cvReceiveQty" runat="server" />
        <asp:HiddenField ID="hfIsChanged" runat="server" Value="N" />
        <asp:HiddenField ID="hfHuIdPageX" runat="server" />
        <asp:HiddenField ID="hfHuIdPageY" runat="server" />
        <div class="GridView">
            <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                OnRowDataBound="GV_List_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Sequence}">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                            <asp:Label ID="lblSeq" runat="server" Text='<%# Bind("Sequence") %>' onmouseup="if(!readOnly)select();" />
                            <asp:TextBox ID="tbSeq" runat="server" onmouseup="if(!readOnly)select();" Visible="false"
                                Width="30" Text='<%# Bind("Sequence") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Code}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' Width="100" />
                            <uc3:textbox ID="tbItemCode" runat="server" Visible="false" Width="250" DescField="Description"
                                ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" InputWidth="150" MustMatch="true" TabIndex="1" />
                            <%--<uc3:textbox ID="tbItemCode1" runat="server" Width="250" DescField="Description"
                                ValueField="Code" ServicePath="FlowMgr.service" ServiceMethod="GetFlowItems"
                                CssClass="inputRequired" InputWidth="150" MustMatch="true" TabIndex="1" />--%>
                            <asp:HiddenField ID="hfFlowDetailId" runat="server" />
                            <asp:HiddenField ID="hfPriceListCode" runat="server" />
                            <asp:HiddenField ID="hfPriceListDetailId" runat="server" />
                            <asp:HiddenField ID="hfUnitPrice" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvItemCode" runat="server" ControlToValidate="tbItemCode"
                                Display="Dynamic" ErrorMessage="${MasterData.Order.OrderDetail.ItemCode.Required}"
                                ValidationGroup="vgAdd" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ItemVersion}" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblItemVersion" runat="server" Text='<%# Bind("ItemVersion") %>' Visible="false" />
                            <asp:TextBox ID="tbItemVersion" runat="server" Width="50" Text='<%# Bind("ItemVersion") %>'
                                TabIndex="-1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Description}">
                        <ItemTemplate>
                            <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("Item.Description") %>' />
                            <asp:TextBox ID="tbItemDescription" runat="server" Visible="false" Width="150" ReadOnly="true"
                                TabIndex="-1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ReferenceItem}">
                        <ItemTemplate>
                            <asp:Label ID="lblReferenceItemCode" runat="server" Text='<%# Bind("ReferenceItemCode") %>' />
                            <uc3:textbox ID="tbRefItemCode" runat="server" Visible="false" Width="200" DescField="ReferenceCode"
                                ValueField="ReferenceCode" ServicePath="ItemReferenceMgr.service" ServiceMethod="GetItemReferenceByParty"
                                InputWidth="80" TabIndex="-1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Uom}">
                        <ItemTemplate>
                            <asp:Label ID="lblUom" runat="server" Text='<%# Bind("Uom.Code") %>' />
                            <uc3:textbox ID="tbUom" runat="server" Visible="false" Width="200" DescField="Description"
                                ServiceParameter="string:#tbItemCode" ValueField="Code" ServicePath="UomMgr.service"
                                InputWidth="50" ServiceMethod="GetItemUom" CssClass="inputRequired" TabIndex="-1" />
                            <asp:RequiredFieldValidator ID="rfvUom" runat="server" ControlToValidate="tbUom"
                                Display="Dynamic" ErrorMessage="${MasterData.Order.OrderDetail.Uom.Required}"
                                ValidationGroup="vgAdd" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.UnitCount}">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>' />
                            <asp:TextBox ID="tbUnitCount" runat="server" CssClass="inputRequired" Visible="false"
                                Width="50" TabIndex="-1" />
                            <asp:RequiredFieldValidator ID="rfvUC" runat="server" ErrorMessage="${MasterData.Order.OrderDetail.UnitCount.Required}"
                                Display="Dynamic" ControlToValidate="tbUnitCount" ValidationGroup="vgAdd" Enabled="false" />
                            <asp:RangeValidator ID="rvUC" ControlToValidate="tbUnitCount" runat="server" Display="Dynamic"
                                ErrorMessage="${Common.Validator.Valid.Number}" MaximumValue="999999999" MinimumValue="0.00000001"
                                Type="Double" ValidationGroup="vgAdd" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.HuLotSize}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbHuLotSize" runat="server" Text='<%# Bind("HuLotSize","{0:0.########}") %>'
                                onfocus="this.blur();" Width="50" TabIndex="-1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.PackageType}" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblPackageType" runat="server" Text='<%#Bind("PackageType") %>' TabIndex="-1" />
                            <cc1:CodeMstrDropDownList ID="ddlPackageType" runat="server" Code="PackageType" IncludeBlankOption="true"
                                Visible="false" TabIndex="-1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.LocationFrom}">
                        <ItemTemplate>
                            <asp:Label ID="lblLocFrom" runat="server" Text='<%# Bind("DefaultLocationFrom.Code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.LocationTo}">
                        <ItemTemplate>
                            <asp:Label ID="lblLocTo" runat="server" Text='<%# Bind("DefaultLocationTo.Name") %>' />
                            <asp:DropDownList ID="ddlLocTo" runat="server" Visible="false" DataValueField="Code"
                                DataTextField="Name" TabIndex="-1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Bom}">
                        <ItemTemplate>
                            <asp:Label ID="lblBom" runat="server" Text='<%# Bind("Bom.Code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.RequiredQty}">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbReqQty" runat="server" Text='<%# Bind("RequiredQty","{0:0.########}") %>'
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnClick="lbReqQty_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.OrderedQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbOrderQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("OrderedQty","{0:0.########}") %>'
                                Width="50" TabIndex="1"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revOrderQty" runat="server" Display="Dynamic"
                                ControlToValidate="tbOrderQty" Enabled="false"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ShippedQty}">
                        <ItemTemplate>
                            <asp:Label ID="lblShippedQty" runat="server" Text='<%# Bind("ShippedQty","{0:0.########}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentShipQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbShipQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("CurrentShipQty","{0:0.########}") %>'
                                Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ReceivedQty}">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedQty" runat="server" Text='<%# Bind("ReceivedQty","{0:0.########}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentReceiveQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbReceiveQty" runat="server" onmouseup="if(!readOnly)select();"
                                Text='<%# Bind("CurrentReceiveQty","{0:0.########}") %>' onchange="setChanged();"
                                Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentRejectQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbRejectQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("CurrentRejectQty","{0:0.########}") %>'
                                onchange="setChanged();" Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.CurrentScrapQty}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbScrapQty" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("CurrentScrapQty","{0:0.########}") %>'
                                onchange="setChanged();" Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.UnitPrice}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbUnitPrice" runat="server" Width="50" onfocus="this.blur();" Text='<%# Bind("UnitPrice","{0:0.###}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.DiscountRate}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbDiscountRate" runat="server" onmouseup="if(!readOnly)select();"
                                Width="50" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Discount}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbDiscount" runat="server" onmouseup="if(!readOnly)select();" Width="50" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Amount}" Visible="false">
                        <ItemTemplate>
                            <asp:TextBox ID="tbPrice" runat="server" Width="50" onfocus="this.blur();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Remark}">
                        <ItemTemplate>
                            <asp:TextBox ID="tbRemark" runat="server" Width="50" Text='<%# Bind("Remark") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.GridView.Action}" Visible="false">
                        <ItemTemplate>
                            <cc1:LinkButton ID="lbtnAdd" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                Text="${Common.Button.New}" OnClick="lbtnAdd_Click" Visible="false" FunctionId="EditOrderDetail"
                                ValidationGroup="vgAdd" TabIndex="1">
                            </cc1:LinkButton>
                            <%-- <cc1:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                Text="${Common.Button.View}" OnClick="lbtnView_Click" FunctionId="ViewOrderDetail">
                            </cc1:LinkButton>--%>
                            <cc1:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'
                                Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
                                FunctionId="DeleteOrderDetail">
                            </cc1:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <table class="mtable" id="tabPrice" runat="server" visible="false">
        <tr>
            <td class="td02">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderDetailPrice" runat="server" Text="${MasterData.Order.OrderDetail.TotalPrice}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderDetailPrice" runat="server" onfocus="this.blur();" Width="150px" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderDiscountRate" runat="server" Text="${MasterData.Order.OrderHead.Discount}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderDiscountRate" runat="server" onChange="orderDiscountRateChanged(this);"
                    onmouseup="if(!readOnly)select();" Width="65px" />%
                <asp:TextBox ID="tbOrderDiscount" runat="server" onChange="orderDiscountChanged(this);"
                    onmouseup="if(!readOnly)select();" Width="63px" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderPrice" runat="server" Text="${MasterData.Order.OrderHead.Price}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderPrice" runat="server" Visible="true" onfocus="this.blur();"
                    Width="150px" />
            </td>
        </tr>
    </table>
</fieldset>
<uc2:OrderTracer ID="ucOrderTracer" runat="server" Visible="false" />
<div style="width:100%;height:200px;">

</div>


