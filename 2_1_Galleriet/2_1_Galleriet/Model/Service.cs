using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace _2_1_Galleriet.Model
{
    public class Service
    {
        //Hämtning av lista med bildinformation
        public IEnumerable<GalleryItem> GetImageItems()
        {
            var images = new List<GalleryItem>(10);
            foreach (string fileName in Gallery.GetImageNames())
            {
                var isThumb = new Regex("^(thumb_).*");
                if (!isThumb.IsMatch(fileName))             //Välj enbart filer som inte är thumb (undvika dubletter)
                {
                    GalleryItem item = new GalleryItem();
                    item.href = "/?image=" + fileName;
                    item.imageurl = "GalleryImages/" + fileName;
                    item.thumburl = "GalleryImages/thumb_" + fileName;
                    item.fileName = fileName;
                    images.Add(item);
                }
            }
            return images;
        }

        //Hämtning av cachead data, används inte eftersom jag inte vet riktigt hur jag ska göra objektjämförelsen för att få ny lista när ny bild laddats upp
        public IEnumerable<GalleryItem> GetCachedImageItems()
        {
            var images = HttpContext.Current.Cache["images"] as IEnumerable<GalleryItem>;
            if (images == null)
            {
                images = GetImageItems();
                HttpContext.Current.Cache.Insert("images", images);
            }
            return images;
        }
    }
}