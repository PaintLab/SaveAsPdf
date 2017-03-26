//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET

using System.IO;
using Fonet;
using PixelFarm.Drawing.Pdf;
namespace FonetExample
{
    //hello dom sample
    class HelloWorld
    {
        static void Main(string[] args)
        {
            MyPdfDocument pdfdoc = new MyPdfDocument();

            //1. create new page and append to doc
            MyPdfPage page = pdfdoc.CreatePage();
            pdfdoc.Pages.Add(page);
            //2. set page property 
            MyPdfCanvas canvas = page.Canvas;
            canvas.DrawString("ABCD12345", 20, 20);
            //
            FonetDriver driver = FonetDriver.Make();
            driver.ImageHandler += str =>
            {
                return null;
            };

            string outputFilename = "bin\\output.pdf";
            using (FileStream outputStream = new FileStream(outputFilename, FileMode.Create, FileAccess.Write))
            {
                driver.Render(pdfdoc, outputStream);
                outputStream.Flush();
                outputStream.Close();
            }

        }
    }
}