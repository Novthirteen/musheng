<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TabNavigator.ascx.cs"
    Inherits="MasterData_Currency_TabNavigator" %>
<div class="AjaxClass  ajax__tab_default">
    <div class="ajax__tab_header">
        <span class='ajax__tab_active' id='tab_Currency' runat="server"><span class='ajax__tab_outer'>
            <span class='ajax__tab_inner'><span class='ajax__tab_tab'>
                <asp:LinkButton ID="lbCurrency" Text="货币" runat="server" OnClick="lbCurrency_Click" /></span></span></span></span><span
                    id='tab_CurrencyEx' runat="server"><span class='ajax__tab_outer'><span class='ajax__tab_inner'><span
                        class='ajax__tab_tab'><asp:LinkButton ID="lbCurrencyEx" Text="汇率" runat="server"
                            OnClick="lbCurrencyEx_Click" /></span></span></span></span>
    </div>
</div>
