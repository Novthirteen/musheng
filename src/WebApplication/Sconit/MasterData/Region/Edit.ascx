<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_Region_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<div id="divFV" runat="server">
    <asp:FormView ID="FV_Region" runat="server" DataSourceID="ODS_Region" DefaultMode="Edit"
        Width="100%" DataKeyNames="Code" OnDataBound="FV_Region_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.Region.UpdateRegion}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Region.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCode" runat="server" Text='<%# Bind("Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblName" runat="server" Text="${MasterData.Region.Name}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbName" runat="server" Text='<%# Bind("Name") %>' Width="250"></asp:TextBox>
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
                                <asp:Button ID="Button1" runat="server" CommandName="Update" Text="${Common.Button.Save}"
                                    CssClass="apply" ValidationGroup="vgSave" />
                                <asp:Button ID="Button2" runat="server" CommandName="Delete" Text="${Common.Button.Delete}"
                                    CssClass="delete" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                                <asp:Button ID="Button3" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                    CssClass="back" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </EditItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_Region" runat="server" TypeName="com.Sconit.Web.RegionMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.Region" UpdateMethod="UpdateRegion"
    OnUpdated="ODS_Region_Updated" OnUpdating="ODS_Region_Updating" DeleteMethod="DeleteRegion"
    OnDeleted="ODS_Region_Deleted" SelectMethod="LoadRegion">
    <SelectParameters>
        <asp:Parameter Name="code" Type="String" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="code" Type="String" />
    </DeleteParameters>
</asp:ObjectDataSource>
