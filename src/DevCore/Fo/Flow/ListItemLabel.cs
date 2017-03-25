//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class ListItemLabel : FObj
    {
        public static FObjMaker<ListItemLabel> GetMaker()
        {
            return new FObjMaker<ListItemLabel>((parent, propertyList) => new ListItemLabel(parent, propertyList));
        }
         

        public ListItemLabel(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
       
        }
        public override string ElementName { get { return "fo:list-item-label"; } }
        public override Status Layout(Area area)
        {
            int numChildren = this.children.Count;

            if (numChildren != 1)
            {
                throw new FonetException("list-item-label must have exactly one block in this version of FO.NET");
            }

            AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
            string id = this.properties.GetProperty("id").GetString();
            area.getIDReferences().InitializeID(id, area);

            Block block = (Block)children[0];

            Status status;
            status = block.Layout(area);
            area.addDisplaySpace(-block.GetAreaHeight());
            return status;
        }
    }
}