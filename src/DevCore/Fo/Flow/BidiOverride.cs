//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class BidiOverride : ToBeImplementedElement
    {
       
        public static FObjMaker<BidiOverride> GetMaker()
        {
            return new FObjMaker<BidiOverride>((parent, propertyList) => new BidiOverride(parent, propertyList));
        }

        protected BidiOverride(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:bidi-override";
        }

        public override Status Layout(Area area)
        {
            AuralProps mAurProps = propMgr.GetAuralProps();
            RelativePositionProps mProps = propMgr.GetRelativePositionProps();
            return base.Layout(area);
        }
    }
}