//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class MultiPropertySet : ToBeImplementedElement
    {
        public static FObjMaker<MultiPropertySet> GetMaker()
        {
            return new FObjMaker<MultiPropertySet>((parent, propertyList) => new MultiPropertySet(parent, propertyList));
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