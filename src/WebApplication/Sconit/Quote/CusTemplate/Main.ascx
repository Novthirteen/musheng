<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_CusTemplate_Main" %>
<%@ Register Src="~/Quote/CusTemplate/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/CusTemplate/New.ascx" TagName="New" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" />
<uc2:New ID="ucNew" runat="server" Visible="false" />