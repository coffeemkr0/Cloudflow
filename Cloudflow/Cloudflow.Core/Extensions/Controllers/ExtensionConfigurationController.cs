using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.ExtensionId = extensionId;
            this.StepConfigurationControllerLogger = log4net.LogManager.GetLogger($"ExtensionConfigurationController.{this.ExtensionId}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assemblyPath));
            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                this.StepConfigurationControllerLogger.Error(ex);
            }
        }
        #endregion

        #region Public Methods
        public Type GetConfigurationType()
        {
            foreach (Lazy<IExtension, IExtensionMetaData> i in _extensions)
            {
                if (Guid.Parse(i.Metadata.ExtensionId) == this.ExtensionId)
                {
                    return i.Metadata.ExtensionType;
                }
            }

            return null;
        }

        public ExtensionConfiguration CreateNewConfiguration()
        {
            foreach (Lazy<IExtension, IExtensionMetaData> i in _extensions)
            {
                if (Guid.Parse(i.Metadata.ExtensionId) == this.ExtensionId)
                {
                    return (ExtensionConfiguration)i.Value;
                }
            }

            return null;
        }

        public ExtensionConfiguration Load(string configuration)
        {
            var configurationType = this.GetConfigurationType();
            var configurationObject = ExtensionConfiguration.Load(this.GetConfigurationType(), configuration);
            return (ExtensionConfiguration)configurationObject;
        }
        #endregion
    }
}
