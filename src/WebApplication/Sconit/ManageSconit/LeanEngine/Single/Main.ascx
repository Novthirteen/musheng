<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="ManageSconit_LeanEngine_Single_Main" %>
<%@ Register Src="Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="Order.ascx" TagName="Order" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" Visible="true" OnbtnSearchClick="ucSearch_btnSearchClick" />
<uc2:List ID="ucList" runat="server" Visible="false" OnlbViewClick="ucList_lbViewClick" />
<uc2:Order ID="ucOrder" runat="server" Visible="false" OnbtnBackClick="ucOrder_btnBackClick" />
