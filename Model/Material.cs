using System;
using System.Collections.Generic;

namespace SAOT
{
    /// <summary>
    /// Definition of a single item material for warehousing and picking.
    /// </summary>
    public class Material
    {
        public string MatId { get; private set; }
        public string Description { get; set; }
        public string DocumentId { get; private set; }
        public string UoM { get; private set; }
        public string MatGroup { get; private set; }
        public string CustomerId { get; private set; }
        public List<string> CrossRefs { get; private set; }


        /// <summary>
        /// Creates a material instance.
        /// </summary>
        /// <param name="matId"></param>
        /// <param name="documentId"></param>
        /// <param name="desc"></param>
        /// <param name="crossRefs"></param>
        public Material(string matId, string documentId, string desc, string uom, string matGroup, string[] crossRefs = null) : this(matId, documentId, desc, uom, matGroup, null, crossRefs)
        {
            
        }

        public Material(string matId, string documentId, string desc, string uom, string matGroup, string customerId, string[] crossRefs = null)
        {
            Assert.IsFalse(string.IsNullOrEmpty(matId));
            Assert.IsFalse(string.IsNullOrEmpty(matId));


            MatId = matId;
            DocumentId = documentId;
            CustomerId = customerId;
            Description = desc;
            UoM = uom;
            MatGroup = matGroup;
            CrossRefs = crossRefs == null ? new List<string>() : new List<string>(crossRefs);
        }
    }
}
