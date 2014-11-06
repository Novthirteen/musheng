<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Production_PLFeedSeq_Search" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblProductLineFacility" runat="server" Text="${Common.Business.ProductionLineFacility}:" />
            </td>
            <td class="ttd02">
                <asp:textbox ID="tbProductLineFacility" runat="server" Visible="true"  />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCode" runat="server" Text="${Production.ProdutLineFeedSeqence.Code}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCode"  runat="server" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFinishGood" runat="server" Text="${Production.ProdutLineFeedSeqence.FinishGood}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFinishGood" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblRawMaterial" runat="server" Text="${Production.ProdutLineFeedSeqence.RawMaterial}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbRawMaterial" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
            <td>
                <div class="buttons">
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                    <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                        CssClass="add" />
                    <asp:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click"
                        CssClass="button2"  />
                    <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                        OnClick="btnExport_Click" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
