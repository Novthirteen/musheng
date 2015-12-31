<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_Template_Main" %>

<%@ Register Src="~/Quote/Template/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Template/New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Template/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Template/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Template/ListCostList.ascx" TagName="ListCostList" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Template/EditCostList.ascx" TagName="EditCostList" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Template/NewCostList.ascx" TagName="NewCostList" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" />
<uc2:New ID="ucNew" runat="server" Visible="false"/>
<uc2:List ID="ucList" runat="server" Visible="false"/>
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<uc2:ListCostList ID="ucListCostList" runat="server" Visible="false"/>
<uc2:EditCostList ID="ucEditCostList" runat="server" Visible="false"/>
<uc2:NewCostList ID="ucNewCostList" runat="server" Visible="false"/>
