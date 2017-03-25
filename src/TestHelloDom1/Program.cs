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
            MyPdfPage page = pdfdoc.CreatePage();
            pdfdoc.Pages.Add(page);
            //
            MyPdfCanvas canvas = page.Canvas;
            canvas.DrawString("Hello World!, from PixelFarm", 20, 20);
            //
            FonetDriver driver = FonetDriver.Make();
            driver.ImageHandler += str =>
            {
                return null;
            };

            driver.Render(pdfdoc, "hello.pdf");
        }
    }
}