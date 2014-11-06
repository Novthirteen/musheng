<%@ Control Language="C#" AutoEventWireup="true" CodeFile="New.ascx.cs" Inherits="MasterData_Facility_New" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<div id="floatdiv">
    <asp:FormView ID="FV_ProductLineFacility" runat="server" DataSourceID="ODS_ProductLineFacility" DefaultMode="Insert"
        DataKeyNames="Code">
        <InsertItemTemplate>
            <fieldset>
                <legend>${MasterData.Flow.FlowDetail.Basic.Info}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Flow.Facility.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbCode" runat="server" Text='<% #Bind("Code") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ErrorMessage="${MasterData.Flow.Facility.Code.Required}"
                                Display="Dynamic" ControlToValidate="tbCode" ValidationGroup="vgSaveGroup" />
                            <asp:CustomValidator ID="cvCode" runat="server" ControlToValidate="tbCode" Display="Dynamic"
                                ValidationGroup="vgSaveGroup" OnServerValidate="checkCodeExists" />
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
                </table>
            </fieldset>
            <div class="tablefooter">
                <asp:Button ID="btnInsert" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                    CssClass="button2" ValidationGroup="vgSaveGroup" />
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </div>
        </InsertItemTemplate>
    </asp:FormView>
</div>

<asp:ObjectDataSource ID="ODS_ProductLineFacility" runat="server" TypeName="com.Sconit.Web.ProductLineFacilityMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Customize.ProductLineFacility" InsertMethod="CreateProductLineFacility"
    OnInserted="ODS_ProductLineFacility_Inserted" OnInserting="ODS_ProductLineFacility_Inserting" SelectMethod="FindProductLineFacility">
</asp:ObjectDataSource>
