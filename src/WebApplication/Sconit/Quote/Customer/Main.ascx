<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_Customer_Main" %>

<%@ Register Src="~/Quote/Customer/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Customer/New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Customer/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Customer/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />