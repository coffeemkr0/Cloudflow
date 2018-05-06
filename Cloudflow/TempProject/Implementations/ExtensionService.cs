using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;
using TempProject.Interfaces;

namespace TempProject.Implementations
{
    public class ExtensionService : IExtensionService
    {
        [ImportMany] protected IEnumerable<Lazy<IExtension, IExtensionMetaData>> Extensions = null;

        private T GetExtensionInstance<T>(Guid extensionId)
        {
            foreach (var i in Extensions)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return (T)i.Value;

            return default(T);
        }

        private Type GetExtensionType(Guid extensionId)
        {
            foreach (var i in Extensions)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Metadata.ExtensionType;

            return null;
        }

        public T LoadConfiguration<T>(ICatalogProvider catalogProvider, Guid extensionId, string configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            //TODO:This deserialization should not be implemented in this service - abstract this out to a dependency
            var configurationObject = JsonConvert.DeserializeObject(configuration, GetExtensionType(extensionId));

            return (T) configurationObject;
        }

        public T LoadConfigurableExtension<T>(ICatalogProvider catalogProvider, Guid extensionId, IExtension configuration)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            //Set the constructor parameter for extensions that have a configuration parameter in their constructor
            container.ComposeExportedValue("Configuration", configuration);

            container.ComposeParts(this);

            return GetExtensionInstance<T>(extensionId);
        }

        public T CreateNewConfiguration<T>(ICatalogProvider catalogProvider, Guid extensionId)
        {
            var container = new CompositionContainer(catalogProvider.GetCatalog());

            container.ComposeParts(this);

            return GetExtensionInstance<T>(extensionId);
        }
    }
}