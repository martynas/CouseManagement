<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="Atrendia.CourseManagement.Frontend.People.Upload" MasterPageFile="~/Support/Global.Master" %>
<%@ Register Src="~/Support/Controls/PlannedCourses.ascx" TagName="PlannedCouses" TagPrefix="cm" %> 

<asp:Content ContentPlaceHolderID="Title" runat="server">Atrendia: Upload People</asp:Content>
<asp:Content ContentPlaceHolderID="Calendar" runat="server">
    <!-- Look ma, no calendar. -->
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="server">

    <asp:Wizard runat="server" ID="mvImportWizard" DisplayCancelButton="false" DisplaySideBar ="false">
        <StartNavigationTemplate />
        <FinishNavigationTemplate />
        <StepNavigationTemplate />
        <WizardSteps>
            <asp:WizardStep ID="viewInitial" runat="server">
            
                <h2>Upload Employees</h2>
                <p class="help-suggestion without-calendar">Follow the step-by-step guide to upload more employees
                    to the training administration site. After upload,
                    these employees might be assigned to training dates.</p>
                    
                <h3 class="top-marginized">1. Download template file</h3>
                <p>Download the example file to your computer. The file
                    shows you what structure you must use for the data
                    to be imported.</p>
                    
                <p class="shifted">
                    <span class="excel-file"><a runat="server" href="~/Media/Contacts_Example.csv">Example file</a> (1 KB)</span> 
                </p>
                
                <h3 class="top-marginized">2. Prepare file</h3>
                <p>Use the example file as a starting point and fill
                    it with relevant data.</p>
                    
                <p>Save the example file in CSV format.</p>
                
                <h3 class="top-marginized">3. Upload file</h3>
                <p>Point to where you placed the CSV file:</p>

                <p class="error" id="pNoFile" runat="server" visible="false">
                    No file was uploaded, please choose one.
                </p>
                
                <p class="shifted">
                    <asp:FileUpload ID="fuContacts" runat="server" />
                </p>
                
                <p>And click the button to proceed:</p>
                
                <p class="shifted">
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="small" 
                        onclick="btnUpload_Click" />
                </p>

            </asp:WizardStep>
            
            <asp:WizardStep ID="viewReview" runat="server">
            
                <h2>Upload Employees (continued)</h2>

                <p id="pParseSuccess" class="success" runat="server" visible="false">You have successfully uploaded a
                CSV file. We extracted the data that is presented below. Please take a minute
                to review it.</p>

                <p id="pParseWarnings" class="success" runat="server" visible="false">You have successfully uploaded a
                CSV file. We extracted the data that is presented below. Please take a minute
                to review it. There were also some issues with the data that are explained
                below the review table.</p>

                <p id="pParseNoData" class="error" runat="server" visible="false">We were unable to
                extract any data from the file you uploaded. 
                (<a href="Upload.aspx">Start over</a>)</p>
            
                <asp:Repeater ID="rptrContacts" runat="server" 
                    onitemdatabound="rptrContacts_ItemDataBound">
                    <HeaderTemplate>
                        <p>
                            <asp:Button ID="btnConfirm" runat="server" Text="Confirm" 
                                onclick="btnConfirm_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                onclick="btnCancel_Click" />
                        </p>
                        <table class="data">
                            <tr>
                                <th>Title</th>
                                <th>First</th>
                                <th>Last</th>
                                <th>Email</th>
                                <th>Phone</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="odd">
                            <td><asp:TextBox ID="tbTitle" runat="server" CssClass="text tiny" Text="CEO" /></td>
                            <td><asp:TextBox ID="tbFirstName" runat="server" CssClass="text medium" Text="Michael" /></td>
                            <td><asp:TextBox ID="tbLastName" runat="server" CssClass="text medium" Text="Hoffman" /></td>
                            <td><asp:TextBox ID="tbEmail" runat="server" CssClass="text medium" Text="mh@atrendia.com" /></td>
                            <td><asp:TextBox ID="tbPhone" runat="server" CssClass="text medium" Text="+1 234 4567" /></td>
                        </tr>
                        <tr id="trMessages" runat="server" class="odd" visible="false">
                            <td colspan="5">
                                <p id="pMessage" runat="server" class="error">There were validation errors.</p>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr>
                            <td><asp:TextBox ID="tbTitle" runat="server" CssClass="text tiny" Text="CEO" /></td>
                            <td><asp:TextBox ID="tbFirstName" runat="server" CssClass="text medium" Text="Michael" /></td>
                            <td><asp:TextBox ID="tbLastName" runat="server" CssClass="text medium" Text="Hoffman" /></td>
                            <td><asp:TextBox ID="tbEmail" runat="server" CssClass="text medium" Text="mh@atrendia.com" /></td>
                            <td><asp:TextBox ID="tbPhone" runat="server" CssClass="text medium" Text="+1 234 4567" /></td>
                        </tr>
                        <tr id="trMessages" runat="server" visible="false">
                            <td colspan="5">
                                <p id="pMessage" runat="server" class="error">There were validation errors.</p>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                        <p>
                            <asp:Button ID="btnConfirmBelow" runat="server" Text="Continue" 
                                onclick="btnConfirm_Click" />
                            <asp:Button ID="btnCancelBelow" runat="server" Text="Cancel" 
                                onclick="btnCancel_Click" />
                        </p>            
                    </FooterTemplate>
                </asp:Repeater>
                
                <asp:Panel ID="pnlWarnings" runat="server" CssClass="upload-errors">
                    <p>
                        The following issues were found during data import:
                    </p>
                    <asp:Repeater ID="rptrWarnings" runat="server" 
                        onitemdatabound="rptrWarnings_ItemDataBound">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <asp:Label ID="lblMessage" runat="server">Description of the error.</asp:Label>
                                (row <asp:Label ID="lblLineNumber" runat="server">3</asp:Label>)
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
                
            </asp:WizardStep>
            
            <asp:WizardStep runat="server" ID="viewPlannedCourses">
                <h2>Upload Employees (continued)</h2>
                
                <p class="success">If you are planing
                to register your employees at Atrendia's courses, please note it below.
                This information will help Atrendia understand your needs better and create a 
                more personalized plan for your company.</p>
                
                <cm:PlannedCouses runat="server" ID="cpCoursePlanning" />
                <asp:Button ID="btnPlannedCoursesConfirm" runat="server" Text="Confirm" 
                    onclick="btnPlannedCoursesConfirm_Click" />
            </asp:WizardStep>
            
            <asp:WizardStep runat="server" ID="viewFinnish">
                <h2>Upload Employees (finished)</h2>
                <p class="success">
                    All the information has been saved succesfully. You can return to  
                        <a href="~/Default.aspx">the main site</a> or 
                        <a href="Default.aspx">another list of contacts.</a>
                </p>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
        
</asp:Content>
