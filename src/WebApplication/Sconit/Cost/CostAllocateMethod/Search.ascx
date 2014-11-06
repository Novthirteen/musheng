<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="Cost_CostAllocateMethod_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCostGroup" runat="server" Text="${Cost.CostAllocateMethod.CostGroup}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCostGroup" runat="server" Visible="true" Width="250" DescField="Description"
                    ValueField="Code" ServicePath="CostGroupMgr.service" ServiceMethod="GetAllCostGroup" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblCostCenter" runat="server" Text="${Cost.CostAllocateMethod.CostCenter}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCostCenter" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostCenterMgr.service" ServiceMethod="GetCostCenterList"
                    ServiceParameter="string:#tbCostGroup" Width="250" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.CostAllocateMethod.CostElement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbCostElement" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                    Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblDependCostElement" runat="server" Text="${Cost.CostAllocateMethod.DependCostElement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbDependCostElement" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="CostElementMgr.service" ServiceMethod="GetAllCostElement"
                    Width="250" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblExpenseElement" runat="server" Text="${Cost.CostAllocateMethod.ExpenseElement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbExpenseElement" runat="server" Visible="true" DescField="Description"
                    ValueField="Code" ServicePath="ExpenseElementMgr.service" ServiceMethod="GetAllExpenseElement"
                    Width="250" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblAllocateBy" runat="server" Text="${Cost.CostAllocateMethod.AllocateBy}:" />
            </td>
            <td class="td02">
                <cc1:codemstrdropdownlist id="ddlAllocateBy" code="AllocateBy" runat="server" includeblankoption="true">
                    </cc1:codemstrdropdownlist>
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
