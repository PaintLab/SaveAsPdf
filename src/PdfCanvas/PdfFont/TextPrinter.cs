//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Layout
{
    using System;
    using System.Collections;
    using System.Text;
    using Fonet.Pdf.Gdi;//TODO review here => Gdi, 


    public class TextPrinter
    {
        char startText;
        char endText;
        StringBuilder _tmpstBuilder;
        bool _useMultibyte;
        float _x, _y;
        bool _useEmDiff;
        float _emDiff;
        FontState _fontState;
        Fonet.Render.Pdf.Fonts.Font _font;
        bool kerningAvailable;
        public TextPrinter()
        {
            _tmpstBuilder = new StringBuilder();
        }
        public void SetTextPos(float x, float y)
        {
            _x = x;
            _y = y;
            _useEmDiff = false;
        }
        public void SetEmDiff(float emDiff)
        {
            _useEmDiff = true;
            _emDiff = emDiff;
        }

        public void Reset(FontState fontState, bool useKerning)
        {

            // This assumes that *all* CIDFonts use a /ToUnicode mapping
            //-----------------------------------------------
            _fontState = fontState;
            String name = fontState.FontName;
            _font = (Fonet.Render.Pdf.Fonts.Font)fontState.FontInfo.GetFontByName(name);
            kerningAvailable = false;
            kerning = null;
            _emDiff = _x = _y = 0;
            _useEmDiff = false;
            _tmpstBuilder.Length = 0;
            //-----------------------------------------------
            if (_useMultibyte = _font.MultiByteFont)
            {
                startText = '<';
                endText = '>';
            }
            else
            {
                startText = '(';
                endText = ')';
            }
            _tmpstBuilder.Length = 0;
            //--
            //kerning
            //TODO: add OpenType Layout Features

            // If no options are supplied, by default we do not enable kerning

            if (useKerning)
            {
                kerning = fontState.Kerning;
                if (kerning != null && (kerning.Count > 0))
                {
                    kerningAvailable = true;
                }
            }
        }
        GdiKerningPairs kerning;
        public void WriteText(string text)
        {
            char[] charBuffer = text.ToCharArray();
            WriteText(charBuffer, 0, charBuffer.Length);
        }
        public void WriteText(char[] textBuffer, int start, int len)
        {

            for (int index = start; index < len; ++index)
            {
                //get glyph index from current font?
                ushort ch = _fontState.MapCharacter(textBuffer[index]);

                if (!_useMultibyte)
                {
                    if (ch > 127)
                    {
                        _tmpstBuilder.Append("\\");
                        _tmpstBuilder.Append(Convert.ToString((int)ch, 8));

                    }
                    else
                    {
                        switch (ch)
                        {
                            case '(':
                            case ')':
                            case '\\':
                                _tmpstBuilder.Append("\\");
                                break;
                        }
                        _tmpstBuilder.Append((char)ch);
                    }
                }
                else
                {
                    _tmpstBuilder.Append(GetUnicodeString(ch));
                }

                if (kerningAvailable && (index + 1) < len)
                {
                    ushort ch2 = _fontState.MapCharacter(textBuffer[index + 1]);
                    AddKerning(_tmpstBuilder, ch, ch2, kerning, startText, endText);
                }
            }
        }
        /**
   * Convert a char to a multibyte hex representation
   */

        private String GetUnicodeString(ushort c)
        {
            StringBuilder sb = new StringBuilder(4);

            byte[] uniBytes = Encoding.BigEndianUnicode.GetBytes(new char[] { (char)c });

            foreach (byte b in uniBytes)
            {
                string hexString = Convert.ToString(b, 16);
                if (hexString.Length == 1)
                {
                    sb.Append("0");
                }
                sb.Append(hexString);
            }

            return sb.ToString();

        }
        private void AddKerning(StringBuilder buf, ushort leftIndex, ushort rightIndex,
                               GdiKerningPairs kerning, char startText, char endText)
        {
            if (kerning.HasPair(leftIndex, rightIndex))
            {
                int width = kerning[leftIndex, rightIndex];
                buf.Append(endText).Append(-width).Append(' ').Append(startText);
            }
        }
        public void PrintContentTo(Fonet.Pdf.PdfContentStream contentStream)
        {

            if (_useEmDiff)
            {
                //-------
                contentStream.InnerWrite(PdfNumber.doubleOut(_emDiff));
                contentStream.InnerWrite(" ");
            }
            else
            {
                contentStream.InnerWrite("1 0 0 1 " + PdfNumber.doubleOut(_x / 1000f) +
                    " " + PdfNumber.doubleOut(_y / 1000f) + " Tm ["); //left open?
            }
            //-------------------------------
            contentStream.InnerWrite(startText.ToString());
            contentStream.InnerWrite(_tmpstBuilder.ToString());
            contentStream.InnerWrite(endText.ToString());
            //-------------------------------
        }

    }

}