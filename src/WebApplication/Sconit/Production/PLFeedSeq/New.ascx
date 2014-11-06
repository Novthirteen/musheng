<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Production_PLFeedSeq_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_ProdutLineFeedSeqence" runat="server" DataSourceID="ODS_ProdutLineFeedSeqence"
        DefaultMode="Insert" Width="100%" DataKeyNames="Code" OnDataBinding="FV_ProdutLineFeedSeqence_OnDataBinding">
        <InsertItemTemplate>
            <fieldset>
                <legend>${Production.ProdutLineFeedSeqence.NewProdutLineFeedSeqence}</legend>
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
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${Production.ProdutLineFeedSeqence.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rtvCode" runat="server" ErrorMessage="${Production.ProdutLineFeedSeqence.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvCodeCheck" runat="server" ControlToValidate="tbCode"
                                ErrorMessage="${Production.ProdutLineFeedSeqence.Code.Exists}" Display="Dynamic"
                                ValidationGroup="vgSave" OnServerValidate="checkCodeExists" />
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
<asp:ObjectDataSource ID="ODS_ProdutLineFeedSeqence" runat="server" TypeName="com.Sconit.Web.ProdutLineFeedSeqenceMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Customize.ProdutLineFeedSeqence" InsertMethod="CreateProdutLineFeedSeqence"
    OnInserted="ODS_ProdutLineFeedSeqence_Inserted" OnInserting="ODS_ProdutLineFeedSeqence_Inserting">
</asp:ObjectDataSource>
