<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="Quote_GPID_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<fieldset>
    <legend>
        <asp:Literal ID="ltlID" runat="server" Text="${Quote.Tooling.ProjectNo}:"></asp:Literal>
        <asp:Literal ID="ltID" runat="server"></asp:Literal>
    </legend>
    <table class="mtable">
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlCustomer" runat="server" Text="${Quote.GPID.Cusomer}:"></asp:Literal>
            </td>
            <td class="td02">
                <uc3:textbox ID="txtCustomer" runat="server" Visible="true" Width="250" MustMatch="false"
                    DescField="Name" ValueField="Code" ServicePath="OrderProductionPlanMgr.service" ServiceMethod="GetCustomer" />
            </td>
            <td class="td01">
                <asp:Literal ID="ltlDesc" runat="server" Text="${Quote.GPID.Desc}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlSatrtDate" runat="server" Text="${Quote.GPID.StartDate}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtStartDate" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlSattus" runat="server" Text="${Quote.GPID.Status}"></asp:Literal>
            </td>
            <td class="td02">
                <asp:CheckBox ID="cbStatus" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlProduct" runat="server" Text="${Quote.GPID.Product}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProdut" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlEndCustomer" runat="server" Text="${Quote.GPID.EndCustomer}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtEndCustomer" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlAddr" runat="server" Text="${Quote.GPID.Addr}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtAddr" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlLifeCycle" runat="server" Text="${Quote.GPID.LifeCycle}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtLifeCycle" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlOTS" runat="server" Text="${Quote.GPID.OTS}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtOTS" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlPPAP" runat="server" Text="${Quote.GPID.PPAP}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtPPAP" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlSOP" runat="server" Text="${Quote.GPID.SOP}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtSOP" runat="server" onClick="WdatePicker({dateFmt:'yyyy-MM-dd'})"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlProjectManager" runat="server" Text="${Quote.GPID.ProjectManager}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtProjectManager" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlBuyer" runat="server" Text="${Quote.GPID.Buyer}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtBuyer" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlTechnology" runat="server" Text="${Quote.GPID.Technology}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtTechnology" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlQuality" runat="server" Text="${Quote.GPID.Quality}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtQuality" runat="server"></asp:TextBox>
            </td>
            <td class="td01">
                <asp:Literal ID="ltlDesc1" runat="server" Text="${Quote.GPID.Desc1}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtDesc1" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td01">
                <asp:Literal ID="ltlDesc2" runat="server" Text="${Quote.GPID.Desc2}:"></asp:Literal>
            </td>
            <td class="td02">
                <asp:TextBox ID="txtDesc2" runat="server"></asp:TextBox>
            </td>
            <td class="td01"></td>
            <td class="td02"></td>
        </tr>
        <tr>
            <td class="td01"></td>
            <td class="td02"></td>
            <td class="td01"></td>
            <td class="td02">
                <asp:Button ID="btnSave" runat="server" Text="${Common.Button.Save}" OnClick="btnSave_Click" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click" />
            </td>
        </tr>
    </table>
</fieldset>