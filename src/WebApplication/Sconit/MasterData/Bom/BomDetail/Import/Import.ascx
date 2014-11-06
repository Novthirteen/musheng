<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Import.ascx.cs" Inherits="MasterData_Bom_BomDetail_Import_Import" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ac1" %>


<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlSelect" runat="server" Text="${Common.FileUpload.PleaseSelect}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlTemplate" runat="server" Text="${Common.Business.Template}:" />
            </td>
            <td class="td02">
                <asp:HyperLink ID="hlTemplate" runat="server" Text="${Common.Business.ClickToDownload}"
                    NavigateUrl="~/Reports/Templates/MuSheng/BomDetailSample.xls"></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <div class="buttons">
                    <asp:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click"
                        CssClass="apply" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                        CssClass="back" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
