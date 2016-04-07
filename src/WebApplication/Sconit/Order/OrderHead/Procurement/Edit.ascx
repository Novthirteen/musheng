﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Order_OrderHead_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<%@ Register Src="~/Order/OrderDetail/List.ascx" TagName="Detail" TagPrefix="uc2" %>



<fieldset>
    <legend>${MasterData.Order.OrderHead}</legend>
    <asp:FormView ID="FV_Order" runat="server" DataSourceID="ODS_Order" DefaultMode="Edit"
        DataKeyNames="OrderNo">
        <EditItemTemplate>
            <table class="mtable">
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblOrderNo" runat="server" Text="${MasterData.Order.OrderHead.OrderNo.Procurement}:" />
                    </td>
                    <td class="td02">
                        <asp:Label ID="tbOrderNo" runat="server" Text='<%#Bind("OrderNo") %>' />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblPriority" runat="server" Text="${MasterData.Order.OrderHead.Priority}:" />
                    </td>
                    <td class="td02">
                        <sc1:CodeMstrLabel ID="ddlPriority" Code="OrderPriority" runat="server" Value='<%#Bind("Priority") %>' />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="ltlSubType" runat="server" Text="${MasterData.Order.OrderHead.SubType}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbSubType" ReadOnly="true" runat="server" Text='<%# Bind("SubType") %>'></asp:TextBox>
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlCurrency" runat="server" Text="${MasterData.Order.OrderHead.Currency}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbCurrency" ReadOnly="true" runat="server" Text='<%# Bind("Currency.Name") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblExtOrderNo" runat="server" Text="${MasterData.Order.OrderHead.ExtOrderNo}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbExtOrderNo" runat="server" Text='<%# Bind("ExternalOrderNo") %>'></asp:TextBox>
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblRefOrderNo" runat="server" Text="${MasterData.Order.OrderHead.Flow.RefOrderNo}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbRefOrderNo" runat="server" Text='<%# Bind("ReferenceOrderNo") %>'></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblStartTime" runat="server" Text="${MasterData.Order.OrderHead.StartTime}:" />
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbStartTime" runat="server" Text='<%# Bind("StartTime") %>' CssClass="inputRequired" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblWindowTime" runat="server" Text="${MasterData.Order.OrderHead.WindowTime}:" />
                    </td>
                    <td class="ttd02">
                        <asp:TextBox ID="tbWinTime" runat="server" Text='<%# Bind("WindowTime") %>' CssClass="inputRequired" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblPartyFrom" runat="server" Text="${MasterData.Order.OrderHead.PartyFrom}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbPartyFrom" runat="server" CodeField="PartyFrom.Code" DescField="PartyFrom.Name" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblPartyTo" runat="server" Text="${MasterData.Order.OrderHead.PartyTo.Region}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbPartyTo" runat="server" CodeField="PartyTo.Code" DescField="PartyTo.Name" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblShipFrom" runat="server" Text="${MasterData.Order.OrderHead.ShipFrom}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbShipFrom" runat="server" CodeField="ShipFrom.Code" DescField="ShipFrom.Address" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblShipTo" runat="server" Text="${MasterData.Order.OrderHead.ShipTo}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbShipTo" runat="server" CodeField="ShipTo.Code" DescField="ShipTo.Address" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblCarrier" runat="server" Text="${MasterData.Order.OrderHead.Carrier}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCarrier" runat="server" CodeField="Carrier.Code" DescField="Carrier.Name" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblCarrierBillAddress" runat="server" Text="${MasterData.Order.OrderHead.Carrier.BillAddress}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbCarrierBillAddress" runat="server" CodeField="CarrierBillAddress.Code"
                            DescField="CarrierBillAddress.Address" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblDockDescription" runat="server" Text="${MasterData.Order.OrderHead.DockDescription}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbDockDescription" runat="server" CodeField="DockDescription" />
                    </td>
                    <td class="td01">
                        <asp:Literal ID="lblStatus" runat="server" Text="${MasterData.Order.OrderHead.Status}:" />
                    </td>
                    <td class="td02">
                        <sc1:ReadonlyTextBox ID="tbStatus" runat="server" CodeField="Status" />
                    </td>
                </tr>
                <tr>
                    <td class="td01">
                        <asp:Literal ID="lblMemo" runat="server" Text="${MasterData.Order.OrderHead.Memo}:" />
                    </td>
                    <td class="td02">
                        <asp:TextBox ID="tbMemo" runat="server" Text='<%# Bind("Memo") %>'  TextMode="MultiLine"
                            Rows="3" Columns="80"></asp:TextBox>
                    </td>
                    <td class="td01">
                        <asp:Literal ID="ltlFlow" runat="server" Text="${Common.Business.Flow}:" />
                    </td>
                    <td class="td02" >
                        <asp:TextBox ID="tbFlow" runat="server" ReadOnly="true" Text='<%# Bind("Flow") %>'></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div id="divMore" style="display: none">
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblGrGapTo" runat="server" Text="${MasterData.Order.OrderHead.GrGapTo}:" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrLabel ID="ddlGrGapTo" Code="GrGapTo" runat="server" Value='<%#Bind("GoodsReceiptGapTo") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblOrderTemplate" runat="server" Text="${MasterData.Order.OrderHead.OrderTemplate}:" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrLabel ID="lOrderTemplate" Code="OrderTemplate" runat="server" Value='<%# Bind("OrderTemplate") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblAsnTemplate" runat="server" Text="${MasterData.Order.OrderHead.ASNTemplate}:" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrLabel ID="lAsnTemplate" Code="AsnTemplate" runat="server" Value='<%# Bind("AsnTemplate") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblReceiptTemplate" runat="server" Text="${MasterData.Order.OrderHead.ReceiptTemplate}:" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrLabel ID="lReceiptTemplate" Code="ReceiptTemplate" runat="server" Value='<%# Bind("ReceiptTemplate") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblHuTemplate" runat="server" Text="${MasterData.Order.OrderHead.HuTemplate}:" />
                        </td>
                        <td class="td02">
                            <sc1:CodeMstrLabel ID="lHuTemplate" Code="HuTemplate" runat="server" Value='<%# Bind("HuTemplate") %>' />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
                <table class="mtable">
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoRelease" runat="server" Text="${MasterData.Order.OrderHead.IsAutoRelease}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoRelease" runat="server" Checked='<%# Bind("IsAutoRelease") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoStart" runat="server" Text="${MasterData.Order.OrderHead.IsAutoStart}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoStart" runat="server" Checked='<%# Bind("IsAutoStart") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoShip" runat="server" Text="${MasterData.Order.OrderHead.IsAutoShip}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoShip" runat="server" Checked='<%# Bind("IsAutoShip") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoReceive" runat="server" Text="${MasterData.Order.OrderHead.IsAutoReceive}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoReceive" runat="server" Checked='<%# Bind("IsAutoReceive") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsAutoBill" runat="server" Text="${MasterData.Order.OrderHead.IsAutoBill}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsAutoBill" runat="server" Checked='<%# Bind("IsAutoBill") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblNeedPrintOrder" runat="server" Text="${MasterData.Order.OrderHead.NeedPrintOrder}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbNeedPrintOrder" runat="server" Checked='<%# Bind("NeedPrintOrder") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblNeedPrintASN" runat="server" Text="${MasterData.Order.OrderHead.NeedPrintASN}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbNeedPrintASN" runat="server" Checked='<%# Bind("NeedPrintASN") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblNeedPrintReceipt" runat="server" Text="${MasterData.Order.OrderHead.NeedPrintReceipt}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbNeedPrintReceipt" runat="server" Checked='<%# Bind("NeedPrintReceipt") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblAllowExceed" runat="server" Text="${MasterData.Order.OrderHead.AllowExceed}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbAllowExceed" runat="server" Checked='<%# Bind("AllowExceed") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsPrinted" runat="server" Text="${MasterData.Order.OrderHead.IsPrinted}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsPrinted" runat="server" Checked='<%# Bind("IsPrinted") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblFulfillUC" runat="server" Text="${MasterData.Order.OrderHead.FulfillUC}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbFulfillUC" runat="server" Checked='<%# Bind("FulfillUnitCount") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsOddCreateHu" runat="server" Text="${MasterData.Order.OrderHead.IsOddCreateHu}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsOddCreateHu" runat="server" Checked='<%# Bind("IsOddCreateHu") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblCreateHuOption" runat="server" Text="${MasterData.Order.OrderHead.CreateHuOption}:" />
                        </td>
                        <td class="ttd02">
                            <sc1:CodeMstrLabel ID="ddlCreateHuOption" Code="CreateHuOption" runat="server" Value='<%#Bind("CreateHuOption") %>' />
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblAutoPrintHu" runat="server" Text="${MasterData.Order.OrderHead.AutoPrintHu}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbAutoPrintHu" runat="server" Checked='<%# Bind("AutoPrintHu") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblCheckDetailOption" runat="server" Text="${MasterData.Order.OrderHead.CheckDetailOption}:" />
                        </td>
                        <td class="ttd02">
                            <sc1:CodeMstrLabel ID="lCheckDetailOption" runat="server" Code="CheckOrderDetOption"
                                Value='<%# Bind("CheckDetailOption") %>' />
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblBillSettleTerm" runat="server" Text="${MasterData.Order.OrderHead.BillSettleTerm}:" />
                        </td>
                        <td class="ttd02">
                            <sc1:CodeMstrLabel ID="ddlBillSettleTerm" runat="server" Code="BillSettleTerm" Value='<%# Bind("BillSettleTerm") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsShipScanHu" runat="server" Text="${MasterData.Order.OrderHead.IsShipScanHu}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsShipScanHu" runat="server" Checked='<%# Bind("IsShipScanHu") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsReceiptScanHu" runat="server" Text="${MasterData.Order.OrderHead.IsReceiptScanHu}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsReceiptScanHu" runat="server" Checked='<%# Bind("IsReceiptScanHu") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lbNeedInspect" runat="server" Text="${MasterData.Flow.NeedInspect}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbNeedInspect" runat="server" Checked='<%# Bind("NeedInspection") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                        <td class="ttd01">
                            <asp:Literal ID="lblIsGoodsReceiveFIFO" runat="server" Text="${MasterData.Flow.IsGoodsReceiveFIFO}:" />
                        </td>
                        <td class="ttd02">
                            <asp:CheckBox ID="cbIsGoodsReceiveFIFO" runat="server" Checked='<%# Bind("IsGoodsReceiveFIFO") %>'
                                Enabled="false"></asp:CheckBox>
                        </td>
                    </tr>
                </table>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCreateDate" runat="server" Text="${MasterData.Order.OrderHead.CreateDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCreateDate" runat="server" CodeField="CreateDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.Order.OrderHead.CreateUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCreateUser" runat="server" CodeField="CreateUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblReleaseDate" runat="server" Text="${MasterData.Order.OrderHead.ReleaseDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbReleaseDate" runat="server" CodeField="ReleaseDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblReleaseUser" runat="server" Text="${MasterData.Order.OrderHead.ReleaseUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbReleaseUser" runat="server" CodeField="ReleaseUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblStartDate" runat="server" Text="${MasterData.Order.OrderHead.StartDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbStartDate" runat="server" CodeField="StartDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblStartUser" runat="server" Text="${MasterData.Order.OrderHead.StartUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbStartUser" runat="server" CodeField="StartUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCompleteDate" runat="server" Text="${MasterData.Order.OrderHead.CompleteDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCompleteDate" runat="server" CodeField="CompleteDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCompleteUser" runat="server" Text="${MasterData.Order.OrderHead.CompleteUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCompleteUser" runat="server" CodeField="CompleteUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCloseDate" runat="server" Text="${MasterData.Order.OrderHead.CloseDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCloseDate" runat="server" CodeField="CloseDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCloseUser" runat="server" Text="${MasterData.Order.OrderHead.CloseUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCloseUser" runat="server" CodeField="CloseUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCancelDate" runat="server" Text="${MasterData.Order.OrderHead.CancelDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCancelDate" runat="server" CodeField="CancelDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblCancelUser" runat="server" Text="${MasterData.Order.OrderHead.CancelUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbCancelUser" runat="server" CodeField="CancelUser.Name" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCancelReason" runat="server" Text="${MasterData.Order.OrderHead.CancelReason}:" />
                        </td>
                        <td class="td02" colspan="3">
                            <sc1:ReadonlyTextBox ID="tbCancelReason" runat="server" CodeField="CancelReason" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblLastModifyDate" runat="server" Text="${MasterData.Order.OrderHead.LastModifyDate}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbLastModifyDate" runat="server" CodeField="LastModifyDate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblLastModifyUser" runat="server" Text="${MasterData.Order.OrderHead.LastModifyUser}:" />
                        </td>
                        <td class="td02">
                            <sc1:ReadonlyTextBox ID="tbLastModifyUser" runat="server" CodeField="LastModifyUser.Name" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <a type="text/html" onclick="More()" href="#" visible="true" id="more">More... </a>
                        </td>
                        <td class="td02">
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
            </div>
        </EditItemTemplate>
    </asp:FormView>
    <div class="tablefooter">
        <sc1:Button ID="btnEdit" runat="server" Text="${Common.Button.Save}" CssClass="button2"
            OnClick="btnEdit_Click" FunctionId="EditOrder" />
        <sc1:Button ID="btnSubmit" runat="server" Text="${MasterData.Order.Button.Submit}"  OnClientClick="return confirm('${Common.Button.Submit.Confirm}')"
            CssClass="button2" OnClick="btnSubmit_Click" FunctionId="SubmitOrder" />
        <sc1:Button ID="btnCancel" runat="server" Text="${Common.Button.Cancel}" CssClass="button2" OnClientClick="return confirm('${Common.Button.Cancel.Confirm}')"
            OnClick="btnCancel_Click" FunctionId="CancelOrder" />
        <sc1:Button ID="btnStart" runat="server" Text="${MasterData.Order.Button.Start}"    OnClientClick="return confirm('${Common.Button.Start.Confirm}')"
            CssClass="button2" OnClick="btnStart_Click" FunctionId="StartOrder" />
        <sc1:Button ID="btnReceive" runat="server" Text="${MasterData.Order.Button.Receive}"  OnClientClick="return confirm('${Common.Button.Receive.Confirm}')"
            CssClass="button2" OnClick="btnReceive_Click" FunctionId="ReceiveOrder" />
        <sc1:Button ID="btnComplete" runat="server" Text="${MasterData.Order.Button.Complete}" OnClientClick="return confirm('${Common.Button.Complete.Confirm}')"
            CssClass="button2" OnClick="btnComplete_Click" FunctionId="CompleteOrder" />
        <sc1:Button ID="btnDelete" runat="server" Text="${Common.Button.Delete}" CssClass="button2"
            OnClick="btnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
            FunctionId="DeleteOrder" />
        <sc1:Button ID="btnPrint" runat="server" Text="${Common.Button.Print}" CssClass="button2"
            OnClick="btnPrint_Click" FunctionId="PrintOrder" />
        <sc1:Button ID="btnExport" runat="server" Text="${Common.Button.Export}" CssClass="button2"
            OnClick="btnExport_Click" FunctionId="ExportOrder" />
        <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
            CssClass="button2" />
    </div>
</fieldset>
<asp:ObjectDataSource ID="ODS_Order" runat="server" TypeName="com.Sconit.Web.OrderMgrProxy"
    DataObjectTypeName="com.Sconit.Web.CustomizedOrderHead" SelectMethod="LoadOrderHead">
    <SelectParameters>
        <asp:Parameter Name="orderNo" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<uc2:Detail ID="ucDetail" runat="server" />
