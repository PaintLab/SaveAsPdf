//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class InitialPropertySet : ToBeImplementedElement
    {
        public static FObjMaker<InitialPropertySet> GetMaker()
        {
            return new FObjMaker<InitialPropertySet>((parent, propertyList) => new InitialPropertySet(parent, propertyList));
        }
         

        protected InitialPropertySet(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
           
        }
        public override string ElementName { get { return "fo:initial-property-set"; } }
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