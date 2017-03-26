//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using Fonet.Layout;

namespace Fonet.Fo
{
    internal class Unknown : FObj
    {

        public static FObjMaker<Unknown> GetMaker()
        {
            return new FObjMaker<Unknown>((parent, propertyList) => new Unknown(parent, propertyList));
        } 
        protected Unknown(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            
        }
        public override string ElementName { get { return "unknown"; } }
        public override Status Layout(Area area)
        {
            return new Status(Status.OK);
        }
    }
}