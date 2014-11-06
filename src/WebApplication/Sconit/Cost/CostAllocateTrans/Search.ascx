<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Cost_CostAllocateTransaction_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCostCenter" runat="server" Text="${Cost.CostAllocateTransaction.CostCenter}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCostCenter" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostCenterMgr.service" ServiceMethod="GetCostCenter"
                    Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblExpenseElement" runat="server" Text="${Cost.CostAllocateTransaction.ExpenseElement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbExpenseElement" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="ExpenseElementMgr.service" ServiceMethod="GetAllExpenseElement"
                    Width="250" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.CostAllocateTransaction.CostElement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCostElement" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                    Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblDependCostElement" runat="server" Text="${Cost.CostAllocateTransaction.DependCostElement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbDependCostElement" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                    Width="250" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartDate" runat="server" Text="${Cost.CostAllocateTransaction.EffDateFrom}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblEndDate" runat="server" Text="${Cost.CostAllocateTransaction.EffDateTo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
            </td>
        </tr>
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
