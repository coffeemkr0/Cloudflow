using System.Collections.Generic;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// Provides functionality for loading a list of categorized item collections so that the collections
    /// can be used when selecting options for creating a new item in object collections.
    /// If a class implements this interface, each collection returned will be rendered to the client as a modal dialog.
    /// </summary>
    public interface ICategorizedItemCollectionLoader
    {
        List<CategorizedItemCollection> GetCategorizedItemCollections();
    }
}
