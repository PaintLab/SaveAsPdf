//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    internal class TableBody : AbstractTableBody
    {
        public static FObjMaker<TableBody> GetMaker()
        {
            return new FObjMaker<TableBody>((parent, propertyList) => new TableBody(parent, propertyList));
        }
         

        public TableBody(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            this.name = "fo:table-body";
        }

    }
}