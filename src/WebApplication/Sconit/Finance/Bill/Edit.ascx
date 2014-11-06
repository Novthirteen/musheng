<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Finance_Bill_Edit" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<%@ Register Src="NewSearch.ascx" TagName="NewSearch" TagPrefix="uc" %>

<script language="javascript" type="text/javascript" src="Js/calcamount.js"></script>

<div id="divscript" runat="server">

    <script type="text/javascript" language="javascript">
        function GVCheckClick() {
            if ($(".GVHeader input:checkbox").attr("checked") == true) {
                $(".GVRow input:checkbox").attr("checked", true);
                $(".GVAlternatingRow input:checkbox").attr("checked", true);
            }
            else {
                $(".GVRow input:checkbox").attr("checked", false);
                $(".GVAlternatingRow input:checkbox").attr("checked", false);
            }
        }
        function discountChanged(obj) {
            CalCulateRowAmountBase(obj, "tbDiscount", "BaseOnDiscount", "hfUnitPrice", "tbQty", "tbDiscount", "tbDiscountRate", "tbAmount", "tbAmountAfterDiscount", '#<%= tbTotalDiscount.ClientID %>', '#<%= tbTotalDiscountRate.ClientID %>', '#<%= tbTotalDetailAmount.ClientID %>', '#<%= tbTotalAmount.ClientID %>', false);
        }
        function qtyChanged(obj) {
            CalCulateRowAmountBase(obj, "tbQty", "BaseOnDiscountRate", "hfUnitPrice", "tbQty", "tbDiscount", "tbDiscountRate", "tbAmount", "tbAmountAfterDiscount", '#<%= tbTotalDiscount.ClientID %>', '#<%= tbTotalDiscountRate.ClientID %>', '#<%= tbTotalDetailAmount.ClientID %>', '#<%= tbTotalAmount.ClientID %>', false);
        }
        function amountChanged(obj) {
            CalCulateRowAmountBase(obj, "tbAmount", "BaseOnDiscountRate", "hfUnitPrice", "tbQty", "tbDiscount", "tbDiscountRate", "tbAmount", "tbAmountAfterDiscount", '#<%= tbTotalDiscount.ClientID %>', '#<%= tbTotalDiscountRate.ClientID %>', '#<%= tbTotalDetailAmount.ClientID %>', '#<%= tbTotalAmount.ClientID %>', false);
        }
        function discountRateChanged(obj) {
            CalCulateRowAmountBase(obj, "tbDiscountRate", "BaseOnDiscountRate", "hfUnitPrice", "tbQty", "tbDiscount", "tbDiscountRate", "tbAmount", "tbAmountAfterDiscount", '#<%= tbTotalDiscount.ClientID %>', '#<%= tbTotalDiscountRate.ClientID %>', '#<%= tbTotalDetailAmount.ClientID %>', '#<%= tbTotalAmount.ClientID %>', false);
        }
        function orderDiscountChanged(obj) {
            CalCulateTotalAmount("BaseOnDiscount", '#<%= tbTotalDiscount.ClientID %>', '#<%= tbTotalDiscountRate.ClientID %>', '#<%= tbTotalDetailAmount.ClientID %>', '#<%= tbTotalAmount.ClientID %>', 0);
        }

        function orderDiscountRateChanged(obj) {
            CalCulateTotalAmount("BaseOnDiscountRate", '#<%= tbTotalDiscount.ClientID %>', '#<%= tbTotalDiscountRate.ClientID %>', '#<%= tbTotalDetailAmount.ClientID %>', '#<%= tbTotalAmount.ClientID %>', 0);
        }
    </script>

</div>
<fieldset>
    <legend>${MasterData.Bill.POBill}</legend>
    <asp:FormView ID="FV_Bill" runat="server" DataSourceID="ODS_Bill" DefaultMode="Edit"
        DataKeyNames="BillNo" OnDataBound="FV_Bill_DataBound">
        <EditItemTemplate>
            <table class="mtable">
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblBillNo" runat="server" Text="${MasterData.Bill.BillNo}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbBillNo" runat="server" CodeField="BillNo" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblRefBillNo" runat="server" Text="${MasterData.Bill.RefBillNo}:" />
                    </td>
                    <td class="td02">
                        <asp:LinkButton ID="lbRefBillNo" runat="server" Text='<%# Bind("ReferenceBillNo") %>'
                            CommandArgument='<%# Bind("ReferenceBillNo") %>' OnClick="lbRefBillNo_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblParty" runat="server" Text="${MasterData.Bill.Supplier}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbParty" runat="server" CodeField="BillAddress.Party.Code"
                            DescField="BillAddress.Party.Name" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblBillAddress" runat="server" Text="${MasterData.Bill.BillAddress}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbBillAddress" runat="server" CodeField="BillAddress.Code"
                            DescField="BillAddress.Address" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblCreateDate" runat="server" Text="${MasterData.Bill.CreateDate}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCreateDate" runat="server" CodeField="CreateDate" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.Bill.CreateUser}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCreateUser" runat="server" CodeField="CreateUser.Code"
                            DescField="CreateUser.Name" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblStatus" runat="server" Text="${MasterData.Bill.Status}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbStatus" runat="server" CodeField="Status" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblExternalBillNo" runat="server" Text="${MasterData.Bill.ExternalBillNo}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbExternalBillNo" runat="server" Text='<%# Bind("ExternalBillNo") %>' />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="ltlBillDate" runat="server" Text="${MasterData.Bill.BillDate}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbStartDate" runat="server" Text='<%# Bind("StartDate","{0:yyyy-MM-dd}") %>'
                            onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="${MasterData.Bill.StartDate.Required}"
                            Display="Dynamic" ControlToValidate="tbStartDate" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblAging" runat="server" Text="${MasterData.Supplier.Aging}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbPaymentAmount" runat="server" Text='<%# Bind("PaymentAmount","{0:0.##}") %>'
                            Visible="false" />
                        <asp:TextBox ID="tbAging" runat="server" Text='<%# Bind("BillAddress.Party.Aging") %>'
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbEndDate" runat="server" Text='<%# Bind("EndDate","{0:yyyy-MM-dd}") %>'
                            onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="${MasterData.Bill.EndDate.Required}"
                            Display="Dynamic" ControlToValidate="tbEndDate" ValidationGroup="vgSave" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblInvoiceDate" runat="server" Text="${MasterData.Bill.InvoiceDate}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbInvoiceDate" runat="server" Text='<%# Bind("InvoiceDate","{0:yyyy-MM-dd}") %>'
                            onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                    </td>
            </table>
        </EditItemTemplate>
    </asp:FormView>
    <table class="mtable">
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:RadioButtonList ID="rblListFormat" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="rblListFormat_IndexChanged">
                    <asp:ListItem Text="${Common.ListFormat.Group}" Value="Group" Selected="True" />
                    <asp:ListItem Text="${Common.ListFormat.Detail}" Value="Detail"  />
                </asp:RadioButtonList>
            </td>
            <td class="td01">
                <sc1:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click"
                    FunctionId="EditBill" ValidationGroup="vgSave" />
                <sc1:Button ID="btnSubmit" runat="server" Text="${Common.Button.Submit}" OnClick="btnSubmit_Click"
                    FunctionId="EditBill" />
            </td>
            <td class="td02">
                <sc1:Button ID="btnDelete" runat="server" Text="${Common.Button.Delete}" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
                    OnClick="btnDelete_Click" FunctionId="EditBill" />
                <sc1:Button ID="btnClose" runat="server" Text="${Common.Button.Close}" OnClientClick="return confirm('${Common.Button.Close.Confirm}')"
                    OnClick="btnClose_Click" FunctionId="CloseBill" />
                <sc1:Button ID="btnCancel" runat="server" Text="${Common.Button.Cancel}" OnClientClick="return confirm('${Common.Button.Cancel.Confirm}')"
                    OnClick="btnCancel_Click" FunctionId="CancelBill" />
                <sc1:Button ID="btnVoid" runat="server" Text="${Common.Button.Void}" OnClientClick="return confirm('${Common.Button.Void.Confirm}')"
                    OnClick="btnVoid_Click" FunctionId="VoidBill" />
                <asp:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" OnClick="btnPrint_Click" />
                <asp:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" OnClick="btnExport_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
    <div class="tablefooter">
    </div>
</fieldset>
<asp:ObjectDataSource ID="ODS_Bill" runat="server" TypeName="com.Sconit.Web.BillMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Bill" SelectMethod="LoadBill">
    <SelectParameters>
        <asp:Parameter Name="billNo" Type="String" />
        <asp:Parameter Name="includeDetail" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>
<fieldset>
    <legend>${MasterData.Bill.BillDetail}</legend>
    <div class="GridView">
        <asp:GridView ID="Gv_List" runat="server" AllowPaging="False" DataKeyNames="Id" AllowSorting="False"
            AutoGenerateColumns="False" OnRowDataBound="GV_List_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <div onclick="GVCheckClick()">
                            <asp:CheckBox ID="CheckAll" runat="server" />
                        </div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                        <asp:CheckBox ID="CheckBoxGroup" name="CheckBoxGroup" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Bill.ReceiptNo}" SortExpression="ActingBill.ReceiptNo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ActingBill.ReceiptNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${MasterData.Bill.ExternalReceiptNo}" SortExpression="ActingBill.ExternalReceiptNo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ActingBill.ExternalReceiptNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.ItemCode}" SortExpression="ActingBill.Item.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ActingBill.Item.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.ItemDescription}" SortExpression="ActingBill.Item.Description">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ActingBill.Item.Description")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.RefCode}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ReferenceItemCode")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.Uom}" SortExpression="ActingBill.Uom.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ActingBill.Uom.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.EffectiveDate}" SortExpression="ActingBill.EffectiveDate">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ActingBill.EffectiveDate", "{0:yyyy-MM-dd}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.UnitPrice}" SortExpression="UnitPrice">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfUnitPrice" runat="server" Value='<%# Bind("UnitPrice") %>' />
                        <%# DataBinder.Eval(Container.DataItem, "UnitPrice", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.Currency}" SortExpression="Currency.Code">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Currency.Code")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BillQty}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ActingBill.BillQty", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BilledQty}">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BilledQty", "{0:0.########}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.CurrentBillQty}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbQty" runat="server" onmouseup="if(!readOnly)select();" Width="50"
                            Text='<%# Bind("BilledQty","{0:0.########}") %>' onchange="qtyChanged(this);"></asp:TextBox>
                        <span style="display: none"></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.Amount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbAmount" runat="server" Width="80px" Text='<%# Bind("Amount","{0:0.########}") %>'
                            onchange="amountChanged(this);">
                        </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.DiscountRate}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbDiscountRate" runat="server" onmouseup="if(!readOnly)select();"
                            Text='<%# Bind("DiscountRate","{0:0.########}") %>' Width="50" onchange="discountRateChanged(this);" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.Discount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbDiscount" runat="server" onmouseup="if(!readOnly)select();" Text='<%# Bind("Discount","{0:0.########}") %>'
                            Width="50" onchange="discountChanged(this);" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.AmountAfterDiscount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbAmountAfterDiscount" runat="server" Width="80px" onfocus="this.blur();"
                            Text='<%# Bind("Amount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:GridView ID="gv_Group" runat="server" AllowPaging="False" AllowSorting="False"
            AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText=" ${Common.Business.ItemCode}" SortExpression="ActingBill.Item.Code">
                    <ItemTemplate>
                        <asp:Literal ID="ltlItemCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Item.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.ItemDescription}" SortExpression="ActingBill.Item.Description">
                    <ItemTemplate>
                        <asp:Literal ID="ltlItemDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Item.Description")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.RefCode}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlReferenceItemCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReferenceItemCode")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText=" ${Common.Business.Uom}" SortExpression="ActingBill.Uom.Code">
                    <ItemTemplate>
                        <asp:Literal ID="ltlUomCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Uom.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.UnitPrice}" SortExpression="UnitPrice">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfUnitPrice" runat="server" Value='<%# Bind("UnitPrice") %>' />
                        <asp:Literal ID="ltlUnitPrice" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "UnitPrice", "{0:0.########}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.Currency}" SortExpression="Currency.Code">
                    <ItemTemplate>
                        <asp:Literal ID="ltlCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Currency.Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BillQty}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlBillQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BillQty", "{0:0.########}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.BilledQty}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlBilledQty" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BilledQty", "{0:0.########}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.ActingBill.Amount}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbAmountAfterDiscount" runat="server" Width="80px" ReadOnly="true"
                            Text='<%# Bind("GroupAmount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table class="mtable">
            <tr>
                <td class="td02">
                    <asp:Button ID="btnAddDetail" runat="server" Text="${Common.Button.New}" OnClick="btnAddDetail_Click" />
                    <asp:Button ID="btnDeleteDetail" runat="server" Text="${Common.Button.Remove}" OnClick="btnDeleteDetail_Click" />
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                    <asp:Literal ID="lblTotalDetailAmount" runat="server" Text="${MasterData.Bill.TotalDetailAmount}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbTotalDetailAmount" runat="server" onfocus="this.blur();" Width="150px" />
                </td>
            </tr>
            <tr>
                <td class="td02">
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                    <asp:Literal ID="lblTotalDiscount" runat="server" Text="${MasterData.Bill.TotalDiscount}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbTotalDiscountRate" runat="server" onChange="orderDiscountRateChanged(this);"
                        Width="65px" />%
                    <asp:TextBox ID="tbTotalDiscount" runat="server" onChange="orderDiscountChanged(this);"
                        Width="65px" />
                </td>
            </tr>
            <tr>
                <td class="td02">
                </td>
                <td class="td02">
                </td>
                <td class="td01">
                    <asp:Literal ID="lblTotalAmount" runat="server" Text="${MasterData.Bill.TotalAmount}:" />
                </td>
                <td class="td02">
                    <asp:TextBox ID="tbTotalAmount" runat="server" Visible="true" onfocus="this.blur();"
                        Width="150px" />
                </td>
            </tr>
        </table>
    </div>
</fieldset>
<div id="floatdiv">
    <uc:NewSearch ID="ucNewSearch" runat="server" Visible="false" />
</div>
