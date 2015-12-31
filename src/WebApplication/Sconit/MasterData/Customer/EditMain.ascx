<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditMain.ascx.cs" Inherits="MasterData_Customer_EditMain" %>
<%@ Register Src="TabNavigator.ascx" TagName="TabNavigator" TagPrefix="uc2" %>
<%@ Register Src="Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="../Address/Main.ascx" TagName="BillAddress" TagPrefix="uc2" %>
<%@ Register Src="../Address/Main.ascx" TagName="ShipAddress" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Customer/QuoteFeeList.ascx" TagName="QuoteFee" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Customer/EditQuoteFee.ascx" TagName="EditQuoteFee" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Customer/NewQuoteFee.ascx" TagName="NewQuoteFee" TagPrefix="uc2" %>

<div>
<uc2:TabNavigator ID="ucTabNavigator" runat="server" Visible="true" />
    <div class="ajax__tab_body">
        <uc2:Edit ID="ucEdit" runat="server" Visible="true" />
        <uc2:BillAddress ID="ucBillAddress" runat="server" Visible="false" />
         <uc2:ShipAddress ID="ucShipAddress" runat="server" Visible="false" />
        <uc2:QuoteFee ID="ucQuoteFee" runat="server" Visible="false"/>
        <uc2:EditQuoteFee ID="ucEditQuoteFee" runat="server" Visible="false"/>
        <uc2:NewQuoteFee ID="ucNewQuoteFee" runat="server" Visible="false" />
    </div>
</div>
