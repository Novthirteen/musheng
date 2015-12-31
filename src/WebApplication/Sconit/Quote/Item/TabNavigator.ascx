<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabNavigator.ascx.cs" Inherits="Quote_Item_TabNavigator" %>

<div class="AjaxClass  ajax__tab_default">
    <div class="ajax__tab_header">
        <span class='ajax__tab_active' id='tab_bom' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span
            class='ajax__tab_tab'>
            <asp:LinkButton ID="lbBomView" Text="${MasterData.Bom.TabName.Bom}" runat="server" OnClick="lbBom_Click" /></span></span></span></span><span
                id='tab_price' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span
                    class='ajax__tab_tab'><asp:LinkButton ID="lbBom" Text="　${Quote.Item.Tab.Price}" runat="server" OnClick="lbPrice_Click" /></span></span></span></span>
    </div>
</div>
