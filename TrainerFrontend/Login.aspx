<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" 
    Inherits="Atrendia.CourseManagement.TrainerFrontend.Login" MasterPageFile="~/Support/Global.Master" %>
<asp:Content ContentPlaceHolderID="Title" runat="server">Trainers Site Login</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    
    <h2>Welcome to Atrendia Course Administration</h2>
    
    <p>This website is intended to be used by trainers of <a href="http://www.atrendia.com/">Atrendia</a>. 
        If you are a client, please visit <a href="http://www.atrendia.com/#>administration site</a> 
        for clients</p>
    
    <asp:Login ID="lfLogin" runat="server">
        <LayoutTemplate>
            <p class="error">
                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
            </p>
            <p>
                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email:</asp:Label><br />
                <asp:TextBox ID="UserName" runat="server" CssClass="text normal focus-on-load"></asp:TextBox>
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="lfLogin">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label><br />
                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="text normal"></asp:TextBox>
                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="lfLogin">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
            </p>
            <p>
                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Login" ValidationGroup="lfLogin" />
            </p>
        </LayoutTemplate>
    </asp:Login>
    
</asp:Content>
