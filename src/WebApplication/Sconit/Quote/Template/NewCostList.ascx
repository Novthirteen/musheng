<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewCostList.ascx.cs" Inherits="Quote_Template_NewCostList" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlName" runat="server" Text="${Quote.Template.CostList}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <%--<asp:Literal ID="ltlNumber" runat="server" Text="${Quote.Template.Number}:"></asp:Literal>--%>
                <asp:Literal ID="ltlUnit" runat="server" Text="${Quote.Template.Unit}:"></asp:Literal>
            </td>
            <td class="td02">
                <%--<asp:TextBox ID="txtNumber" runat="server" TextMode="MultiLine" Height="100px" Width="400px"></asp:TextBox>--%>
                <asp:TextBox ID="txtUnit" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <%--<asp:Literal ID="ltlUnit" runat="server" Text="${Quote.Template.Unit}:"></asp:Literal>--%>
                <asp:Literal ID="ltlNumber" runat="server" Text="${Quote.Template.Number}:"></asp:Literal>
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
                <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset>