using Cloudflow.Core.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Runtime
{
    public class TriggerConfigurationController
    {
        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<TriggerConfiguration, ITriggerConfigurationMetaData>> _triggerConfigurations = null;

        private CompositionContainer _container;
        #endregion

        #region Properties
        public Guid TriggerExtensionId { get; }

        public log4net.ILog TriggerConfigurationControllerLogger { get; }
        #endregion

        #region Constructors
        public TriggerConfigurationController(Guid triggerExtensionId, string assemblyPath)
        {
            this.TriggerExtensionId = triggerExtensionId;
            this.TriggerConfigurationControllerLogger = log4net.LogManager.GetLogger($"TriggerConfigurationController.{this.TriggerExtensionId}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assemblyPath));
            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.TriggerConfigurationControllerLogger.Error(compositionException);
            }
        }
        #endregion

        #region Private Methods
        private Type GetConfigurationType()
        {
            foreach (Lazy<TriggerConfiguration, ITriggerConfigurationMetaData> i in _triggerConfigurations)
            {
                if (Guid.Parse(i.Metadata.TriggerExtensionId) == this.TriggerExtensionId)
                {
                    return i.Metadata.Type;
                }
            }

            return null;
        }
        #endregion

        #region Public Methods
        public TriggerConfiguration CreateNewConfiguration()
        {
            foreach (Lazy<TriggerConfiguration, ITriggerConfigurationMetaData> i in _triggerConfigurations)
            {
                if (Guid.Parse(i.Metadata.TriggerExtensionId) == this.TriggerExtensionId)
                {
                    return i.Value;
                }
            }

            return null;
        }

        public TriggerConfiguration Load(string configuration)
        {
            var configurationType = this.GetConfigurationType();
            var configurationObject = TriggerConfiguration.Load(configurationType, configuration);
            return (TriggerConfiguration)configurationObject;
        }
        #endregion
    }
}
