using System;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    public class CategorizedItemSelectorAttribute : Attribute
    {
        #region Constructors

        public CategorizedItemSelectorAttribute(string collectionId, string objectFactoryExtensionId)
        {
            CollectionId = Guid.Parse(collectionId);
            ObjectFactoryExtensionId = Guid.Parse(objectFactoryExtensionId);
        }

        #endregion

        #region Properties

        public Guid CollectionId { get; set; }

        public Guid ObjectFactoryExtensionId { get; set; }

        #endregion
    }
}