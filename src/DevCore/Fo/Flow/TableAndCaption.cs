//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class TableAndCaption : ToBeImplementedElement
    {
        public static FObjMaker<TableAndCaption> GetMaker()
        {
            return new FObjMaker<TableAndCaption>((parent, propertyList) => new TableAndCaption(parent, propertyList));
        }
         

        protected TableAndCaption(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:table-and-caption";
        }

        public override Status Layout(Area area)
        {
            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            AuralProps mAurProps = propMgr.GetAuralProps();
            BorderAndPadding bap = propMgr.GetBorderAndPadding();
            BackgroundProps bProps = propMgr.GetBackgroundProps();
            MarginProps mProps = propMgr.GetMarginProps();
            RelativePositionProps mRelProps = propMgr.GetRelativePositionProps();
            return base.Layout(area);
        }
    }
}