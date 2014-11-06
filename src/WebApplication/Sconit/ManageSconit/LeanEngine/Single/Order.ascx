<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Order.ascx.cs" Inherits="ManageSconit_LeanEngine_Single_Order" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<%@ Register Src="~/Hu/Transformer.ascx" TagName="Transformer" TagPrefix="uc2" %>



<fieldset>
    <legend>${MasterData.Flow.Basic.Info}</legend>
    <asp:FormView ID="FV_FormView" runat="server" DefaultMode="ReadOnly" DataKeyNames="Code">
        <ItemTemplate>
            <table class="mtable">
                <tr id="trOrder" runat="server" visible="false">
                    <td class="td01">
                        <asp:Literal ID="ltlOrderNo" runat="server" Text="${InProcessLocation.InProcessLocationDetail.OrderNo}:" />
                    </td>
                    <td class="td02">
                        <asp:Literal ID="tbOrderNo" runat="server" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlStatus" runat="server" Text="${Common.CodeMaster.Status}:" />
                    </td>
                    <td class="td02">
                        <asp:Literal ID="tbStatus" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="ltlCode" runat="server" Text="${MasterData.Flow.Code}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbCode" runat="server" ReadOnly="true" Text='<%# Bind("Code") %>' />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlDescription" runat="server" Text="${MasterData.Flow.Description}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbDescription" runat="server" ReadOnly="true" Text='<%# Bind("Description") %>' />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="ltlSupplier" runat="server" Text="${MasterData.Flow.Party.From.Supplier}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbSupplier" runat="server" CodeField="PartyFrom.Code" DescField="PartyFrom.Name" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlCustomer" runat="server" Text="${MasterData.Flow.Party.To.Customer}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCustomer" runat="server" CodeField="PartyTo.Code" DescField="PartyTo.Name" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblFlowStrategy" runat="server" Text="${MasterData.Item.FlowDetail.FlowStrategy}:" />
                    </td>
                    <td class="td02">
                        <sc1:CodeMstrDropDownList ID="ddlStrategy" Code="FlowStrategy" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStrategy_SelectedIndexChanged" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlWindowTime" runat="server" Text="${MasterData.Order.OrderHead.WindowTime}:" />
                    </td>
                    <td class="td02">
                        <asp:UpdatePanel ID="UP_Info" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="tbWindowTime" runat="server" Text='<%# Bind("NextWinTime","{0:yyyy-MM-dd HH:mm}") %>'
                                    onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                                <asp:Literal ID="ltlTo" runat="server" Text="到" />
                                <asp:TextBox ID="tbNextWindowTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                                <asp:Label ID="lblWindowTime" runat="server" Text='<%# Bind("NextWinTime","{0:yyyy-MM-dd HH:mm}") %>' />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlStrategy" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:FormView>
    <table class="mtable">
        <tr>
            <td colspan="3" />
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" CssClass="query"
                    OnClick="btnSearch_Click" />
                <asp:Button ID="btnCreate" runat="server" Text="${Common.Button.Create}" CssClass="apply"
                    OnClick="btnCreate_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" CssClass="back"
                    OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset>
    <legend>${MasterData.Order.OrderDetail}</legend>
    <div class="GridView">
        <uc2:Transformer ID="ucTransformer" runat="server" Visible="false" />
    </div>
</fieldset>
