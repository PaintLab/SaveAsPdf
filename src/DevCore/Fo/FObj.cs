//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using System;
using System.Collections;
using Fonet.DataTypes;
using Fonet.Layout;

namespace Fonet.Fo
{
    delegate T FObjCreator<T>(FObj parent, PropertyList propertyList);

    abstract class FObj : FONode
    {
        internal abstract class MakerBase
        {
            public abstract FObj InnerMake(FObj parent, PropertyList propertyList);
        }
        internal abstract class Maker<T> : MakerBase
            where T : FObj
        {
            public abstract T Make(FObj parent, PropertyList propertyList);
            public sealed override FObj InnerMake(FObj parent, PropertyList propertyList)
            {
                return Make(parent, propertyList);
            }
        }
        internal class FObjMaker<T> : Maker<T>
            where T : FObj
        {
            FObjCreator<T> _creatorDel;
            public FObjMaker(FObjCreator<T> creatorDel)
            {
                this._creatorDel = creatorDel;
            }
            public override T Make(FObj parent, PropertyList propertyList)
            {
                return _creatorDel(parent, propertyList);
            }
        }

        public PropertyList properties;

        protected PropertyManager propMgr;

        protected string name;

        private Hashtable markerClassNames;

        protected FObj(FObj parent, PropertyList propertyList)
            : base(parent)
        {
            this.properties = propertyList;
            propertyList.FObj = this;
            this.propMgr = MakePropertyManager(propertyList);
            this.name = "default FO";
            SetWritingMode();
        }

        protected PropertyManager MakePropertyManager(PropertyList propertyList)
        {
            return new PropertyManager(propertyList);
        }

        protected internal virtual void AddCharacters(char[] data, int start, int length)
        {
            // ignore
        }

        public override Status Layout(Area area)
        {
            return new Status(Status.OK);
        }

        public string GetName()
        {
            return name;
        }

        protected internal virtual void Start()
        {
            // do nothing by default
        }

        protected internal virtual void End()
        {
            // do nothing by default
        }

        public override Property GetProperty(string name)
        {
            return (properties.GetProperty(name));
        }

        public virtual int GetContentWidth()
        {
            return 0;
        }

        public virtual void RemoveID(IDReferences idReferences)
        {
            if (((FObj)this).properties.GetProperty("id") == null
                || ((FObj)this).properties.GetProperty("id").GetString() == null)
            {
                return;
            }
            idReferences.RemoveID(((FObj)this).properties.GetProperty("id").GetString());
            int numChildren = this.children.Count;
            for (int i = 0; i < numChildren; i++)
            {
                FONode child = (FONode)children[i];
                if ((child is FObj))
                {
                    ((FObj)child).RemoveID(idReferences);
                }
            }
        }

        public virtual bool GeneratesReferenceAreas()
        {
            return false;
        }

        protected virtual void SetWritingMode()
        {
            FObj p;
            FObj parent;
            for (p = this;
                !p.GeneratesReferenceAreas() && (parent = p.getParent()) != null;
                p = parent)
            {
                ;
            }
            this.properties.SetWritingMode(p.GetProperty("writing-mode").GetEnum());
        }

        public void AddMarker(string markerClassName)
        {
            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    FONode child = (FONode)children[i];
                    if (!child.MayPrecedeMarker())
                    {
                        throw new FonetException(
                            String.Format("A fo:marker must be an initial child of '{0}'", GetName()));
                    }
                }
            }
            if (markerClassNames == null)
            {
                markerClassNames = new Hashtable();
                markerClassNames.Add(markerClassName, String.Empty);
            }
            else if (!markerClassNames.ContainsKey(markerClassName))
            {
                markerClassNames.Add(markerClassName, String.Empty);
            }
            else
            {
                throw new FonetException(
                    String.Format("marker-class-name '{0}' already exists for this parent",
                                  markerClassName));
            }
        }

    }
}