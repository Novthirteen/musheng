<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MasterData_Item_Main" %>
<%@ Register Src="~/MasterData/Item/EditMain.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Item/List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Item/New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="~/MasterData/Item/Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="Import/Main.ascx" TagName="Import" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" Visible="true" OnBtnImportClick="ucSearch_BtnImportClick"/>
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:Import ID="ucImport" runat="server" Visible="false" OnBtnBackClick="ucImport_BtnBackClick"/>
