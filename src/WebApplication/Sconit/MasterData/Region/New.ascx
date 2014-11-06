<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_Region_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_Region" runat="server" DataSourceID="ODS_Region" DefaultMode="Insert"
        DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.Region.AddRegion}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Region.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<%# Bind("Code") %>' CssClass="inputRequired"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="${MasterData.Region.Code.Empty}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvInsert" runat="server" ControlToValidate="tbCode" ErrorMessage="${MasterData.Region.Code.Exists}"
                                Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="checkRegionExists" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblName" runat="server" Text="${MasterData.Region.Name}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblInspectLocation" runat="server" Text="${MasterData.Region.InspectLocation}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbInspectLocation" runat="server" Visible="true" DescField="Name"
                                ValueField="Code" Width="250" ServicePath="LocationMgr.service" ServiceMethod="GetLocation" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblRejectLocation" runat="server" Text="${MasterData.Region.RejectLocation}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbRejectLocation" runat="server" Visible="true" DescField="Name"
                                ValueField="Code" Width="250" ServicePath="LocationMgr.service" ServiceMethod="GetLocation" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCostCenter" runat="server" Text="${MasterData.Region.CostCenter}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbCostCenter" runat="server" Visible="true" DescField="Description"
                                ValueField="Code" ServicePath="CostCenterMgr.service" ServiceMethod="GetAllCostCenter"
                                Width="250" CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="frvCostCenter" runat="server" ErrorMessage="${MasterData.Region.CostCenter.Empty}"
                                Display="Dynamic" ControlToValidate="tbCostCenter" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${MasterData.Region.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%#Bind("IsActive") %>' />
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
                            <div class="buttons">
                                <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="${Common.Button.Save}"
                                    CssClass="add" ValidationGroup="vgSave" />
                                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </InsertItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_Region" runat="server" TypeName="com.Sconit.Web.RegionMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Region" InsertMethod="CreateRegion">
</asp:ObjectDataSource>
