﻿//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Layout.Inline
{
    using Fonet.Render.Pdf;
    using Fonet.Fo.Properties;
    internal class ForeignObjectArea : InlineArea
    {
        protected int xOffset = 0;
        protected TextAlign align;
        protected VerticalAlign valign;
        protected int scaling;
        protected Area foreignObject;
        protected int cheight;
        protected int cwidth;
        protected int awidth;
        protected int aheight;
        protected int width;
        private bool wauto;
        private bool hauto;
        private bool cwauto;
        private bool chauto;
        private Overflow overflow;

        public ForeignObjectArea(FontState fontState, int width)
            : base(fontState, width, 0, 0, 0)
        {
        }

        public override void render(PdfRenderer renderer)
        {
            if (foreignObject != null)
            {
                renderer.RenderForeignObjectArea(this);
            }
        }

        public override int getContentWidth()
        {
            return getEffectiveWidth();
        }

        public override int GetHeight()
        {
            return getEffectiveHeight();
        }

        public override int getContentHeight()
        {
            return getEffectiveHeight();
        }

        public override int getXOffset()
        {
            return this.xOffset;
        }

        public void setStartIndent(int startIndent)
        {
            xOffset = startIndent;
        }

        public void setObject(Area fobject)
        {
            foreignObject = fobject;
        }

        public Area getObject()
        {
            return foreignObject;
        }

        public void setSizeAuto(bool wa, bool ha)
        {
            wauto = wa;
            hauto = ha;
        }

        public void setContentSizeAuto(bool wa, bool ha)
        {
            cwauto = wa;
            chauto = ha;
        }

        public bool isContentWidthAuto()
        {
            return cwauto;
        }

        public bool isContentHeightAuto()
        {
            return chauto;
        }

        public void setAlign(TextAlign align)
        {
            this.align = align;
        }

        public TextAlign getAlign()
        {
            return this.align;
        }

        public override void setVerticalAlign(VerticalAlign align)
        {
            this.valign = align;
        }

        public override VerticalAlign getVerticalAlign()
        {
            return this.valign;
        }

        public void setOverflow(Overflow o)
        {
            this.overflow = o;
        }

        public Overflow getOverflow()
        {
            return this.overflow;
        }

        public override void SetHeight(int height)
        {
            this.height = height;
        }

        public void SetWidth(int width)
        {
            this.width = width;
        }

        public void setContentHeight(int cheight)
        {
            this.cheight = cheight;
        }

        public void SetContentWidth(int cwidth)
        {
            this.cwidth = cwidth;
        }

        public void setScaling(int scaling)
        {
            this.scaling = scaling;
        }

        public int scalingMethod()
        {
            return this.scaling;
        }

        public void setIntrinsicWidth(int w)
        {
            awidth = w;
        }

        public void setIntrinsicHeight(int h)
        {
            aheight = h;
        }

        public int getIntrinsicHeight()
        {
            return aheight;
        }

        public int getIntrinsicWidth()
        {
            return awidth;
        }

        public int getEffectiveHeight()
        {
            if (this.hauto)
            {
                if (this.chauto)
                {
                    return aheight;
                }
                else
                {
                    return this.cheight;
                }
            }
            else
            {
                return this.height;
            }
        }

        public int getEffectiveWidth()
        {
            if (this.wauto)
            {
                if (this.cwauto)
                {
                    return awidth;
                }
                else
                {
                    return this.cwidth;
                }
            }
            else
            {
                return this.width;
            }
        }

    }
}