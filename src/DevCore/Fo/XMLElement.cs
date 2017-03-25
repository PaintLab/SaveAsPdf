//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using System;
using Fonet.Layout;
using Fonet.Layout.Inline;

namespace Fonet.Fo
{
    internal class XMLElement : XMLObj
    {
        private string nmspace = String.Empty;

        class Maker : FObj.Maker<XMLElement>
        {
            private string tag;

            internal Maker(string t)
            {
                tag = t;
            }
            public override XMLElement Make(FObj parent, PropertyList propertyList)
            {
                return new XMLElement(parent, propertyList, tag);
            }
        }

        public static FObj.Maker<XMLElement> GetMaker(string tag)
        {
            return new Maker(tag);
        }

        public XMLElement(FObj parent, PropertyList propertyList, string tag)
            : base(parent, propertyList, tag)
        {
            Init();
        }
        public override string ElementName { get { return this.tagName; } }
        public override Status Layout(Area area)
        {
            if (!(area is ForeignObjectArea))
            {
                throw new FonetException("XML not in fo:instream-foreign-object");
            }

            return new Status(Status.OK);
        }

        private void Init()
        {
            CreateBasicDocument();
        }

        public override string GetNameSpace()
        {
            return nmspace;
        }
    }
}