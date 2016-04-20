<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Order_OrderIssueDetail_Main" %>
<%@ Register Src="Search.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="FlowInfo.ascx" TagName="FlowInfo" TagPrefix="uc2" %>
<%@ Register Src="List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/Warehouse/InProcessLocation/ViewMain.ascx" TagName="ViewShipList"
    TagPrefix="uc2" %>
<%@ Register Src="~/PickList/PickListInfo.ascx" TagName="PickListInfo" TagPrefix="uc2" %>
<uc2:New ID="ucSearch" runat="server" Visible="true" />
<uc2:FlowInfo ID="ucFlowInfo" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />

