<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MRP_Schedule_ProductionPlan_List" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>

<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblItem" runat="server" Text="${MasterData.Order.OrderDetail.Item.Code}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbItemCode" runat="server" Width="250" DescField="Description" ValueField="Code"
                    ServicePath="ItemMgr.service" OnTextChanged="tbItemCode_TextChange" ServiceMethod="GetCacheAllItem" CssClass="inputRequired"
                    InputWidth="150" MustMatch="true" TabIndex="1" AutoPostBack="true" />
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
            <asp:TextBox ID="tbFlow" runat="server" onfocus="this.blur();" ></asp:TextBox>
                <%--<uc3:textbox ID="tbFlow" runat="server" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" AutoPostBack="true"
                    ServiceParameter="string:#tbItemCode" MustMatch="true" Width="250" CssClass="inputRequired"
                    ServiceMethod="GetFlowList" onfocus="this.blur();" />
                <asp:RequiredFieldValidator ID="rfvFlow" runat="server" ErrorMessage="${MasterData.Order.OrderHead.Flow.Required}"
                    Display="Dynamic" ControlToValidate="tbFlow" ValidationGroup="vgCreate" />--%>
            </td>
            <td class="td01">
                <asp:Literal ID="Literal1" runat="server" Text="${MRP.Schedule.PlanInTime}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbPlanInTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"  />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="${MasterData.Order.OrderHead.WinTime.Required}"
                    Display="Dynamic" ControlToValidate="tbPlanInTime" ValidationGroup="vgCreate" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProductLineFacility" runat="server" Text="${MasterData.Order.OrderHead.ProductLineFacility}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txProductLineFacility" runat="server" onfocus="this.blur();" ></asp:TextBox>
                <%--<uc3:textbox ID="txProductLineFacility" runat="server" Visible="true" DescField="Code"
                    ValueField="Code" ServicePath="ProductLineFacilityMgr.service" AutoPostBack="true"
                    MustMatch="true" Width="250" ServiceMethod="GetProductLineFacility" ServiceParameter="string:#tbFlow" />--%>
            </td>
            <td class="td01">
                <asp:Literal ID="Literal2" runat="server" Text="${MasterData.Order.OrderHead.StartTime}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="txtStartTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" />
                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="vgCreate" OnServerValidate="CheckStartTime"
                    Display="Dynamic" />
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
                <cc1:Button ID="btnConfirm" runat="server" Text="${Common.Button.Create}" OnClick="btnConfirm_Click"
                    CssClass="button2" ValidationGroup="vgCreate" FunctionId="EditOrder" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </td>
        </tr>
    </table>
</fieldset>