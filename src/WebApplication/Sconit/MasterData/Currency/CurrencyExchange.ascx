<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CurrencyExchange.ascx.cs"
    Inherits="MasterData_Currency_CurrencyExchange" %>
<%@ Register Src="~/Controls/TextBox.ascx" TagName="textbox" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script language="javascript" type="text/javascript">
    function checkCurrency(source, args) {
        var baseCurrency = $('#<%= ((Controls_TextBox)(this.FV_CurrencyExchange.FindControl("tbBaseCurrency"))).ClientID + "_suggest" %>').val();
        var exchangeCurrency = $('#<%= ((Controls_TextBox)(this.FV_CurrencyExchange.FindControl("tbAltCurrency"))).ClientID + "_suggest" %>').val();
        //alert(baseUom + " | "+altUom);

        if (baseCurrency == exchangeCurrency) {
            args.IsValid = false;
        }
    }
</script>

<fieldset id="fldSearch" runat="server">
    <table class="mtable">
        <tr>
            <td class="ttd01">
                <asp:Literal ID="ltlBaseCurrency" runat="server" Text="${MasterData.CurrencyExchange.BaseCurrency}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbBaseCurrency" runat="server"></asp:TextBox>
            </td>
            <td class="ttd01">
                <asp:Literal ID="ltlAltCurrency" runat="server" Text="${MasterData.CurrencyExchange.AltCurrency}:" />
            </td>
            <td class="ttd02">
                <asp:TextBox ID="tbAltCurrency" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <div class="tablefooter">
        <asp:Button ID="SearchBtn" runat="server" Text="${Common.Button.Search}" OnClick="btnSearch_Click"
            CssClass="button2" />
        <asp:Button ID="btnInsert" runat="server" Text="${Common.Button.New}" OnClick="btnInsert_Click"
            CssClass="button2" />
    </div>
</fieldset>
<div id="floatdiv">
    <fieldset id="fldInsert" runat="server" visible="false">
        <legend>${MasterData.CurrencyExchange.NewCurrencyExchange}</legend>
        <asp:FormView ID="FV_CurrencyExchange" runat="server" DataSourceID="ODS_FV_CurrencyExchange"
            DefaultMode="Insert">
            <InsertItemTemplate>
                <table class="mtable">
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlBaseCurrency" runat="server" Text="${MasterData.CurrencyExchange.BaseCurrency}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbBaseCurrency" runat="server" Visible="true" DescField="Name" MustMatch="true"
                                CssClass="inputRequired" ValueField="Code" ServicePath="CurrencyMgr.service"
                                ServiceMethod="GetAllCurrency" Width="250" />
                            <asp:RequiredFieldValidator ID="rfvBaseCurrency" runat="server" ErrorMessage="${MasterData.CurrencyExchange.BaseCurrency.Empty}"
                                Display="Dynamic" ControlToValidate="tbBaseCurrency" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlAltCurrency" runat="server" Text="${MasterData.CurrencyExchange.AltCurrency}:" />
                        </td>
                        <td class="td02">
                            <uc3:textbox ID="tbAltCurrency" runat="server" Visible="true" DescField="Name" MustMatch="true"
                                CssClass="inputRequired" ValueField="Code" ServicePath="CurrencyMgr.service"
                                ServiceMethod="GetAllCurrency" Width="250" />
                            <asp:RequiredFieldValidator ID="rfvAltCurrency" runat="server" ErrorMessage="${MasterData.CurrencyExchange.AltCurrency.Empty}"
                                Display="Dynamic" ControlToValidate="tbAltCurrency" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvCurrency" runat="server" ErrorMessage="${MasterData.CurrencyExchange.Same.Currency}"
                                ValidationGroup="vgSave" ClientValidationFunction="checkCurrency" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="ltlBaseQty" runat="server" Text="${MasterData.CurrencyExchange.BaseQty}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbBaseQty" runat="server" Text='<%# Bind("BaseQty") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvBaseQty" runat="server" ErrorMessage="${MasterData.CurrencyExchange.BaseQty.Empty}"
                                Display="Dynamic" ControlToValidate="tbBaseQty" ValidationGroup="vgSave" />
                            <asp:RegularExpressionValidator ID="revBaseQty" ControlToValidate="tbBaseQty" runat="server"
                                ErrorMessage="${Common.Validator.Valid.Number}" ValidationExpression="^[0-9]+(.[0-9]{1,8})?$"
                                Display="Dynamic" ValidationGroup="vgSave" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="ltlAltQty" runat="server" Text="${MasterData.CurrencyExchange.AlterQty}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbAltQty" runat="server" Text='<%# Bind("ExchangeQty") %>' CssClass="inputRequired" />
                            <asp:RequiredFieldValidator ID="rfvAltQty" runat="server" ErrorMessage="${MasterData.CurrencyExchange.AltQty.Empty}"
                                Display="Dynamic" ControlToValidate="tbAltQty" ValidationGroup="vgSave" />
                            <asp:RegularExpressionValidator ID="revAltQty" ControlToValidate="tbAltQty" runat="server"
                                ErrorMessage="${Common.Validator.Valid.Number}" ValidationExpression="^[0-9]+(.[0-9]{1,8})?$"
                                Display="Dynamic" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="td01">
                            <asp:Literal ID="lblStartDate" runat="server" Text="${Common.Business.StartDate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbStartDate" runat="server" Text='<%# Bind("StartDate") %>' CssClass="inputRequired" />
                            <cc1:CalendarExtender ID="ceStartDate" TargetControlID="tbStartDate" Format="yyyy-MM-dd"
                                runat="server">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="${MasterData.CurrencyExchange.StartDate.Empty}"
                                Display="Dynamic" ControlToValidate="tbStartDate" ValidationGroup="vgSave" />
                            <asp:CustomValidator ID="cvStartDate" runat="server" ControlToValidate="tbStartDate"
                                ErrorMessage="*" Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="CV_ServerValidate" />
                        </td>
                        <td class="td01">
                            <asp:Literal ID="lblEndDate" runat="server" Text="${Common.Business.EndDate}:" />
                        </td>
                        <td class="td02">
                            <asp:TextBox ID="tbEndDate" runat="server" Text='<%#Bind("EndDate") %>' />
                            <cc1:CalendarExtender ID="ceEndDate" TargetControlID="tbEndDate" Format="yyyy-MM-dd"
                                runat="server" />
                            <asp:CustomValidator ID="cvEndDate" runat="server" ControlToValidate="tbEndDate"
                                ErrorMessage="*" Display="Dynamic" ValidationGroup="vgSave" OnServerValidate="CV_ServerValidate" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="ttd01">
                        </td>
                        <td>
                            <asp:Button ID="btnNew" runat="server" CommandName="Insert" Text="${Common.Button.Save}"
                                CssClass="button2" ValidationGroup="vgSave" />
                            <asp:Button ID="btnBack" runat="server" Text="${Common.Button.Back}" OnClick="btnBack_Click"
                                CssClass="button2" />
                        </td>
                    </tr>
                </table>
            </InsertItemTemplate>
        </asp:FormView>
    </fieldset>
</div>
<fieldset id="fldGV" runat="server" visible="false">
    <asp:GridView ID="GV_CurrencyExchange" runat="server" AutoGenerateColumns="False"
        DataSourceID="ODS_GV_CurrencyExchange" OnDataBound="GV_CurrencyExchange_OnDataBind"
        DataKeyNames="Id" AllowPaging="True" OnRowUpdating="GV_CurrencyExchange_RowUpdating">
        <Columns>
            <asp:TemplateField HeaderText="${MasterData.CurrencyExchange.Id}">
                <ItemTemplate>
                    <asp:Literal ID="lblId" runat="server" Text='<%# Eval("Id") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.CurrencyExchange.BaseCurrency}">
                <ItemTemplate>
                    <asp:Literal ID="lblBaseCurrency" runat="server" Text='<%# Eval("BaseCurrency") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.CurrencyExchange.BaseQty}">
                <ItemTemplate>
                    <asp:Literal ID="lblBaseQty" runat="server" Text='<%# Eval("BaseQty","{0:0.########}")  %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbBaseQty" runat="server" Text='<%# Bind("BaseQty","{0:0.########}")  %>'
                        Width="50" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.CurrencyExchange.AltCurrency}">
                <ItemTemplate>
                    <asp:Literal ID="lblAltCurrency" runat="server" Text='<%# Eval("ExchangeCurrency") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.CurrencyExchange.AlterQty}">
                <ItemTemplate>
                    <asp:Literal ID="tbAlterQty" runat="server" Text='<%# Eval("ExchangeQty","{0:0.########}")  %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="lblBaseQty" runat="server" Text='<%# Bind("ExchangeQty","{0:0.########}")  %>'
                        Width="50" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.CurrencyExchange.StartDate}">
                <ItemTemplate>
                    <asp:Literal ID="lblStartDate" runat="server" Text='<%# Eval("StartDate","{0:yyyy-MM-dd}")  %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.CurrencyExchange.EndDate}">
                <ItemTemplate>
                    <asp:Literal ID="lblEndDate" runat="server" Text='<%# Eval("EndDate","{0:yyyy-MM-dd}")  %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="tbEndDate" runat="server" Text='<%# Bind("EndDate","{0:yyyy-MM-dd}")  %>'
                        Width="65" />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" ItemStyle-Width="100px"
                HeaderText="Action" DeleteText="&lt;span onclick=&quot;JavaScript:return confirm('${Common.Delete.Confirm}?')&quot;&gt;${Common.Button.Delete}&lt;/span&gt;">
            </asp:CommandField>
        </Columns>
    </asp:GridView>
    <asp:Label runat="server" ID="lblResult" Text="${Common.GridView.NoRecordFound}"
        Visible="false" />
</fieldset>
<asp:ObjectDataSource ID="ODS_GV_CurrencyExchange" runat="server" TypeName="com.Sconit.Web.CurrencyExchangeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.CurrencyExchange" SelectMethod="GetCurrencyExchange"
    UpdateMethod="UpdateCurrencyExchange" OnUpdating="ODS_GV_CurrencyExchange_OnUpdating"
    DeleteMethod="DeleteCurrencyExchange" OnDeleting="ODS_GV_CurrencyExchange_OnDeleting"
    OnDeleted="ODS_GV_CurrencyExchange_OnDeleted">
    <SelectParameters>
        <asp:ControlParameter ControlID="tbBaseCurrency" Name="baseCurrency" PropertyName="Text"
            Type="String" />
        <asp:ControlParameter ControlID="tbAltCurrency" Name="exchangeCurrency" PropertyName="Text"
            Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_FV_CurrencyExchange" runat="server" TypeName="com.Sconit.Web.CurrencyExchangeMgrProxy"
    DataObjectTypeName="com.Sconit.Entity.MasterData.CurrencyExchange" InsertMethod="CreateCurrencyExchange"
    OnInserted="ODS_CurrencyExchange_Inserted" OnInserting="ODS_CurrencyExchange_Inserting">
</asp:ObjectDataSource>
