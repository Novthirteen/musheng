<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_Item_Main" %>

<%@ Register Src="~/Quote/Item/New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Item/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Item/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Item/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>

<%@ Register Src="~/Quote/Item/NewList.ascx" TagName="NewList" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Item/NewSearch.ascx" TagName="NewSearch" TagPrefix="uc2" %>

<%@ Register Src="~/Quote/Item/PriceList.ascx" TagName="PriceList" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Item/TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>

<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" />
<div class="ajax__tab_body">
    <table class="mtable">
        <tr>
            <td>
                <uc2:NewSearch ID="ucNewSearch" runat="server" Visible="false" />
                <uc2:PriceList ID="ucPriceList" runat="server" Visible="false" />
            </td>
        </tr>
    </table>
</div>
<uc2:Search ID="ucSearch" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<%--<uc2:NewSearch ID="ucNewSearch" runat="server" Visible="" />--%>
<uc2:NewList ID="ucNewList" runat="server" Visible="false" />