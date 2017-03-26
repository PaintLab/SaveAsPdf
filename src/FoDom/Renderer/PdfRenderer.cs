//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using System;
using System.Collections;
using System.IO;
using System.Text;
using Fonet.DataTypes;
using Fonet.Fo.Properties;
using Fonet.Image;
using Fonet.Layout;
using Fonet.Layout.Inline;
using Fonet.Pdf;
using Fonet.Render.Pdf.Fonts;
using Fonet.Pdf.Gdi;

namespace Fonet.Render.Pdf
{

    internal sealed class PdfRenderer
    {


        /// <summary>
        ///     The current vertical position in millipoints from bottom.
        /// </summary>
        private int currentYPosition = 0;

        /// <summary>
        ///     The current horizontal position in millipoints from left.
        /// </summary>
        private int currentXPosition = 0;

        /// <summary>
        ///     The horizontal position of the current area container.
        /// </summary>
        private int currentAreaContainerXPosition = 0;

        /// <summary>
        ///     The PDF Document being created.
        /// </summary>
        private PdfCreator pdfCreator;

        /// <summary>
        ///     The /Resources object of the PDF document being created.
        /// </summary>
        private PdfResources pdfResources;

        /// <summary>
        ///     The current stream to add PDF commands to.
        /// </summary>
        private PdfContentStream currentStream;

        /// <summary>
        ///     The current annotation list to add annotations to.
        /// </summary>
        private PdfAnnotList currentAnnotList;

        /// <summary>
        ///     The current page to add annotations to.
        /// </summary>
        private PdfPage currentPage;

        private float currentLetterSpacing = Single.NaN;

        /// <summary>
        ///     True if a TJ command is left to be written.
        /// </summary>
        private bool textOpen = false;

        /// <summary>
        ///     The previous Y coordinate of the last word written.
        /// </summary>
        /// <remarks>
        ///     Used to decide if we can draw the next word on the same line.
        /// </remarks>
        private int prevWordY = 0;

        /// <summary>
        ///     The previous X coordinate of the last word written.
        /// </summary>
        /// <remarks>
        ///     Used to calculate how much space between two words.
        /// </remarks>
        private int prevWordX = 0;

        /// <summary>
        /// The  width of the previous word.
        /// </summary>
        /// <remarks>
        ///     Used to calculate space between.
        /// </remarks>
        private int prevWordWidth = 0;



        /// <summary>
        ///     User specified rendering options.
        /// </summary>
        private PdfRendererOptions options;

        /// <summary>
        ///     The current (internal) font name.
        /// </summary>
        private string currentFontName;

        /// <summary>
        ///     The current font size in millipoints.
        /// </summary>
        private int currentFontSize;

        /// <summary>
        ///     The current color/gradient to fill shapes with.
        /// </summary>
        private PdfColor? currentFill = null;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevUnderlineXEndPos;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevUnderlineYEndPos;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevUnderlineSize;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private PdfColor? prevUnderlineColor;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevOverlineXEndPos;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevOverlineYEndPos;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevOverlineSize;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private PdfColor? prevOverlineColor;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevLineThroughXEndPos;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevLineThroughYEndPos;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private int prevLineThroughSize;

        /// <summary>
        ///     Previous values used for text-decoration drawing.
        /// </summary>
        private PdfColor? prevLineThroughColor;

        /// <summary>
        ///     Provides triplet to font resolution.
        /// </summary>
        private FontInfo fontInfo;

        /// <summary>
        ///     Handles adding base 14 and all system fonts.
        /// </summary>
        private FontSetup fontSetup;

        /// <summary>
        ///     The IDReferences for this document.
        /// </summary>
        private IDReferences idReferences;

        /// <summary>
        ///     Create the PDF renderer.
        /// </summary>
        internal PdfRenderer(Stream stream)
        {
            this.pdfCreator = new PdfCreator(stream);
        }

        /// <summary>
        ///     Assigns renderer options to this PdfRenderer
        /// </summary>
        /// <remarks>
        ///     This property will only accept an instance of the PdfRendererOptions class
        /// </remarks>
        /// <exception cref="ArgumentException">
        ///     If <i>value</i> is not an instance of PdfRendererOptions
        /// </exception>
        public PdfRendererOptions Options
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (!(value is PdfRendererOptions))
                {
                    throw new ArgumentException("Options must be an instance of PdfRendererOptions");
                }

                // Guaranteed to work because of above check
                options = value as PdfRendererOptions;
            }
        }

        public void StartRenderer()
        {
            if (options != null)
            {
                pdfCreator.SetOptions(options);
            }
            pdfCreator.OutputHeader();
        }

        public void StopRenderer()
        {
            fontSetup.AddToResources(new PdfFontCreator(pdfCreator), pdfCreator.GetResources());
            pdfCreator.OutputTrailer();

            pdfCreator = null;
            pdfResources = null;
            currentStream = null;
            currentAnnotList = null;
            currentPage = null;

            idReferences = null;
            currentFontName = String.Empty;
            currentFill = null;
            prevUnderlineColor = null;
            prevOverlineColor = null;
            prevLineThroughColor = null;
            fontSetup = null;
            fontInfo = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="fontInfo"></param>
        public void SetupFontInfo(FontInfo fontInfo)
        {
            this.fontInfo = fontInfo;
            this.fontSetup = new FontSetup(
                fontInfo, (options == null) ? FontType.Link : options.FontType);
        }

        public void RenderSpanArea(SpanArea area)
        {
            foreach (Box b in area.getChildren())
            {
                b.render(this); // column areas
            }

        }

        public void RenderBodyAreaContainer(BodyAreaContainer area)
        {
            int saveY = this.currentYPosition;
            int saveX = this.currentAreaContainerXPosition;

            if (area.getPosition() == Position.ABSOLUTE)
            {
                // Y position is computed assuming positive Y axis, adjust for negative postscript one
                this.currentYPosition = area.GetYPosition();
                this.currentAreaContainerXPosition = area.getXPosition();
            }
            else if (area.getPosition() == Position.RELATIVE)
            {
                this.currentYPosition -= area.GetYPosition();
                this.currentAreaContainerXPosition += area.getXPosition();
            }

            this.currentXPosition = this.currentAreaContainerXPosition;
            int rx = this.currentAreaContainerXPosition;
            int ry = this.currentYPosition;

            int w = area.getAllocationWidth();
            int h = area.getMaxHeight();

            DoBackground(area, rx, ry, w, h);

            // floats & footnotes stuff
            RenderAreaContainer(area.getBeforeFloatReferenceArea());
            RenderAreaContainer(area.getFootnoteReferenceArea());

            // main reference area
            foreach (Box b in area.getMainReferenceArea().getChildren())
            {
                b.render(this); // span areas
            }

            if (area.getPosition() != Position.STATIC)
            {
                this.currentYPosition = saveY;
                this.currentAreaContainerXPosition = saveX;
            }
            else
            {
                this.currentYPosition -= area.GetHeight();
            }

        }

        public void RenderAreaContainer(AreaContainer area)
        {
            int saveY = this.currentYPosition;
            int saveX = this.currentAreaContainerXPosition;

            if (area.getPosition() == Position.ABSOLUTE)
            {
                // XPosition and YPosition give the content rectangle position
                this.currentYPosition = area.GetYPosition();
                this.currentAreaContainerXPosition = area.getXPosition();
            }
            else if (area.getPosition() == Position.RELATIVE)
            {
                this.currentYPosition -= area.GetYPosition();
                this.currentAreaContainerXPosition += area.getXPosition();
            }
            else if (area.getPosition() == Position.STATIC)
            {
                this.currentYPosition -= area.getPaddingTop()
                    + area.getBorderTopWidth();
            }

            this.currentXPosition = this.currentAreaContainerXPosition;
            DoFrame(area);

            foreach (Box b in area.getChildren())
            {
                b.render(this);
            }

            // Restore previous origin
            this.currentYPosition = saveY;
            this.currentAreaContainerXPosition = saveX;
            if (area.getPosition() == Position.STATIC)
            {
                this.currentYPosition -= area.GetHeight();
            }
        }

        public void RenderBlockArea(BlockArea area)
        {
            // KLease: Temporary test to fix block positioning
            // Offset ypos by padding and border widths
            this.currentYPosition -= (area.getPaddingTop()
                + area.getBorderTopWidth());
            DoFrame(area);
            foreach (Box b in area.getChildren())
            {
                b.render(this);
            }
            this.currentYPosition -= (area.getPaddingBottom()
                + area.getBorderBottomWidth());
        }

        public void RenderLineArea(LineArea area)
        {
            int rx = this.currentAreaContainerXPosition + area.getStartIndent();
            int ry = this.currentYPosition;
            int w = area.getContentWidth();
            int h = area.GetHeight();

            this.currentYPosition -= area.getPlacementOffset();
            this.currentXPosition = rx;

            int bl = this.currentYPosition;

            foreach (Box b in area.getChildren())
            {
                if (b is InlineArea)
                {
                    InlineArea ia = (InlineArea)b;
                    this.currentYPosition = ry - ia.getYOffset();
                }
                else
                {
                    this.currentYPosition = ry - area.getPlacementOffset();
                }
                b.render(this);
            }

            this.currentYPosition = ry - h;
            this.currentXPosition = rx;
        }

        /**
        * add a line to the current stream
        *
        * @param x1 the start x location in millipoints
        * @param y1 the start y location in millipoints
        * @param x2 the end x location in millipoints
        * @param y2 the end y location in millipoints
        * @param th the thickness in millipoints
        * @param r the red component
        * @param g the green component
        * @param b the blue component
        */

        private void AddLine(int x1, int y1, int x2, int y2, int th,
                             PdfColor stroke)
        {
            CloseText();
            currentStream.DrawLine(x1, y1, x2, y2, th, stroke);
        }

        /**
        * add a line to the current stream
        *
        * @param x1 the start x location in millipoints
        * @param y1 the start y location in millipoints
        * @param x2 the end x location in millipoints
        * @param y2 the end y location in millipoints
        * @param th the thickness in millipoints
        * @param rs the rule style
        * @param r the red component
        * @param g the green component
        * @param b the blue component
        */

        private void AddLine(int x1, int y1, int x2, int y2, int th, RuleStyle rs,
                             PdfColor stroke)
        {
            CloseText();
            currentStream.DrawLine(x1, y1, x2, y2, th, rs, stroke);

        }

        /**
        * add a rectangle to the current stream
        *
        * @param x the x position of left edge in millipoints
        * @param y the y position of top edge in millipoints
        * @param w the width in millipoints
        * @param h the height in millipoints
        * @param stroke the stroke color/gradient
        */

        private void AddRect(int x, int y, int w, int h, PdfColor stroke)
        {
            CloseText();
            currentStream.DrawRect(x, y, w, h, stroke);
        }

        /**
        * add a filled rectangle to the current stream
        *
        * @param x the x position of left edge in millipoints
        * @param y the y position of top edge in millipoints
        * @param w the width in millipoints
        * @param h the height in millipoints
        * @param fill the fill color/gradient
        * @param stroke the stroke color/gradient
        */

        private void AddRect(int x, int y, int w, int h,
                            PdfColor stroke,
                            PdfColor fill)
        {
            CloseText();
            currentStream.DrawAndFillRect(x, y, w, h, stroke, fill);
        }

        /**
        * add a filled rectangle to the current stream
        *
        * @param x the x position of left edge in millipoints
        * @param y the y position of top edge in millipoints
        * @param w the width in millipoints
        * @param h the height in millipoints
        * @param fill the fill color/gradient
        */

        private void AddFilledRect(int x, int y, int w, int h,
                                   PdfColor fill)
        {
            CloseText();
            currentStream.FillRect(x, y, w, h, fill);
        }

        /**
        * render image area to PDF
        *
        * @param area the image area to render
        */

        public void RenderImageArea(ImageArea area)
        {
            int x = this.currentXPosition + area.getXOffset();
            int y = this.currentYPosition;
            int w = area.getContentWidth();
            int h = area.GetHeight();

            this.currentYPosition -= h;

            FonetImage img = area.getImage();

            PdfXObject xobj = this.pdfCreator.AddImage(img);
            CloseText();
            currentStream.FillImage(x, y, w, h, xobj);



            this.currentXPosition += area.getContentWidth();
        }

        /**
        * render a foreign object area
        */

        public void RenderForeignObjectArea(ForeignObjectArea area)
        {
            // if necessary need to scale and align the content
            this.currentXPosition = this.currentXPosition + area.getXOffset();
            // TODO: why was this here? this.currentYPosition = this.currentYPosition;
            switch (area.getAlign())
            {
                case TextAlign.START:
                    break;
                case TextAlign.END:
                    break;
                case TextAlign.CENTER:
                case TextAlign.JUSTIFY:
                    break;
            }
            switch (area.getVerticalAlign())
            {
                case VerticalAlign.BASELINE:
                    break;
                case VerticalAlign.MIDDLE:
                    break;
                case VerticalAlign.SUB:
                    break;
                case VerticalAlign.SUPER:
                    break;
                case VerticalAlign.TEXT_TOP:
                    break;
                case VerticalAlign.TEXT_BOTTOM:
                    break;
                case VerticalAlign.TOP:
                    break;
                case VerticalAlign.BOTTOM:
                    break;
            }
            CloseText();

            // in general the content will not be text
            currentStream.EndTextObject();
            // align and scale
            currentStream.SaveGraphicsState();
            switch (area.scalingMethod())
            {
                case Scaling.UNIFORM:
                    break;
                case Scaling.NON_UNIFORM:
                    break;
            }
            // if the overflow is auto (default), scroll or visible
            // then the contents should not be clipped, since this
            // is considered a printing medium.
            switch (area.getOverflow())
            {
                case Overflow.VISIBLE:
                case Overflow.SCROLL:
                case Overflow.AUTO:
                    break;
                case Overflow.HIDDEN:
                    break;
            }

            area.getObject().render(this);
            currentStream.RestoreGraphicsState();
            currentStream.BeginTextObject();
            this.currentXPosition += area.getEffectiveWidth();
            // this.currentYPosition -= area.getEffectiveHeight();
        }


        TextPrinter _textPrinter = new TextPrinter();

        /**
        * render inline area to PDF
        *
        * @param area inline area to render
        */
        public void RenderWordArea(WordArea area)
        {


            FontState fontState = area.GetFontState();
            String name = fontState.FontName;
            int size = fontState.FontSize;
            // This assumes that *all* CIDFonts use a /ToUnicode mapping
            Font font = (Font)fontState.FontInfo.GetFontByName(name);
            if ((!name.Equals(this.currentFontName)) ||
                (size != this.currentFontSize))
            {
                CloseText();

                this.currentFontName = name;
                this.currentFontSize = size;

                currentStream.SetFont(name, size);
            }

            // Do letter spacing (must be outside of [...] TJ]
            float letterspacing = ((float)fontState.LetterSpacing) / 1000f;
            if (letterspacing != this.currentLetterSpacing)
            {
                this.currentLetterSpacing = letterspacing;
                CloseText(); //?
                currentStream.SetLetterSpacing(letterspacing);
            }

            //--------------------------------------------
            PdfColor? a_color = this.currentFill;
            PdfColor areaObj_color = area.GetColor();

            if (a_color == null || !areaObj_color.IsEq(a_color.Value))
            {
                //change area color
                a_color = areaObj_color;

                CloseText(); //?
                this.currentFill = a_color;
                currentStream.SetFontColor(a_color.Value);
            }
            //--------------------------------------------

            int rx = this.currentXPosition;
            int bl = this.currentYPosition;
            int areaContentW = area.getContentWidth();
            if (area.getUnderlined())
            {
                AddUnderLine(rx, bl, areaContentW, size, a_color.Value);
            }
            if (area.getOverlined())
            {
                AddOverLine(rx, bl, areaContentW, size, fontState.Ascender, a_color.Value);
            }
            if (area.getLineThrough())
            {
                AddLineThrough(rx, bl, areaContentW, size, fontState.Ascender, a_color.Value);
            }
            //--------------------------------------------


            _textPrinter.Reset(fontState, options != null && options.Kerning);
            if (!textOpen || bl != prevWordY)
            {
                CloseText();
                //set text matrix


                _textPrinter.SetTextPos(rx, bl);
                //pdf.Append("1 0 0 1 " + PdfNumber.doubleOut(rx / 1000f) +
                //    " " + PdfNumber.doubleOut(bl / 1000f) + " Tm [" + startText);
                prevWordY = bl;
                textOpen = true; //***
            }
            else
            {
                // express the space between words in thousandths of an em
                int space = prevWordX - rx + prevWordWidth;
                float emDiff = (float)space / (float)currentFontSize * 1000f;
                // this prevents a problem in Acrobat Reader where large
                // numbers cause text to disappear or default to a limit
                if (emDiff < -33000)
                {
                    CloseText();
                    _textPrinter.SetTextPos(rx, bl);
                    //pdf.Append("1 0 0 1 " + PdfNumber.doubleOut(rx / 1000f) +
                    //    " " + PdfNumber.doubleOut(bl / 1000f) + " Tm [" + startText);
                    textOpen = true;//***
                }
                else
                {
                    _textPrinter.SetEmDiff(emDiff);
                    //pdf.Append(PdfNumber.doubleOut(emDiff));
                    //pdf.Append(" ");
                    //pdf.Append(startText);
                }
            }

            prevWordWidth = areaContentW;
            prevWordX = rx;

            string s = area.GetTextContent();
            if (area is PageNumberInlineArea)
            {
                //need to resolve to page number 
                s = idReferences.getPageNumber(s);
            }
            _textPrinter.WriteText(s);
            //-------
            _textPrinter.PrintContentTo(currentStream);
            //-------
            this.currentXPosition += area.getContentWidth();

        }



        /**
        * Checks to see if we have some text rendering commands open
        * still and writes out the TJ command to the stream if we do
        */

        private void CloseText()
        {
            if (textOpen)
            {
                currentStream.CloseText();

                textOpen = false;
                prevWordX = 0;
                prevWordY = 0;
            }
        }




        public void Render(Page page)
        {
            this.pdfResources = this.pdfCreator.GetResources();
            //
            this.idReferences = page.getIDReferences();
            this.pdfCreator.SetIDReferences(idReferences);
            //
            this.RenderPage(page);
            this.pdfCreator.FlushOutput();
        }


        /**
        * render page into PDF
        *
        * @param page page to render
        */

        public void RenderPage(Page page)
        {
            BodyAreaContainer body;
            AreaContainer before, after, start, end;

            currentStream = this.pdfCreator.MakeContentStream();
            body = page.getBody();
            before = page.getBefore();
            after = page.getAfter();
            start = page.getStart();
            end = page.getEnd();

            this.currentFontName = "";
            this.currentFontSize = 0;
            this.currentLetterSpacing = Single.NaN;

            currentStream.BeginTextObject();

            RenderBodyAreaContainer(body);

            if (before != null)
            {
                RenderAreaContainer(before);
            }

            if (after != null)
            {
                RenderAreaContainer(after);
            }

            if (start != null)
            {
                RenderAreaContainer(start);
            }

            if (end != null)
            {
                RenderAreaContainer(end);
            }
            CloseText();

            // Bug fix for issue 1823
            this.currentLetterSpacing = Single.NaN;

            float w = page.getWidth();
            float h = page.GetHeight();
            currentStream.EndTextObject();

            var idList = new System.Collections.Generic.List<string>();
            foreach (string id in page.getIDList())
            {
                idList.Add(id);
            }

            currentPage = this.pdfCreator.MakePage(
                this.pdfResources, currentStream,
                Convert.ToInt32(Math.Round(w / 1000)),
                Convert.ToInt32(Math.Round(h / 1000)), idList.ToArray());

            if (page.hasLinks() || currentAnnotList != null)
            {
                if (currentAnnotList == null)
                {
                    currentAnnotList = this.pdfCreator.MakeAnnotList();
                }
                currentPage.SetAnnotList(currentAnnotList);

                ArrayList lsets = page.getLinkSets();
                foreach (LinkSet linkSet in lsets)
                {
                    linkSet.align();
                    String dest = linkSet.getDest();
                    LinkKind linkType = linkSet.getLinkType();
                    ArrayList rsets = linkSet.getRects();
                    foreach (LinkedRectangle lrect in rsets)
                    {
                        currentAnnotList.Add(
                            this.pdfCreator.MakeLink(lrect.getRectangle(),
                            dest, linkType).GetReference());
                    }
                }
                currentAnnotList = null;
            }
            else
            {
                // just to be on the safe side
                currentAnnotList = null;
            }

            // ensures that color is properly reset for blocks that carry over pages
            this.currentFill = null;
        }



        private void DoFrame(Area area)
        {
            int w, h;
            int rx = this.currentAreaContainerXPosition;
            w = area.getContentWidth();
            if (area is BlockArea)
            {
                rx += ((BlockArea)area).getStartIndent();
            }
            h = area.getContentHeight();
            int ry = this.currentYPosition;

            rx = rx - area.getPaddingLeft();
            ry = ry + area.getPaddingTop();
            w = w + area.getPaddingLeft() + area.getPaddingRight();
            h = h + area.getPaddingTop() + area.getPaddingBottom();

            DoBackground(area, rx, ry, w, h);

            BorderAndPadding bp = area.GetBorderAndPadding();

            int left = area.getBorderLeftWidth();
            int right = area.getBorderRightWidth();
            int top = area.getBorderTopWidth();
            int bottom = area.getBorderBottomWidth();

            // If style is solid, use filled rectangles
            if (top != 0)
            {
                AddFilledRect(rx, ry, w, top,
                              bp.getBorderColor(BorderAndPadding.TOP).ToPdfColor());
            }
            if (left != 0)
            {
                AddFilledRect(rx - left, ry - h - bottom, left, h + top + bottom,
                              bp.getBorderColor(BorderAndPadding.LEFT).ToPdfColor());
            }
            if (right != 0)
            {
                AddFilledRect(rx + w, ry - h - bottom, right, h + top + bottom,
                              bp.getBorderColor(BorderAndPadding.RIGHT).ToPdfColor());
            }
            if (bottom != 0)
            {
                AddFilledRect(rx, ry - h - bottom, w, bottom,
                              bp.getBorderColor(BorderAndPadding.BOTTOM).ToPdfColor());
            }
        }

        /// <summary>
        ///     Renders an area's background.
        /// </summary>
        /// <param name="area">The area whose background is to be rendered.</param>
        /// <param name="x">The x position of the left edge in millipoints.</param>
        /// <param name="y">The y position of top edge in millipoints.</param>
        /// <param name="w">The width in millipoints.</param>
        /// <param name="h">The height in millipoints.</param>
        private void DoBackground(Area area, int x, int y, int w, int h)
        {
            if (h == 0 || w == 0)
            {
                return;
            }

            BackgroundProps props = area.getBackground();
            if (props == null)
            {
                return;
            }

            if (props.backColor.Alpha == 0)
            {
                AddFilledRect(x, y, w, -h, props.backColor.ToPdfColor());
            }

            if (props.backImage != null)
            {
                int imgW = props.backImage.Width * 1000;
                int imgH = props.backImage.Height * 1000;

                int dx = x;
                int dy = y;
                int endX = x + w;
                int endY = y - h;
                int clipW = w % imgW;
                int clipH = h % imgH;

                bool repeatX = true;
                bool repeatY = true;
                switch (props.backRepeat)
                {
                    case BackgroundRepeat.REPEAT:
                        break;
                    case BackgroundRepeat.REPEAT_X:
                        repeatY = false;
                        break;
                    case BackgroundRepeat.REPEAT_Y:
                        repeatX = false;
                        break;
                    case BackgroundRepeat.NO_REPEAT:
                        repeatX = false;
                        repeatY = false;
                        break;
                    case BackgroundRepeat.INHERIT:
                        break;
                    default:
                        FonetDriver.ActiveDriver.FireFonetWarning("Ignoring invalid background-repeat property");
                        break;
                }

                while (dy > endY)
                { // looping through rows 
                    while (dx < endX)
                    { // looping through cols 
                        if (dx + imgW <= endX)
                        {
                            // no x clipping 
                            if (dy - imgH >= endY)
                            {
                                // no x clipping, no y clipping 
                                DrawImageScaled(dx, dy, imgW, imgH, props.backImage);
                            }
                            else
                            {
                                // no x clipping, y clipping 
                                DrawImageClipped(dx, dy, 0, 0, imgW, clipH, props.backImage);
                            }
                        }
                        else
                        {
                            // x clipping
                            if (dy - imgH >= endY)
                            {
                                // x clipping, no y clipping 
                                DrawImageClipped(dx, dy, 0, 0, clipW, imgH, props.backImage);
                            }
                            else
                            {
                                // x clipping, y clipping
                                DrawImageClipped(dx, dy, 0, 0, clipW, clipH, props.backImage);
                            }
                        }

                        if (repeatX)
                        {
                            dx += imgW;
                        }
                        else
                        {
                            break;
                        }
                    } // end looping through cols

                    dx = x;

                    if (repeatY)
                    {
                        dy -= imgH;
                    }
                    else
                    {
                        break;
                    }
                } // end looping through rows 
            }
        }

        /// <summary>
        ///     Renders an image, rendered at the image's intrinsic size.
        ///     This by default calls drawImageScaled() with the image's
        ///     intrinsic width and height, but implementations may
        ///     override this method if it can provide a more efficient solution.
        /// </summary>
        /// <param name="x">The x position of left edge in millipoints.</param>
        /// <param name="y">The y position of top edge in millipoints.</param>
        /// <param name="image">The image to be rendered.</param>
        private void DrawImage(int x, int y, FonetImage image)
        {
            int w = image.Width * 1000;
            int h = image.Height * 1000;
            DrawImageScaled(x, y, w, h, image);
        }

        /// <summary>
        ///     Renders an image, scaling it to the given width and height.
        ///     If the scaled width and height is the same intrinsic size 
        ///     of the image, the image is not scaled
        /// </summary>
        /// <param name="x">The x position of left edge in millipoints.</param>
        /// <param name="y">The y position of top edge in millipoints.</param>
        /// <param name="w">The width in millipoints.</param>
        /// <param name="h">The height in millipoints.</param>
        /// <param name="image">The image to be rendered.</param>
        private void DrawImageScaled(
            int x, int y, int w, int h, FonetImage image)
        {
            PdfXObject xobj = this.pdfCreator.AddImage(image);
            CloseText();
            currentStream.FillImage(x, y, w, h, xobj);
        }

        /// <summary>
        ///     Renders an image, clipping it as specified.
        /// </summary>
        /// <param name="x">The x position of left edge in millipoints.</param>
        /// <param name="y">The y position of top edge in millipoints.</param>
        /// <param name="clipX">The left edge of the clip in millipoints.</param>
        /// <param name="clipY">The top edge of the clip in millipoints.</param>
        /// <param name="clipW">The clip width in millipoints.</param>
        /// <param name="clipH">The clip height in millipoints.</param>
        /// <param name="image">The image to be rendered.</param>
        private void DrawImageClipped(
            int x, int y, int clipX, int clipY,
            int clipW, int clipH, FonetImage image)
        {
            float cx1 = ((float)x) / 1000f;
            float cy1 = ((float)y - clipH) / 1000f;

            float cx2 = ((float)x + clipW) / 1000f;
            float cy2 = ((float)y) / 1000f;

            int imgX = x - clipX;
            int imgY = y - clipY;

            int imgW = image.Width * 1000;
            int imgH = image.Height * 1000;

            PdfXObject xobj = this.pdfCreator.AddImage(image);
            CloseText();

            currentStream.ClipImage(
                cx1, cy1, cx2, cy2,
                imgX, imgY, imgW, imgH, xobj);
        }


        /**
         * render display space
         *
         * @param space the display space to render
         */

        public void RenderDisplaySpace(DisplaySpace space)
        {
            int d = space.getSize();
            this.currentYPosition -= d;
        }


        void AddUnderLine(int x, int y, int w, int lineH,
                           PdfColor theAreaColor)
        {
            int yPos = y - lineH / 10;
            AddLine(x, yPos, x + w, yPos, lineH / 14,
                    theAreaColor);
            // save position for underlining a following InlineSpace
            prevUnderlineXEndPos = x + w;
            prevUnderlineYEndPos = yPos;
            prevUnderlineSize = lineH / 14;
            prevUnderlineColor = theAreaColor;
        }
        void AddOverLine(int x, int y, int w, int lineH,
                        int fontAscender,
                        PdfColor theAreaColor)
        {
            int yPos = y + fontAscender + lineH / 10;
            AddLine(x, yPos, x + w, yPos, lineH / 14,
                    theAreaColor);
            prevOverlineXEndPos = x + w;
            prevOverlineYEndPos = yPos;
            prevOverlineSize = lineH / 14;
            prevOverlineColor = theAreaColor;
        }
        void AddLineThrough(int x, int y, int w, int lineH,
                        int fontAscender,
                        PdfColor theAreaColor)
        {
            int yPos = y + fontAscender * 3 / 8;
            AddLine(x, yPos, x + w, yPos, lineH / 14,
                    theAreaColor);
            prevLineThroughXEndPos = x + w;
            prevLineThroughYEndPos = yPos;
            prevLineThroughSize = lineH / 14;
            prevLineThroughColor = theAreaColor;
        }

        /**
         * render inline space
         *
         * @param space space to render
         */

        public void RenderInlineSpace(InlineSpace space)
        {
            this.currentXPosition += space.getSize();
            if (space.getUnderlined())
            {
                if (prevUnderlineColor != null)
                {
                    AddLine(prevUnderlineXEndPos, prevUnderlineYEndPos,
                            prevUnderlineXEndPos + space.getSize(),
                            prevUnderlineYEndPos, prevUnderlineSize,
                            prevUnderlineColor.Value);
                    // save position for a following InlineSpace
                    prevUnderlineXEndPos = prevUnderlineXEndPos + space.getSize();
                }
            }
            if (space.getOverlined())
            {
                if (prevOverlineColor != null)
                {
                    AddLine(prevOverlineXEndPos, prevOverlineYEndPos,
                            prevOverlineXEndPos + space.getSize(),
                            prevOverlineYEndPos, prevOverlineSize,
                            prevOverlineColor.Value);
                    prevOverlineXEndPos = prevOverlineXEndPos + space.getSize();
                }
            }
            if (space.getLineThrough())
            {
                if (prevLineThroughColor != null)
                {
                    AddLine(prevLineThroughXEndPos, prevLineThroughYEndPos,
                            prevLineThroughXEndPos + space.getSize(),
                            prevLineThroughYEndPos, prevLineThroughSize,
                            prevLineThroughColor.Value);
                    prevLineThroughXEndPos = prevLineThroughXEndPos + space.getSize();
                }
            }
        }

        /**
         * render leader area
         *
         * @param area area to render
         */

        public void RenderLeaderArea(LeaderArea area)
        {
            int rx = this.currentXPosition;
            int ry = this.currentYPosition;
            int w = area.getContentWidth();
            int h = area.GetHeight();
            int th = area.getRuleThickness();
            RuleStyle st = area.getRuleStyle();

            // checks whether thickness is = 0, because of bug in pdf (or where?),
            // a line with thickness 0 is still displayed
            if (th != 0)
            {
                switch (st)
                {
                    case RuleStyle.DOUBLE:
                        AddLine(rx, ry, rx + w, ry, th / 3, st,
                                new PdfColor(area.getRed(), area.getGreen(),
                                             area.getBlue()));
                        AddLine(rx, ry + (2 * th / 3), rx + w, ry + (2 * th / 3),
                                th / 3, st,
                                new PdfColor(area.getRed(), area.getGreen(),
                                             area.getBlue()));
                        break;
                    case RuleStyle.GROOVE:
                        AddLine(rx, ry, rx + w, ry, th / 2, st,
                                new PdfColor(area.getRed(), area.getGreen(),
                                             area.getBlue()));
                        AddLine(rx, ry + (th / 2), rx + w, ry + (th / 2), th / 2, st,
                                new PdfColor(255, 255, 255));
                        break;
                    case RuleStyle.RIDGE:
                        AddLine(rx, ry, rx + w, ry, th / 2, st,
                                new PdfColor(255, 255, 255));
                        AddLine(rx, ry + (th / 2), rx + w, ry + (th / 2), th / 2, st,
                                new PdfColor(area.getRed(), area.getGreen(),
                                             area.getBlue()));
                        break;
                    default:
                        AddLine(rx, ry, rx + w, ry, th, st,
                                new PdfColor(area.getRed(), area.getGreen(),
                                             area.getBlue()));
                        break;
                }
                this.currentXPosition += area.getContentWidth();
                this.currentYPosition += th;
            }
        }

    }
}