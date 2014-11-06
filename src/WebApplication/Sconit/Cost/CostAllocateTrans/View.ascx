<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="Cost_CostAllocateTransaction_View" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="floatdiv">
    <fieldset>
        <table class="mtable">
           <tr>
                <td class="td01">
                    <asp:Literal ID="lblCostCenter" runat="server" Text="${Cost.CostAllocateTransaction.CostCenter}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tbCostCenter" runat="server" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblExpenseElement" runat="server" Text="${Cost.CostAllocateTransaction.ExpenseElement}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tbExpenseElement" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.CostAllocateTransaction.CostElement}:" />
                </td>
                <td class="td02">
                      <asp:Label ID="tbCostElement" runat="server" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblDependCostElement" runat="server" Text="${Cost.CostAllocateTransaction.DependCostElement}:" />
                </td>
                <td class="td02">
                   <asp:Label ID="tbDependCostElement" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblAllocateBy" runat="server" Text="${Cost.CostAllocateTransaction.AllocateBy}:" />
                </td>
                <td class="td02">
                    <cc1:CodeMstrLabel ID="ddlAllocateBy" runat="server" Code="AllocateBy" Value='<%# Bind("AllocateBy") %>' />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblAmount" runat="server" Text="${Cost.CostAllocateTransaction.Amount}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tbAmount" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblEffDate" runat="server" Text="${Cost.CostAllocateTransaction.EffDate}:" />
                </td>
                <td class="td02">
                     <asp:Label ID="tbEffDate" runat="server" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblItems" runat="server" Text="${Cost.CostAllocateTransaction.Items}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tbItems" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblOrders" runat="server" Text="${Cost.CostAllocateTransaction.Orders}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tbOrders" runat="server" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblItemCategorys" runat="server" Text="${Cost.CostAllocateTransaction.ItemCategorys}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tbItemCategorys" runat="server" />
                </td>
            </tr>
             <tr>
                <td class="td01">
                    <asp:Literal ID="lblReferenceItems" runat="server" Text="${Cost.CostAllocateTransaction.ReferenceItems}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tbReferenceItems" runat="server" />
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
                        <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                            CssClass="back" />
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
