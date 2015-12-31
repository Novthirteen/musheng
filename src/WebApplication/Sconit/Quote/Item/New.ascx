<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Quote_Item_New" %>

<script type="text/javascript">
    $(function () {
        $("#<%=txtSingleNum.ClientID %>").blur(
            function () {
                var a = $("#<%=txtSingleNum.ClientID %>").val();
                var b = $("#<%=txtPurchasePrice.ClientID%>").val();
                $("#<%=txtPrice.ClientID%>").attr("value", a * b);
            }
            )

        $("#<%=txtPurchasePrice.ClientID %>").blur(
            function () {
                var a = $("#<%=txtSingleNum.ClientID %>").val();
                var b = $("#<%=txtPurchasePrice.ClientID%>").val();
                $("#<%=txtPrice.ClientID%>").attr("value", a * b);
            }
            )

        <%--$("#<%=txtPinConversion.ClientID%>").focus(
            function () {
                $("#<%=txtPinConversion.ClientID%>").attr("value",<%=ret()%>);
            }
            )--%>
    })
</script>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Item.Code}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtItemCode" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSupplier" runat="server" Text="${MasterData.ItemQuote.Supplier}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSupplier" runat="server" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlCategory" runat="server" Text="${MasterData.ItemQuote.Category}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtCategory" runat="server" ></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlBrand" runat="server" Text="${MasterData.ItemQuote.Brand}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtBrand" runat="server" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlModel" runat="server" Text="${MasterData.ItemQuote.Model}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtModel" runat="server" ></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSingleNum" runat="server" Text="${MasterData.ItemQuote.SingleNum}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSingleNum" runat="server" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPurchasePrice" runat="server" Text="${MasterData.ItemQuote.PurchasePrice}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPurchasePrice" runat="server" ></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlPrice" runat="server" Text="${MasterData.ItemQuote.Price}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPrice" runat="server" Enabled="false" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPinNum" runat="server" Text="${MasterData.ItemQuote.PinNum}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPinNum" runat="server" ></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlPinConversion" runat="server" Text="${MasterData.ItemQuote.PinConversion}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPinConversion" runat="server" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlPoint" runat="server" Text="${MasterData.ItemQuote.Point}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPoint" runat="server" Enabled="false" ></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlProjectId" runat="server" Text="${Quote.Tooling.ProjectNo}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProjectId" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click" />
                <asp:Button ID="btnBack" runat="server"  Text="${Common.Button.Back}" OnClick="btnBack_Click"/>
            </td>
        </tr>
    </table>
</fieldset>