//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class MultiCase : ToBeImplementedElement
    {
        public static FObjMaker<MultiCase> GetMaker()
        {
            return new FObjMaker<MultiCase>((parent, propertyList) => new MultiCase(parent, propertyList));
        }


        protected MultiCase(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
           
        }
        public override string ElementName { get { return "fo:multi-case"; } }
        public override Status Layout(Area area)
        {
            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            return base.Layout(area);
        }
    }
}