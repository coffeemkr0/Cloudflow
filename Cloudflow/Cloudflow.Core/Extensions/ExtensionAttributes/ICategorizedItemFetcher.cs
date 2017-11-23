using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    //Notes:
    //Why did I use an interface like this?
    //Sticking purely to attributes to try to load the categorized item colleciton was turning out to be impossible.
    //Attributes cannot except delegates in their constructors, so it was not possible to specify a method to call in the attribute declaration.

    //I could have used reflection or extensibility in the attribute to callback to a static method on a specified class or plugin, but using static methods made
    //it unsafe if there were multiple collections on the same class.
    
    //I also needed a way for the class with the collection to be able to figure out how it loads the items for a given collection.
    //For example, I needed the Triggers collection to be able to load only trigger extensions.
    //There is a clear example of how this is implemented in the EditJobViewModel.

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

        object CreateInstance(string propertyName, string metaData);
    }
}
