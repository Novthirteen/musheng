<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditMain.ascx.cs" Inherits="Cost_CostGroup_EditMain" %>
<%@ Register Src="TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>
<%@ Register Src="Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="CostCenter/Main.ascx" TagName="CostCenter" TagPrefix="uc2" %>
<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" />
<div class="ajax__tab_body">
    <uc2:Edit ID="ucEdit" runat="server" Visible="true" />
    <uc2:CostCenter ID="ucCostCenter" runat="server" Visible="false" />
</div>
</div> 