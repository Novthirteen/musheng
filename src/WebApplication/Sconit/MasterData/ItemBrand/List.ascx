﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="List.ascx.cs" Inherits="MasterData_ItemBrand_List" %>
<%@ Register Assembly="com.Sconit.Control" Namespace="com.Sconit.Control" TagPrefix="cc1" %>
<fieldset>
    <div class="GridView">
        <cc1:GridView ID="GV_List" runat="server" AutoGenerateColumns="False" DataKeyNames="Code"
            SkinID="GV" AllowMultiColumnSorting="false" AutoLoadStyle="false" SeqNo="0" SeqText="No."
            ShowSeqNo="true" AllowSorting="true" AllowPaging="True" PagerID="gp" Width="100%"
            CellMaxLength="10" TypeName="com.Sconit.Web.CriteriaMgrProxy" SelectMethod="FindAll"
            SelectCountMethod="FindCount">
            <Columns>
                
                <asp:BoundField DataField="Code" HeaderText="${MasterData.ItemBrand.Code}" SortExpression="Code" />
                <asp:BoundField DataField="Description" HeaderText="${MasterData.ItemBrand.Description}" SortExpression="Description" />
                <asp:BoundField DataField="Abbreviation" HeaderText="${MasterData.ItemBrand.Abbreviation}" SortExpression="Abbreviation" />
                <asp:BoundField DataField="ManufactureParty" HeaderText="${MasterData.ItemBrand.ManufactureParty}" SortExpression="ManufactureParty" />  
                <asp:BoundField DataField="Origin" HeaderText="${MasterData.ItemBrand.Origin}" SortExpression="Origin" />  
                <asp:BoundField DataField="ManufactureAddress" HeaderText="${MasterData.ItemBrand.ManufactureAddress}" SortExpression="ManufactureAddress" />  
                <asp:BoundField DataField="IsActive" HeaderText="${MasterData.ItemBrand.IsActive}" SortExpression="IsActive" />  
                <asp:TemplateField HeaderText="${Common.GridView.Action}">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Edit}" OnClick="lbtnEdit_Click" />
                        <asp:LinkButton ID="lbtnDelete" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Code") %>'
                            Text="${Common.Button.Delete}" OnClick="lbtnDelete_Click" OnClientClick="return confirm('${Common.Button.Delete.Confirm}')" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <cc1:GridPager ID="gp" runat="server" GridViewID="GV_List" PageSize="10">
        </cc1:GridPager>
    </div>
</fieldset>