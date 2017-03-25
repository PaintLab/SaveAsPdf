//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class MultiSwitch : ToBeImplementedElement
    {

        public static FObjMaker<MultiSwitch> GetMaker()
        {
            return new FObjMaker<MultiSwitch>((parent, propertyList) => new MultiSwitch(parent, propertyList));
        }

        protected MultiSwitch(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:multi-switch";
        }

        public override Status Layout(Area area)
        {
            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            return base.Layout(area);
        }
    }
}