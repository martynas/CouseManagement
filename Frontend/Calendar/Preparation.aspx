<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preparation.aspx.cs" Inherits="Atrendia.CourseManagement.Frontend.Calendar.Preparation" MasterPageFile="~/Support/Global.Master" %>
<%@ Register Src="~/Support/Controls/AccountManager.ascx" TagPrefix="cm" TagName="AccountManager" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    Atrendia: Preparation
</asp:Content>
<asp:Content ContentPlaceHolderID="ResponsibleUser" runat="server">
    <cm:AccountManager ID="amResponsibleUser" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <h2>Preparation: <asp:Label ID="lblHeading" runat="server">Heading</asp:Label></h2>
    <p>
        <strong>Deadline</strong>: <asp:Label ID="lblDeadline" runat="server" Text="Jan 1, 2009" />
    </p>
    <asp:Panel ID="pnlNotDone" runat="server">
    <p>
        <asp:Button ID="btnMarkAsDone" runat="server" Text="Mark as Done" 
            onclick="btnMarkAsDone_Click" />
    </p>
    </asp:Panel>
    <p style="margin-left: 30px">
        <asp:Label ID="lblNotes" runat="server">Notes here.</asp:Label>
    </p>
</asp:Content>
