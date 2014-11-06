<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabNavigator.ascx.cs" Inherits="Picklist_TabNavigator" %>

<div class="AjaxClass  ajax__tab_default">
    <div class="ajax__tab_header">
        <span class='ajax__tab_active' id='tab_Picklist' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span 
        class='ajax__tab_tab'><asp:LinkButton ID="lbPicklistView" Text="${MasterData.Picklist.TabName.Picklist}" runat="server" OnClick="lbPicklist_Click" /></span></span></span></span><span 
        id='tab_Picklist_Batch' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span 
        class='ajax__tab_tab'><asp:LinkButton  ID="lbPicklist_Batch" Text="${MasterData.Picklist.TabName.Batch}" runat="server" OnClick="lbPicklist_Batch_Click" /></span></span></span></span>
     </div>

