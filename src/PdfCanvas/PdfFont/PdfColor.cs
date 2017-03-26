//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using System.Globalization;
using System.Text;

namespace Fonet
{
    public struct PdfColor
    {
        //1. should be struct ?
        //2. convert to float?
        private float red;
        private float green;
        private float blue;
        public PdfColor(float red, float green, float blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        // components from 0 to 255
        public PdfColor(int red, int green, int blue) : this(
            ((float)red) / 255f,
            ((float)green) / 255f,
            ((float)blue) / 255f
            )
        { }

        public float getRed()
        {
            return (this.red);
        }

        public float getGreen()
        {
            return (this.green);
        }

        public float getBlue()
        {
            return (this.blue);
        }
        public bool IsEq(PdfColor another)
        {
            return this.red == another.red
                && this.green == another.green
                && this.blue == another.blue;
        }
        public string getColorSpaceOut(bool fillNotStroke)
        {
            StringBuilder p = new StringBuilder();

            // according to pdfspec 12.1 p.399
            // if the colors are the same then just use the g or G operator
            bool same = false;
            if (this.red == this.green && this.red == this.blue)
            {
                same = true;
            }

            // output RGB
            if (fillNotStroke)
            {
                if (same)
                {
                    p.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0:0.0####} g\n",
                        this.red);
                }
                else
                {
                    p.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0:0.0####} {1:0.0####} {2:0.0####} rg\n",
                        this.red, this.green, this.blue);
                }
            }
            else
            {
                if (same)
                {
                    p.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0:0.0####} G\n",
                        this.red);
                }
                else
                {
                    p.AppendFormat(
                        CultureInfo.InvariantCulture,
                        "{0:0.0####} {1:0.0####} {2:0.0####} RG\n",
                        this.red, this.green, this.blue);
                }
            }

            return p.ToString();
        }
    }
}