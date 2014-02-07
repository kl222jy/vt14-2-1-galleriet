using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace _2_1_Galleriet.Model
{
    public class Gallery
    {
        private static readonly Regex ApprovedExtensions;
        private static readonly string PhysicalUploadedImagesPath;
        private static readonly Regex SanitizePath;

        ////Hade det inte varit lämpligare med egenskaper?
        //private static Regex ApprovedExtensions 
        //{ 
        //    get 
        //    {
        //        var approved = new Regex("^.*\.(gif|jpg|png)$");
        //        return approved;
        //    } 
        //}
        //private static string PhysicalUploadedImagesPath
        //{
        //    get
        //    {
        //        var BasePath = AppDomain.CurrentDomain.GetData("APPBASE").ToString();
        //        return Path.Combine(BasePath, "GalleryImages");
        //    }
        //}
        //private static Regex SanitizePath
        //{
        //    get
        //    {
        //        var invalidChars = new string(Path.GetInvalidFileNameChars());
        //        return new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
        //    }
        //}

        static Gallery() 
        {
            ApprovedExtensions = new Regex("^.*\\.(gif|jpg|png)$");

            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "GalleryImages");

            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
        }

        public static IEnumerable<string> GetImageNames() 
        {
            var di = new DirectoryInfo(PhysicalUploadedImagesPath);
            var files = di.EnumerateFiles();
            var imageNames = new List<string>(10);

            foreach (var file in files)
	        {
                if (ApprovedExtensions.IsMatch(file.Name))
	            {
		             imageNames.Add(file.Name);
	            }
	        }

            return imageNames.AsReadOnly();
        }

        public static bool ImageExists(string name) 
        {
            if (GetImageNames().Contains(name))
	        {
		        return true;
	        }
            else
	        {
                return false;
	        }
        }

        private static bool IsValidImage(Image image)
        {
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid || image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string SaveImage(Stream stream, string fileName)
        {
            if (ImageExists(fileName))
            {
                return "Filnamnet används redan.";
                //throw new ApplicationException("Filnamnet används redan.");         //TODO: lägg till hantering
            }

            Image tempImage = System.Drawing.Image.FromStream(stream);
            Image tempThumb = tempImage.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);

            if (!IsValidImage(tempImage))
            {
                return "Bildfilen accepteras inte av servern.";
                //throw new ApplicationException("Filen är inte en giltig bild.");
            }

            try
            {
                tempImage.Save(Path.Combine(PhysicalUploadedImagesPath, fileName));
                tempThumb.Save(Path.Combine(PhysicalUploadedImagesPath, "thumb_" + fileName));
            }
            catch (Exception)
            {
                return "Något gick snett när bilden skulle sparas.";
            }

            return "Uppladdningen lyckades.";
        }

        //public struct ImageItem 
        //{
        //    public string thumburl;
        //    public string imageurl;
        //    public string href;
        //}

        public class ImageItem
        {
            public string thumburl;
            public string imageurl;
            public string href;
        }

        public static IEnumerable<ImageItem> GetImageItems
        {
            get
            {
                var images = new List<ImageItem>(10);
                foreach (string fileName in GetImageNames())
                {
                    var isThumb = new Regex("^(thumb_).*");
                    if (!isThumb.IsMatch(fileName))
                    {
                        ImageItem item = new ImageItem();
                        item.href = "/?image=" + fileName;
                        item.imageurl = "GalleryImages/" + fileName;
                        item.thumburl = "GalleryImages/thumb_" + fileName;
                        images.Add(item);
                    }
                }
                return images;
            }
        }
    }
}