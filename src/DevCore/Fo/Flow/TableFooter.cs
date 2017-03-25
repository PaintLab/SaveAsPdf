//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    internal class TableFooter : AbstractTableBody
    {
        public static FObjMaker<TableFooter> GetMaker()
        {
            return new FObjMaker<TableFooter>((parent, propertyList) => new TableFooter(parent, propertyList));
        }

        public override int GetYPosition()
        {
            return areaContainer.GetCurrentYPosition() - spaceBefore;
        }

        public override void SetYPosition(int value)
        {
            areaContainer.setYPosition(value + 2 * spaceBefore);
        }

        public TableFooter(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
        }
        public override string ElementName { get { return "fo:table-footer"; } }
    }
}