<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateSelect.ascx.cs" Inherits="Controls_DateSelect" %>



<table class="mtable">
    <tr>
        <td class="td01">
            <asp:Literal ID="lblTimePeriodType" runat="server" Text="${Common.CodeMaster.TimePeriodType}:" />
        </td>
        <td class="td02">
            <asp:DropDownList ID="ddlTimePeriodType" runat="server" DataTextField="Description"
                DataValueField="Value" AutoPostBack="true" OnSelectedIndexChanged="ddlTimePeriodType_SelectedIndexChanged" />
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td class="td01">
            <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
        </td>
        <td class="td02">
            <asp:UpdatePanel ID="UP_Info_Start" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="tbStartDate" runat="server" AutoPostBack="true" OnTextChanged="tbStartDate_TextChanged"
                        onClick="WdatePicker({dateFmt:'yyyy-MM-dd',isShowWeek:true})" Width="100" />
                    <asp:Label ID="lblStartInfo" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTimePeriodType" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="tbStartDate" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
        <td class="td01">
            <asp:Literal ID="lblEndDate" runat="server" Text="${Common.Business.EndDate}:" />
        </td>
        <td class="td02">
            <asp:UpdatePanel ID="UP_Info_End" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="tbEndDate" runat="server" AutoPostBack="true" OnTextChanged="tbEndDate_TextChanged"
                        onClick="WdatePicker({dateFmt:'yyyy-MM-dd',isShowWeek:true})" Width="100" />
                    <asp:Label ID="lblEndInfo" runat="server" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlTimePeriodType" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="tbStartDate" EventName="TextChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
