<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_2_1_Galleriet._Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Galleriet</h1>
        <asp:Image ID="CurrentImage" runat="server" CssClass="img-responsive" />
    </div>
    <asp:Panel ID="SuccessPanel" runat="server" Visible="False">
        <div class="alert alert-success">
            <a class="close" data-dismiss="alert">x</a>
            <strong>Lyckades</strong>, uppladdningen lyckades.
        </div>
    </asp:Panel>
    <asp:Panel ID="FailPanel" runat="server" Visible="false">
        <div class="alert alert-danger">
            <a class="close" data-dismiss="alert">x</a>
            <strong>Misslyckades</strong><asp:Literal ID="FailLiteral" runat="server"></asp:Literal>
        </div>
    </asp:Panel>
    <div class="row">
        <div class="col-md-12 uploadedImages">
            <ul class="list-inline galleryImages">
                <asp:ListView ID="SavedImages" runat="server" ItemType="_2_1_Galleriet.Model.GalleryItem" SelectMethod="SavedImages_GetThumbs">
                    <ItemTemplate>
                        <li <%# isActive(Item.fileName) %>><a href="<%# Item.href %>">
                            <img src="<%# Item.thumburl %>" /></a></li>
                    </ItemTemplate>
                </asp:ListView>
            </ul>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="BulletList" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-6 UploadImage">
            <asp:FileUpload ID="ImageFileUpload" runat="server" CssClass="btn btn-default" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidatorImageFileUpload" runat="server" ErrorMessage="En fil måste väljas" ControlToValidate="ImageFileUpload" Display="None"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidatorImageFileUpload" runat="server" ErrorMessage="Filändelsen måste vara gif, jpg eller png" ControlToValidate="ImageFileUpload" Display="None" ValidationExpression="^.+\.(gif|jpg|png)$"></asp:RegularExpressionValidator>
            <asp:CustomValidator ID="ModelValidator" runat="server" ErrorMessage="" Visible="False" Display="None"></asp:CustomValidator>
        </div>
        <div class="col-md-6">
            <asp:Button ID="UploadButton" runat="server" Text="Ladda upp" OnClick="UploadButton_Click" CssClass="btn btn-default" />
        </div>
    </div>
</asp:Content>
