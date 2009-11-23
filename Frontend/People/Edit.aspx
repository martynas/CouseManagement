<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" 
    Inherits="Atrendia.CourseManagement.Frontend.People.Edit" MasterPageFile="~/Support/Global.Master" %>

<asp:Content ContentPlaceHolderID="Title" runat="server">Atrendia: Person details</asp:Content>
<asp:Content ContentPlaceHolderID="Calendar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">
    <asp:MultiView runat="server" ID="mvHeader">
        <asp:View runat="server" ID="vEdit" >
            <h2>Edit Participant's Data</h2>
            <p class="help-suggestion without-calendar">Here you can update data of a participant. Please press "Save" when you are done or 
            "Cancel" if you want to leave data unchanged.</p>
        </asp:View>
        
        <asp:view runat="server" ID="vCreate" >
            <h2>Enter a New Participant</h2>
            <p class="help-suggestion without-calendar">Here you can enter data of a new participant.</p>
        </asp:view>
    </asp:MultiView>
    
    <asp:ValidationSummary runat ="server" ID="vSummary" HeaderText="<p class='error'>Please fix following errors:" DisplayMode="List" />
    
    <p>
        <table class="editlist">
            <tr>
                <th>Title:</th>
                <td><asp:TextBox runat="server" ID="tbTitle" Text='<%# EditedContact.Title %>' CssClass="text normal" />
                </td>
            </tr>
            <tr>
                <th>First name:</th>
                <td><asp:TextBox runat="server" ID="tbFirstName" Text='<%# Bind("EditedContact.FirstName") %>' CssClass="text normal" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbFirstName" Text="*" 
                        ErrorMessage="First name is mandatory" />
                </td>
            </tr>
            <tr>
                <th>Last name:</th>
                <td><asp:TextBox runat="server" ID="tbLastName" Text="<%# EditedContact.LastName %>" CssClass="text normal" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbLastName" Text="*" 
                        ErrorMessage="Last name is mandatory" />
                </td>
            </tr>
            <tr>
                <th>Email:</th>
                <td><asp:TextBox runat="server" ID="tbEmail" Text="<%# EditedContact.Email %>" CssClass="text normal" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="tbEmail" Text="*" 
                        ErrorMessage="Email is mandatory" />
                </td>
            </tr>
            <tr>
                <th>Mobile phone:</th>
                <td><asp:TextBox runat="server" ID="tbMobilePhone" Text="<%# EditedContact.MobilePhone %>" CssClass="text normal" />
                </td>
            </tr>
        </table>
    </p>
    
    <p>
        <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_click" />
        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_click" />
    </p>
</asp:Content>
