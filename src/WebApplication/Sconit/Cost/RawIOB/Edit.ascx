<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Modules_Cost_RawIOB_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<asp:FormView ID="FV_RawIOB" runat="server" DataSourceID="ODS_RawIOB" DefaultMode="Edit"
    DataKeyNames="Id" OnDataBound="FV_RawIOB_DataBound">
    <EditItemTemplate>
        <fieldset>
            <legend>${Common.Edit}</legend>
            <table class="mtable">
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.FinanceCalendar}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbFinanceCalendar" runat="server" Text='<%# Bind("FinanceCalendar") %>'
                            ReadOnly="true" />
                    </td>
                    <td class="td01">
                    </td>
                    <td class="td02">
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.Item}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbItem" runat="server" Text='<%# Bind("Item") %>' ReadOnly="true" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.Uom}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbUom" runat="server" Text='<%# Bind("Uom") %>' ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.CreateTime}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbCreate" runat="server" Text='<%# Bind("CreateTime") %>' ReadOnly="true" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.CreateUser}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbCreatUser" runat="server" Text='<%# Bind("CreateUser") %>' ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        ${Cost.RawIOB.LastModifyTime}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbLastModifyTime" runat="server" Text='<%# Bind("LastModifyTime") %>'
                            ReadOnly="true" />
                    </td>
                    <td class="td01">
                        ${Cost.RawIOB.LastModifyUser}:
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbLastModifyUser" runat="server" Text='<%# Bind("LastModifyUser") %>'
                            ReadOnly="true" />
                    </td>
                </tr>
            </table>
            <hr />
            <table class="mtable">
                <tr>
                    <td class="ttd01">
                        ${Cost.RawIOB.StartAmount}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbStartAmount" runat="server" Text='<%# Bind("StartAmount") %>'
                            ReadOnly="true" />
                    </td>
                    <td class="ttd01">
                        ${Cost.RawIOB.StartCost}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbStartCost" runat="server" Text='<%# Bind("StartCost") %>' ReadOnly="true" />
                    </td>
                    <td class="ttd01">
                        ${Cost.RawIOB.StartQty}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbStartQty" runat="server" Text='<%# Bind("StartQty") %>' ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="ttd01">
                        ${Cost.RawIOB.InAmount}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbInAmount" runat="server" Text='<%# Bind("InAmount") %>' ReadOnly="true" />
                    </td>
                    <td class="ttd01">
                        ${Cost.RawIOB.InCost}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbInCost" runat="server" Text='<%# Bind("InCost") %>' ReadOnly="true" />
                    </td>
                    <td class="ttd01">
                        ${Cost.RawIOB.InQty}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbInQty" runat="server" Text='<%# Bind("InQty") %>' ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td class="ttd01">
                        ${Cost.RawIOB.DiffAmount}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbDiffAmount" runat="server" Text='<%# Bind("DiffAmount") %>' CssClass="inputRequired" onmouseup="if(!readOnly)select();"
                            onchange="CalCost(this, 'tbStartAmount', 'tbStartQty', 'tbInAmount', 'tbInQty', 'tbEndAmount', 'tbEndCost','tbEndQty','tbDiffAmount','tbDiffCost');" />
                        <asp:RequiredFieldValidator ID="rfvDiffAmount" runat="server" ControlToValidate="tbDiffAmount"
                            Display="Dynamic" ErrorMessage="${Common.Error.NotNull}" ValidationGroup="vgSave" />
                        <asp:RangeValidator ID="rvDiffAmount" runat="server" ControlToValidate="tbDiffAmount"
                            ErrorMessage="${Common.Validator.Valid.Number}" Display="Dynamic" Type="Double"
                            MinimumValue="-999999999" MaximumValue="999999999" ValidationGroup="vgSave" />
                    </td>
                    <td class="ttd01">
                        ${Cost.RawIOB.DiffCost}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbDiffCost" runat="server" Text='<%# Bind("DiffCost") %>' onfocus="this.blur();"/>
                    </td>
                    <td class="ttd01">
                    </td>
                    <td class="ttd02">
                    </td>
                </tr>
                <tr>
                    <td class="ttd01">
                        ${Cost.RawIOB.EndAmount}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbEndAmount" runat="server" Text='<%# Bind("EndAmount") %>' onfocus="this.blur();" />
                    </td>
                    <td class="ttd01">
                        ${Cost.RawIOB.EndCost}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbEndCost" runat="server" Text='<%# Bind("EndCost") %>' onfocus="this.blur();" />
                    </td>
                    <td class="ttd01">
                        ${Cost.RawIOB.EndQty}:
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbEndQty" runat="server" Text='<%# Bind("EndQty") %>' ReadOnly="true" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <div class="tablefooter">
            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="${Common.Button.Save}"
                CssClass="button2" ValidationGroup="vgSave" />
            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="${Common.Button.Delete}"
                CssClass="button2" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" Visible="false"/>
            <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                CssClass="button2" />
        </div>
    </EditItemTemplate>
</asp:FormView>
<asp:ObjectDataSource ID="ODS_RawIOB" runat="server" TypeName="com.Sconit.Web.RawIOBMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.RawIOB" UpdateMethod="UpdateRawIOB"
    OnUpdated="ODS_RawIOB_Updated" OnUpdating="ODS_RawIOB_Updating" SelectMethod="LoadRawIOB"
    DeleteMethod="DeleteRawIOB" OnDeleted="ODS_RawIOB_Deleted">
    <SelectParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
    </UpdateParameters>
</asp:ObjectDataSource>

<script type="text/javascript" language="javascript">
    function CalCost(obj, tbStartAmount, tbStartQty, tbInAmount, tbInQty, tbEndAmount, tbEndCost, tbEndQty, tbDiffAmount, tbDiffCost) {
        var tbDiffAmountId = "#" + $(obj).attr("id");
        var parentId = tbDiffAmountId.substring(0, tbDiffAmountId.length - tbDiffAmount.length);
        var tbStartAmountId = parentId + tbStartAmount;
        var tbStartQtyId = parentId + tbStartQty;
        var tbInAmountId = parentId + tbInAmount;
        var tbInQtyId = parentId + tbInQty;
        var tbEndAmountId = parentId + tbEndAmount;
        var tbEndCostId = parentId + tbEndCost;
        var tbEndQtyId = parentId + tbEndQty;
        var tbDiffCostId = parentId + tbDiffCost;

        if ($(tbEndQtyId).val() != 0) {
            //期末成本=（期初金额+本期采购金额）/（期初数量+本期采购数量）
            var endCost = ($(tbStartAmountId).val() * 1.0 + $(tbInAmountId).val() * 1.0) / ($(tbStartQtyId).val() * 1.0 + $(tbInQtyId).val() * 1.0);
            
            //期末金额=期末成本*期末数量
            var endAmount = ($(tbEndQtyId).val() * endCost);
            //差异成本
            var diffCost = $(tbDiffAmountId).val() / $(tbEndQtyId).val();
            $(tbDiffCostId).attr('value', diffCost.toFixed(6));
            //调整期末成本
            $(tbEndCostId).attr('value', ((endCost * 1.0) + (diffCost * 1.0)));
            //调整后期末金额
            $(tbEndAmountId).attr('value', ((endAmount * 1.0) + ($(tbDiffAmountId).val() * 1.0)));
        }
    }

</script>

