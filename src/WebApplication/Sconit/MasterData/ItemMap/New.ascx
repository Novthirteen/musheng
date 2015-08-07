<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_ItemMap_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>

<fieldset>
    <legend>${MasterData.ItemMap.NewItemMap}</legend>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlItem" runat="server" Text="${MasterData.ItemDisCon.Item}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tdItem" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                    Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                    CssClass="inputRequired" />
                <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tdItem" ErrorMessage="${MasterData.ItemMap.CodeExist1}"
                    Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkItemExists" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlMapItem" runat="server" Text="${MasterData.ItemDisCon.DisconItem}" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tdMapItem" runat="server" Visible="true" DescField="Description"
                    ImageUrlField="ImageUrl" Width="280" ValueField="Code" ServicePath="ItemMgr.service"
                    ServiceMethod="GetCacheAllItem" CssClass="inputRequired" />
                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="tdMapItem"
                    ErrorMessage="${MasterData.ItemMap.CodeExist2}" Display="Dynamic" ValidationGroup="vgSave"
                    OnServerValidate="checkItemExists1" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlStartDate" runat="server" Text="${MasterData.ItemDisCon.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" CssClass="inputRequired"
                    onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                <asp:RequiredFieldValidator ID="StartDate2" runat="server" ErrorMessage="${MasterData.ItemDisCon.StartDate}${MasterData.ItemDisCon.Empty}"
                    Display="Dynamic" ControlToValidate="tbStartDate" ValidationGroup="vgSave" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${MasterData.ItemDisCon.EndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
            </td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <div class="buttons">
                    <asp:Button ID="btnInsert" runat="server" Text="${Common.Button.Save}" OnClick="btnInsert_Click"
                        CssClass="apply" ValidationGroup="vgSave" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                        CssClass="back" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
