using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _2_1_Galleriet
{
    public partial class _Default : Page
    {
        //private Model.Gallery gallery = new Model.Gallery();

        protected void Page_Load(object sender, EventArgs e)
        {
            string currentImage = Request.QueryString["image"];

            if (Model.Gallery.ImageExists(currentImage))
            {
                CurrentImage.ImageUrl = "GalleryImages/" + currentImage;
            }

            //SavedImages.DataSource = Model.Gallery.GetImageNames();
            //IEnumerable<Model.Gallery.ImageItem> thumbs = new List<Model.Gallery.ImageItem>(10);
            //thumbs = Model.Gallery.GetImageItems;
            //SavedImages.DataSource = thumbs;
            //SavedImages.DataBind();
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {

            var fileName = ImageFileUpload.FileName;
            var fileContent = ImageFileUpload.FileContent;

            string result = Model.Gallery.SaveImage(fileContent, fileName);

            if (result == "Uppladdningen lyckades")
            {
                //visa bilden och ett ok meddelande
            }
            else
            {
                ModelValidator.IsValid = false;
                ModelValidator.ErrorMessage = result;
            }
        }

        public IEnumerable<Model.Gallery.ImageItem> SavedImages_GetThumbs()
        {
            return Model.Gallery.GetImageItems;
        }
    }
}