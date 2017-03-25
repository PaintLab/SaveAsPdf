//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using System;
    using Fonet.Layout;

    internal class MultiToggle : ToBeImplementedElement
    {
        public static FObjMaker<MultiToggle> GetMaker()
        {
            return new FObjMaker<MultiToggle>((parent, propertyList) => new MultiToggle(parent, propertyList));
        }


        protected MultiToggle(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
           
        }
        public override string ElementName { get { return "fo:multi-toggle"; } }

        public override Status Layout(Area area)
        {
            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            return base.Layout(area);
        }
    }
}