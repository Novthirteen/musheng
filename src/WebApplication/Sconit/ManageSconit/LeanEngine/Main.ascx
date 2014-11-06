<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="ManageSconit_LeanEngine_Main" %>
<%@ Register Src="TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>
<%@ Register Src="~/ManageSconit/LeanEngine/Single/Main.ascx" TagName="Single" TagPrefix="uc2" %>
<%@ Register Src="~/ManageSconit/LeanEngine/Multi/Main.ascx" TagName="Multi" TagPrefix="uc2" %>

<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" OnlbSingleClick="ucTabNavigator_lbSingleClick"
    OnlbMultiClick="ucTabNavigator_lbMultiClick" />
<div class="ajax__tab_body">
    <uc2:Single ID="ucSingle" runat="server" Visible="true" />
    <uc2:Multi ID="ucMulti" runat="server" Visible="false" />
</div>
</div> 