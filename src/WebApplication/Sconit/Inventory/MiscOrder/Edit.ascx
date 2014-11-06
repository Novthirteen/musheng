﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_MiscOrder_TransEdit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ac1" %>



<script language="javascript" type="text/javascript">
    function GenerateItem(obj){ 
        var objId = $(obj).attr("id");
        var parentId = objId.substring(0,objId.length-"tbItemCode_suggest".length);
        if($(obj).val() != "")
        {
            Sys.Net.WebServiceProxy.invoke('Webservice/ItemMgrWS.asmx',  'GenerateItemProxy',false,
                {"itemCode":$(obj).val()},
            function OnSucceeded(result, eventArgs)
            {        
                $('#'+ parentId + 'tbItemDescriptionText').attr('value',result.Description);
                $('#'+ parentId + 'tblItemUomText').attr('value',result.Uom.Code);
            },
            function OnFailed(error)
            {
              alert(error.get_message());
            }
           );
        }
   }     
</script>

<div id="divFV" runat="server">
    <fieldset>
    <legend>${Common.Business.BasicInfo}</legend>
        <table class="mtable">
            <tr>
                <td class="td01">
                    <asp:Literal ID="Literal4" runat="server" Text="${Common.Business.Id}:" />
                </td>
                <td class="td02">
                    <asp:Literal ID="tbMiscOrderID" runat="server" Visible="true" />
                </td>
                <td class="td01">
                    <asp:Literal ID="Literal5" runat="server" Text="${Common.Business.CreateDate}:" />
                </td>
                <td class="td02">
                    <asp:Literal ID="tbMiscOrderCreateDate" runat="server" Visible="true" />
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.RegionEmpty}"
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
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.LocationEmpty}"
                        Display="Dynamic" ControlToValidate="tbMiscOrderLocation" ValidationGroup="vgCreate" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="Literal1" runat="server" Text="${MasterData.MiscOrder.EffectDate}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tvMiscOrderEffectDate" runat="server" Visible="false" />
                    <asp:TextBox ID="tbMiscOrderEffectDate" runat="server" CssClass="inputRequired" onClick="WdatePicker()" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="${MasterData.MiscOrder.WarningMessage.EffectDateEmpty}"
                        Display="Dynamic" ControlToValidate="tbMiscOrderRegion" ValidationGroup="vgCreate" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblMiscReason" runat="server" Text="${Common.Business.Reason}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tvMiscOrderReason" runat="server" Visible="false" />
                    <cc1:CodeMstrDropDownList ID="ddlReason" Code="StockOutReason" runat="server">
                    </cc1:CodeMstrDropDownList>
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="lblRefOrderNo" runat="server" Text="${MasterData.MiscOrder.RefOrderNo}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="lbRefNo" runat="server" Visible="false" />
                    <asp:TextBox ID="tbRefNo" runat="server" Visible="true" />
                </td>
                <td class="td01">
                    <asp:Literal ID="lblCreateUser" runat="server" Text="${MasterData.MiscOrder.CreateUser}:" />
                </td>
                <td class="td02">
                    <asp:Literal ID="lbCreateUser" runat="server" Visible="true" />
                </td>
            </tr>
            <tr>
                <td class="td01">
                    <asp:Literal ID="Literal2" runat="server" Text="${Common.Business.Remark}:" />
                </td>
                <td class="td02">
                    <asp:Label ID="tvMiscOrderDescription" runat="server" Visible="false" />
                    <asp:TextBox ID="tbMiscOrderDescription" runat="server" Visible="true" />
                </td>
                <td class="td01">
                </td>
                <td class="td02">
                </td>
            </tr>
            <tr>
                <td colspan="3" />
                <td class="td02">
                    <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click"
                        class="button2" ValidationGroup="vgCreate" Visible="false" />
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
        <asp:GridView ID="MiscOrderDetailsGV" runat="server" AllowPaging="False" DataKeyNames="Id"
            AllowSorting="False" AutoGenerateColumns="False" OnRowDataBound="MiscOrderDetailsGV_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="${Common.Business.ItemCode}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' />
                        <uc3:textbox ID="tbItemCode" runat="server" Visible="false" Width="250" DescField="Description"
                            ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem"
                            CssClass="inputRequired" InputWidth="80" MustMatch="true" />
                        <asp:RequiredFieldValidator ID="rfvItemCode" runat="server" ErrorMessage="${MasterData.MiscOrder.ItemCode.Required}"
                            Display="Dynamic" ControlToValidate="tbItemCode" ValidationGroup="vgAddDetail" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Description}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbItemDescriptionText" runat="server" Text='<%# Bind("Item.Description") %>'
                            Visible="true" onfocus="this.blur();" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Uom}">
                    <ItemTemplate>
                        <asp:Label ID="tvlItemUom" runat="server" Visible="false" />
                        <asp:TextBox ID="tblItemUomText" runat="server" Text='<%# Bind("Item.Uom.Code") %>'
                            Visible="true" onfocus="this.blur();"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.ItemQty}">
                    <ItemTemplate>
                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty","{0:0.########}") %>' Visible="false" />
                        <asp:TextBox ID="tbQty" runat="server" Visible="true" Width="65px" Text='<%# Bind("Qty") %>' />
                        <asp:RegularExpressionValidator ID="revQty" runat="server" Display="Dynamic" ControlToValidate="tbQty"
                            Enabled="false"></asp:RegularExpressionValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.GridView.Action}" Visible="true">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnAdd" runat="server" Text="${Common.Button.New}" OnClick="lbtnAdd_Click"
                            ValidationGroup="vgAddDetail">
                        </asp:LinkButton>
                        <asp:LinkButton ID="lbtnDelete" runat="server" Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click"
                            OnClientClick="return confirm('${Common.Button.Delete.Confirm}')">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
</div>
