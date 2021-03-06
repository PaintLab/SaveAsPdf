﻿//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using Fonet.Fo.Properties;
using Fonet.Layout;

namespace Fonet.Fo.Pagination
{
    internal class RegionAfter : Region
    {
        public static FObjMaker<RegionAfter> GetMaker()
        {
            return new FObjMaker<RegionAfter>((parent, propertyList) => new RegionAfter(parent, propertyList));
        }
         

        public const string REGION_CLASS = "after";

        private int precedence;

        protected RegionAfter(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            precedence = this.properties.GetProperty("precedence").GetEnum();
        }

        public override RegionArea MakeRegionArea(int allocationRectangleXPosition,
                                                  int allocationRectangleYPosition,
                                                  int allocationRectangleWidth,
                                                  int allocationRectangleHeight)
        {
            BorderAndPadding bap = propMgr.GetBorderAndPadding();
            BackgroundProps bProps = propMgr.GetBackgroundProps();
            int extent = this.properties.GetProperty("extent").GetLength().MValue();

            RegionArea area = new RegionArea(
                allocationRectangleXPosition,
                allocationRectangleYPosition - allocationRectangleHeight + extent,
                allocationRectangleWidth,
                extent);
            area.setBackground(bProps);

            return area;
        }


        protected override string GetDefaultRegionName()
        {
            return "xsl-region-after";
        }

        
        public override string ElementName { get { return "fo:region-after"; } }
        public override string GetRegionClass()
        {
            return REGION_CLASS;
        }

        public bool getPrecedence()
        {
            return (precedence == Precedence.TRUE ? true : false);
        }
    }
}