﻿//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class MultiPropertySet : ToBeImplementedElement
    {
        new internal class Maker : FObj.Maker
        {
            public override FObj Make(FObj parent, PropertyList propertyList)
            {
                return new MultiPropertySet(parent, propertyList);
            }
        }

        new public static FObj.Maker GetMaker()
        {
            return new Maker();
        }

        protected MultiPropertySet(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:multi-property-set";
        }

        public override Status Layout(Area area)
        {
            return base.Layout(area);
        }
    }
}