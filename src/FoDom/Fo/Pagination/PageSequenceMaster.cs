//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Pagination
{
    using System.Collections;

    internal class PageSequenceMaster : FObj
    {
        public static FObjMaker<PageSequenceMaster> GetMaker()
        {
            return new FObjMaker<PageSequenceMaster>((parent, propertyList) => new PageSequenceMaster(parent, propertyList));
        }


        private LayoutMasterSet layoutMasterSet;

        private ArrayList subSequenceSpecifiers;

        protected PageSequenceMaster(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            subSequenceSpecifiers = new ArrayList();

            if (parent.ElementName.Equals("fo:layout-master-set"))
            {
                this.layoutMasterSet = (LayoutMasterSet)parent;
                string pm = this.properties.GetProperty("master-name").GetString();
                if (pm == null)
                {
                    FonetDriver.ActiveDriver.FireFonetWarning(
                        "page-sequence-master does not have a page-master-name and so is being ignored");
                }
                else
                {
                    this.layoutMasterSet.addPageSequenceMaster(pm, this);
                }
            }
            else
            {
                throw new FonetException("fo:page-sequence-master must be child "
                    + "of fo:layout-master-set, not "
                    + parent.ElementName);
            }
        }

        public override string ElementName { get { return "fo:page-sequence-master"; } }
        protected internal void AddSubsequenceSpecifier(SubSequenceSpecifier pageMasterReference)
        {
            subSequenceSpecifiers.Add(pageMasterReference);
        }

        protected internal SubSequenceSpecifier getSubSequenceSpecifier(int sequenceNumber)
        {
            if (sequenceNumber >= 0
                && sequenceNumber < GetSubSequenceSpecifierCount())
            {
                return (SubSequenceSpecifier)subSequenceSpecifiers[sequenceNumber];
            }
            return null;
        }

        protected internal int GetSubSequenceSpecifierCount()
        {
            return subSequenceSpecifiers.Count;
        }

        public void Reset()
        {
            foreach (SubSequenceSpecifier s in subSequenceSpecifiers)
            {
                s.Reset();
            }
        }
    }
}