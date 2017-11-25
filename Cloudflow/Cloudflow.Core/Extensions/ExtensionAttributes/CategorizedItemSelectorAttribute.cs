using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    public class CategorizedItemSelectorAttribute : Attribute
    {
        #region Properties
        public Guid CollectionId { get; set; }

        public Guid ObjectFactoryExtensionId { get; set; }
        #endregion

        #region Constructors
        public CategorizedItemSelectorAttribute(string collectionId, string objectFactoryExtensionId)
        {
            this.CollectionId = Guid.Parse(collectionId);
            this.ObjectFactoryExtensionId = Guid.Parse(objectFactoryExtensionId);
        }
        #endregion
    }
}
