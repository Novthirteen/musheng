<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Import2.ascx.cs" Inherits="Order_OrderHead_OrderImport_Import2" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ac1" %>
<%@ Register Src="~/MRP/PlanSchedule/DateSelect.ascx" TagName="DateSelect" TagPrefix="uc2" %>



<fieldset>
    <table class="mtable">
        <uc2:DateSelect ID="ucDateSelect" runat="server" />
        <tr>
            <td class="td01">
                <asp:Literal ID="lblFlow" runat="server" Text="${Common.Business.Message.Flow}" />
            </td>
            <td class="td02">
                <uc3:textbox ID="tbFlow" runat="server" Visible="true" DescField="Description" ValueField="Code"
                    ServicePath="FlowMgr.service" MustMatch="true" Width="250" ServiceMethod="GetFlowList"
                    TabIndex="-1" />
            </td>
            <td class="td01">
                ${Common.Business.TimeType}:
            </td>
            <td class="td02">
                <asp:RadioButtonList ID="rblDateType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="${MasterData.Order.OrderHead.WindowTime}" Value="winTime" Selected="True" />
                    <asp:ListItem Text="${MasterData.Order.OrderHead.StartTime}" Value="startTime" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlSelect" runat="server" Text="${Common.FileUpload.PleaseSelect}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:FileUpload ID="fileUpload" ContentEditable="false" runat="server" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlTemplate" runat="server" Text="${Common.Business.Template}:" />
            </td>
            <td class="td02">
                <table>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblListFormat" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="${Common.Business.Template1}" Value="Template1" Selected="True" />
                                <asp:ListItem Text="${Common.Business.Template2}" Value="Template2" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:HyperLink ID="hlTemplate1" runat="server" Text="${Common.Business.Template1}"></asp:HyperLink>
                            <asp:HyperLink ID="hlTemplate2" runat="server" Text="${Common.Business.Template2}"></asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" />
            <td class="ttd02">
                <div class="buttons">
                    <asp:Button ID="btnImport" runat="server" Text="${Common.Button.Import}" OnClick="btnImport_Click"
                        CssClass="apply" />
                    <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                        CssClass="back" />
                </div>
            </td>
        </tr>
    </table>
</fieldset>
