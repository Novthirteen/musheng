<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditCostList.ascx.cs" Inherits="Quote_Template_EditCostList" %>

<fieldset>
    <legend>
        <asp:Literal ID="ltlCCName" runat="server"></asp:Literal>
    </legend>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlCostList" runat="server" Text="${Quote.Template.CostList}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtCostList" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <%--<asp:Literal ID="ltlNumber" runat="server" Text="${Quote.Template.Number}:"></asp:Literal>--%>
                <asp:Literal ID="ltlUnit" runat="server" Text="${Quote.Template.Unit}:"></asp:Literal>
            </td>
            <td class="td02">
                <%--<asp:TextBox ID="txtNumber" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
                <asp:TextBox ID="txtUnit" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlNumber" runat="server" Text="${Quote.Template.Number}:"></asp:Literal>
                <%--<asp:Literal ID="ltlUnit" runat="server" Text="${Quote.Template.Unit}:"></asp:Literal>--%>
            </td>
            <td class="td02">
                <%--<asp:TextBox ID="txtUnit" runat="server"></asp:TextBox>--%>
                <asp:TextBox ID="txtNumber" runat="server" TextMode="MultiLine" Height="100px" Width="400px"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlPrice" runat="server" Text="${Quote.Template.Price}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPrice" runat="server" TextMode="MultiLine" Height="100px" Width="400px"></asp:TextBox>
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