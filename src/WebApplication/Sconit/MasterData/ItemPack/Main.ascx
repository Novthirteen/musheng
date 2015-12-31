<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MasterData_ItemPack_Main" %>

<%@ Register Src="~/MasterData/ItemPack/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemPack/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemPack/New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemPack/Edit.ascx" TagName="Edit" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />