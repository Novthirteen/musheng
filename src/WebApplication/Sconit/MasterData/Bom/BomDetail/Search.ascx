<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MasterData_Bom_BomDetail_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <table class="mtable">
        <tr>  
            <td class="ttd01">
                <asp:Literal ID="lblParCode" runat="server" Text="${MasterData.Bom.ParCode}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbParCode" runat="server" Visible="true" />
            </td>
            <td class="ttd01">
                <asp:Literal ID="lblCompCode" runat="server" Text="${MasterData.Bom.CompCode}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbCompCode" runat="server" Visible="true" />
            </td>
        </tr>
        <tr>
            <td class="ttd01">
                <asp:Literal ID="lblIncludeInactive" runat="server" Text="${MasterData.Bom.IncludeInactive}:" />
            </td>
            <td class="ttd02">
                <asp:CheckBox ID="cbIncludeInactive" runat="server"  OnCheckedChanged="cbIncludeInactive_CheckedChanged" AutoPostBack="true"/>
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <div class="buttons">
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                    <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                        CssClass="add" />
                    <cc1:Button ID="btnImport" runat="server" OnClick="btnImport_Click" Text="${Common.Button.Import}"
                        CssClass="button2" FunctionId="BomImport" />
                    <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
                        OnClick="btnExport_Click" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
