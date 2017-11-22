using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    /// <summary>
    /// An interface that a class should implement when the class has a collection property with the CategorizedItemCollectionAttribute attribute specifed.
    /// The interface provieds functionality for loading the items that should be displayed for a categorized item selector.
    /// </summary>
    public interface ICategorizedItemFetcher
    {
        /// <summary>
        /// When implemented, this method should provied a collection of categorized items for the given property name.
        /// </summary>
        /// <param name="propertyName">The Name of the property that the items are being loaded for.</param>
        CategorizedItemCollection GetItems(string propertyName);
    }
}
