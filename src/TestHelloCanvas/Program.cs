using System;
using Fonet.Pdf;
using System.IO;
namespace TestHelloCanvas
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string outputFilename = "bin\\output.pdf";
            using (FileStream outputStream = new FileStream(outputFilename, FileMode.Create, FileAccess.Write))
            {
                //----------------
                PdfCreator pdfCreator = new PdfCreator(outputStream);

                PdfResources pdfRes = pdfCreator.GetResources();
                PdfContentStream contentStream = pdfCreator.MakeContentStream();
                //----------------
                Fonet.Layout.FontInfo fontInfo = new Fonet.Layout.FontInfo();
                var fontSetup = new Fonet.Render.Pdf.FontSetup(
                        fontInfo, Fonet.Render.Pdf.FontType.Link);

                Fonet.Layout.FontState fontState = new Fonet.Layout.FontState(
                    fontInfo, "sans-serif", "normal",
                    "normal", 18000,
                    52);
                //----------------
                pdfCreator.OutputHeader();
                //----------------

                //simple page
                contentStream.BeginTextObject();
                contentStream.SetFont("F1", 18 * 1000);
                contentStream.SetFontColor(new Fonet.PdfColor(0, 0, 0));
                //----------------
                Fonet.Layout.TextPrinter textPrinter = new Fonet.Layout.TextPrinter();

                for (int i = 0; i < 5; ++i)
                {
                    textPrinter.Reset(fontState, false);
                    textPrinter.SetTextPos((i * 100) * 1000, (i * 100) * 1000);
                    textPrinter.WriteText("Hello World! " + i);
                    textPrinter.PrintContentTo(contentStream);
                    contentStream.CloseText();
                }
                //------------


                contentStream.DrawLine(0 * 1000, 0 * 1000, 70 * 1000, 50 * 1000,
                    1 * 1000,
                    new Fonet.PdfColor(255, 0, 0));


                contentStream.EndTextObject();
                //---------------- 
                int w = 800;
                int h = 600;
                PdfPage page = pdfCreator.MakePage(pdfRes, contentStream,
                    w,
                    h,
                    new string[0]);


                fontSetup.AddToResources(new PdfFontCreator(pdfCreator), pdfRes);
                pdfCreator.OutputTrailer();
                //----------------
                outputStream.Flush();
                outputStream.Close();
            }
        }
    }
}