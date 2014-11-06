<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabNavigator.ascx.cs" Inherits="ManageSconit_LeanEngine_TabNavigator" %>

<div class="AjaxClass  ajax__tab_default">
    <div class="ajax__tab_header">
        <span class='ajax__tab_active' id='tab_single' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span 
        class='ajax__tab_tab'><asp:LinkButton ID="lbSingle" Text="${LeanEngine.TabName.Single}" runat="server" OnClick="lbSingle_Click" /></span></span></span></span><span 
        id='tab_multi' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span 
        class='ajax__tab_tab'><asp:LinkButton  ID="lbMulti" Text="MRP" runat="server" OnClick="lbMulti_Click" /></span></span></span></span>
     </div>

