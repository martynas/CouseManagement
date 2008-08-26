<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" 
    Inherits="Atrendia.CourseManagement.TrainerFrontend.PasswordRecovery" 
    MasterPageFile="~/Support/Global.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Trainers Site Password Reset</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
    
    <h2>Reset your password</h2>
    
    <p>Here you can reset your passoword if you have forgotten it. Please provide your e-mail and 
        press "Reset" and your password will be sent by e-mail.</p>
    
    <asp:PasswordRecovery ID="prRecovery" runat="server">
        <UserNameTemplate>
            <p class="error">
                <asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
            </p>
            <p>
                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Email:</asp:Label><br />
                <asp:TextBox ID="UserName" runat="server" CssClass="text normal focus-on-load"/>
                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="prRecovery">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Reset" ValidationGroup="prRecovery" />
            </p>
        </UserNameTemplate>
    </asp:PasswordRecovery>
    
</asp:Content>
