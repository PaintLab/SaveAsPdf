//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Fonet.Pdf
{
    public class PdfContentStream : PdfStream
    {
        protected MemoryStream stream;
        protected PdfWriter streamData;

        static Encoding s_defaultEnc = Encoding.UTF8;
        public PdfContentStream(PdfObjectId objectId)
            : base(objectId)
        {
            this.stream = new MemoryStream();
            this.streamData = new PdfWriter(stream);
        }

        public void WriteLine(PdfObject obj)
        {
            Debug.Assert(obj != null);
            if (obj.IsIndirect || obj is PdfObjectReference)
            {
                throw new ArgumentException("Cannot write indirect PdfObject", "obj");
            }
            streamData.WriteLine(obj);
        }



        internal void InnerWrite(string s)
        {
            streamData.Write(s_defaultEnc.GetBytes(s));
        }
        public void SetFont(string fontname, int size)
        {
            InnerWrite("/" + fontname + " " +
                   PdfNumber.doubleOut(size / 1000f) + " Tf\n");
        }
        public void SetLetterSpacing(float letterspacing)
        {
            InnerWrite(PdfNumber.doubleOut(letterspacing) + " Tc\n");
        }

        public void SetFontColor(PdfColor c)
        {
            InnerWrite(c.getColorSpaceOut(true));
        }
        public void CloseText()
        {
            InnerWrite("] TJ\n");
        }
        public void BeginTextObject()
        {
            InnerWrite("BT\n");
        }
        public void EndTextObject()
        {
            InnerWrite("ET\n");
        }
        public void SaveGraphicsState()
        {
            InnerWrite("q\n");
        }
        public void RestoreGraphicsState()
        {
            InnerWrite("Q\n");
        }
        public void DrawLine(float x1, float y1, float x2, float y2, float th, PdfColor stroke)
        {
            InnerWrite("ET\nq\n" + stroke.getColorSpaceOut(false)
            + PdfNumber.doubleOut(x1 / 1000f) + " " + PdfNumber.doubleOut(y1 / 1000f) + " m "
            + PdfNumber.doubleOut(x2 / 1000f) + " " + PdfNumber.doubleOut(y2 / 1000f) + " l "
            + PdfNumber.doubleOut(th / 1000f) + " w S\n" + "Q\nBT\n");
        }
        public void DrawLine(float x1, float y1, float x2, float y2, float th, RuleStyle rs, PdfColor stroke)
        {
            InnerWrite("ET\nq\n" + stroke.getColorSpaceOut(false)
               + SetRuleStylePattern(rs) + PdfNumber.doubleOut(x1 / 1000f) + " "
               + PdfNumber.doubleOut(y1 / 1000f) + " m " + PdfNumber.doubleOut(x2 / 1000f) + " "
               + PdfNumber.doubleOut(y2 / 1000f) + " l " + PdfNumber.doubleOut(th / 1000f) + " w S\n"
               + "Q\nBT\n");
        }
        static String SetRuleStylePattern(RuleStyle style)
        {
            string rs = "";
            switch (style)
            {
                case RuleStyle.SOLID:
                    rs = "[] 0 d ";
                    break;
                case RuleStyle.DASHED:
                    rs = "[3 3] 0 d ";
                    break;
                case RuleStyle.DOTTED:
                    rs = "[1 3] 0 d ";
                    break;
                case RuleStyle.DOUBLE:
                    rs = "[] 0 d ";
                    break;
                default:
                    rs = "[] 0 d ";
                    break;
            }
            return rs;
        }
        public void DrawRect(float x, float y, float w, float h, PdfColor stroke)
        {
            InnerWrite("ET\nq\n" + stroke.getColorSpaceOut(false)
               + PdfNumber.doubleOut(x / 1000f) + " " + PdfNumber.doubleOut(y / 1000f) + " "
               + PdfNumber.doubleOut(w / 1000f) + " " + PdfNumber.doubleOut(h / 1000f) + " re s\n"
               + "Q\nBT\n");
        }
        public void FillRect(float x, float y, float w, float h, PdfColor fill)
        {
            InnerWrite("ET\nq\n" + fill.getColorSpaceOut(true)
             + PdfNumber.doubleOut(x / 1000f) + " " + PdfNumber.doubleOut(y / 1000f) + " "
             + PdfNumber.doubleOut(w / 1000f) + " " + PdfNumber.doubleOut(h / 1000f) + " re f\n"
             + "Q\nBT\n");
        }
        public void DrawAndFillRect(float x, float y, float w, float h, PdfColor stroke, PdfColor fill)
        {
            InnerWrite("ET\nq\n" + fill.getColorSpaceOut(true)
               + stroke.getColorSpaceOut(false) + PdfNumber.doubleOut(x / 1000f)
               + " " + PdfNumber.doubleOut(y / 1000f) + " " + PdfNumber.doubleOut(w / 1000f) + " "
               + PdfNumber.doubleOut(h / 1000f) + " re b\n" + "Q\nBT\n");
        }
        public void FillImage(float x, float y, float w, float h, PdfXObject xobj)
        {
            InnerWrite("ET\nq\n" + PdfNumber.doubleOut(((float)w) / 1000f) + " 0 0 "
                  + PdfNumber.doubleOut(((float)h) / 1000f) + " "
                  + PdfNumber.doubleOut(((float)x) / 1000f) + " "
                  + PdfNumber.doubleOut(((float)(y - h)) / 1000f) + " cm\n" + "/" + xobj.Name.Name
                  + " Do\nQ\nBT\n");
        }

        public void ClipImage(float cx1, float cy1, float cx2, float cy2,
            float imgX, float imgY, float imgW, float imgH, PdfXObject xobj)
        {
            InnerWrite("ET\nq\n" +
                // clipping
                PdfNumber.doubleOut(cx1) + " " + PdfNumber.doubleOut(cy1) + " m\n" +
                PdfNumber.doubleOut(cx2) + " " + PdfNumber.doubleOut(cy1) + " l\n" +
                PdfNumber.doubleOut(cx2) + " " + PdfNumber.doubleOut(cy2) + " l\n" +
                PdfNumber.doubleOut(cx1) + " " + PdfNumber.doubleOut(cy2) + " l\n" +
                "W\n" +
                "n\n" +
                // image matrix
                PdfNumber.doubleOut(((float)imgW) / 1000f) + " 0 0 " +
                PdfNumber.doubleOut(((float)imgH) / 1000f) + " " +
                PdfNumber.doubleOut(((float)imgX) / 1000f) + " " +
                PdfNumber.doubleOut(((float)imgY - imgH) / 1000f) + " cm\n" +
                "s\n" +
                // the image itself
                "/" + xobj.Name.Name + " Do\nQ\nBT\n");
        }
        internal void WriteLine(string s)
        {
            streamData.WriteLine(s_defaultEnc.GetBytes(s));
        }

        protected internal override void Write(PdfWriter writer)
        {
            data = stream.ToArray();
            base.Write(writer);
        }
    }
}