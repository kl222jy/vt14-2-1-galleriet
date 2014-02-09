﻿using System;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //Vilken bild som ska visas i stort format
            string currentImage = Request.QueryString["image"];

            //Om det finns en fil som motsvarar filnamnet - visa
            if (Model.Gallery.ImageExists(currentImage))
            {
                CurrentImage.ImageUrl = "GalleryImages/" + currentImage;
            }

            //Vad som ska visas i bildlistan, sätts här för att förändringar ska visas.
            SavedImages.SelectMethod = "SavedImages_GetThumbs";
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
            catch (Exception ex)
            {
                ModelValidator.IsValid = false;
                FailPanel.Visible = true;
                FailLiteral.Text = ", " + ex.Message;
            }
 
        }

        //Metod som kopplas till listview för att visa bildlista
        public IEnumerable<Model.Gallery.ImageItem> SavedImages_GetThumbs()
        {
            return Model.Gallery.GetImageItems;
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