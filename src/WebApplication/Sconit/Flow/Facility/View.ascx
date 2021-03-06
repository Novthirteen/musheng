<%@ Control Language="C#" AutoEventWireup="true" CodeFile="View.ascx.cs" Inherits="MasterData_Facility_View" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="sc1" %>
<div id="floatdiv">
    <asp:FormView ID="FV_ProductLineFacility" runat="server" DataSourceID="ODS_ProductLineFacility" DefaultMode="ReadOnly"
        OnDataBound="FV_ProductLineFacility_DataBound" DataKeyNames="Code">
        <ItemTemplate>
            <fieldset>
                <legend>${MasterData.Flow.Facility.Info}</legend>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblCode" runat="server" Text="${MasterData.Flow.Facility.Code}:" />
                        </td>
                        <td class="td02">
                            <asp:Literal ID="tbCode" runat="server" Text='<%# Bind("Code") %>' />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblIsActive" runat="server" Text="${Common.Business.IsActive}:" />
                        </td>
                        <td class="td02">
                            <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# Eval("IsActive") %>' Enabled="false" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <div class="tablefooter">
                <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                    CssClass="button2" />
            </div>
        </ItemTemplate>
    </asp:FormView>
</div>
<asp:ObjectDataSource ID="ODS_ProductLineFacility" runat="server" TypeName="com.Sconit.Web.ProductLineFacilityMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.Customize.ProductLineFacility" SelectMethod="FindProductLineFacility">
    <SelectParameters>
        <asp:Parameter Name="Id" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
