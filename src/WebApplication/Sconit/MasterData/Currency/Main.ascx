<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MasterData_Currency_Main" %>
<%@ Register Src="~/MasterData/Currency/TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Currency/CurrencyExchange.ascx" TagName="CurrencyEx" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Currency/Currency.ascx" TagName="Currency" TagPrefix="uc2" %>

<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" />
<div class="ajax__tab_body">
    <uc2:Currency ID="ucCurrency" runat="server" Visible="true" />
    <uc2:CurrencyEx ID="ucCurrencyEx" runat="server" Visible="false" />
</div>
