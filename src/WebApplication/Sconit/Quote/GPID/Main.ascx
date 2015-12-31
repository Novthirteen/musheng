<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_GPID_Main" %>

<%@ Register Src="~/Quote/GPID/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/GPID/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/GPID/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/GPID/New.ascx" TagName="New" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />