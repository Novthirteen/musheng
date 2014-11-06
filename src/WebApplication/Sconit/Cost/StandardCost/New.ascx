<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Cost_StandardCost_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV">
    <asp:FormView ID="FV_StandardCost" runat="server" DataSourceID="ODS_StandardCost"
        DefaultMode="Insert" Width="100%" DataKeyNames="Id">
        <InsertItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblItemCode" runat="server" Text="${Cost.StandardCost.Item}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbItem" runat="server" Visible="true" Width="250" DescField="Description"
                                ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvItem" runat="server" ControlToValidate="tbItem"
                                Display="Dynamic" ErrorMessage="${Cost.StandardCost.Item.Required}" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvItem" runat="server" ControlToValidate="tbItem" ErrorMessage="${Cost.StandardCost.Exists}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkStandardCostExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.StandardCost.CostElement}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbCostElement" runat="server" Visible="true" DescField="Description"
                                ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                                Width="250" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvCostElement" runat="server" ControlToValidate="tbCostElement"
                                Display="Dynamic" ErrorMessage="${Cost.StandardCost.CostElement.Required}" ValidationGroup="vgSave" />
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCostGroup" runat="server" Text="${Cost.StandardCost.CostGroup}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbCostGroup" runat="server" Visible="true" DescField="Description"
                                ValueField="Code" ServicePath="CostGroupMgr.service" ServiceMethod="GetAllCostGroup"
                                Width="250" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvCostGroup" runat="server" ControlToValidate="tbCostElement"
                                Display="Dynamic" ErrorMessage="${Cost.StandardCost.CostGroup.Required}" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCost" runat="server" Text="${Cost.StandardCost.Cost}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCost" runat="server" Text='<%# Bind("Cost") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="tbCost"
                                Display="Dynamic" ErrorMessage="${Cost.StandardCost.Cost.Required}" ValidationGroup="vgSave" />
                            <asp:RangeValidator ID="rvCost" ControlToValidate="tbCost" runat="server" Display="Dynamic"
                                ErrorMessage="${Common.Validator.Valid.Number}" MaximumValue="9999999999" MinimumValue="0"
                                Type="Double" ValidationGroup="vgSave" />
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
                                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_StandardCost" runat="server" TypeName="com.Sconit.Web.StandardCostMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.StandardCost" InsertMethod="CreateStandardCost"
    OnInserting="ODS_StandardCost_Inserting" OnInserted="ODS_StandardCost_Inserted">
</asp:ObjectDataSource>
