//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class MultiProperties : ToBeImplementedElement
    {
        public static FObjMaker<MultiProperties> GetMaker()
        {
            return new FObjMaker<MultiProperties>((parent, propertyList) => new MultiProperties(parent, propertyList));
        }
         
        protected MultiProperties(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            
        }
        public override string ElementName { get { return "fo:multi-properties"; } }
        public override Status Layout(Area area)
        {
            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            return base.Layout(area);
        }
    }
}