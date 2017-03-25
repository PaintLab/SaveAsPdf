//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    internal class TableHeader : AbstractTableBody
    {
        public static FObjMaker<TableHeader> GetMaker()
        {
            return new FObjMaker<TableHeader>((parent, propertyList) => new TableHeader(parent, propertyList));
        }
         
        public TableHeader(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:table-header";
        }

    }
}