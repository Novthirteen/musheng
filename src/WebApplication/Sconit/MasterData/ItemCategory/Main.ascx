<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MasterData_ItemCategory_Main" %>
<%@ Register Src="~/MasterData/ItemCategory/Edit.ascx"TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemCategory/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemCategory/New.ascx"TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/ItemCategory/Search.ascx" TagName="Search" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" Visible="True" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:New ID="ucNew" runat="server" Visible="false" />