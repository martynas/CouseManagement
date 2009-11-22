<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" 
    Inherits="Atrendia.CourseManagement.Frontend.People.View" MasterPageFile="~/Support/Global.Master" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">Atrendia: Person details</asp:Content>
<asp:Content ContentPlaceHolderID="Calendar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <h2><asp:Label ID="lblTitle" runat="server">Mr Michael Hoffman</asp:Label></h2>

    <p style="font-size: 16px; letter-spacing: -1px">
        <strong>Email</strong> 
            <asp:HyperLink ID="hlEmail" runat="server">mh@atrendia.com</asp:HyperLink>
        <asp:PlaceHolder ID="phPhone" runat="server" Visible="false">
            <strong style="margin-left: 10px">Phone</strong> 
            <asp:Label ID="lblPhone" runat="server">+1 234 56789</asp:Label>
        </asp:PlaceHolder>
    </p>    
    <p>
        <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_click" />
    </p>
    
    <asp:MultiView ID="mvTrainings" runat="server" ActiveViewIndex=1>
        <asp:View ID="pnlEmpty" runat="server">
        </asp:View>
           
        <asp:View ID="pnlNoTrainings" runat="server" >
            <p>This person has not attended a training yet.</p>
        </asp:View>
    
        <asp:View ID="pnlTrainings" runat="server">
            <h3>Trainings</h3>
            <p>The user is attending these trainings:</p>
            
            <asp:Repeater ID="rptrActivities" runat="server" 
                onitemdatabound="rptrActivities_ItemDataBound">
            <ItemTemplate>
                <p><strong><asp:Label ID="lblDate" runat="server">Jan 10 2009</asp:Label></strong>
                   <asp:Label ID="lblTagCourse" runat="server" Visible="false" CssClass="tag training">training</asp:Label>
                   <asp:Label ID="lblTagPreparation" runat="server" Visible="false" CssClass="tag preparation">preparation</asp:Label>
                   <asp:HyperLink ID="hlTitle" runat="server">Activity title</asp:HyperLink></p>
            </ItemTemplate>
            </asp:Repeater>
            
        </asp:View>
    </asp:MultiView>
    
</asp:Content>