//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo
{
    internal class Declarations : ToBeImplementedElement
    {
        public static FObjMaker<Declarations> GetMaker()
        {
            return new FObjMaker<Declarations>((parent, propertyList) => new Declarations(parent, propertyList));
        }

        protected Declarations(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:declarations";
        }
    }
}