using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class ExtensionService : IExtensionService
    {
        [ImportMany] protected IEnumerable<Lazy<IExtension, IExtensionMetaData>> Extensions = null;

        public ExtensionService(ICatalogProvider catalogProvider) : this(catalogProvider, null)
        {
        }

        public ExtensionService(ICatalogProvider catalogProvider, IExtension configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            //Set the constructor parameter for extensions that have a configuration parameters in their constructor
            container.ComposeExportedValue("Configuration", configuration);

            container.ComposeParts(this);
        }

        public IExtension GetExtension(Guid extensionId)
        {
            foreach (var i in Extensions)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            return null;
        }
    }
}