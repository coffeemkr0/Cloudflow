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
    public class StepConfigurationController
    {
        #region Private Members
        [ImportMany]
        IEnumerable<Lazy<StepConfiguration, IStepConfigurationMetaData>> _stepConfigurations = null;

        private CompositionContainer _container;
        #endregion

        #region Properties
        public Guid StepExtensionId { get; }

        public log4net.ILog StepConfigurationControllerLogger { get; }
        #endregion

        #region Constructors
        public StepConfigurationController(Guid stepExtensionId, string assemblyPath)
        {
            this.StepExtensionId = stepExtensionId;
            this.StepConfigurationControllerLogger = log4net.LogManager.GetLogger($"StepConfigurationController.{this.StepExtensionId}");

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(assemblyPath));
            _container = new CompositionContainer(catalog);

            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                this.StepConfigurationControllerLogger.Error(compositionException);
            }
        }
        #endregion

        #region Private Methods
        private Type GetConfigurationType()
        {
            foreach (Lazy<StepConfiguration, IStepConfigurationMetaData> i in _stepConfigurations)
            {
                if (Guid.Parse(i.Metadata.StepExtensionId) == this.StepExtensionId)
                {
                    return i.Metadata.Type;
                }
            }

            return null;
        }
        #endregion

        #region Public Methods
        public StepConfiguration CreateNewConfiguration()
        {
            foreach (Lazy<StepConfiguration, IStepConfigurationMetaData> i in _stepConfigurations)
            {
                if (Guid.Parse(i.Metadata.StepExtensionId) == this.StepExtensionId)
                {
                    return i.Value;
                }
            }

            return null;
        }

        public StepConfiguration LoadFromFile(string fileName)
        {
            var configurationType = this.GetConfigurationType();
            var configurationObject = StepConfiguration.LoadFromFile(configurationType, fileName);
            return (StepConfiguration)configurationObject;
        }
        #endregion
    }
}
