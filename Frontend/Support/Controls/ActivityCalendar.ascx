<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityCalendar.ascx.cs" 
    Inherits="Atrendia.CourseManagement.Frontend.Support.Controls.ActivityCalendar" %>
<%@ Register Src="~/Support/Controls/MiniCalendar.ascx" TagPrefix="cm" TagName="MiniCalendar" %>
<div id="calendar">
    <h2><asp:HyperLink ID="hlMonth" runat="server" NavigateUrl="~/Calendar/?mode=month">January 2008</asp:HyperLink></h2>
    <cm:MiniCalendar ID="miniCalendar" runat="server" />
</div>
