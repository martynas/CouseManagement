<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrainerActivityCalendar.ascx.cs" 
    Inherits="Atrendia.CourseManagement.TrainerFrontend.Support.Controls.TrainerActivityCalendar" %>
<%@ Register Src="~/Support/Controls/MiniCalendar.ascx" TagPrefix="cm" TagName="MiniCalendar" %>
<div id="calendar">
    <h2>
        <asp:HyperLink ID="hlPrevious" runat="server" NavigateUrl="~/Calendar/?mode=month" Text="<" />
        <asp:HyperLink ID="hlMonth" runat="server" NavigateUrl="~/Calendar/?mode=month">January 2008</asp:HyperLink>
        <asp:HyperLink ID="hlNext" runat="server" NavigateUrl="~/Calendar/?mode=month" Text=">" />
    </h2>
    <cm:MiniCalendar ID="miniCalendar" runat="server" />
</div>
