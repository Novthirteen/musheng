<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="MasterData_Facility_Edit" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="floatdiv">
    <asp:FormView ID="FV_ProductLineFacility" runat="server" DataSourceID="ODS_ProductLineFacility" DefaultMode="Edit"
        DataKeyNames="Id" OnDataBound="FV_ProductLineFacility_DataBound">
        <EditItemTemplate>
            <fieldset>
                <legend>${MasterData.Flow.Facility.Info}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Flow.Facility.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<% #Bind("Code") %>' CssClass="inputRequired" Enabled="false" />
                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="${MasterData.Flow.Facility.Code.Required}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSaveGroup" />
                            
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${Common.Business.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# Bind("IsActive") %>' />
                        </td>
                    </tr>
                     <tr>
                        <td class="td01">
                            <asp:Literal ID="lblRouting" runat="server" Text="${Menu.MasterData.Routing}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbRouting" runat="server" Visible="true" Width="250" DescField="Description"
                                ValueField="Code" ServicePath="RoutingMgr.service" ServiceMethod="GetAllRouting"/>
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblDescription" runat="server" Text="${MasterData.Flow.Routing.Description}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="lDescription" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblPointTime" runat="server" Text="${Menu.MasterData.PointTime}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="txtPointTime" runat="server" Text='<% #Bind("PointTime") %>' />
                        </td>
                        <td class="td01">
                        </td>
                        <td class="td02">
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="tablefooter">
                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="${Common.Button.Save}"
                    CssClass="button2" ValidationGroup="vgSaveGroup" />
                <asp:Button ID="btnDelete" runat="server" CommandName="Delete" Text="${Common.Button.Delete}"
                    CssClass="button2" OnClientClick="return confirm('${Common.Button.Delete.Confirm}?')" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </div>
        </EditItemTemplate>
    </asp:FormView>
</div>

<asp:ObjectDataSource ID="ODS_ProductLineFacility" runat="server" TypeName="com.Sconit.Web.ProductLineFacilityMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Customize.ProductLineFacility" OnUpdating="ODS_ProductLineFacility_Updating"
    OnUpdated="ODS_ProductLineFacility_Updated" UpdateMethod="UpdateProductLineFacility" DeleteMethod="DeleteProductLineFacility"
    OnDeleted="ODS_ProductLineFacility_Deleted" SelectMethod="FindProductLineFacility">
    <SelectParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:Parameter Name="Id" Type="Int32"  />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Id" Type="Int32"  ConvertEmptyStringToNull="true" />
        <asp:Parameter Name="IsActive" Type="Boolean" ConvertEmptyStringToNull="true" />
    </UpdateParameters>
</asp:ObjectDataSource>
