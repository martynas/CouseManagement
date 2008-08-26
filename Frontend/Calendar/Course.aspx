<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Course.aspx.cs" Inherits="Atrendia.CourseManagement.Frontend.Calendar.Course" MasterPageFile="~/Support/Global.Master" %>
<%@ Register Src="~/Support/Controls/AccountManager.ascx" TagPrefix="cm" TagName="AccountManager" %>
<%@ Register Src="~/Support/Controls/TrainingActivityHeading.ascx" TagPrefix="cm" TagName="TrainingActivityHeading" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">
    Atrendia: Course
</asp:Content>
<asp:Content ContentPlaceHolderID="ResponsibleUser" runat="server">
    <cm:AccountManager ID="amResponsibleUser" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <cm:TrainingActivityHeading ID="tahActivityHeading" runat="server" LongVersion="true" />
    
    <h3 runat="server" id="hDescription">Description</h3>
    <p style="margin-left: 30px" runat="server" id="pContentsNotes" >
        <asp:Label ID="lblContentsNotes" runat="server">Training content notes here.</asp:Label>
    </p>
    <p style="margin-left: 30px" runat="server" id="pNotes" >
        <asp:Label ID="lblNotes" runat="server">Notes here.</asp:Label>
    </p>
            
    <h3>Participants</h3>
    <asp:Panel ID="pnlNoParticipants" runat="server" Visible="false">
        <p>No participants registered at this time.</p>
    </asp:Panel>
    
    <asp:Repeater ID="rptrParticipants" runat="server" 
        onitemdatabound="rptrParticipants_ItemDataBound" 
        onitemcommand="rptrParticipants_ItemCommand">
    <HeaderTemplate>
        <table class="data">
        <tr>
            <th style="padding-left: 5px"><input class="select-all" type="checkbox"/></th>
            <th style="width: 30px">Title</th>
            <th style="width: 150px">First, last</th>
            <th style="width: 100px">E-mail</th>
            <th style="width: 80px">Phone</th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td runat="server" id="tdSelected"><asp:CheckBox ID="cbSelected" runat="server" />
                <asp:HiddenField runat="server" ID="hfContactCDMId" /></td>
            <td><asp:Label ID="lbTitle" runat="server">CEO</asp:Label></td>
            <td><asp:HyperLink ID="hlName" runat="server">Michael Hoffman</asp:HyperLink></td>
            <td><asp:HyperLink ID="hlEmail" runat="server">mh@atrendia.com</asp:HyperLink></td>
            <td><asp:Label ID="lbPhone" runat="server">+1 234 5678</asp:Label></td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr class="odd">
            <td><asp:CheckBox ID="cbSelected" runat="server" />
                <asp:HiddenField runat="server" ID="hfContactCDMId" /></td>
            <td><asp:Label ID="lbTitle" runat="server">CEO</asp:Label></td>
            <td><asp:HyperLink ID="hlName" runat="server">Michael Hoffman</asp:HyperLink></td>
            <td><asp:HyperLink ID="hlEmail" runat="server">mh@atrendia.com</asp:HyperLink></td>
            <td><asp:Label ID="lbPhone" runat="server">+1 234 5678</asp:Label></td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
        <p style="width: 520px">
            <span style="float: right">
                <strong><asp:Label ID="lblSeatsTaken" runat="server">2</asp:Label></strong>
                of
                <strong><asp:Label ID="lblSeatsTotal" runat="server">10</asp:Label></strong>
                available seats booked.
            </span>
            <asp:Button ID="btnRemoveFromTraining" runat="server" Text="Remove selected"
                        OnClientClick="return confirm('Are you sure you want to remove selected participants?');"
                        CommandName="Remove" />
        </p>
    </FooterTemplate>
    </asp:Repeater>
    
    <h3 class="clear" runat="server" id="hEmployees">All employees</h3>
    
    <asp:Repeater ID="rptrContacts" runat="server" 
        onitemdatabound="rptrContacts_ItemDataBound" 
        onitemcommand="rptrContacts_ItemCommand">
    <HeaderTemplate>
        <table class="data">
        <tr>
            <th style="padding-left: 5px"><input class="select-all" type="checkbox"/></th>
            <th style="width: 30px">Title</th>
            <th style="width: 150px">First, last</th>
            <th style="width: 100px">E-mail</th>
            <th style="width: 80px">Phone</th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td><asp:CheckBox ID="cbSelected" runat="server" />
                <asp:HiddenField runat="server" ID="hfContactCDMId" /></td>
            <td><asp:Label ID="lbTitle" runat="server">CEO</asp:Label></td>
            <td><asp:HyperLink ID="hlName" runat="server">Michael Hoffman</asp:HyperLink></td>
            <td><asp:HyperLink ID="hlEmail" runat="server">mh@atrendia.com</asp:HyperLink></td>
            <td><asp:Label ID="lbPhone" runat="server">+1 234 5678</asp:Label></td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr class="odd">
            <td><asp:CheckBox ID="cbSelected" runat="server" />
                <asp:HiddenField runat="server" ID="hfContactCDMId" /></td>
            <td><asp:Label ID="lbTitle" runat="server">CEO</asp:Label></td>
            <td><asp:HyperLink ID="hlName" runat="server">Michael Hoffman</asp:HyperLink></td>
            <td><asp:HyperLink ID="hlEmail" runat="server">mh@atrendia.com</asp:HyperLink></td>
            <td><asp:Label ID="lbPhone" runat="server">+1 234 5678</asp:Label></td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
        <p>
            <asp:Button ID="btnAdd" runat="server" CommandName="Add"
                        Text="Add selected to training" />
        </p>
    </FooterTemplate>
    </asp:Repeater>

</asp:Content>
