<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MRP_Schedule_CustomerSchedule_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${Common.Business.Flow}:" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" DescField="Description" ValueField="Code"
                    CssClass="inputRequired" ServicePath="FlowMgr.service" MustMatch="true" Width="250"
                    ServiceMethod="GetFlowList" />
            </td>
            <td class="td01">
                ${MRP.Schedule.ScheduleNoRef}
            </td>
            <td class="td02">
                <asp:TextBox ID="tbRefScheduleNo" runat="server" CssClass="inputRequired" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd',isShowWeek:true})"
                    Width="100" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndDate" runat="server" Text="${Common.Business.EndDate}:" />
            </td>
            <td class="td02">
                <asp:TextBox ID="tbEndDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd',isShowWeek:true})"
                    Width="100" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbItemRef" runat="server" Text="${MRP.Schedule.ItemRef}" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSelect" runat="server" Text="${Common.FileUpload.PleaseSelect}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
                <asp:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click"
                    ValidationGroup="vgSave" />
            </td>
        </tr>
        <tr>
            <td class="td01">
            </td>
            <td class="td02">
                <table>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblTemplateType" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblTemplateType_OnSelectedIndexChanged">
                                <asp:ListItem Text="${Common.Business.Template1}" Value="Template1" Selected="True" />
                                <asp:ListItem Text="${Common.Business.Template2}" Value="Template2" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:HyperLink ID="hlTemplate" runat="server" Text="${Common.Business.Template1}" 
                            NavigateUrl="~/Reports/Templates/ImportTemplates/CustomerSchedule.xls"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td />
            <td class="td02">
                <div class="buttons">
                    <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click"
                        Visible="false" />
                    <asp:Button ID="btnRelease" runat="server" Text="${Common.Button.Submit}" OnClick="btnRelease_Click"
                        Visible="false" />
                    <asp:Button ID="btnDelete" runat="server" Text="${Common.Button.Delete}" OnClick="btnDelete_Click"
                        Visible="false" />
                    <asp:Button ID="btnCancel" runat="server" Text="${Common.Button.Cancel}" OnClick="btnCancel_Click"
                        Visible="false" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
<div id="div_List_Detail" runat="server">
    <fieldset>
        <legend>
            <asp:Literal ID="ltllegend" runat="server" />
        </legend>
        <asp:GridView ID="GV_List_Detail" runat="server" AutoGenerateColumns="false" OnRowDataBound="GV_List_Detail_RowDataBound"
            CellPadding="0">
            <Columns>
                <asp:TemplateField HeaderText="${MRP.Schedule.Seq}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlSequence" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Type}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlType" runat="server" Text='<%# Eval("Type")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MRP.Schedule.ShipDate}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlStartTime" runat="server" Text='<%# Eval("StartTime","{0:yyyy-MM-dd}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="${MRP.Schedule.WinDate}" DataField="DateFrom" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:TemplateField HeaderText="${MRP.Schedule.Item}">
                    <ItemTemplate>
                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("Item")%>' ToolTip='<%# Eval("ItemDescription")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="${MRP.Schedule.ItemDescription}" DataField="ItemDescription" />
                <asp:BoundField HeaderText="${Common.Business.RefCode}" DataField="ItemReference" />
                <asp:TemplateField HeaderText="${Common.Business.Uom}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlUom" runat="server" Text='<%# Eval("Uom")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MRP.Schedule.UnitCount}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlUnitCount" runat="server" Text='<%# Eval("UnitCount","{0:0.##}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MRP.Schedule.Location}">
                    <ItemTemplate>
                        <asp:Literal ID="ltlLocation" runat="server" Text='<%# Eval("Location")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${Common.Business.Qty}">
                    <ItemTemplate>
                        <asp:TextBox ID="tbQty" runat="server" Width="50" Text='<%# Bind("Qty","{0:0.##}")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
</div>
