<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_Tooling_Main" %>

<%@ Register Src="~/Quote/Tooling/Search.ascx" TagName="Sesrch" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Tooling/New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Tooling/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Tooling/List.ascx" TagName="List" TagPrefix="uc2" %>

<uc2:Sesrch ID="ucSearch" runat="server" Visible="true" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false"/>