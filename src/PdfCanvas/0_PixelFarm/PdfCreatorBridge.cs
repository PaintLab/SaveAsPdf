//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET

using System;
using Fonet.Image;
namespace Fonet
{
    public delegate FonetImage LoadImageDelegate(string href);

    public static class PdfCreatorBridge
    {
        static LoadImageDelegate s_loadImg;

        public static void SetLoadImageDelegate(LoadImageDelegate loadImg)
        {
            s_loadImg = loadImg;
        }
        public static void Warning(string str)
        {

        }
        public static void Msg(string str) { }
        public static void Error(string str)
        {
            throw new NotSupportedException(str);
        }
        public static FonetImage LoadImage(string href)
        {
            return s_loadImg(href);

            // If an image handler has been registered on the driver, then
            // give it a chance to handle the loading of image data.

            //Uri absoluteURL = null;
            //UriSpecificationParser up = new UriSpecificationParser(href);
            //string path = up.Uri;

            //try
            //{
            //    absoluteURL = new Uri(path);
            //}
            //catch
            //{
            //    // If the href contains only a path then file is assumed
            //    if (File.Exists(path))
            //    {
            //        absoluteURL = new Uri("file://" + Path.Combine(Directory.GetCurrentDirectory(), path));

            //    }
            //    else
            //    {
            //        // Examine base directory which is specified by the user via the 
            //        // FonetDriver.BaseDirectory property
            //        string baseDir = FonetDriver.ActiveDriver.BaseDirectory.FullName;

            //        string baseDirPath = Path.Combine(baseDir, path);
            //        if (File.Exists(baseDirPath))
            //        {
            //            absoluteURL = new Uri("file://" + Path.Combine(Directory.GetCurrentDirectory(), baseDirPath));

            //        }
            //        else
            //        {
            //            throw new FonetImageException("Unable to retrieve graphic from " + path);
            //        }
            //    }
            //}

            //return new FonetImage(
            //    absoluteURL.AbsoluteUri,
            //    ExtractImageData(absoluteURL));


        }
    }
}