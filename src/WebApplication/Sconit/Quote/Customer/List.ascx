<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Quote_Customer_List" %>

<fieldset>
    <%--<legend>
        <asp:Literal ID="lblCurrentParty" runat="server" Text="${MasterData.Party.CurrentParty}:" />
        <asp:Literal ID="lbCurrentParty" runat="server" />
    </legend>--%>
    <asp:GridView ID="GV_List" runat="server" AutoGenerateColumns="false" OnRowEditing="edit" OnRowCancelingEdit="cancel" OnRowUpdating="update" OnRowDeleting="delete">
        <Columns>
            <asp:TemplateField HeaderText="${MasterData.Customer.Code}">
                <ItemTemplate>
                    <asp:Literal ID="ltlCustomerCode" runat="server" Text='<%# Eval("Code")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${MasterData.Customer.Name}">
                <ItemTemplate>
                    <asp:Literal ID="ltlCustomerName" runat="server" Text='<%# Eval("Name")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.SType}">
                <ItemTemplate>
                    <asp:Literal ID="ltlSType" runat="server" Text='<%# Eval("SType")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.BillPeriod}">
                <ItemTemplate>
                    <asp:Literal ID="ltlBillPeriod" runat="server" Text='<%# Eval("BillPeriod")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.P_LossRate}">
                <ItemTemplate>
                    <asp:Literal ID="ltlP_LossRate" runat="server" Text='<%# Eval("P_LossRate")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.P_ManageFee}">
                <ItemTemplate>
                    <asp:Literal ID="ltlP_ManageFee" runat="server" Text='<%# Eval("P_ManageFee")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.P_FinanceFee}">
                <ItemTemplate>
                    <asp:Literal ID="ltlP_FinanceFee" runat="server" Text='<%# Eval("P_FinanceFee")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.P_Profit}">
                <ItemTemplate>
                    <asp:Literal ID="ltlP_Profit" runat="server" Text='<%# Eval("P_Profit")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.M_ManageFee}">
                <ItemTemplate>
                    <asp:Literal ID="ltlM_ManageFee" runat="server" Text='<%# Eval("M_ManageFee")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.M_FinanceFee}">
                <ItemTemplate>
                    <asp:Literal ID="ltlM_FinanceFee" runat="server" Text='<%# Eval("M_FinanceFee")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.DeliveryAdd}">
                <ItemTemplate>
                    <asp:Literal ID="ltlDeliveryAdd" runat="server" Text='<%# Eval("DeliveryAdd")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.DeliveryCity}">
                <ItemTemplate>
                    <asp:Literal ID="ltlDeliveryCity" runat="server" Text='<%# Eval("DeliveryCity")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.Status}">
                <ItemTemplate>
                    <asp:Literal ID="ltlStatus" runat="server" Text='<%# Eval("Status")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.StartDate}">
                <ItemTemplate>
                    <asp:Literal ID="ltlStartDate" runat="server" Text='<%# Eval("StartDate")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Quote.Fee.EndDate}">
                <ItemTemplate>
                    <asp:Literal ID="ltlEndDate" runat="server" Text='<%# Eval("EndDate")%>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="${Common.GridView.Action}">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnEdit" runat="server" Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'></asp:LinkButton>
                    <asp:LinkButton ID="lbtnDelete" runat="server" Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:CommandField ShowEditButton="true" HeaderText="${Common.GridView.Action}" />
            <asp:CommandField ShowDeleteButton="true" HeaderText="${Common.GridView.Action}" />--%>
        </Columns>
    </asp:GridView>
</fieldset>