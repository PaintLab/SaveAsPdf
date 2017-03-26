//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet
{
    using System.IO;
    using Fonet.Pdf;
    public class CorePdfRenderer
    {  
        PdfCreator pdfCreator;
        public CorePdfRenderer(PdfCreator pdfCreator)
        {
            this.pdfCreator = pdfCreator;
        }
    }
}