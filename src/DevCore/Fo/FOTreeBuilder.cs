//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Fonet.Fo.Pagination;

namespace Fonet.Fo
{
    /// <summary>
    ///     Builds the formatting object tree.
    /// </summary>
    internal sealed class FOTreeBuilder
    {
        /// <summary>
        ///     Table mapping element names to the makers of objects
        ///     representing formatting objects.
        /// </summary>
        private Hashtable fobjTable = new Hashtable();

        private ArrayList namespaces = new ArrayList();

        /// <summary>
        ///     Class that builds a property list for each formatting object.
        /// </summary>
        private Hashtable propertylistTable = new Hashtable();

        /// <summary>
        ///     Current formatting object being handled.
        /// </summary>
        private FObj currentFObj = null;

        /// <summary>
        ///     The root of the formatting object tree.
        /// </summary>
        private FObj rootFObj = null;

        /// <summary>
        ///     Set of names of formatting objects encountered but unknown.
        /// </summary>
        private Hashtable unknownFOs = new Hashtable();

        /// <summary>
        ///     The class that handles formatting and rendering to a stream.
        /// </summary>
        private StreamRenderer streamRenderer;

        internal FOTreeBuilder() { }

        /// <summary>
        ///     Sets the stream renderer that will be used as output.
        /// </summary>
        internal void SetStreamRenderer(StreamRenderer streamRenderer)
        {
            this.streamRenderer = streamRenderer;
        }

        /// <summary>
        ///     Add a mapping from element name to maker.
        /// </summary>
        internal void AddElementMapping(string namespaceURI, Hashtable table)
        {
            this.fobjTable.Add(namespaceURI, table);
            //this.namespaces.Add(String.Intern(namespaceURI));
            this.namespaces.Add(namespaceURI);
        }

        /// <summary>
        ///     Add a mapping from property name to maker.
        /// </summary>
        internal void AddPropertyMapping(string namespaceURI, Hashtable list)
        {
            PropertyListBuilder plb;
            plb = (PropertyListBuilder)this.propertylistTable.GetValueOrNull(namespaceURI);
            if (plb == null)
            {
                plb = new PropertyListBuilder();
                plb.AddList(list);
                this.propertylistTable.Add(namespaceURI, plb);
            }
            else
            {
                plb.AddList(list);
            }
        }

        private FObj.MakerBase GetFObjMaker(string uri, string localName)
        {
            Hashtable table = (Hashtable)fobjTable[uri];
            if (table != null)
            {
                return (FObj.MakerBase)table[localName];
            }
            else
            {
                return null;
            }
        }
        private T CreateAndAppend<T>(
           FObj parent,
           FObj.Maker<T> maker,
           string uri,
           string localName,
           Attributes attlist)
            where T : FObj
        {
            PropertyListBuilder currentListBuilder =
                (PropertyListBuilder)this.propertylistTable[uri];
            PropertyList list = null;
            if (currentListBuilder != null)
            {
                list = currentListBuilder.MakeList(uri, localName, attlist, currentFObj);
            }
            T result = maker.Make(parent, list);
            if (parent != null)
            {
                parent.AddChild(result);
            }
            return result;
        }
        private void StartElement(
            string uri,
            string localName,
            Attributes attlist)
        {


            FObj.MakerBase fobjMaker = GetFObjMaker(uri, localName);

            PropertyListBuilder currentListBuilder =
                (PropertyListBuilder)this.propertylistTable[uri];

            bool foreignXML = false;
            if (fobjMaker == null)
            {
                string fullName = uri + "^" + localName;
                if (!this.unknownFOs.ContainsKey(fullName))
                {
                    this.unknownFOs.Add(fullName, "");
                    FonetDriver.ActiveDriver.FireFonetError("Unknown formatting object " + fullName);
                }
                //if (namespaces.Contains(String.Intern(uri)))
                if (namespaces.Contains(uri))
                {
                    fobjMaker = Unknown.GetMaker();
                }
                else
                {
                    fobjMaker = UnknownXMLObj.GetMaker(uri, localName);
                    foreignXML = true;
                }
            }

            PropertyList list = null;
            if (currentListBuilder != null)
            {
                list = currentListBuilder.MakeList(uri, localName, attlist, currentFObj);
            }
            else if (foreignXML)
            {
                list = null;
            }
            else
            {
                if (currentFObj == null)
                {
                    throw new FonetException("Invalid XML or missing namespace");
                }
                list = currentFObj.properties;
            }
            //
            FObj fobj = fobjMaker.InnerMake(currentFObj, list);

            if (rootFObj == null)
            {
                rootFObj = fobj;
                if (!fobj.ElementName.Equals("fo:root"))
                {
                    throw new FonetException("Root element must" + " be root, not " + fobj.ElementName);
                }
            }
            else if (!(fobj is PageSequence))
            {
                currentFObj.AddChild(fobj);
            }

            currentFObj = fobj;
        }

        private void EndElement()
        {
            if (currentFObj != null)
            {
                currentFObj.End();

                // If it is a page-sequence, then we can finally render it.
                // This is the biggest performance problem we have, we need
                // to be able to render prior to this point.
                if (currentFObj is PageSequence)
                {
                    streamRenderer.Render((PageSequence)currentFObj);

                }

                currentFObj = currentFObj.getParent();
            }
        }
        struct AttrKeyValue
        {
            public readonly string name;
            public readonly string value;
            public AttrKeyValue(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }

        static Attributes CreateAttributes(params AttrKeyValue[] keyValues)
        {
            Attributes attrs = new Attributes();
            int j = keyValues.Length;
            ArrayList attrArr = attrs.attArray;
            for (int i = 0; i < j; ++i)
            {
                AttrKeyValue kv = keyValues[i];
                SaxAttribute saxAttr = new SaxAttribute();
                saxAttr.Name = kv.name;
                saxAttr.NamespaceURI = "";
                saxAttr.Value = kv.value;
                attrArr.Add(saxAttr);
            }
            return attrs;
        }
        internal void Parse(PixelFarm.Drawing.Pdf.MyPdfDocument doc)
        {
            //TODO: use generic here

            var root_maker = Root.GetMaker();
            //
            var layout_master_set_maker = LayoutMasterSet.GetMaker();
            var simplpe_page_master = SimplePageMaster.GetMaker();
            var region_body = RegionBody.GetMaker();
            var region_before = RegionBefore.GetMaker();
            var region_after = RegionAfter.GetMaker();

            //
            var pageSeq_maker = PageSequence.GetMaker();
            var flow_maker = Flow.Flow.GetMaker();
            var block_maker = Flow.Block.GetMaker();


            string nsuri = "http://www.w3.org/1999/XSL/Format"; 

            streamRenderer.StartRenderer();
            
            //1. root
            Root rootObj = CreateAndAppend<Root>(null,
                root_maker,
                nsuri,
                "root",
                CreateAttributes());
            //2.  
            LayoutMasterSet masterSet = CreateAndAppend(rootObj, layout_master_set_maker, nsuri,
                "layout-master-set",
                CreateAttributes());
            {
                SimplePageMaster simpleMaster = CreateAndAppend(
                    masterSet, simplpe_page_master, nsuri,
                    "simple-page-master",
                    CreateAttributes(
                    new AttrKeyValue("master-name", "simple"),
                    new AttrKeyValue("page-height", "29.7cm"),
                    new AttrKeyValue("margin-top", "1cm"),
                    new AttrKeyValue("margin-bottom", "2cm"),
                    new AttrKeyValue("margin-left", "2.5cm"),
                    new AttrKeyValue("margin-right", "2.5cm")
                    ));
                RegionBody rgnBody = CreateAndAppend(simpleMaster, region_body, nsuri, "region-body",
                    CreateAttributes(new AttrKeyValue("margin-top", "3cm")));
                RegionBefore rgnBefore = CreateAndAppend(simpleMaster, region_before, nsuri, "region-before",
                    CreateAttributes(new AttrKeyValue("extent", "3cm")));
                RegionAfter rgnAfter = CreateAndAppend(simpleMaster, region_after, nsuri, "region-after",
                    CreateAttributes(new AttrKeyValue("extent", "1.5cm")));
                simpleMaster.End();
            }

            List<PixelFarm.Drawing.Pdf.MyPdfPage> pages = doc.Pages;
            int page_count = pages.Count;
            for (int i = 0; i < page_count; ++i)
            {
                PixelFarm.Drawing.Pdf.MyPdfPage p = pages[i];
                PixelFarm.Drawing.Pdf.MyPdfCanvas canvas = p.Canvas;

                PageSequence page_seq = CreateAndAppend(rootObj, pageSeq_maker, nsuri, "page-sequence",
                        CreateAttributes(new AttrKeyValue("master-reference", "simple")));

                Flow.Flow flow_obj = CreateAndAppend(page_seq, flow_maker, nsuri, "flow",
                    CreateAttributes(new AttrKeyValue("flow-name", "xsl-region-body")));

                Flow.Block block_obj = CreateAndAppend(
                    flow_obj, block_maker, nsuri, "block",
                     CreateAttributes(new AttrKeyValue("font-size", "18pt"),
                        new AttrKeyValue("color", "black"),
                        new AttrKeyValue("text-align", "center")
                     ));

                //very simple
                List<PixelFarm.Drawing.Pdf.MyPdfTextBlock> textElems = canvas.TextElems;
                //first sample ,

                int elem_count = textElems.Count;
                for (int n = 0; n < elem_count; ++n)
                {
                    PixelFarm.Drawing.Pdf.MyPdfTextBlock textBlock = textElems[n];
                    string txt = textBlock.Text;
                    //
                    char[] charBuff = txt.ToCharArray();
                    block_obj.AddCharacters(charBuff, 0, charBuff.Length);
                }

                streamRenderer.Render(page_seq);
            }
            FonetDriver.ActiveDriver.FireFonetInfo("Parsing of document complete, stopping renderer");
            streamRenderer.StopRenderer();

        }
        internal void Parse(XmlReader reader)
        {
            int buflen = 500;
            char[] buffer = new char[buflen];
            try
            {
                object nsuri = reader.NameTable.Add("http://www.w3.org/2000/xmlns/");

                FonetDriver.ActiveDriver.FireFonetInfo("Building formatting object tree");
                streamRenderer.StartRenderer();

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            Attributes atts = new Attributes();
                            while (reader.MoveToNextAttribute())
                            {
                                if (!reader.NamespaceURI.Equals(nsuri))
                                {
                                    SaxAttribute newAtt = new SaxAttribute();
                                    newAtt.Name = reader.Name;
                                    newAtt.NamespaceURI = reader.NamespaceURI;
                                    newAtt.Value = reader.Value;
                                    atts.attArray.Add(newAtt);
                                }
                            }
                            reader.MoveToElement();

                            StartElement(reader.NamespaceURI, reader.LocalName, atts.TrimArray());
                            if (reader.IsEmptyElement)
                            {
                                EndElement();
                            }
                            break;
                        case XmlNodeType.EndElement:
                            EndElement();
                            break;
                        case XmlNodeType.Text:

                            char[] chars = reader.ReadContentAsString().ToCharArray();
                            //char[] chars = reader.ReadString().ToCharArray();
                            if (currentFObj != null)
                            {
                                currentFObj.AddCharacters(chars, 0, chars.Length);
                            }
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                goto case XmlNodeType.Element;
                            }
                            if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                goto case XmlNodeType.EndElement;
                            }
                            break;
                        default:
                            break;
                    }
                }
                FonetDriver.ActiveDriver.FireFonetInfo("Parsing of document complete, stopping renderer");
                streamRenderer.StopRenderer();
            }
            catch (Exception exception)
            {
                FonetDriver.ActiveDriver.FireFonetError(exception.ToString());
            }
            finally
            {
                if (reader.ReadState != ReadState.Closed)
                {
                    reader.Close();
                }
            }
        }

    }

    internal class Attributes
    {
        internal ArrayList attArray = new ArrayList(3);

        // called by property list builder
        internal int getLength()
        {
            return attArray.Count;
        }

        // called by property list builder
        internal string getQName(int index)
        {
            SaxAttribute saxAtt = (SaxAttribute)attArray[index];
            return saxAtt.Name;
        }

        // called by property list builder
        internal string getValue(int index)
        {
            SaxAttribute saxAtt = (SaxAttribute)attArray[index];
            return saxAtt.Value;
        }

        // called by property list builder
        internal string getValue(string name)
        {
            foreach (SaxAttribute att in attArray)
            {
                if (att.Name.Equals(name))
                {
                    return att.Value;
                }
            }
            return null;
        }

        // only called above
        internal Attributes TrimArray()
        {
            return this;
            //attArray.TrimToSize();
            //return this;
        }
    }

    // Only used by FO tree builder
    internal struct SaxAttribute
    {
        public string Name;
        public string NamespaceURI;
        public string Value;
    }

}