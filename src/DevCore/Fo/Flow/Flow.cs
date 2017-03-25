//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Fo.Flow
{
    using System.Collections;
    using Fonet.Fo.Pagination;
    using Fonet.Layout;

    internal class Flow : FObj
    {
        public static FObjMaker<Flow> GetMaker()
        {
            return new FObjMaker<Flow>((parent, propertyList) => new Flow(parent, propertyList));
        }



        private PageSequence pageSequence;
        private ArrayList markerSnapshot;
        private string _flowName;
        private int contentWidth;
        private Status _status = new Status(Status.AREA_FULL_NONE);

        protected Flow(FObj parent, PropertyList propertyList)
            : base(parent, propertyList)
        {
            if (parent.ElementName.Equals("fo:page-sequence"))
            {
                this.pageSequence = (PageSequence)parent;
            }
            else
            {
                throw new FonetException("flow must be child of "
                    + "page-sequence, not "
                    + parent.ElementName);
            }
            SetFlowName(GetProperty("flow-name").GetString());

            if (pageSequence.IsFlowSet)
            {
                if (this.ElementName.Equals("fo:flow"))
                {
                    throw new FonetException("Only a single fo:flow permitted"
                        + " per fo:page-sequence");
                }
                else
                {
                    throw new FonetException(this.ElementName
                        + " not allowed after fo:flow");
                }
            }
            pageSequence.AddFlow(this);
        }

        protected virtual void SetFlowName(string name)
        {
            if (name == null || name.Equals(""))
            {
                FonetDriver.ActiveDriver.FireFonetWarning(
                    "A 'flow-name' is required for " + ElementName + ".");
                _flowName = "xsl-region-body";
            }
            else
            {
                _flowName = name;
            }
        }

        public string GetFlowName()
        {
            return _flowName;
        }

        public override Status Layout(Area area)
        {
            return Layout(area, null);
        }

        public virtual Status Layout(Area area, Region region)
        {
            if (this.marker == MarkerStart)
            {
                this.marker = 0;
            }

            BodyAreaContainer bac = (BodyAreaContainer)area;

            bool prevChildMustKeepWithNext = false;
            ArrayList pageMarker = this.getMarkerSnapshot(new ArrayList());

            int numChildren = this.children.Count;
            if (numChildren == 0)
            {
                throw new FonetException("fo:flow must contain block-level children");
            }
            for (int i = this.marker; i < numChildren; i++)
            {
                FObj fo = (FObj)children[i];

                if (bac.isBalancingRequired(fo))
                {
                    bac.resetSpanArea();

                    this.Rollback(markerSnapshot);
                    i = this.marker - 1;
                    continue;
                }
                Area currentArea = bac.getNextArea(fo);
                currentArea.setIDReferences(bac.getIDReferences());
                if (bac.isNewSpanArea())
                {
                    this.marker = i;
                    markerSnapshot = this.getMarkerSnapshot(new ArrayList());
                }
                SetContentWidth(currentArea.getContentWidth());

                _status = fo.Layout(currentArea);

                if (_status.isIncomplete())
                {
                    if ((prevChildMustKeepWithNext) && (_status.laidOutNone()))
                    {
                        this.marker = i - 1;
                        FObj prevChild = (FObj)children[this.marker];
                        prevChild.RemoveAreas();
                        prevChild.ResetMarker();
                        prevChild.RemoveID(area.getIDReferences());
                        _status = new Status(Status.AREA_FULL_SOME);
                        return _status;
                    }
                    if (bac.isLastColumn())
                    {
                        if (_status.getCode() == Status.FORCE_COLUMN_BREAK)
                        {
                            this.marker = i;
                            _status =
                                new Status(Status.FORCE_PAGE_BREAK);
                            return _status;
                        }
                        else
                        {
                            this.marker = i;
                            return _status;
                        }
                    }
                    else
                    {
                        if (_status.isPageBreak())
                        {
                            this.marker = i;
                            return _status;
                        }
                        ((ColumnArea)currentArea).incrementSpanIndex();
                        i--;
                    }
                }
                if (_status.getCode() == Status.KEEP_WITH_NEXT)
                {
                    prevChildMustKeepWithNext = true;
                }
                else
                {
                    prevChildMustKeepWithNext = false;
                }
            }
            return _status;
        }

        protected void SetContentWidth(int contentWidth)
        {
            this.contentWidth = contentWidth;
        }

        public override int GetContentWidth()
        {
            return this.contentWidth;
        }
        public override string ElementName { get { return "fo:flow"; } }



        public Status getStatus()
        {
            return _status;
        }

        public override bool GeneratesReferenceAreas()
        {
            return true;
        }
    }
}