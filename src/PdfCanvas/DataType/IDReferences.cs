//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.DataTypes
{
    using System.Collections;
    using Fonet.Pdf;

    public abstract class IDReferences
    {
        private Hashtable idReferences, idValidation, idUnvalidated;
        public const int ID_PADDING = 5000;

        public IDReferences()
        {
            idReferences = new Hashtable();
            idValidation = new Hashtable();
            idUnvalidated = new Hashtable();
        }



        public void CreateID(string id)
        {
            if (id != null && !id.Equals(""))
            {
                if (DoesUnvalidatedIDExist(id))
                {
                    RemoveFromUnvalidatedIDList(id);
                    RemoveFromIdValidationList(id);
                }
                else if (doesIDExist(id))
                {
                    throw new System.Exception("The id \"" + id
                        + "\" already exists in this document");
                }
                else
                {
                    createNewId(id);
                    RemoveFromIdValidationList(id);
                }

            }
        }

        public void CreateUnvalidatedID(string id)
        {
            if (id != null && !id.Equals(""))
            {
                if (!doesIDExist(id))
                {
                    createNewId(id);
                    AddToUnvalidatedIdList(id);
                }
            }
        }

        public void AddToUnvalidatedIdList(string id)
        {
            idUnvalidated[id] = "";
        }

        public void RemoveFromUnvalidatedIDList(string id)
        {
            idUnvalidated.Remove(id);
        }

        public bool DoesUnvalidatedIDExist(string id)
        {
            return idUnvalidated.ContainsKey(id);
        }



        public void AddToIdValidationList(string id)
        {
            idValidation[id] = "";
        }

        public void RemoveFromIdValidationList(string id)
        {
            idValidation.Remove(id);
        }

        public void RemoveID(string id)
        {
            idReferences.Remove(id);
        }

        public bool IsEveryIdValid()
        {
            return (idValidation.Count == 0);
        }

        public string GetInvalidIds()
        {
            System.Text.StringBuilder list = new System.Text.StringBuilder();
            foreach (object o in idValidation.Keys)
            {
                list.Append("\n\"");
                list.Append(o.ToString());
                list.Append("\" ");
            }
            return list.ToString();
        }

        public bool doesIDExist(string id)
        {
            return idReferences.ContainsKey(id);
        }

        public bool doesGoToReferenceExist(string id)
        {
            IDNode node = (IDNode)idReferences[id];
            return node.IsThereInternalLinkGoTo();
        }

        public PdfGoTo getInternalLinkGoTo(string id)
        {
            IDNode node = (IDNode)idReferences[id];
            return node.GetInternalLinkGoTo();
        }

        public PdfGoTo createInternalLinkGoTo(string id, PdfObjectId objectId)
        {
            IDNode node = (IDNode)idReferences[id];
            node.CreateInternalLinkGoTo(objectId);
            return node.GetInternalLinkGoTo();
        }

        public void createNewId(string id)
        {
            IDNode node = new IDNode(id);
            idReferences[id] = node;
        }

        public PdfGoTo getPDFGoTo(string id)
        {
            IDNode node = (IDNode)idReferences[id];
            return node.GetInternalLinkGoTo();
        }

        public void setInternalGoToPageReference(string id,
                                                 PdfObjectReference pageReference)
        {
            IDNode node = (IDNode)idReferences[id];
            if (node != null)
            {
                node.SetInternalLinkGoToPageReference(pageReference);
            }
        }

        public void setPageNumber(string id, int pageNumber)
        {
            IDNode node = (IDNode)idReferences[id];
            node.SetPageNumber(pageNumber);
        }

        public string getPageNumber(string id)
        {
            if (doesIDExist(id))
            {
                IDNode node = (IDNode)idReferences[id];
                return node.GetPageNumber();
            }
            else
            {
                AddToIdValidationList(id);
                return null;
            }
        }

        public void setPosition(string id, int x, int y)
        {
            IDNode node = (IDNode)idReferences[id];
            node.SetPosition(x, y);
        }

        public ICollection getInvalidElements()
        {
            return idValidation.Keys;
        }
    }

}