//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Pagination
{
    using System;
    using System.Collections;

    internal class Root : FObj
    {

        public static FObjMaker<Root> GetMaker()
        {
            return new FObjMaker<Root>((parent, propertyList) => new Root(parent, propertyList));
        }


        private LayoutMasterSet layoutMasterSet;

        private ArrayList pageSequences;

        private int runningPageNumberCounter = 0;

        protected internal Root(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
              
            pageSequences = new ArrayList();
            if (parent != null)
            {
                throw new FonetException("root must be root element");
            }
        }
        public override string ElementName
        {
            get { return "fo:root"; }
        }
        protected internal int getRunningPageNumberCounter()
        {
            return this.runningPageNumberCounter;
        }

        protected internal void setRunningPageNumberCounter(int count)
        {
            this.runningPageNumberCounter = count;
        }

        public int getPageSequenceCount()
        {
            return pageSequences.Count;
        }

        public PageSequence getSucceedingPageSequence(PageSequence current)
        {
            int currentIndex = pageSequences.IndexOf(current);
            if (currentIndex == -1)
            {
                return null;
            }
            if (currentIndex < (pageSequences.Count - 1))
            {
                return (PageSequence)pageSequences[currentIndex + 1];
            }
            else
            {
                return null;
            }
        }

        public LayoutMasterSet getLayoutMasterSet()
        {
            return this.layoutMasterSet;
        }

        public void setLayoutMasterSet(LayoutMasterSet layoutMasterSet)
        {
            this.layoutMasterSet = layoutMasterSet;
        }

    }
}