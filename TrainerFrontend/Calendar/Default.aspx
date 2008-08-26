<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" 
    Inherits="Atrendia.CourseManagement.TrainerFrontend.Overview.Default" MasterPageFile="~/Support/Global.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">Overview</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <h2>Upcoming training sessions</h2>
    
    <asp:MultiView ID="mvCoursePane" runat="server" ActiveViewIndex="0" >
        <asp:View runat="server" ID="vCourseNone">
            <p>There are none.</p>
        </asp:View>
        <asp:View runat="server" ID="vCourses">
            <asp:Repeater ID="rptrCourses" runat="server" onitemdatabound="rptrCourses_ItemDataBound">
                <HeaderTemplate>
                    <table class="data">
                        <tr>
                            <th>Date</th>
                            <th>Time</th>
                            <th>Course Activity Title</th>
                            <th>Company</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:Label ID="lblDate" runat="server" CssClass="date">7 Jun</asp:Label></td>
                        <td><asp:Label ID="lblTime" runat="server" CssClass="time">18:00-19:00</asp:Label></td>
                        <td><asp:HyperLink ID="hlActivity" runat="server" Text='<%# Eval("Title") %>' /></td>
                        <td><asp:HyperLink ID="hlCompany" runat="server" Text='SEB' /></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td><asp:Label ID="lblDate" runat="server" CssClass="date">7 Jun</asp:Label></td>
                        <td><asp:Label ID="lblTime" runat="server" CssClass="time">18:00-19:00</asp:Label></td>
                        <td><asp:HyperLink ID="hlActivity" runat="server" Text='<%# Eval("Title") %>' /></td>
                        <td><asp:HyperLink ID="hlCompany" runat="server" Text='SEB' /></td>
                    </tr>
                </AlternatingItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </asp:View>
    </asp:MultiView>
    
</asp:Content>