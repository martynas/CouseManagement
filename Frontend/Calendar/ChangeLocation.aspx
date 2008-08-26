<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeLocation.aspx.cs" 
    Inherits="Atrendia.CourseManagement.Frontend.Calendar.ChangeLocation" 
    MasterPageFile="~/Support/Global.Master" %>
<%@ Register Src="~/Support/Controls/AccountManager.ascx" TagPrefix="cm" TagName="AccountManager" %>
<%@ Register Src="~/Support/Controls/TrainingActivityHeading.ascx" TagPrefix="cm" TagName="TrainingActivityHeading" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Atrendia: Change Location
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ResponsibleUser" runat="server">
    <cm:accountmanager ID="amResponsibleUser" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Content" runat="server">
    <cm:TrainingActivityHeading ID="tahActivityHeading" runat="server" LongVersion="false" />
    
    <h3>Please specify the training location</h3>
    <p>
        <asp:TextBox runat="server" ID="tbLocation" TextMode="MultiLine" Rows="5" Width="40%"/>
    </p><p>
        <asp:Button runat="server" ID="btnSave" Text="Save" onclick="btnSave_Click" />
        <asp:Button runat="server" ID="btnCancel" Text="Cancel" onclick="btnCancel_Click" />
    </p>
</asp:Content>