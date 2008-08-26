<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrainingActivityHeading.ascx.cs" 
    Inherits="Atrendia.CourseManagement.TrainerFrontend.Support.Controls.TrainingActivityHeading" %>

<asp:Panel ID="pnlMain" runat="server">
    <h2>
        Training of <asp:Label ID="lblDate" runat="server">Jan 10</asp:Label>:
        <asp:Label ID="lblHeading" runat="server">Heading</asp:Label>
    </h2>
    <p class="warning" runat="server" id="pPastActivityWarning" visible="false">
        The activity has passed or already finished, therefor cannot be edited.</p>
    
    <p style="font-size: 16px; letter-spacing: -1px">
        <strong>Company</strong> 
            <asp:Label ID="lblCompanyAlias" runat="server" style="margin-right: 10px">Not Specified</asp:Label>
        <strong>Contact</strong> 
            <asp:Label ID="lblContact" runat="server">Not Specified</asp:Label>
    </p>
    <p style="font-size: 16px; letter-spacing: -1px">
        <asp:PlaceHolder ID="phTime" runat="server">
            <strong>Time</strong> 
                <asp:Label ID="lblTime" runat="server" style="margin-right: 10px">10:00-18:00</asp:Label>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phLV1" runat="server" >
            <strong>Location</strong> 
                <asp:Label ID="lblLocation" runat="server" Text="Unknown" style="margin-right: 3px"/>
            <asp:HyperLink runat="server" ID="nuChangeLocation" Text="Change"
                NavigateUrl="~/Calendar/ChangeLocation.aspx?activity=" style="margin-right: 10px"/>
        </asp:PlaceHolder>
        <strong>Trainer</strong>
            <asp:Label ID="lblTrainer" runat="server">Bo Larsen</asp:Label>
    </p>
    <asp:PlaceHolder ID="phLV2" runat="server">
        <p style="font-size: 16px; letter-spacing: -1px">
            <strong>E-mail platform</strong>
                <asp:Label ID="lblEmailPlatform" runat="server" style="margin-right: 10px">Unknown</asp:Label>
            <strong>Add-in version</strong>
                <asp:Label ID="lblAddinVersion" runat="server" style="margin-right: 10px">Unknown</asp:Label>
            <strong>Language</strong>
                <asp:Label ID="lblLanguage" runat="server" style="margin-right: 10px">Unknown</asp:Label>
        </p>
    </asp:PlaceHolder>
</asp:Panel>