//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    internal class Wrapper : FObjMixed
    {

        public static FObjMaker<Wrapper> GetMaker()
        {
            return new FObjMaker<Wrapper>((parent, propertyList) => new Wrapper(parent, propertyList));
        } 
        public Wrapper(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        { 
        } 
        protected internal override void AddCharacters(char[] data, int start, int length)
        {
            FOText ft = new FOText(data, start, length, this);
            children.Add(ft);
        }
        public override string ElementName { get { return "fo:wrapper"; } }
    }
}