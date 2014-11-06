﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewHu.ascx.cs" Inherits="MasterData_MiscOrder_NewHu" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>



<div id="divFV" runat="server">
    <fieldset>
        <legend>${Common.Business.BasicInfo}</legend>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblMiscOrderCode" runat="server" Text="${Common.Business.Id}:" />
                </td>
                <td class="td02">
                    <asp:Literal ID="tbMiscOrderCode" runat="server" Visible="true" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblRefOrderNo" runat="server" Text="${MasterData.MiscOrder.RefOrderNo}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="lbRefNo" runat="server" Visible="false" />
                    <asp:TextBox ID="tbRefNo" runat="server" Visible="true" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblRegion" runat="server" Text="${Common.Business.Region}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tvMiscOrderRegion" runat="server" Visible="false" />
                    <uc3:textbox ID="tbMiscOrderRegion" runat="server" Visible="true" Width="250" DescField="Name"
                        ValueField="Code" MustMatch="true" ServicePath="RegionMgr.service" ServiceMethod="GetRegion"
                        CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvRegion" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.RegionEmpty}"
                        Display="Dynamic" ControlToValidate="tbMiscOrderRegion" ValidationGroup="vgCreate" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblLocation" runat="server" Text="${Common.Business.Location}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tvMiscOrderLocation" runat="server" Visible="false" />
                    <uc3:textbox ID="tbMiscOrderLocation" runat="server" Visible="true" DescField="Name"
                        ValueField="Code" Width="250" ServicePath="LocationMgr.service" ServiceMethod="GetLocation"
                        ServiceParameter="string:#tbMiscOrderRegion" CssClass="inputRequired" />
                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.LocationEmpty}"
                        Display="Dynamic" ControlToValidate="tbMiscOrderLocation" ValidationGroup="vgCreate" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblEffectDate" runat="server" Text="${MasterData.MiscOrder.EffectDate}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tvMiscOrderEffectDate" runat="server" Visible="false" />
                    <asp:TextBox ID="tbMiscOrderEffectDate" runat="server" CssClass="inputRequired" onClick="WdatePicker()" />
                    <asp:RequiredFieldValidator ID="rfvEffectDate" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.EffectDateEmpty}"
                        Display="Dynamic" ControlToValidate="tbMiscOrderRegion" ValidationGroup="vgCreate" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblRemark" runat="server" Text="${Common.Business.Remark}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tvMiscOrderDescription" runat="server" Visible="false" />
                    <asp:TextBox ID="tbMiscOrderDescription" runat="server" Visible="true" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblSubjectCode" runat="server" Text="${MasterData.MiscOrder.SubjectCode}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbSubjectCode" runat="server" Visible="true" Width="250" DescField="SubjectName"
                        ValueField="SubjectCode" ServicePath="SubjectListMgr.service" ServiceMethod="GetAllSubject"
                         />
                 <%--   <asp:RequiredFieldValidator ID="rfvSubjectCode" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.SubjectCodeEmpty}"
                        Display="Dynamic" ControlToValidate="tbSubjectCode" ValidationGroup="vgCreate" />--%>
                    <asp:Label ID="tvSubjectCode" runat="server" Visible="false" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblCostCenterCode" runat="server" Text="${MasterData.MiscOrder.CostCenterCode}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbCostCenterCode" runat="server" Visible="true" Width="250" DescField="CostCenterName"
                        ValueField="CostCenterCode" ServicePath="SubjectListMgr.service" ServiceMethod="GetSubjectList"
                        ServiceParameter="string:#tbSubjectCode"  />
                <%--    <asp:RequiredFieldValidator ID="rfvCostCenterCode" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.CostCenterCodeEmpty}"
                        Display="Dynamic" ControlToValidate="tbCostCenterCode" ValidationGroup="vgCreate" />--%>
                    <asp:Label ID="tvCostCenterCode" runat="server" Visible="false" />
                </td>
            </tr>
             <tr>
                <td class="td01">
                    <asp:Literal ID="lblAccountCode" runat="server" Text="${MasterData.MiscOrder.AccountCode}:" />
                </td>
                <td class="td02">
                    <uc3:textbox ID="tbAccountCode" runat="server" Visible="true" Width="250" DescField="AccountCode"
                        ValueField="AccountCode" ServicePath="SubjectListMgr.service" ServiceMethod="GetAccount"
                          ServiceParameter="string:#tbSubjectCode,string:#tbCostCenterCode"  />
       <%--        <%--     <asp:RequiredFieldValidator ID="rfvAccountCode" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.AccountCodeEmpty}"
                        Display="Dynamic" ControlToValidate="tbAccountCode" ValidationGroup="vgCreate" />--%>
                    <asp:Label ID="tvAccountCode" runat="server" Visible="false" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblProjectCode" runat="server" Text="${MasterData.MiscOrder.ProjectCode}:" />
                </td>
                <td class="td02">
                    <asp:Literal ID="lbProjectCode" runat="server" Visible="false" />
                    <asp:TextBox ID="tbProjectCode" runat="server" Visible="true" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblCreateDate" runat="server" Text="${Common.Business.CreateDate}:" />
                </td>
                <td class="td02">
                    <asp:Literal ID="tbMiscOrderCreateDate" runat="server" Visible="true" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.MiscOrder.CreateUser}:" />
                </td>
                <td class="td02">
                    <asp:Literal ID="lbCreateUser" runat="server" Visible="true" />
                </td>
            </tr>
            <tr>
                <td colspan="3" />
                <td class="td02">
                    <asp:Button ID="btnSubmit" runat="server" Text="${Common.Button.Submit}" class="button2"
                        OnClick="btnSubmit_Click" Visible="true" ValidationGroup="vgCreate" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                        CssClass="button2" Visible="true" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>${Common.Business.OrderDetails}</legend>
        <div>
            <div id="divMessage" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50%;">
                        </td>
                        <td style="margin-right: 5px; width: 50%; text-align: right">
                            <asp:Literal ID="ltlHuScan" runat="server" Text="<%$Resources:Language,ScanBarcode%>" />
                            <asp:TextBox ID="tbHuScan" runat="server" OnTextChanged="tbHuScan_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="MiscOrderDetailsGV" runat="server" AllowPaging="False" DataKeyNames="Id"
                AllowSorting="False" AutoGenerateColumns="False" OnRowDataBound="MiscOrderDetailsGV_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemCode%>">
                        <ItemTemplate>
                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataItemDesc%>">
                        <ItemTemplate>
                            <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("Item.Description") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataUom%>">
                        <ItemTemplate>
                            <asp:Label ID="lblUomCode" runat="server" Text='<%# Bind("Item.Uom.Code") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataHuId%>">
                        <ItemTemplate>
                            <asp:Label ID="lblHuId" runat="server" Text='<%# Bind("HuId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Language,MasterDataQty%>">
                        <ItemTemplate>
                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty","{0:0.########}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="${Common.GridView.Action}" Visible="true">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnDelete" runat="server" Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click"
                                OnClientClick="return confirm('${Common.Button.Delete.Confirm}')">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
     </fieldset>
</div>