﻿//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using System;
using Fonet.DataTypes;

namespace Fonet.Fo.Properties
{
    internal class GenericCondLength : CondLengthProperty.Maker
    {
        internal class Enums
        {
            internal class Conditionality
            {
                public const int DISCARD = Constants.DISCARD;

                public const int RETAIN = Constants.RETAIN;

            }

        }

        private static readonly PropertyMaker s_LengthMaker = new LengthProperty.Maker("conditional-length-template.length");

        private class SP_ConditionalityMaker : EnumProperty.Maker
        {
            protected internal SP_ConditionalityMaker(string sPropName) : base(sPropName) { }

            protected internal static readonly EnumProperty s_propDISCARD = new EnumProperty(Enums.Conditionality.DISCARD);

            protected internal static readonly EnumProperty s_propRETAIN = new EnumProperty(Enums.Conditionality.RETAIN);

            public override Property CheckEnumValues(string value)
            {
                if (value.Equals("discard"))
                {
                    return s_propDISCARD;
                }

                if (value.Equals("retain"))
                {
                    return s_propRETAIN;
                }

                return base.CheckEnumValues(value);
            }

        }

        private static readonly PropertyMaker s_ConditionalityMaker =
            new SP_ConditionalityMaker("conditional-length-template.conditionality");


        new public static PropertyMaker Maker(string propName)
        {
            return new GenericCondLength(propName);
        }

        protected GenericCondLength(string name)
            : base(name)
        {
            m_shorthandMaker = GetSubpropMaker("length");

        }

        private PropertyMaker m_shorthandMaker;

        public override Property CheckEnumValues(string value)
        {
            return m_shorthandMaker.CheckEnumValues(value);
        }

        protected override bool IsCompoundMaker()
        {
            return true;
        }

        protected override PropertyMaker GetSubpropMaker(string subprop)
        {
            if (subprop.Equals("length"))
            {
                return s_LengthMaker;
            }

            if (subprop.Equals("conditionality"))
            {
                return s_ConditionalityMaker;
            }

            return base.GetSubpropMaker(subprop);
        }

        protected override Property SetSubprop(Property baseProp, string subpropName, Property subProp)
        {
            CondLength val = baseProp.GetCondLength();
            val.SetComponent(subpropName, subProp, false);
            return baseProp;
        }

        public override Property GetSubpropValue(Property baseProp, string subpropName)
        {
            CondLength val = baseProp.GetCondLength();
            return val.GetComponent(subpropName);
        }

        private Property m_defaultProp = null;

        public override Property Make(PropertyList propertyList)
        {
            if (m_defaultProp == null)
            {
                m_defaultProp = MakeCompound(propertyList, propertyList.getParentFObj());
            }
            return m_defaultProp;
        }

        protected override Property MakeCompound(PropertyList pList, FObj fo)
        {
            CondLength p = new CondLength();
            Property subProp;

            subProp = GetSubpropMaker("length").Make(pList,
                                                     getDefaultForLength(), fo);
            p.SetComponent("length", subProp, true);

            subProp = GetSubpropMaker("conditionality").Make(pList,
                                                             getDefaultForConditionality(), fo);
            p.SetComponent("conditionality", subProp, true);

            return new CondLengthProperty(p);
        }

        protected virtual String getDefaultForLength()
        {
            return "";

        }

        protected virtual String getDefaultForConditionality()
        {
            return "";

        }

        public override Property ConvertProperty(Property p, PropertyList pList, FObj fo)
        {
            if (p is CondLengthProperty)
            {
                return p;
            }
            if (!(p is EnumProperty))
            {
                p = m_shorthandMaker.ConvertProperty(p, pList, fo);
            }
            if (p != null)
            {
                Property prop = MakeCompound(pList, fo);
                CondLength pval = prop.GetCondLength();

                pval.SetComponent("length", p, false);
                return prop;
            }
            else
            {
                return null;
            }
        }

    }
}