<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="Order_OrderHead_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
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
    }

</script>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${MasterData.Order.OrderDetail.Item.Code}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItemCode" runat="server" Width="250" DescField="Description" ValueField="Code"
                    ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" CssClass="inputRequired"
                    InputWidth="150" MustMatch="true" TabIndex="1" />
                <asp:RequiredFieldValidator ID="rfvItemCode" runat="server" ErrorMessage="${MasterData.Order.OrderDetail.ItemCode.Required}"
                    Display="Dynamic" ControlToValidate="tbItemCode" ValidationGroup="vgCreate" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblOrderQty" runat="server" Text="${MasterData.Order.OrderDetail.OrderedQty}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbOrderQty" runat="server" onmouseup="if(!readOnly)select();" TabIndex="2"
                    CssClass="inputRequired"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revOrderQty" runat="server" Display="Dynamic"
                    ControlToValidate="tbOrderQty" Enabled="true"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="rfvOrderQty" runat="server" ErrorMessage="${MasterData.Order.OrderHead.OrderQty.Required}"
                    Display="Dynamic" ControlToValidate="tbOrderQty" ValidationGroup="vgCreate" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Flow.Flow.Production}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" OnTextChanged="tbFlow_TextChanged" AutoPostBack="true"
                    ServiceParameter="string:#tbItemCode" MustMatch="true" Width="250" CssClass="inputRequired"
                    ServiceMethod="GetProductionFlowCode" />
                <asp:RequiredFieldValidator ID="rfvFlow" runat="server" ErrorMessage="${MasterData.Order.OrderHead.Flow.Required}"
                    Display="Dynamic" ControlToValidate="tbFlow" ValidationGroup="vgCreate" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblItemVersion" runat="server" Text="${MasterData.Order.OrderDetail.ItemVersion}:" Visible="false"/>
            </td>
            <td class="td02">
                <asp:TextBox ID="tbItemVersion" runat="server" Visible="false" CssClass="inputRequired"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvItemVersion" runat="server" ErrorMessage="${MasterData.Order.OrderDetail.ItemVersion.Empty}"
                    Display="Dynamic" ControlToValidate="tbItemVersion" ValidationGroup="vgCreate" Enabled="false"/>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProductLineFacility" runat="server" Text="${MasterData.Order.OrderHead.ProductLineFacility}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="txProductLineFacility" runat="server" Visible="true" DescField="Code"
                    ValueField="Code" ServicePath="ProductLineFacilityMgr.service" AutoPostBack="true"
                    MustMatch="true" Width="250" ServiceMethod="GetProductLineFacility" ServiceParameter="string:#tbFlow" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlShift" runat="server" Text="${MasterData.WorkCalendar.Shift}:" />
            </td>
            <td class="td02">
                <uc:Shift ID="ucShift" runat="server" />
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
                                onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" AutoPostBack="true" OnTextChanged="tbWinTime_TextChanged" />
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
                <asp:TextBox ID="tbStartTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                <asp:HiddenField ID="hfLeadTime" runat="server" Value="0" />
                <asp:HiddenField ID="hfEmTime" runat="server" Value="0" />
                <asp:CustomValidator ID="cvStartTime" runat="server" ValidationGroup="vgCreate" OnServerValidate="CheckStartTime"
                    Display="Dynamic" />
            </td>
            <td class="td01">
                <asp:Literal ID="lblRefOrderNo" runat="server" Text="${MasterData.Order.OrderHead.Flow.RefOrderNo}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbRefOrderNo" runat="server"></asp:TextBox>
            </td>
        </tr>
        <%-- <tr>
            <td class="td01">
                <asp:Literal ID="lblNeedSortAndColor" runat="server" Text="${MasterData.Order.OrderHead.NeedSortAndColor}:" />
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbNeedSortAndColor" runat="server" Checked="true" />
            </td>
        </tr>--%>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <cc1:Button ID="btnConfirm" runat="server" Text="${Common.Button.Create}" OnClick="btnConfirm_Click"
                    CssClass="button2" ValidationGroup="vgCreate" FunctionId="EditOrder" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>
