<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MiniCalendar.ascx.cs" Inherits="Atrendia.CourseManagement.Frontend.Controls.MiniCalendar" %>

<asp:Repeater ID="rptrWeeks" runat="server" OnItemDataBound="rptrWeeks_ItemDataBound">
    <HeaderTemplate>
        <table>
    </HeaderTemplate>
    <ItemTemplate>        
       <asp:Repeater ID="rptrDays" runat="server" OnItemDataBound="rptrDays_ItemDataBound">
       <HeaderTemplate><tr></HeaderTemplate>
       <ItemTemplate><td ID="tdDay" runat="server"></td></ItemTemplate>
       <FooterTemplate></tr></FooterTemplate>
       </asp:Repeater>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
