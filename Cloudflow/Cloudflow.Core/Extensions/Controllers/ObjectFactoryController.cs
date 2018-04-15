using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class ObjectFactoryController
    {
        #region Constructors

        public ObjectFactoryController(string objectFactoryAssemblyPath, Guid objectFactoryExtensionId,
            string factoryData)
        {
            _extensionId = objectFactoryExtensionId;

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(objectFactoryAssemblyPath));
            _extensionsContainer = new CompositionContainer(catalog);
            _extensionsContainer.ComposeExportedValue("factoryData", factoryData);

            _extensionsContainer.ComposeParts(this);
        }

        #endregion

        #region Public Methods

        public object CreateObject(string instanceData)
        {
            foreach (var i in _extensions)
                if (Guid.Parse(i.Metadata.ExtensionId) == _extensionId)
                    return ((ObjectFactory) i.Value).CreateObject(instanceData);

            return null;
        }

        #endregion

        #region Private Members

        [ImportMany] private readonly IEnumerable<Lazy<IExtension, IExtensionMetaData>> _extensions = null;

        private readonly CompositionContainer _extensionsContainer;
        private readonly Guid _extensionId;

        #endregion
    }
}