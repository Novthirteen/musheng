<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MasterData_PriceList_PriceList_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlParty" runat="server" Text="${MasterData.Customer.Code}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbParty" runat="server" Width="250" DescField="Name" ValueField="Code"
                    MustMatch="true" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlCode" runat="server" Text="${MasterData.PriceList.Code}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbCode" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlItem" runat="server" Text="${MasterData.Item.Code}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItem" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td class="td01">
                包含无效:
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbInclude" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td />
            <td class="td02">
                <div class="buttons">
                    <asp:Button ID="btnSearchItemPriceList" runat="server" Text="${Common.Button.Search}物料价格"
                        OnClick="btnSearchItemPriceList_Click" CssClass="query" />
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="true" OnRowDataBound="GV_List_RowDataBound"
    CellPadding="0" AllowSorting="false">
    <Columns>
        <asp:TemplateField HeaderText="Seq">
            <ItemTemplate>
                <%#Container.DataItemIndex + 1%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
