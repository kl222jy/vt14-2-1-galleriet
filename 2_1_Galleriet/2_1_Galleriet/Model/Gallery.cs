using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace _2_1_Galleriet.Model
{
    public class Gallery
    {
        private static readonly Regex ApprovedExtensions;
        private static readonly string PhysicalUploadedImagesPath;
        private static readonly Regex SanitizePath;

        ////Varför inte egenskaper?
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
            ApprovedExtensions = new Regex(@"^.*\.(gif|jpg|png)$");

            PhysicalUploadedImagesPath = Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), "GalleryImages");

            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
        }

        //Hämtar en lista med samtliga sparade bilder
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

        //Kontrollerar om filens format är tillåtet
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
            //Tar bort otillåtna tecken från filnamnet
            fileName = SanitizePath.Replace(fileName, "");

            //Borde aldrig inträffa, men om samtliga tecken städats bort bör filen inte sparas
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "filnamnet är tomt eller innehöll bara ogiltiga tecken";
            }


            //if (ImageExists(fileName))
            //{
            //    return "filnamnet används redan.";
            //}

            var isNumber = new Regex(@"\d");

            //Kontrollera att filnamnet inte redan existerar..
            while (ImageExists(fileName))
            {
                var sb = new StringBuilder(fileName);

                //Kontrollera om sista tecken innan filändelse är en siffra, öka isf med 1
                if (isNumber.IsMatch(fileName.Substring(fileName.Length - 5, 1)))
                {
                    int i = int.Parse(fileName.Substring(fileName.Length - 5, 1));
                    i += 1;

                    sb.Remove(fileName.Length - 5, 1);
                    sb.Insert(fileName.Length - 5, i);
                }
                //Lägg annars till numrering
                else
                {
                    sb.Insert(fileName.Length - 4, 1);
                }

                fileName = sb.ToString();
            }



            //Bild från stream
            Image tempImage = System.Drawing.Image.FromStream(stream);
            //Thumb från bild
            Image tempThumb = tempImage.GetThumbnailImage(60, 45, null, System.IntPtr.Zero);

            //Kontrollera att bilden har ett tillåtet format
            if (!IsValidImage(tempImage))
            {
                return "bildfilen accepteras inte av servern.";
            }

            try
            {
                tempImage.Save(Path.Combine(PhysicalUploadedImagesPath, fileName));
                tempThumb.Save(Path.Combine(PhysicalUploadedImagesPath, "thumb_" + fileName));
            }
            catch (Exception)
            {
                return "något gick snett när bilden skulle sparas.";
            }

            return "Uppladdningen lyckades.";
        }

        public struct ImageItem
        {
            public string thumburl;
            public string imageurl;
            public string href;
        }

        //Returnerar alla bilder som ImageItem för smidig hantering
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