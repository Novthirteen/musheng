<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Cost_StandardCost_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_StandardCost" runat="server" DataSourceID="ODS_StandardCost"
        DefaultMode="Edit" Width="100%" DataKeyNames="Id" OnDataBound="FV_StandardCost_DataBound">
        <EditItemTemplate>
            <fieldset>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblItemCode" runat="server" Text="${Cost.StandardCost.Item}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="tbItem" Text='<%# Bind("Item") %>' runat="server" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCostElement" runat="server" Text="${Cost.StandardCost.CostElement}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="tbCostElement" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCostGroup" runat="server" Text="${Cost.StandardCost.CostGroup}:" />
                        </td>
                        <td class="td02">
                            <asp:Label ID="tbCostGroup"  runat="server" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCost" runat="server" Text="${Cost.StandardCost.Cost}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCost" runat="server" Text='<%# Bind("Cost","{0:0.########}") %>' CssClass="inputRequired" />
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
<asp:ObjectDataSource ID="ODS_StandardCost" runat="server" TypeName="com.Sconit.Web.StandardCostMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Cost.StandardCost" UpdateMethod="UpdateStandardCost"
    OnUpdating="ODS_StandardCost_Updating" OnUpdated="ODS_StandardCost_Updated" SelectMethod="LoadStandardCost"
    DeleteMethod="DeleteStandardCost" OnDeleted="ODS_StandardCost_Deleted">
    <SelectParameters>
        <asp:Parameter Name="id" Type="Int32"/>
    </SelectParameters>
</asp:ObjectDataSource>
