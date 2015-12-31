<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_ProductInfo_Main" %>

<%@ Register Src="~/Quote/ProductInfo/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/ProductInfo/New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/ProductInfo/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register src="~/Quote/ProductInfo/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false"/>
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />