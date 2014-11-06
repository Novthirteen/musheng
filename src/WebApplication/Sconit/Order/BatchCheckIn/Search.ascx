<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Order_BatchCheckIn_Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Flow.Flow.Distribution}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service"  Width="250" />
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
