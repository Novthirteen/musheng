<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabNavigator.ascx.cs" Inherits="Cost_CostGroup_TabNavigator" %>
        
<div class="AjaxClass  ajax__tab_default">
    <div class="ajax__tab_header">
    
        <span class='ajax__tab_active' id='tab_CostGroup' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span 
        class='ajax__tab_tab'><asp:LinkButton ID="lbCostGroup" Text="${Cost.CostGroup}" runat="server" OnClick="lbCostGroup_Click" /></span></span></span></span><span 
        id='tab_CostCenter' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span 
        class='ajax__tab_tab'><asp:LinkButton ID="lbCostCenter" Text="${Cost.CostCenter}" runat="server" OnClick="lbCostCenter_Click" /></span></span></span></span>
    </div>