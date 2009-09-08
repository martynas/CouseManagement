<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlannedCourses.ascx.cs" 
    Inherits="Atrendia.CourseManagement.Frontend.Support.Controls.PlannedCourses" %>

<asp:Repeater ID="rptTable" runat="server" OnItemDataBound="rptrTable_ItemDataBound" >
    <HeaderTemplate>
        <table class="data">
            <tr>
                <th>Contacts</th>
                <asp:Repeater runat="server" ID="rptTableHeader" OnItemDataBound="rptrTableHeader_ItemDataBound">
                    <ItemTemplate>
                        <th><asp:Literal runat="server" ID="ltProductTitle" /> <br />
                        <asp:CheckBox runat="server" ID="cbProductTitle" /></th>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
    </HeaderTemplate>
    
    <ItemTemplate>
        <tr>
            <td>
                <asp:Literal runat="server" ID="ltContact" />
                <asp:HiddenField runat="server" ID="hfContactID" />
            </td>
            <asp:Repeater runat="server" ID="rptTableItem" OnItemDataBound="rptrTableItem_ItemDataBound">
                <ItemTemplate>
                    <td>
                        <asp:CheckBox runat="server" ID="cbProduct" />
                    </td>
                </ItemTemplate>
            </asp:Repeater>
        </tr>
    </ItemTemplate>
    
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>