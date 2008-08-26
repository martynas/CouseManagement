<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atrendia.CourseManagement.Frontend.Overview.Default" MasterPageFile="~/Support/Global.Master" %>
<asp:Content ContentPlaceHolderID="Title" runat="server">Overview</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    
    <h2>Welcome to Atrendia Course Administration</h2>
    
    <p class="help-suggestion">
        Looking for help? Take a minute to review <a href="#">our user guide</a>.
    </p>
    
    <p>
        We should present a brief introduction here, maybe some really short
        instructions on what to do here. We could also add some changing
        information here &ndash; like company news or special offers.
    </p>
    
    <div class="clear"></div>
    <div class="front-box">
        <h3>Preparation</h3>
        <asp:Panel ID="pnlPreparationNone" runat="server" Visible="false">
            <p>There are none.</p>
        </asp:Panel>
        <asp:Repeater ID="rptrPreparation" runat="server" 
                            onitemdatabound="rptrPreparation_ItemDataBound">
        <ItemTemplate>
            <p>
                <asp:Label ID="lblDate" runat="server" CssClass="date">7 Jun</asp:Label>
                <asp:Label ID="lblDone" runat="server" CssClass="tag done">done</asp:Label>
                <asp:HyperLink ID="hlActivity" runat="server" Text='<%# Eval("Title") %>' />
            </p>
        </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="front-box">
        <h3>Upcoming training sessions</h3>
        <asp:Panel ID="pnlCourseNone" runat="server" Visible="false">
            <p>There are none.</p>
        </asp:Panel>
        <asp:Repeater ID="rptrCourses" runat="server" onitemdatabound="rptrCourses_ItemDataBound">
        <ItemTemplate>
            <p>
                <asp:Label ID="lblDate" runat="server" CssClass="date">7 Jun</asp:Label>
                <asp:Label ID="lblTime" runat="server" CssClass="time">18:00-19:00</asp:Label>
                <asp:HyperLink ID="hlActivity" runat="server" Text='<%# Eval("Title") %>' />
            </p>
        </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="clear" style="margin-bottom: 20px"></div>

    <div class="front-box">
        <h3>Common tasks</h3>
        <p>
            <a runat="server" href="~/Calendar/Default.aspx">View agenda</a>
        </p>
        <p>
            <a runat="server" href="~/People/Upload.aspx">Upload a list of employees</a>
        </p>
    </div>

    <div class="front-box">
        <h3>Useful links</h3>
        <p>
            <a href="http://www.atrendia.com/">Atrendia Aps</a>
        </p>
        <p>
            <a href="http://www.atrendia.com/">Download Outlook plugin</a>
        </p>
        <p>
            <a href="http://www.atrendia.com/">Something totally different</a>
        </p>
    </div>
    
    
</asp:Content>