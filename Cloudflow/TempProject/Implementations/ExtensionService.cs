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
        [ImportMany] protected IEnumerable<Lazy<IJob, IExtensionMetaData>> Jobs = null;
        [ImportMany] protected IEnumerable<Lazy<ITrigger, IExtensionMetaData>> Triggers = null;
        [ImportMany] protected IEnumerable<Lazy<IStep, IExtensionMetaData>> Steps = null;

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

        public IJob GetJob(Guid extensionId)
        {
            foreach (var i in Jobs)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            return null;
        }

        public IStep GetStep(Guid extensionId)
        {
            foreach (var i in Steps)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            return null;
        }

        public ITrigger GetTrigger(Guid extensionId)
        {
            foreach (var i in Triggers)
                if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    return i.Value;

            return null;
        }
    }
}