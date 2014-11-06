<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Order_NewOrder_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Src="~/MRP/ShiftPlan/Manual/Shift.ascx" TagName="Shift" TagPrefix="uc" %>

<script language="javascript" type="text/javascript">
    function setStartTime() {
        var leadTime = $('#<%= hfLeadTime.ClientID %>').val();
        var emTime = $('#<%= hfEmTime.ClientID %>').val();
        var winTime = $('#<%= tbWinTime.ClientID %>').val();
        var isUrgent = $('#<%= cbIsUrgent.ClientID %>').attr('checked');
        var dateStr = "";
        if (winTime != "") {
            var startDate = new Date(Date.parse(winTime.replace(/-/g, '/')));
            if (isUrgent) {
                startDate = new Date(startDate.valueOf() - emTime * 60 * 60 * 1000);
            } else {
                startDate = new Date(startDate.valueOf() - leadTime * 60 * 60 * 1000);
            }
            var dateMinute = startDate.getMinutes();

            if (dateMinute < 10) {
                dateMinute = "0" + dateMinute;
            }
            dateStr = startDate.getFullYear() + "-" + (startDate.getMonth() + 1) + "-" + startDate.getDate() + " " +
            startDate.getHours() + ":" + dateMinute;
        }
        $('#<%= tbStartTime.ClientID %>').attr('value', dateStr);
        $('#<%= tbSettleTime.ClientID %>').attr('value', winTime);
    }
   
</script>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Flow.Flow.Procurement}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" OnTextChanged="tbFlow_TextChanged" AutoPostBack="true"
                    MustMatch="true" Width="250" CssClass="inputRequired" ServiceMethod="GetFlowList" />
                <asp:RequiredFieldValidator ID="rfvFlow" runat="server" ErrorMessage="${MasterData.Order.OrderHead.Flow.Required}"
                    Display="Dynamic" ControlToValidate="tbFlow" ValidationGroup="vgCreate" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblSettleTime" runat="server" Text="${MasterData.Order.OrderHead.SettleTime}:" Visible = "false" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbSettleTime" runat="server" CssClass="hidden" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblWinDate" runat="server" Text="${MasterData.Order.OrderHead.WindowTime}:" />
            </td>
            <td class="td02">
                <table style="border: 0;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="tbWinTime" runat="server" Text='<%# Bind("WindowTime") %>' CssClass="inputRequired"
                                onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                            <asp:RequiredFieldValidator ID="rfvWinDate" runat="server" ErrorMessage="${MasterData.Order.OrderHead.WinTime.Required}"
                                Display="Dynamic" ControlToValidate="tbWinTime" ValidationGroup="vgCreate" />
                        </td>
                        <td>
                            <asp:CheckBox ID="cbIsUrgent" runat="server" Text="${MasterData.Order.OrderHead.IsUrgent}" />
                        </td>
                    </tr>
                </table>
            </td>
            <td class="td01">
                <asp:Literal ID="lblExtOrderNo" runat="server" Text="${MasterData.Order.OrderHead.ExtOrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbExtOrderNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartTime" runat="server" Text="${MasterData.Order.OrderHead.StartTime}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartTime" runat="server" CssClass="inputRequired" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                <asp:HiddenField ID="hfLeadTime" runat="server" Value="0" />
                <asp:HiddenField ID="hfEmTime" runat="server" Value="0" />
                <asp:CustomValidator ID="cvStartTime" runat="server" ErrorMessage="${MasterData.Order.OrderHead.StartTime.Later.Than.Now}"
                    ValidationGroup="vgCreate" OnServerValidate="CheckStartTime" Display="Dynamic"
                    Enabled="false" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblRefOrderNo" runat="server" Text="${MasterData.Order.OrderHead.Flow.RefOrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbRefOrderNo" runat="server"></asp:TextBox>
                <asp:Button ID="btnRefOrderNo" runat="server" Text="${MasterData.Flow.ReferenceFlow}" OnClick="btnRefOrderNo_Click"
                    CssClass="button2" ValidationGroup="vgCreate" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlShift" runat="server" Text="${MasterData.WorkCalendar.Shift}:"
                    Visible="false" />
            </td>
            <td class="td02">
                <uc:Shift ID="ucShift" runat="server" Visible="false" />
                <asp:CheckBox ID="cbReleaseOrder" runat="server" Text="${MasterData.Order.OrderHead.ReleaseOrder}" />
                <asp:CheckBox ID="cbContinuousCreate" runat="server" Text="${MasterData.Order.OrderHead.ContinuousCreate}" />
                <asp:CheckBox ID="cbNeedInspect" runat="server" Text="${MasterData.Flow.NeedInspect}"
                    Visible="false" />
                <asp:CheckBox ID="cbEnableBinding" runat="server" Text="${MasterData.Flow.EnableBinding}"
                    Visible="false" />
            </td>
            <td class="td01">            
            </td>
            <td class="td02">
                <cc1:Button ID="btnConfirm" runat="server" Text="${Common.Button.Create}" OnClick="btnConfirm_Click"
                    CssClass="button2" ValidationGroup="vgCreate" FunctionId="EditOrder" OnClientClick="return confirm('${Common.Order.Confirm.Create}')" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
<fieldset runat="server" id="fdDetail">
    <legend>${MasterData.Order.OrderDetail}</legend>
    <div class="GridView">
        <asp:GridView ID="GV_List" runat="server" AllowSorting="True" AutoGenerateColumns="False"
            OnRowDataBound="GV_List_RowDataBound" OnRowCreated="GV_List_RowCreated">
            <Columns>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Sequence}">
                    <ItemTemplate>
                        <asp:Label ID="lblSeq" runat="server" Text='<%# Bind("Sequence") %>' />
                        <asp:HiddenField ID="hfId" runat="server" Value='<%# Bind("Id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Code}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Bind("Item.Code") %>' Width="100" />
                        <uc3:textbox ID="tbItemCode" runat="server" Visible="false" Width="250" DescField="Description"
                            ValueField="Code" ServicePath="FlowMgr.service" ServiceMethod="GetFlowItems"
                            OnTextChanged="tbItemCode_TextChanged" ServiceParameter="string:ABSS" CssClass="inputRequired"
                            InputWidth="150" MustMatch="true" TabIndex="1" AutoPostBack="true" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Item.Description}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("Item.Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.ReferenceItem}">
                    <ItemTemplate>
                        <asp:Label ID="lblReferenceItemCode" runat="server" Text='<%# Bind("ReferenceItemCode") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Uom}">
                    <ItemTemplate>
                        <asp:Label ID="lblUom" runat="server" Text='<%# Bind("Uom.Code") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.UnitCount}">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCount" runat="server" Text='<%# Bind("UnitCount","{0:0.########}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.LocationFrom}">
                    <ItemTemplate>
                        <asp:Label ID="lblLocFrom" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.LocationTo}">
                    <ItemTemplate>
                        <asp:Label ID="lblLocTo" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.OrderedQty}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbOrderQty" runat="server" onmouseup="if(!readOnly)select();" Width="50"
                            Text='<%# Bind("OrderedQty","{0:0.########}") %>' TabIndex="1" />
                        <asp:RangeValidator ID="revOrderQty" ControlToValidate="tbOrderQty" runat="server"
                            Display="Dynamic" ErrorMessage="*" MaximumValue="999999999" MinimumValue="-999999999"
                            Type="Double" ValidationGroup="vgCreate" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Order.OrderDetail.Remark}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbRemark" runat="server" Width="100" Text='<%# Bind("Remark") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
</fieldset>
