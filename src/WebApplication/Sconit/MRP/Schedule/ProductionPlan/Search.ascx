<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.ascx.cs" Inherits="MRP_Schedule_ProductionPlan_Search" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %><script type="text/javascript">
                                                                                                function diag() {
                                                                                                    var str = prompt("移动到哪一行？", "");
                                                                                                    if (str) {
                                                                                                        $('#<%= hfMoveNum.ClientID %>').val(str);
                                                                                                    }
                                                                                                    else {
                                                                                                        return false;
                                                                                                    }
                                                                                                }
</script>



<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <%--<asp:Literal ID="lblOrderNo" runat="server" Text="${Warehouse.LocTrans.OrderNo}:" />--%>
                <asp:Literal ID="lblFlow" runat="server" Text="${MasterData.Order.OrderHead.Flow}:" />
            </td>
            <td class="td02">
                <%--<asp:TextBox ID="tbOrderNo" runat="server" Visible="true" />--%>
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" MustMatch="true" Width="250" ServiceMethod="GetFlowList" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlProductLineFacility" runat="server" Text="${MasterData.Order.OrderHead.ProductLineFacility}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbProductLineFacility" runat="server" Visible="true" DescField="Code"
                    ValueField="Code" ServicePath="ProductLineFacilityMgr.service" AutoPostBack="true"
                    MustMatch="true" Width="250" ServiceMethod="GetProductLineFacility" ServiceParameter="string:#tbFlow"
                    Text='<%# Bind("ProductLineFacility") %>' />
            </td>
        </tr>
        
        <tr>
            <td class="td01">
                <%--<asp:Literal ID="lblItem" runat="server" Text="${Common.Business.Item}:" />--%>
            </td>
            <td class="td02">
                <%--<uc3:textbox ID="tbItem" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Description" ValueField="Code" ServicePath="ItemMgr.service" ServiceMethod="GetCacheAllItem" />--%>
            </td>
            <td class="td01">
            </td>
            <td class="td02">
            </td>
        </tr>
        <tr>
            <td  class="td01">
            </td>
            <td class="td02">
            </td>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:Button ID="btnSearch" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
                    CssClass="button2" />
                <cc1:Button ID="btnNew" runat="server" Text="${Common.Button.New}" OnClick="btnNew_Click"
                    CssClass="button2" FunctionId="EditOrder" />
            </td>
        </tr>
    </table>
</fieldset>
<asp:HiddenField ID="hfMoveNum" runat="server" />
<%----------------------------------------------------------------------------------------------------------------------------------------------------------%>
<fieldset id="fld_Details" runat="server" visible="false">
    <legend>${MRP.Schedule.ProductionPlan}</legend>
    <asp:RadioButton ID="radBtnDown" runat="server" GroupName="Move" Text="${Common.Button.RadMoveDown}" />
    <asp:RadioButton ID="radBtnUp" runat="server" GroupName="Move" Text="${Common.Button.RadMoveUp}" />
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="false" OnRowEditing="edit" OnRowCancelingEdit="cancel" 
        OnRowUpdating="update" OnRowDeleting="delete" CellPadding="0">
        <Columns>
            <asp:TemplateField HeaderText="${Common.GridView.Seq}">
                <ItemTemplate>
                    <asp:Literal ID="lbId" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.OrderPlanNo}">
                <ItemTemplate>
                    <asp:Literal ID="lblOrderPlanNo" runat="server" Text='<%# Eval("OrderPlanNo")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.Status}">
                <ItemTemplate>
                    <asp:Literal ID="ltlStatus1" runat="server" Text='<%# Eval("Status")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtStatus" runat="server" Width="30px" Text='<%# Eval("Status")%>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.ProductionLineCode}">
                <ItemTemplate>
                    <asp:Literal ID="ltlStatus" runat="server" Text='<%# Eval("ProductionLineCode")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.Item1}">
                <ItemTemplate>
                    <asp:Literal ID="lblItem" runat="server" Text='<%# Eval("Item")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.OrderQty}">
                <ItemTemplate>
                    <asp:Literal ID="lblOrderQty" runat="server" Text='<%# Eval("OrderQty")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtOrderQty" runat="server" Text='<%# Eval("OrderQty")%>' Width="50px" ></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="${MRP.Schedule.PlanInTime}">
                <ItemTemplate>
                    <asp:Literal ID="lblPlanInTime" runat="server" Text='<%# Eval("PlanInTime","{0:yyyy-MM-dd}")%>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="${MRP.Schedule.StartTime}">
                <ItemTemplate>
                    <asp:Literal ID="lblStartTime" runat="server" Text='<%# Eval("StartTime","{0:yyyy-MM-dd HH:mm}")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtStartTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" Width="110px" Text='<%# Eval("StartTime","{0:yyyy-MM-dd HH:mm}")%>' ></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.PlanEndTime}">
                <ItemTemplate>
                    <asp:Literal ID="lblPlanEndTime" runat="server" Text='<%# Eval("PlanEndTime","{0:yyyy-MM-dd HH:mm}")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.ActualEndTime}">
                <ItemTemplate>
                    <asp:Literal ID="lblActualEndTime" runat="server" Text='<%# Eval("ActualEndTime","{0:yyyy-MM-dd HH:mm}")%>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtActualEndTime" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" Width="110px" Text='<%# Eval("ActualEndTime","{0:yyyy-MM-dd HH:mm}")%>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.EndTime}">
                <ItemTemplate>
                    <asp:Literal ID="lblEndTime" runat="server" Text='<%# Eval("EndTime","{0:yyyy-MM-dd HH:mm}")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.PlanOrderHours}">
                <ItemTemplate>
                    <asp:Literal ID="lblPlanOrderHours" runat="server" Text='<%# Eval("PlanOrderHours")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.OrderNum}">
                <ItemTemplate>
                    <asp:Literal ID="lblOrderNum" runat="server" Text='<%# Eval("OrderNum")%>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MRP.Schedule.CreateUser}">
                <ItemTemplate>
                    <asp:Literal ID="ltlCreateUser" runat="server" Text='<%# Eval("CreateUser")%>' />
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="lb" runat="server" ></asp:TextBox>
                </InsertItemTemplate>
            </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="${MRP.Schedule.CreateTime}">
                <ItemTemplate>
                    <asp:Literal ID="ltlWindowTime" runat="server" Text='<%# Bind("WindowTime","{0:yyyy-MM-dd HH:mm}")%>' />
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="${Common.Button.Move}">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnExchange" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderPlanNo") %>'
                        Text="${Common.Button.Move}" OnClientClick="return diag();" OnClick="lbtnExchange_Click">
                    </asp:LinkButton>
                    <%--<asp:LinkButton ID="lbtnMove" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderPlanNo") %>'
                        Text="${Common.Button.Move}" OnClientClick="return diag();" OnClick="lbtnMove_Click">
                    </asp:LinkButton>--%>
                    <%--<asp:LinkButton ID="lbtnDown" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderPlanNo") %>'
                        Text="${Common.Button.Down}" OnClick="lbtnDown_Click">
                    </asp:LinkButton>--%>
                    <asp:LinkButton ID="lbtnAdd" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderPlanNo")+","+ Container.DataItemIndex %>'
                        Text="${Common.Button.New}" OnClick="lbtnAdd_Click" />
                    <asp:LinkButton ID="lbtnTransToOrder" runat="server" Text="${Common.Button.TransToOrder}" 
                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OrderPlanNo") %>' OnClick="btnTransToOrder_Click">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="true" ShowCancelButton="true" ShowDeleteButton="true" HeaderText="${Common.GridView.Action}"
            DeleteText="&lt;span onclick=&quot;JavaScript:return confirm('${Common.Delete.Confirm}?')&quot;&gt;${Common.Button.Delete}&lt;/span&gt;" />
        </Columns>
    </asp:GridView>
    <asp:Literal ID="ltl_Result" runat="server" Text="${Common.GridView.NoRecordFound}"
        Visible="false" />
        <%--<asp:Button ID="btnTransToOrder" runat="server" OnClick="btnTransToOrder_Click" Text="${Common.Button.TransToOrder}"/>--%>
</fieldset> 
<div>
    <span style="color:Red;">
        <asp:Literal ID="ltStock" runat="server" Visible="false" Text="test"></asp:Literal>
    </span>
</div>
