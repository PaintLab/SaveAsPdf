//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class InlineContainer : ToBeImplementedElement
    {
        public static FObjMaker<InlineContainer> GetMaker()
        {
            return new FObjMaker<InlineContainer>((parent, propertyList) => new InlineContainer(parent, propertyList));
        }

         
        protected InlineContainer(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:inline-container";

            BorderAndPadding bap = propMgr.GetBorderAndPadding();
            BackgroundProps bProps = propMgr.GetBackgroundProps();
            MarginInlineProps mProps = propMgr.GetMarginInlineProps();
            RelativePositionProps mRelProps = propMgr.GetRelativePositionProps();
        }
    }
}