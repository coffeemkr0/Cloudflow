using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class ObjectFactoryController
    {
        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<IExtension, IExtensionMetaData>> _extensions = null;
        private CompositionContainer _extensionsContainer;
        private Guid _extensionId;
        #endregion

        #region Constructors
        public ObjectFactoryController(string factoryData, Guid extensionId, Assembly assembly)
        {
            _extensionId = extensionId;

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            _extensionsContainer = new CompositionContainer(catalog);
            _extensionsContainer.ComposeExportedValue<string>("factoryData", factoryData);

            _extensionsContainer.ComposeParts(this);
        }
        #endregion

        #region Public Methods
        public object CreateObject(string instanceData)
        {
            foreach (Lazy<IExtension, IExtensionMetaData> i in _extensions)
            {
                if (Guid.Parse(i.Metadata.ExtensionId) == _extensionId)
                {
                    return ((ObjectFactory)i.Value).CreateObject(instanceData);
                }
            }

            return null;
        }
        #endregion
    }
}
