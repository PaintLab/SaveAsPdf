﻿//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Fo.Properties;
    using Fonet.Layout;

    internal class BlockContainer : FObj
    {
        public static FObjMaker<BlockContainer> GetMaker()
        {
            return new FObjMaker<BlockContainer>((parent, propertyList) => new BlockContainer(parent, propertyList));
        }


        private int position;
        private int top;
        private int bottom;
        private int left;
        private int right;
        private int width;
        private int height;
        private SpanKind span;
        private AreaContainer areaContainer;



        protected BlockContainer(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.span = this.properties.GetSpanKind();
        }
        public override string ElementName { get { return "fo:block-container"; } }
        public override Status Layout(Area area)
        {
            if (this.marker == MarkerStart)
            {
                AbsolutePositionProps mAbsProps = propMgr.GetAbsolutePositionProps();
                BorderAndPadding bap = propMgr.GetBorderAndPadding();
                BackgroundProps bProps = propMgr.GetBackgroundProps();
                MarginProps mProps = propMgr.GetMarginProps();

                this.marker = 0;
                this.position = this.properties.GetProperty("position").GetEnum();
                this.top = this.properties.GetProperty("top").GetLength().MValue();
                this.bottom = this.properties.GetProperty("bottom").GetLength().MValue();
                this.left = this.properties.GetProperty("left").GetLength().MValue();
                this.right = this.properties.GetProperty("right").GetLength().MValue();
                this.width = this.properties.GetProperty("width").GetLength().MValue();
                this.height = this.properties.GetProperty("height").GetLength().MValue();
                span = this.properties.GetSpanKind(); 
                area.GetMyRefs().InitializeID(this.properties.GetId(), area);
            }

            AreaContainer container = (AreaContainer)area;
            if ((this.width == 0) && (this.height == 0))
            {
                width = right - left;
                height = bottom - top;
            }

            this.areaContainer =
                new AreaContainer(propMgr.GetFontState(container.getFontInfo()),
                                  container.getXPosition() + left,
                                  container.GetYPosition() - top, width, height,
                                  position);

            areaContainer.setPage(area.getPage());
            areaContainer.setBackground(propMgr.GetBackgroundProps());
            areaContainer.setBorderAndPadding(propMgr.GetBorderAndPadding());
            areaContainer.start();

            areaContainer.setAbsoluteHeight(0);
            areaContainer.setIDReferences(area.getIDReferences());

            int numChildren = this.children.Count;
            for (int i = this.marker; i < numChildren; i++)
            {
                FObj fo = (FObj)children[i];
                Status status = fo.Layout(areaContainer);
            }

            areaContainer.end();
            if (position == Position.ABSOLUTE)
            {
                areaContainer.SetHeight(height);
            }
            area.addChild(areaContainer);

            return new Status(Status.OK);
        }

        public override int GetContentWidth()
        {
            if (areaContainer != null)
            {
                return areaContainer.getContentWidth();
            }
            else
            {
                return 0;
            }
        }

        public override bool GeneratesReferenceAreas()
        {
            return true;
        }

        public SpanKind GetSpan()
        {
            return this.span;
        }
    }
}