<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Main.ascx.cs" Inherits="Production_Feed_Main" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%--<%@ Register Src="NewMain.ascx" TagName="New" TagPrefix="uc2" %>

<uc2:New ID="ucNew" runat="server" Visible="true" />--%>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProductLine" runat="server" Text="${MasterData.Production.Feed.ProductLine}:" />
            </td>
            <td class="td02">
                <uc3:textbox id="tbProductLine" runat="server" descfield="Description" valuefield="Code"
                    servicepath="FlowMgr.service" servicemethod="GetProductionFlow" width="250" cssclass="inputRequired"
                    ontextchanged="tbProductLine_TextChanged" autopostback="true" />
                <asp:RequiredFieldValidator ID="rfvProductLine" runat="server" ControlToValidate="tbProductLine"
                    ErrorMessage="${MasterData.Production.Feed.ProductLine.Required}" Display="Dynamic"
                    ValidationGroup="vgSaveGroup" />
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnConfirm" runat="server" OnClick="btnConfirm_Click" Text="${Common.Button.Confirm}"
                    CssClass="button2" ValidationGroup="vgSaveGroup" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset>
    <legend>${MasterData.Production.Feeded.Detail}</legend>
    <asp:UpdatePanel ID="UP_GV_List" runat="server">
        <ContentTemplate>
            <div id="divInputHu" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%;text-align: right;">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                        <td style="margin-right: 5px; width: 50%; text-align: right">
                            <asp:Literal ID="ltlHuScan" runat="server" Text="<%$Resources:Language,ScanBarcode%>" />
                            <asp:TextBox ID="tbHuScan" runat="server" OnTextChanged="tbHuScan_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:Button ID="btnHuScan" runat="server" Text="<%$Resources:Language,ButtonScan%>"
                                CssClass="hidden" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GridView">
                <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataOperation%>">
                            <ItemTemplate>
                                <asp:Label ID="lblOperation" runat="server" Text='<%# Bind("Operation") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemCode%>">
                            <ItemTemplate>
                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("ItemCode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemDesc%>">
                            <ItemTemplate>
                                <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("ItemDescription") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataUom%>">
                            <ItemTemplate>
                                <asp:Label ID="lblUom" runat="server" Text='<%# Bind("UomCode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataUnitCount%>">
                            <ItemTemplate>
                                <asp:Label ID="lblUnitCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataLocation%>">
                            <ItemTemplate>
                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("LocationCode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,FeedOrderedQty%>">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderedQty" runat="server" Text='<%# Bind("OrderedQty","{0:0.########}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,FeedQty%>">
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty","{0:0.########}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Language,FeedCurrentQty%>">
                            <ItemTemplate>
                                <asp:TextBox ID="tbCurrentQty" runat="server" onmouseup="if(!readOnly)select();"
                                    Text='<%# Bind("CurrentQty","{0:0.########}") %>' Width="50"></asp:TextBox>
                                <asp:RangeValidator ID="rvQty" runat="server" ControlToValidate="tbCurrentQty" ErrorMessage="${Common.Validator.Valid.Number}"
                                    Display="Dynamic" Type="Double" MinimumValue="0" MaximumValue="99999999" ValidationGroup="vgSaveGroup" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
