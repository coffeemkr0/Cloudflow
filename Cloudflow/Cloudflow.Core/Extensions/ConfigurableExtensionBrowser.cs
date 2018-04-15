using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using log4net;

namespace Cloudflow.Core.Extensions
{
    public class ConfigurableExtensionBrowser
    {
        #region Constructors

        public ConfigurableExtensionBrowser(string extensionAssemblyPath)
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(extensionAssemblyPath));
            _container = new CompositionContainer(catalog);
            _container.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", null);

            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        #endregion

        #region Private Members

        [ImportMany]
        private readonly IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;

        private readonly CompositionContainer _container;

        private static readonly ILog _logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        public List<IConfigurableExtensionMetaData> GetConfigurableExtensions(ConfigurableExtensionTypes extensionType)
        {
            switch (extensionType)
            {
                case ConfigurableExtensionTypes.Job:
                    return _extensions.Where(i => i.Metadata.ExtensionType.IsSubclassOf(typeof(Job)))
                        .Select(i => i.Metadata).ToList();
                case ConfigurableExtensionTypes.Trigger:
                    return _extensions.Where(i => i.Metadata.ExtensionType.IsSubclassOf(typeof(Trigger)))
                        .Select(i => i.Metadata).ToList();
                case ConfigurableExtensionTypes.Step:
                    return _extensions.Where(i => i.Metadata.ExtensionType.IsSubclassOf(typeof(Step)))
                        .Select(i => i.Metadata).ToList();
                case ConfigurableExtensionTypes.Condition:
                    return _extensions.Where(i => i.Metadata.ExtensionType.IsSubclassOf(typeof(Condition)))
                        .Select(i => i.Metadata).ToList();
                default:
                    throw new ArgumentException($"The Extension type {extensionType.ToString()} is not supported",
                        "extensionType");
            }
        }

        public IConfigurableExtensionMetaData GetConfigurableExtension(Guid id)
        {
            return _extensions.First(i => Guid.Parse(i.Metadata.ExtensionId) == id).Metadata;
        }

        #endregion
    }
}