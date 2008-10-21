<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" 
    Inherits="Atrendia.CourseManagement.TrainerFrontend.Profile.Default"
    MasterPageFile="~/Support/Global.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">Profile</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Calendar" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <h2>Profile</h2>
    <p>Here you can check you personal information and change password.</p>

    <asp:ChangePassword ID="ChangePassword1" runat="server" DisplayUserName="False">
        <ChangePasswordTemplate>
            <p class="error">
                <asp:Literal EnableViewState="False" ID="FailureText" runat="server" />
            </p>
            <p>
                <asp:Label ID="Label1" AssociatedControlID="CurrentPassword" runat="server">Current Password:</asp:Label>            
                <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" CssClass="text normal focus-on-load" />
                <asp:RequiredFieldValidator ControlToValidate="CurrentPassword" 
                    ErrorMessage="Password is required." ID="CurrentPasswordRequired" 
                    runat="server" ToolTip="Password is required." 
                    ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="Label2" AssociatedControlID="NewPassword" runat="server">New Password:</asp:Label>            
                <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" CssClass="text normal" />
                <asp:RequiredFieldValidator ControlToValidate="NewPassword" 
                    ErrorMessage="Password is required." ID="rfvNewPassword" 
                    runat="server" ToolTip="New Password is required." 
                    ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="Label3" AssociatedControlID="ConfirmNewPassword" runat="server">New Password:</asp:Label>            
                <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" CssClass="text normal" />
                <asp:RequiredFieldValidator ControlToValidate="ConfirmNewPassword" 
                    ErrorMessage="Password is required." ID="rfvConfirmNewPassword" 
                    runat="server" ToolTip="Confirm New Password is required." 
                    ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
            </p>
            <asp:CompareValidator ControlToCompare="NewPassword" 
                ControlToValidate="ConfirmNewPassword" Display="Dynamic" 
                ErrorMessage="The confirm New Password must match the New Password entry."
                ID="NewPasswordCompare" runat="server" ValidationGroup="ChangePassword1"/>
            <p>
                <asp:Button CommandName="ChangePassword" ID="ChangePasswordPushButton" 
                    runat="server" Text="Change Password" ValidationGroup="ChangePassword1" />
                <asp:Button CommandName="Cancel" ID="CancelPushButton" CausesValidation="False"
                    runat="server" Text="Cancel" />
            </p>
        </ChangePasswordTemplate>
        <SuccessTemplate>
            <p>Your password has been changed. Return to <a href="Default.aspx">profile page</a></p>
        </SuccessTemplate>
    </asp:ChangePassword>
</asp:Content>
