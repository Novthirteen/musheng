<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Quote_Quotes_Main" %>

<%@ Register Src="~/Quote/Quotes/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Quotes/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Quotes/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Quotes/SubmitView.ascx" TagName="SubmitView" TagPrefix="uc2" %>
<%@ Register src="~/Quote/Quotes/ProjectSearch.ascx" TagName="ProjectSearch" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Quotes/ProjectList.ascx" TagName="ProjectList" TagPrefix="uc2" %>
<%@ Register Src="~/Quote/Quotes/View.ascx" TagName="View" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" Visible="false" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:SubmitView ID="ucSubmitView" runat="server" Visible="false" />
<uc2:ProjectSearch ID="ucProjectSearch" runat="server" />
<uc2:ProjectList ID="ucProjectList" runat="server" Visible="false" />
<uc2:View ID="ucView" runat="server" Visible="false"/>
