//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using Fonet.DataTypes;
using Fonet.Layout;

namespace Fonet.Fo
{
    internal class Title : ToBeImplementedElement
    {

        public static FObjMaker<Title> GetMaker()
        {
            return new FObjMaker<Title>((parent, propertyList) => new Title(parent, propertyList));
        }


        protected Title(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
             
        }
        public override string ElementName { get { return "fo:title"; } }
        public override Status Layout(Area area)
        {
            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            AuralProps mAurProps = propMgr.GetAuralProps();
            BorderAndPadding bap = propMgr.GetBorderAndPadding();
            BackgroundProps bProps = propMgr.GetBackgroundProps();
            FontState fontState = propMgr.GetFontState(area.getFontInfo());
            MarginInlineProps mProps = propMgr.GetMarginInlineProps();

            Property prop;
            prop = this.properties.GetProperty("baseline-shift");
            if (prop is LengthProperty)
            {
                Length bShift = prop.GetLength();
            }
            else if (prop is EnumProperty)
            {
                int bShift = prop.GetEnum();
            }
            ColorType col = this.properties.GetProperty("color").GetColorType();
            Length lHeight = this.properties.GetProperty("line-height").GetLength();
            int lShiftAdj = this.properties.GetProperty(
                "line-height-shift-adjustment").GetEnum();
            int vis = this.properties.GetProperty("visibility").GetEnum();
            Length zIndex = this.properties.GetProperty("z-index").GetLength();

            return base.Layout(area);
        }
    }
}