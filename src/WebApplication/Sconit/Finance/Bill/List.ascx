﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="Finance_Bill_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="BillNo"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="True" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount" OnRowDataBound="GV_List_RowDataBound" DefaultSortExpression="BillNo"
            DefaultSortDirection="Descending">
            <Columns>
                <asp:BoundField DataField="BillNo" HeaderText="${MasterData.Bill.BillNo}" SortExpression="BillNo" />
                <asp:TemplateField HeaderText="${MasterData.Bill.Supplier}" SortExpression="BillAddress.Party.Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "BillAddress.Party.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.ExternalBillNo}" SortExpression="ExternalBillNo">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ExternalBillNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CreateDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="${MasterData.Bill.CreateDate}"
                    SortExpression="CreateDate" />
                <asp:TemplateField HeaderText="${MasterData.Bill.CreateUser}" SortExpression="CreateUser">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "CreateUser.Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="${MasterData.Bill.Status}" SortExpression="Status">
                    <ItemTemplate>
                        <cc1:CodeMstrLabel ID="lblStatus" runat="server" Code="Status" Value='<%# Bind("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="InvoiceDate" DataFormatString="{0:yyyy-MM-dd}" HeaderText="${MasterData.Bill.InvoiceDate}" />
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <cc1:LinkButton ID="lbtnView" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BillNo") %>'
                            Text="${Common.Button.View}" OnClick="lbtnView_Click" FunctionId="ViewBill">
                        </cc1:LinkButton>
                        <cc1:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BillNo") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" FunctionId="EditBill">
                        </cc1:LinkButton>
                        <cc1:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BillNo") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')"
                            FunctionId="DeleteBill">
                        </cc1:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>
