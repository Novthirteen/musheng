<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Cost_CostAllocateMethod_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="divFV">
    <fieldset>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblExpenseElement" runat="server" Text="${Cost.CostAllocateMethod.ExpenseElement}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbExpenseElement" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="ExpenseElementMgr.service" ServiceMethod="GetAllExpenseElement"
                        Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvExpenseElement" runat="server" ControlToValidate="tbExpenseElement"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateMethod.ExpenseElement.Required}"
                        ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>${Cost.CostAllocateMethod.AllocationRules}</legend>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblDependCostElement" runat="server" Text="${Cost.CostAllocateMethod.DependCostElement}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbDependCostElement" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                        Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvDependCostElement" runat="server" ControlToValidate="tbDependCostElement"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateMethod.DependCostElement.Required}"
                        ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblAllocateBy" runat="server" Text="${Cost.CostAllocateMethod.AllocateBy}:" />
                </td>
                <td class="td02">
                    <cc1:CodeMstrDropDownList ID="ddlAllocateBy" Code="AllocateBy" runat="server" IncludeBlankOption="true">
                    </cc1:CodeMstrDropDownList>
                    <asp:RequiredFieldValidator ID="rfvAllocateBy" runat="server" ControlToValidate="ddlAllocateBy"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateMethod.AllocateBy.Required}"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>${Cost.CostAllocateMethod.AllocatedTo}</legend>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblCostGroup" runat="server" Text="${Cost.CostAllocateMethod.CostGroup}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCostGroup" runat="server" Visible="true" Width="250" DescField="Description"
                        ValueField="Code" ServicePath="CostGroupMgr.service" ServiceMethod="GetAllCostGroup"
                        CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvCostGroup" runat="server" ControlToValidate="tbCostGroup"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateMethod.CostGroup.Required}"
                        ValidationGroup="vgSave" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblCostCenter" runat="server" Text="${Cost.CostAllocateMethod.CostCenter}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCostCenter" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="CostCenterMgr.service" ServiceMethod="GetCostCenterList"
                        ServiceParameter="string:#tbCostGroup" Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvCostCenter" runat="server" ControlToValidate="tbCostElement"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateMethod.CostCenter.Required}"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.CostAllocateMethod.CostElement}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCostElement" runat="server" Visible="true" DescField="Description"
                        ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                        Width="250" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvCostElement" runat="server" ControlToValidate="tbCostElement"
                        Display="Dynamic" ErrorMessage="${Cost.CostAllocateMethod.CostElement.Required}"
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
