<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_2_1_Galleriet._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Galleriet</h1>
        <asp:Image ID="CurrentImage" runat="server" CssClass="img-responsive" />
    </div>

    <div class="row">
        <div class="col-md-12 UploadedImages">
            <ul class="galleryImages">
                <%--<asp:ListView ID="SavedImages" runat="server" ItemType="_2_1_Galleriet.Model.Gallery.ImageItem" SelectMethod="SavedImages_GetData">--%>
                <asp:ListView ID="SavedImages" runat="server" ItemType="_2_1_Galleriet.Model.Gallery+ImageItem" SelectMethod="SavedImages_GetThumbs">
                    <ItemTemplate>
                        <li><a href="<%# Item.href %>"><img src="<%# Item.thumburl %>" /></a></li>
                    </ItemTemplate>
                </asp:ListView>
            </ul>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:ValidationSummary ID="ValidationSummary" runat="server" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 UploadImage">
            <asp:FileUpload ID="ImageFileUpload" runat="server" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorImageFileUpload" runat="server" ErrorMessage="En fil måste väljas" ControlToValidate="ImageFileUpload" Display="None"></asp:RequiredFieldValidator>
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidatorImageFileUpload" runat="server" ErrorMessage="Filändelsen måste vara gif, jpg eller png" ControlToValidate="ImageFileUpload" Display="None" ValidationExpression="^.*\\.(gif|jpg|png)$"></asp:RegularExpressionValidator>--%>
            <asp:CustomValidator ID="ModelValidator" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
        </div>
        <div class="col-md-6">
            <asp:Button ID="UploadButton" runat="server" Text="Ladda upp" OnClick="UploadButton_Click" />
        </div>
    </div>
</asp:Content>
