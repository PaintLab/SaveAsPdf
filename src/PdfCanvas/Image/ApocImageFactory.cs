//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Image
{
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Security;

    /// <summary>
    /// Creates FonetImage instances.
    /// </summary>
    public class FonetImageFactory
    {

        //internal static FonetImage MakeFromResource(string key)
        //{

        //    Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(key); 
        //    byte[] buffer = new byte[s.Length];
        //    s.Read(buffer, 0, buffer.Length);

        //    return new FonetImage("file://" + key, buffer);
        //}

        /// <summary>
        ///     Creates a FonetImage from the supplied resource locator.  The 
        ///     FonetImageFactory does cache images, therefore this method may 
        ///     return a reference to an existing FonetImage
        /// </summary>
        /// <param name="href">A Uniform Resource Identifier</param>
        /// <returns>A reference to a  FonetImage</returns>
        /// <exception cref="FonetImageException"></exception>
        public static FonetImage Make(string href)
        {
            return PdfCreatorBridge.LoadImage(href);

        }

        //private static Stream GetImageStream(Uri uri)
        //{
        //    try
        //    {
        //        WebRequest request = WebRequest.CreateDefault(uri);

        //        // Apply user specified timeout.
        //        request.Timeout = FonetDriver.ActiveDriver.Timeout;

        //        // Apply authentication credentials.
        //        request.Credentials = FonetDriver.ActiveDriver.Credentials;

        //        WebResponse response = request.GetResponse();

        //        return response.GetResponseStream();
        //    }
        //    catch (SecurityException se)
        //    {
        //        throw new FonetImageException(
        //            String.Format("Detected security exception while fetching image from {0}: {1}", uri, se.Message));
        //    }
        //    catch (UriFormatException ue)
        //    {
        //        throw new FonetImageException(
        //            String.Format("Badly formed Uri {0}: {1}", uri, ue.Message));
        //    }
        //    catch (WebException we)
        //    {
        //        throw new FonetImageException(
        //            String.Format("Encountered web exception while fetching image from {0}: {1}", uri, we.Message));
        //    }
        //    catch (Exception e)
        //    {
        //        throw new FonetImageException(
        //            String.Format("Encountered unexpected exception while fetching image from {0}: {1}", uri, e.Message));
        //    }

        //}

        //private static byte[] ExtractImageData(Uri absoluteURL)
        //{
        //    // Otherwise load the image data using a WebRequest.
        //    Stream imageStream = GetImageStream(absoluteURL);

        //    // Read the data stream into a byte array.
        //    try
        //    {
        //        MemoryStream ms = new MemoryStream();
        //        byte[] buf = new byte[4096];
        //        int numBytesRead = 0;

        //        // Read contents of JPEG into MemoryStream
        //        while ((numBytesRead = imageStream.Read(buf, 0, 4096)) != 0)
        //        {
        //            ms.Write(buf, 0, numBytesRead);
        //        }

        //        ms.Flush();
        //        ms.Close();

        //        return ms.ToArray();

        //    }
        //    finally
        //    {
        //        imageStream.Flush();
        //        imageStream.Close();
        //        imageStream = null;
        //    }
        //}
    }
}