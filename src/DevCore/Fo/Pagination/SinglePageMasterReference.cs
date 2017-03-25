//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Pagination
{
    internal class SinglePageMasterReference : PageMasterReference, SubSequenceSpecifier
    {

        public static FObjMaker<SinglePageMasterReference> GetMaker()
        {
            return new FObjMaker<SinglePageMasterReference>((parent, propertyList) => new SinglePageMasterReference(parent, propertyList));
        }

        private const int FIRST = 0;

        private const int DONE = 1;

        private int state;

        public SinglePageMasterReference(
            FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.state = FIRST;
        }

        public override string GetNextPageMaster(int currentPageNumber,
                                                 bool thisIsFirstPage,
                                                 bool isEmptyPage)
        {
            if (this.state == FIRST)
            {
                this.state = DONE;
                return MasterName;
            }
            else
            {
                return null;
            }
        }

        public override void Reset()
        {
            this.state = FIRST;
        }
        public override string ElementName { get { return "fo:single-page-master-reference"; } }
       

    }
}