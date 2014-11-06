<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Order_BatchPrint_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblRegion" runat="server" Text="${MasterData.Order.PrintOrder.Region}:" />
            </td>
            <td class="td02">
                <uc3:textbox id="tbRegion" runat="server" visible="true" width="250" descfield="Name"
                    valuefield="Code" servicepath="PartyMgr.service" servicemethod="GetFromParty"/>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlOrderNo" runat="server" Text="${Warehouse.LocTrans.OrderNo}:" />
            </td>
            <td class="td02">
                 <asp:TextBox ID="tbOrderNo" runat="server"  />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartDate" runat="server" Text="${MasterData.Order.PrintOrder.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblStatus" runat="server" Text="${Common.CodeMaster.Status}:" />
            </td>
            <td class="td02">
                <asp:DropDownList ID="ddlStatus" runat="server" DataTextField="Description" DataValueField="Value" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <td class="td01">
                </td>
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
