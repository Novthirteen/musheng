<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Production_PLFeedSeq_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc2" %>
<div id="divFV">
    <asp:FormView ID="FV_ProdutLineFeedSeqence" runat="server" DataSourceID="ODS_ProdutLineFeedSeqence"
        DefaultMode="Edit" Width="100%" DataKeyNames="Id" OnDataBound="FV_ProdutLineFeedSeqence_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${Production.ProdutLineFeedSeqence.UpdateProdutLineFeedSeqence}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblProductLineCode" runat="server" Text="${Common.Business.ProductionLineFacility}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbProductLineFacility" runat="server" Text='<%# Bind("ProductLineFacility") %>'
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvProductLineCode" runat="server" ErrorMessage="${Production.ProdutLineFeedSeqence.ProductLineFacility.Empty}"
                                Display="Dynamic" ControlToValidate="tbProductLineFacility" ValidationGroup="vgSave" />
                            <asp:HiddenField ID="hfId" Value='<%# Bind("Id") %>' runat="server"/>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${Production.ProdutLineFeedSeqence.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${Production.ProdutLineFeedSeqence.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvCodeCheck" runat="server" ControlToValidate="tbCode" ErrorMessage="${Production.ProdutLineFeedSeqence.Code.Exists}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkCodeExists" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlFinishGoodCode" runat="server" Text="${Production.ProdutLineFeedSeqence.FinishGood}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbFinishGoodCode" runat="server" Visible="true" Width="250" MustMatch="true"
                                DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvFinishGoodCode" runat="server" ErrorMessage="${Production.ProdutLineFeedSeqence.FinishGood.Empty}"
                                Display="Dynamic" ControlToValidate="tbFinishGoodCode" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblRawMaterialCode" runat="server" Text="${Production.ProdutLineFeedSeqence.RawMaterial}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbRawMaterialCode" runat="server" Visible="true" Width="250" DescField="Description"
                                ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                                CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvRawMaterialCode" runat="server" ControlToValidate="tbRawMaterialCode"
                                Display="Dynamic" ErrorMessage="${Production.ProdutLineFeedSeqence.RawMaterial.Required}"
                                ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlSequence" runat="server" Text="${Production.ProdutLineFeedSeqence.Sequence}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbSequence" runat="server" Text='<%# Bind("Sequence") %>' CssClass="inputRequired" />
                            <asp:RangeValidator ID="rvSequence" runat="server" ControlToValidate="tbSequence"
                                ErrorMessage="${Common.Validator.Valid.Number}" Display="Dynamic" Type="Integer"
                                MinimumValue="0" MaximumValue="99999999" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvSequenceCheck" runat="server" ControlToValidate="tbSequence"
                                ErrorMessage="${Production.ProdutLineFeedSeqence.Sequence.Exists}" Display="Dynamic"
                                ValidationGroup="vgSave" OnServerValidate="checkSequenceExists" />
                        </td>
                        <%--<td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${Common.Business.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# Bind("IsActive") %>' />
                        </td>--%>
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
                                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="${Common.Button.Delete}"
                                    CssClass="delete" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
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
<asp:ObjectDataSource ID="ODS_ProdutLineFeedSeqence" runat="server" TypeName="com.Sconit.Web.ProdutLineFeedSeqenceMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Customize.ProdutLineFeedSeqence" UpdateMethod="UpdateProdutLineFeedSeqence"
    OnUpdated="ODS_ProdutLineFeedSeqence_Updated" SelectMethod="LoadProdutLineFeedSeqence"
    OnUpdating="ODS_ProdutLineFeedSeqence_Updating" DeleteMethod="DeleteProdutLineFeedSeqence"
    OnDeleted="ODS_ProdutLineFeedSeqence_Deleted">
    <SelectParameters>
        <asp:Parameter Name="id" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
