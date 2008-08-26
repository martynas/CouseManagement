<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Atrendia.CourseManagement.Frontend.People.Default" MasterPageFile="~/Support/Global.Master" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">Overview</asp:Content>
<asp:Content ContentPlaceHolderID="Calendar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <h2>People</h2>
    
    <p id="pHelpUpload" runat="server" class="help-suggestion">
        Want to add contacts? You can <a href="Upload.aspx">upload them</a>.
    </p>
    
    <p id="pInfoUpload" runat="server" class="help-suggestion" visible="false">
        <asp:Label ID="lblInfoUpload" runat="server">3 contacts were added.</asp:Label>
    </p>
    
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
            <td><asp:CheckBox ID="cbSelected" runat="server" /></td>
            <td><asp:Label ID="lbTitle" runat="server">CEO</asp:Label></td>
            <td><asp:HyperLink ID="hlName" runat="server">Michael Hoffman</asp:HyperLink></td>
            <td><asp:HyperLink ID="hlEmail" runat="server">mh@atrendia.com</asp:HyperLink></td>
            <td><asp:Label ID="lbPhone" runat="server">+1 234 5678</asp:Label></td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr class="odd">
            <td><asp:CheckBox ID="cbSelected" runat="server" /></td>
            <td><asp:Label ID="lbTitle" runat="server">CEO</asp:Label></td>
            <td><asp:HyperLink ID="hlName" runat="server">Michael Hoffman</asp:HyperLink></td>
            <td><asp:HyperLink ID="hlEmail" runat="server">mh@atrendia.com</asp:HyperLink></td>
            <td><asp:Label ID="lbPhone" runat="server">+1 234 5678</asp:Label></td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
        <p>
            <asp:Button ID="btDelete" runat="server" CommandName="Delete"
                        Text="Delete"
                        OnClientClick="return confirm('Are you sure you want to delete selected contacts?');" />
        </p>
    </FooterTemplate>
    </asp:Repeater>

</asp:Content>