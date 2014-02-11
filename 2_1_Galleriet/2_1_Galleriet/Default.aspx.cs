using _2_1_Galleriet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _2_1_Galleriet
{
    public partial class _Default : Page
    {
        private Service _service;

        private Service Service {
            get 
            {
                return _service ?? (_service = new Service());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Vilken bild som ska visas i stort format
            string currentImage = Request.QueryString["image"];

            //Om det finns en fil som motsvarar filnamnet - visa
            if (Model.Gallery.ImageExists(currentImage))
            {
                CurrentImage.ImageUrl = "GalleryImages/" + currentImage;
            }
        }

        //Bilduppladdning
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            var fileName = ImageFileUpload.FileName;

            var fileContent = ImageFileUpload.FileContent;

            //Försök spara bilden
            try
            {
                string result = Model.Gallery.SaveImage(fileContent, fileName);
                SuccessPanel.Visible = true;
            }
            catch (ArgumentException ex)
            {
                ModelValidator.IsValid = false;
                FailPanel.Visible = true;

                //Skriv enbart ut meddelanden för egenhändigt kastade undantag (borde varit en customexception för att inte fånga andra fel)
                if (ex is ArgumentException)
                {
                    FailLiteral.Text = ", " + ex.Message;
                }
                else
                {
                    FailLiteral.Text = "Något gick snett";
                }
            }
        }

        //Metod som kopplas till listview för att visa bildlista
        public IEnumerable<GalleryItem> SavedImages_GetThumbs()
        {
            return Service.GetImageItems(); //Service.GetCachedImageItems();
        }

        public string isActive(string fileName)
        {
            if (fileName == Request.QueryString["image"])
            {
                return "class='active'";
            }
            return null;
        }
    }
}