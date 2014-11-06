<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="MasterData_Flow_View" %>
<%@ Register Src="ViewFlow.ascx" TagName="Flow" TagPrefix="uc2" %>
<%@ Register Src="../ViewStrategy.ascx" TagName="Strategy" TagPrefix="uc2" %>
<%@ Register Src="../FlowDetail/Search.ascx" TagName="DetailSearch" TagPrefix="uc2" %>
<%@ Register Src="../FlowDetail/ViewList.ascx" TagName="DetailList" TagPrefix="uc2" %>
<%@ Register Src="../FlowDetail/View.ascx" TagName="ViewDetail" TagPrefix="uc2" %>
<%@ Register Src="../FlowRouting/ViewRouting.ascx" TagName="Routing" TagPrefix="uc2" %>
<%@ Register Src="../FlowRouting/List.ascx" TagName="RoutingList" TagPrefix="uc2" %>
<%@ Register Src="../FlowRouting/ViewRouting.ascx" TagName="ReturnRouting" TagPrefix="uc2" %>
<%@ Register Src="../FlowRouting/List.ascx" TagName="ReturnRoutingList" TagPrefix="uc2" %>
<%@ Register Src="../ViewLocTransMain.ascx" TagName="LocTrans" TagPrefix="uc2" %>
<%@ Register Src="../ViewActingBillMain.ascx" TagName="ActingBill" TagPrefix="uc2" %>
<%@ Register Src="../Facility/Search.ascx" TagName="FacilitySearch" TagPrefix="uc2" %>
<%@ Register Src="../Facility/ViewList.ascx" TagName="FacilityList" TagPrefix="uc2" %>
<%@ Register Src="../Facility/View.ascx" TagName="ViewFacility" TagPrefix="uc2" %>
<fieldset>
    <legend>${MasterData.Flow.Flow.Production}</legend>
    <uc2:Flow ID="ucFlow" runat="server" Visible="true" />
</fieldset>
<uc2:Strategy ID="ucStrategy" runat="server" Visible="true" />
<fieldset id="fdDetail" runat="server" visible="false">
    <legend>${MasterData.Flow.Detail.Production}</legend>
    <uc2:DetailSearch ID="ucDetailSearch" runat="server" Visible="false" />
    <uc2:DetailList ID="ucDetailList" runat="server" Visible="true" />
    <uc2:ViewDetail ID="ucViewDetail" runat="server" Visible="false" />
</fieldset>

<fieldset id="fdFacility" runat="server" visible="false">
    <legend>${MasterData.Flow.Facility.Info}</legend>
    <uc2:FacilitySearch ID="ucFacilitySearch" runat="server" Visible="false" />
    <uc2:FacilityList ID="ucFacilityList" runat="server" Visible="true" />
    <uc2:ViewFacility ID="ucViewFacility" runat="server" Visible="false" />
</fieldset>


<fieldset id="fdProductionRouting" runat="server" visible="false">
    <legend>${MasterData.Flow.Routing.Production}</legend>
    <fieldset id="fdRouting" runat="server" visible="false">
        <legend>${MasterData.Flow.Routing.Production}</legend>
        <uc2:Routing ID="ucRouting" runat="server" Visible="true" />
        <uc2:RoutingList ID="ucRoutingList" runat="server" Visible="false" />
    </fieldset>
    <fieldset id="fdReturn" runat="server" visible="false">
        <legend>${MasterData.Flow.ReturnRouting}</legend>
        <uc2:ReturnRouting ID="ucReturnRouting" runat="server" Visible="true" />
        <uc2:ReturnRoutingList ID="ucReturnRoutingList" runat="server" Visible="false" />
    </fieldset>
</fieldset>
<fieldset id="fdLocTrans" runat="server">
    <legend>${MasterData.Flow.LocTrans}</legend>
    <uc2:LocTrans ID="ucLocTrans" runat="server" />
</fieldset>
<div class="tablefooter">
    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" CssClass="button2"
        OnClick="btnBack_Click" />
</div>
