//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class Inline : FObjMixed
    {

        public static FObjMaker<Inline> GetMaker()
        {
            return new FObjMaker<Inline>((parent, propertyList) => new Inline(parent, propertyList));
        }

        public override string ElementName { get { return "fo:inline"; } }
        public Inline(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {

            if (parent.GetName().Equals("fo:flow"))
            {
                throw new FonetException("inline formatting objects cannot"
                    + " be directly under flow");
            }

            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            AuralProps mAurProps = propMgr.GetAuralProps();
            BorderAndPadding bap = propMgr.GetBorderAndPadding();
            BackgroundProps bProps = propMgr.GetBackgroundProps();
            MarginInlineProps mProps = propMgr.GetMarginInlineProps();
            RelativePositionProps mRelProps = propMgr.GetRelativePositionProps();
            ts = propMgr.getTextDecoration(parent);
        }

        protected internal override void AddCharacters(char[] data, int start, int length)
        {
            FOText ft = new FOText(data, start, length, this);
            ft.setUnderlined(ts.getUnderlined());
            ft.setOverlined(ts.getOverlined());
            ft.setLineThrough(ts.getLineThrough());
            children.Add(ft);
        }
    }
}