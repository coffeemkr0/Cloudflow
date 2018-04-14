using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class ExtensionConfigurationController
    {
        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<IExtension, IExtensionMetaData>> _extensions = null;

        private CompositionContainer _container;
        #endregion

        #region Properties
        public Guid ExtensionId { get; }

        public log4net.ILog StepConfigurationControllerLogger { get; }
        #endregion

        #region Constructors
        public ExtensionConfigurationController(Guid extensionId, string assemblyPath)
        {
            ExtensionId = extensionId;
            StepConfigurationControllerLogger = log4net.LogManager.GetLogger($"ExtensionConfigurationController.{ExtensionId}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assemblyPath));
            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                StepConfigurationControllerLogger.Error(ex);
            }
        }
        #endregion

        #region Public Methods
        public Type GetConfigurationType()
        {
            foreach (var i in _extensions)
            {
                if (Guid.Parse(i.Metadata.ExtensionId) == ExtensionId)
                {
                    return i.Metadata.ExtensionType;
                }
            }

            return null;
        }

        public ExtensionConfiguration CreateNewConfiguration()
        {
            foreach (var i in _extensions)
            {
                if (Guid.Parse(i.Metadata.ExtensionId) == ExtensionId)
                {
                    return (ExtensionConfiguration)i.Value;
                }
            }

            return null;
        }

        public ExtensionConfiguration Load(string configuration)
        {
            var configurationType = GetConfigurationType();
            var configurationObject = ExtensionConfiguration.Load(GetConfigurationType(), configuration);
            return (ExtensionConfiguration)configurationObject;
        }
        #endregion
    }
}
