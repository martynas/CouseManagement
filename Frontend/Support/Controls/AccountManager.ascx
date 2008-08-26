<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountManager.ascx.cs" Inherits="Atrendia.CourseManagement.Frontend.AccountManager" %>

<div id="your-contact">
    <h2>Atrendia Contact</h2>
    <div class="details">
        <p><strong><asp:Label ID="lblTitle" runat="server" /></strong></p>
        <p ID="pDirectPhone" runat="server" visible="false">
            Tel. <asp:Label ID="lblDirectPhone" runat="server" />
        </p>
        <p ID="pMobilePhone" runat="server" visible="false">
            Cell. <asp:Label ID="lblMobilePhone" runat="server" />
        </p>
        <p ID="pEmail" runat="server" visible="false">
            <span class="email"><asp:HyperLink ID="hlEmail" runat="server" /></span>
        </p>
    </div>
</div>
