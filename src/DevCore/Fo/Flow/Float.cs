//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class Float : ToBeImplementedElement
    {
        public static FObjMaker<Float> GetMaker()
        {
            return new FObjMaker<Float>((parent, propertyList) => new Float(parent, propertyList));
        }
         

        protected Float(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
    
        }
        public override string ElementName { get { return "fo:float"; } }
        public override Status Layout(Area area)
        {
            return base.Layout(area);
        }
    }
}