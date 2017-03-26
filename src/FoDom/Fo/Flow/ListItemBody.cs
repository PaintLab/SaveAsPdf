//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using Fonet.Layout;

    internal class ListItemBody : FObj
    {
        public static FObjMaker<ListItemBody> GetMaker()
        {
            return new FObjMaker<ListItemBody>((parent, propertyList) => new ListItemBody(parent, propertyList));
        }
         

        public ListItemBody(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
             
        }
        public override string ElementName { get { return "fo:list-item-body"; } }
        public override Status Layout(Area area)
        {
            if (this.marker == MarkerStart)
            {
                AccessibilityProps mAccProps = propMgr.GetAccessibilityProps();
                this.marker = 0;
                string id = this.properties.GetProperty("id").GetString();
                area.GetMyRefs().InitializeID(id, area);
            }

            int numChildren = this.children.Count;
            for (int i = this.marker; i < numChildren; i++)
            {
                FObj fo = (FObj)children[i];

                Status status;
                if ((status = fo.Layout(area)).isIncomplete())
                {
                    this.marker = i;
                    if ((i == 0) && (status.getCode() == Status.AREA_FULL_NONE))
                    {
                        return new Status(Status.AREA_FULL_NONE);
                    }
                    else
                    {
                        return new Status(Status.AREA_FULL_SOME);
                    }
                }
            }
            return new Status(Status.OK);
        }
    }
}