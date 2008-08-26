<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Atrendia.CourseManagement.Frontend.Login" MasterPageFile="~/Support/Global.Master" %>
<asp:Content ContentPlaceHolderID="Title" runat="server">Overview</asp:Content>
<asp:Content ContentPlaceHolderID="Menu" runat="server">
    <ul>
        <li class="active"><a href="~/Login.aspx" runat="server">Login</a></li>
    </ul>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    
    <h2>Welcome to Atrendia Course Administration</h2>
    
    <p>This website allows you to manage the participation of your company in <a href="http://www.atrendia.com/">
        Atrendia</a>
        courses.</p>
    
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
