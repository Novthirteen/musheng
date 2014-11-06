<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MasterData_ItemDisCon_Main" %>
<%@ Register Src="~/MasterData/ItemDisCon/Edit.ascx"TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemDisCon/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemDisCon/New.ascx"TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemDisCon/Search.ascx" TagName="Search" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" Visible="True" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:New ID="ucNew" runat="server" Visible="false" />