//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Layout.Inline
{
    class PageNumberInlineArea : WordArea
    {
        string _pageNumberId;
        public PageNumberInlineArea(
            FontState fontState, float red, float green,
            float blue, string refid, int width)
            : base(fontState, red, green, blue, "?", width)
        {
            _pageNumberId = refid;
        }
        public override string GetTextContent()
        {
            return _pageNumberId;
        }

    }
}