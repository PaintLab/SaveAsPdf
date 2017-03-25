//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using Fonet.Layout;

namespace Fonet.Fo.Flow
{
    internal class TableCaption : ToBeImplementedElement
    {
        public static FObjMaker<TableCaption> GetMaker()
        {
            return new FObjMaker<TableCaption>((parent, propertyList) => new TableCaption(parent, propertyList));

        }
        

        protected TableCaption(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:table-caption";
        }

        public override Status Layout(Area area)
        {
            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            AuralProps mAurProps = propMgr.GetAuralProps();
            BorderAndPadding bap = propMgr.GetBorderAndPadding();
            BackgroundProps bProps = propMgr.GetBackgroundProps();
            RelativePositionProps mRelProps = propMgr.GetRelativePositionProps();
            return base.Layout(area);
        }
    }
}