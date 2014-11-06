<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_ItemDisCon_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc2" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_ItemDisCon" runat="server" DataSourceID="ODS_ItemDisCon" DefaultMode="Edit"
        Width="100%" DataKeyNames="Id" OnDataBound="FV_ItemDisCon_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.ItemDisCon.UpdateItemDisCon}</legend>
                <table class="mtable">
                   
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlItem" runat="server" Text="${MasterData.ItemDisCon.Item}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tdItem" runat="server" Visible="true" DescField="Description" ImageUrlField="ImageUrl"
                                Width="280" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tdItem" ErrorMessage="${MasterData.ItemDisCon.CodeExist1}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkItemExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlDiscontinueItem" runat="server" Text="${MasterData.ItemDisCon.DisconItem}" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tdDiscontinueItem" runat="server" Visible="true" DescField="Description"
                                ImageUrlField="ImageUrl" Width="280" ValueField="Code" ServicePath="ItemMgr.service"
                                ServiceMethod="GetCacheAllItem" CssClass="inputRequired" />
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="tdDiscontinueItem"
                                ErrorMessage="${MasterData.ItemDisCon.CodeExist2}" Display="Dynamic" ValidationGroup="vgSave"
                                OnServerValidate="checkItemExists1" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlUnitQty" runat="server" Text="${MasterData.ItemDisCon.UnitQty}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbUnitQty" runat="server" Text='<%# Bind("UnitQty") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="UnitQty" runat="server" ErrorMessage="${MasterData.ItemDisCon.UnitQty}${MasterData.ItemDisCon.Empty}"
                                Display="Dynamic" ControlToValidate="tbUnitQty" ValidationGroup="vgSave" />
                            <asp:RangeValidator ID="rvUC" ControlToValidate="tbUnitQty" runat="server" Display="Dynamic"
                                ErrorMessage="${Common.Validator.Valid.Number}" MaximumValue="999999999" MinimumValue="0.00000001"
                                Type="Double" ValidationGroup="vgSaveGroup" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlPriority" runat="server" Text="${MasterData.ItemDisCon.Priority}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbPriority" runat="server" Text='<%# Bind("Priority") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="${MasterData.ItemDisCon.Priority}${MasterData.ItemDisCon.Empty}"
                                Display="Dynamic" ControlToValidate="tbPriority" ValidationGroup="vgSave" />
                            <asp:RangeValidator ID="rvSeq" runat="server" ControlToValidate="tbPriority" ErrorMessage="${Common.Validator.Valid.Number}"
                                Display="Dynamic" Type="Integer" MinimumValue="0" MaximumValue="99999999" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlStartDate" runat="server" Text="${MasterData.ItemDisCon.StartDate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbStartDate" runat="server" Text='<%# Bind("StartDate") %>' CssClass="inputRequired"
                                onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                            <asp:RequiredFieldValidator ID="StartDate2" runat="server" ErrorMessage="${MasterData.ItemDisCon.StartDate}${MasterData.ItemDisCon.Empty}"
                                Display="Dynamic" ControlToValidate="tbStartDate" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="Literal6" runat="server" Text="${MasterData.ItemDisCon.EndDate}:" />
                            <asp:HiddenField ID="tbCreateDate" runat="server" Value='<%# Bind("CreateDate") %>' />
                            <asp:HiddenField ID="tbCreateUser" runat="server" Value='<%# Bind("CreateUser") %>' />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbEndDate" runat="server" Text='<%# Bind("EndDate") %>' onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                        </td>
                    </tr>
                     <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlBom" runat="server" Text="Bom" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tdBom" runat="server" />
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="tdBom"
                                ErrorMessage="${MasterData.ItemDisCon.CodeExist3}" Display="Dynamic" ValidationGroup="vgSave"
                                OnServerValidate="checkItemExists3" />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                            <div class="buttons">
                                <asp:Button ID="btnSave" runat="server" CommandName="Update" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <cc2:Button ID="btnDelete" runat="server" CommandName="Delete" Text="${Common.Button.Delete}"
                                    CssClass="delete" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" FunctionId="DeleteItemDisContinue" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </EditItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_ItemDisCon" runat="server" TypeName="com.Sconit.Web.ItemDisConMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.ItemDiscontinue" UpdateMethod="UpdateItemDiscontinue"
    OnUpdated="ODS_ItemDisCon_Updated" SelectMethod="LoadItemDiscontinue" OnUpdating="ODS_ItemDisCon_Updating"
    DeleteMethod="DeleteItemDiscontinue" OnDeleted="ODS_ItemDisCon_Deleted">
    <SelectParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
