<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Cost_CostAllocateTransaction_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="divFV">
    <fieldset>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblExpenseElement" runat="server" Text="${Cost.CostAllocateTransaction.ExpenseElement}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbExpenseElement" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="ExpenseElementMgr.service" ServiceMethod="GetAllExpenseElement"
                        Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvExpenseElement" runat="server" ControlToValidate="tbExpenseElement"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateTransaction.ExpenseElement}${Common.Business.Error.Required}"
                        ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblAmount" runat="server" Text="${Cost.CostAllocateTransaction.Amount}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbAmount" runat="server" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ErrorMessage="${Cost.CostAllocateTransaction.Amount}${Common.Business.Error.Required}"
                        Display="Dynamic" ControlToValidate="tbAmount" ValidationGroup="vgSave" />
                    <asp:RangeValidator ID="rvAmount" ControlToValidate="tbAmount" runat="server" Display="Dynamic"
                        ErrorMessage="${Common.Validator.Valid.Number}" MaximumValue="999999999" MinimumValue="0.00000001"
                        Type="Double" ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblEffDate" runat="server" Text="${Cost.CostAllocateTransaction.EffDate}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbEffDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"
                        CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvEffDate" runat="server" ControlToValidate="tbEffDate"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateTransaction.EffDate}${Common.Business.Error.Required}"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>${Cost.CostAllocateTransaction.AllocationRules}</legend>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblDependCostElement" runat="server" Text="${Cost.CostAllocateTransaction.DependCostElement}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbDependCostElement" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                        Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvDependCostElement" runat="server" ControlToValidate="tbDependCostElement"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateTransaction.DependCostElement}${Common.Business.Error.Required}"
                        ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblAllocateBy" runat="server" Text="${Cost.CostAllocateTransaction.AllocateBy}:" />
                </td>
                <td class="td02">
                    <cc1:CodeMstrDropDownList ID="ddlAllocateBy" Code="AllocateBy" runat="server" IncludeBlankOption="true">
                    </cc1:CodeMstrDropDownList>
                    <asp:RequiredFieldValidator ID="rfvAllocateBy" runat="server" ControlToValidate="ddlAllocateBy"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateTransaction.AllocateBy}${Common.Business.Error.Required}"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblItemCategorys" runat="server" Text="${Cost.CostAllocateTransaction.ItemCategorys}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbItemCategorys" runat="server" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblItems" runat="server" Text="${Cost.CostAllocateTransaction.Items}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbItems" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblOrders" runat="server" Text="${Cost.CostAllocateTransaction.Orders}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbOrders" runat="server" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblReferenceItems" runat="server" Text="${Cost.CostAllocateTransaction.ReferenceItems}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbReferenceItems" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>${Cost.CostAllocateTransaction.AllocatedTo}</legend>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblCostGroup" runat="server" Text="${Cost.CostGroup}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCostGroup" runat="server" Visible="true" Width="250" DescField="Description"
                        ValueField="Code" ServicePath="CostGroupMgr.service" ServiceMethod="GetAllCostGroup" />
                    <asp:RequiredFieldValidator ID="rfvCostGroup" runat="server" ControlToValidate="tbCostGroup"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateMethod.CostGroup.Required}"
                        ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblCostCenter" runat="server" Text="${Cost.CostAllocateTransaction.CostCenter}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCostCenter" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="CostCenterMgr.service" ServiceMethod="GetAllCostCenter"
                        Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvCostCenter" runat="server" ControlToValidate="tbCostCenter"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateTransaction.CostCenter}${Common.Business.Error.Required}"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.CostAllocateTransaction.CostElement}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCostElement" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                        Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvCostElement" runat="server" ControlToValidate="tbCostElement"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateTransaction.CostElement}${Common.Business.Error.Required}"
                        ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
            </tr>
        </table>
    </fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <div class="buttons">
                    <asp:Button ID="btnInsert" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click"
                        CssClass="apply" ValidationGroup="vgSave" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                        CssClass="back" />
                </div>
            </td>
        </tr>
    </table>
</div>
