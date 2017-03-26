//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet
{
    using System.Collections;
    using System.Text;
    using Fonet.Layout;
    using Fonet.DataTypes;
    using Fonet.Pdf;

    class MyIDRefs : IDReferences
    {
        public void InitializeID(string id, Area area)
        {
            CreateID(id);
            ConfigureID(id, area);
        }
        public void ConfigureID(string id, Area area)
        {
            if (id != null && !id.Equals(""))
            {

                setPosition(id,
                            area.getPage().getBody().getXPosition()
                                + area.getTableCellXOffset() - ID_PADDING,
                            area.getPage().getBody().GetYPosition()
                                - area.getAbsoluteHeight() + ID_PADDING);
                setPageNumber(id, area.getPage().getNumber());
                area.getPage().addToIDList(id);
            }
        }
    }


}