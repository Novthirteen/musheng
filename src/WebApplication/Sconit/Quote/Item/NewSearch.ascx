<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewSearch.ascx.cs" Inherits="Quote_Item_NewSearch" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProductID" runat="server" Text="${Quote.Product.ID}:"></asp:Literal>
            </td>
            <td class="td02">
                <uc3:textbox ID="txtProjectId" runat="server" Visible="true" Width="250" MustMatch="false" AutoPostBack="true"
                    DescField="ProductNo" ValueField="Id" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetProduct" OnTextChanged="txtPID_Change" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlBOM" runat="server" Text="${Quote.Item.BOM}:"></asp:Literal>
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" Width="250" MustMatch="false" AutoPostBack="true"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" OnTextChanged="txtItem_Change" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:RadioButton ID="rbXLS" runat="server" Checked="true" />XLS模板
            </td>
            <td class="td01"></td>
            <td class="td02">
                <asp:FileUpload ID="fuXLS" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click" />
                <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click" />
            </td>
        </tr>
    </table>
</fieldset>
