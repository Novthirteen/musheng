<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Cost_StandardCost_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItemCode" runat="server" Text="${Cost.StandardCost.Item}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItemCode" runat="server" Visible="true" Width="250" DescField="Description"
                    ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.StandardCost.CostElement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCostElement" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                    Width="250" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCostGroup" runat="server" Text="${Cost.StandardCost.CostGroup}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCostGroup" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostGroupMgr.service" ServiceMethod="GetAllCostGroup"
                    Width="250" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
        </tr>
        <tr>
            <td colspan="3">
            </td>
            <td class="td02">
                <div class="buttons">
                    <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                        CssClass="query" />
                    <asp:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                        CssClass="add" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
