<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Production_PLFeedSeqInfo_Main" %>
<%@ Register Src="List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="Search.ascx" TagName="Search" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" Visible="True" OnBtnImportClick="ucSearch_BtnImportClick" />
<uc2:List ID="ucList" runat="server" Visible="false" />