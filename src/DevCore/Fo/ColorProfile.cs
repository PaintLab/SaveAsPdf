//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo
{
    internal class ColorProfile : ToBeImplementedElement
    {
        class Maker : FObj.Maker<ColorProfile>
        {
            public override ColorProfile Make(FObj parent, PropertyList propertyList)
            {
                return new ColorProfile(parent, propertyList);
            }
        }

        public static FObj.Maker<ColorProfile> GetMaker()
        {
            return new ColorProfile.Maker();
        }

        protected ColorProfile(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:color-profile";
        }

    }
}