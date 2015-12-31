<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_ItemPack_New" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlSpec" runat="server" Text="${MasterData.ItemPack.Spec}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSpec" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlDesc" runat="server" Text="${MasterData.ItemPack.Desc}"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPinNum" runat="server" Text="${MasterData.ItemPack.PinNum}"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPinNum" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlPinConversion" runat="server" Text="${MasterData.ItemPack.PinConversion}"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPinConversion" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset> 