<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atrendia.CourseManagement.Frontend.Calendar.Default" MasterPageFile="~/Support/Global.Master" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">Calendar</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">

    <div>
        <asp:HyperLink ID="hlPrev" runat="server" style="float: left" Text="&larr;" />
        <asp:HyperLink ID="hlNext" runat="server" style="float: right" Text="&rarr;" />
        <h2 style="text-align: center">
            My Calendar: 
            <asp:Label ID="lblRange" runat="server">Jan 10th, 2009</asp:Label>
        </h2>
    </div>
    <div style="clear: left"></div>
    
    <p class="calendar-views">
        <asp:HyperLink ID="hlAgendaView" runat="server" CssClass="agenda" Text="Agenda view" />
        <asp:HyperLink ID="hlMonthView" runat="server" CssClass="month" Text="Month view" />
        <asp:HyperLink ID="hlWeekView" runat="server" CssClass="week" Text="Week view" />
        <asp:HyperLink ID="hlDayView" runat="server" CssClass="day" Text="Day view" />
    </p>
    
    <p id="pPastContainer" runat="server" visible="false" style="text-align: right">
        (<asp:HyperLink ID="hlPastView" runat="server" Text="Click here to show past activities" />)
    </p>

    <asp:Repeater ID="rptrActivities" runat="server" onitemdatabound="rptrActivities_ItemDataBound">
    <ItemTemplate>
        <asp:Panel ID="pnlActivity" runat="server">
            <p>
                <asp:Label ID="lblDate" runat="server" CssClass="date">Jan 1</asp:Label>
                <asp:Label ID="lblTime" runat="server" CssClass="time" Visible="false">10:00-18:00</asp:Label>
                <asp:Label ID="lblTagCourse" runat="server" Visible="false" CssClass="tag training">training</asp:Label>
                <asp:Label ID="lblTagPreparation" runat="server" Visible="false" CssClass="tag preparation">preparation</asp:Label>
                <asp:Label ID="lblDone" runat="server" Visible="false" CssClass="tag done">Done</asp:Label>
                <asp:HyperLink ID="hlActivity" runat="server">Module I (35-2)</asp:HyperLink>
                <asp:PlaceHolder ID="phParticipants" runat="server" Visible="false">
                    <span class="participants">
                        <asp:Label ID="lblParticipantsIn" runat="server">3</asp:Label>
                        /
                        <asp:Label ID="lblParticipantsMax" runat="server">10</asp:Label>
                    </span>
                </asp:PlaceHolder>
            </p>
            <p style="margin-left: 30px; margin-bottom: 20px">
                <asp:Label ID="lblNotes" runat="server" />
            </p>
        </asp:Panel>
    </ItemTemplate>
    </asp:Repeater>
    
    <asp:Panel ID="pnlNoActivities" runat="server" Visible="false">
        <p>No activities.</p>
    </asp:Panel>

</asp:Content>