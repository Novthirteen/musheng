<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="MasterData_Bom_BomDetail_Main" %>
<%@ Register Src="Search.ascx" TagName="Search" TagPrefix="uc2" %>
<%@ Register Src="List.ascx" TagName="List" TagPrefix="uc2" %>
<%@ Register Src="New.ascx" TagName="New" TagPrefix="uc2" %>
<%@ Register Src="Edit.ascx" TagName="Edit" TagPrefix="uc2" %>
<%@ Register Src="Import/Main.ascx" TagName="Import" TagPrefix="uc2" %>

<uc2:Search ID="ucSearch" runat="server" Visible="true" OnBtnImportClick="ucSearch_BtnImportClick" />
<uc2:List ID="ucList" runat="server" Visible="false" />
<uc2:New ID="ucNew" runat="server" Visible="false" />
<uc2:Edit ID="ucEdit" runat="server" Visible="false" />
<uc2:Import ID="ucImport" runat="server" Visible="false" OnBtnBackClick="ucImport_BtnBackClick"/>