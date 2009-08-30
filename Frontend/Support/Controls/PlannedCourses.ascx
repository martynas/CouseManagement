<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlannedCourses.ascx.cs" 
    Inherits="Atrendia.CourseManagement.Frontend.Support.Controls.PlannedCourses" %>

<asp:Repeater ID="rptTable" runat="server" onitemdatabound="rptrTable_ItemDataBound" >
    <HeaderTemplate>
        <table>
            <tr>
                <td>Contacts</td>
                <asp:Repeater runat="server" ID="rptTableHeader">
                    <ItemTemplate>
                        <td><asp:Literal runat="server" ID="ltProductTitle" /></td>
                    </ItemTemplate>
                </asp:Repeater>
            </tr>
    </HeaderTemplate>
    
    <ItemTemplate>
        <tr>
            <td>
                <asp:Literal runat="server" ID="ltContact" />
            </td>
            <asp:Repeater runat="server" ID="rptTableItem">
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