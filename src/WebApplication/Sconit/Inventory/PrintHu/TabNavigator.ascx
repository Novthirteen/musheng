﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabNavigator.ascx.cs"
Inherits="Inventory_PrintHu_TabNavigator" %>
<div class="AjaxClass  ajax__tab_default">
<div class="ajax__tab_header">
<span class='ajax__tab_active' id='tab_asn' runat="server">
<span class='ajax__tab_outer'>
<span class='ajax__tab_inner'>
<span class='ajax__tab_tab'>
<asp:LinkButton ID="lbAsn" Text="${Inventory.PrintHu.TabName.Asn}"  runat="server" OnClick="lbAsn_Click" />
</span>
</span>
</span>
</span>
<span id='tab_item' runat="server">
<span class='ajax__tab_outer'>
<span class='ajax__tab_inner'>
<span class='ajax__tab_tab'>
<asp:LinkButton ID="lbItem" Text="${Inventory.PrintHu.TabName.Item}" runat="server" OnClick="lbItem_Click" />
</span>
</span>
</span>
</span>
<span id='tab_flow' runat="server">
<span class='ajax__tab_outer'>
<span class='ajax__tab_inner'>
<span class='ajax__tab_tab'>
<asp:LinkButton ID="lbFlow" Text="${Inventory.PrintHu.TabName.Flow}" runat="server" OnClick="lbFlow_Click" />
</span>
</span>
</span>
</span>
<span id='tab_order' runat="server">
<span class='ajax__tab_outer'>
<span class='ajax__tab_inner'>
<span class='ajax__tab_tab'>
<asp:LinkButton ID="lbOrder" Text="${Inventory.PrintHu.TabName.Order}" runat="server" OnClick="lbOrder_Click" />
</span>
</span>
</span>
</span>

<span id='tab_receipt' runat="server">
<span class='ajax__tab_outer'>
<span class='ajax__tab_inner'>
<span class='ajax__tab_tab'>
<asp:LinkButton ID="lbReceipt" Text="${Inventory.PrintHu.TabName.Receipt}" runat="server" OnClick="lbReceipt_Click" />
</span>
</span>
</span>
</span>
<span id='tab_inventory' runat="server">
<span class='ajax__tab_outer'>
<span class='ajax__tab_inner'>
<span class='ajax__tab_tab'>
<asp:LinkButton ID="lbInventory" Text="${Inventory.PrintHu.TabName.Inventory}" runat="server" OnClick="lbInventory_Click" />
</span>
</span>
</span>
</span>
</div>
