using Cloudflow.Core.Configuration;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class ConditionController
    {
        #region Private Members
        private CompositionContainer _conditionsContainer;
        [ImportMany]
        IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;
        #endregion

        #region Properties
        public log4net.ILog ConditionControllerLoger { get; }

        public Condition Condition { get; }
        #endregion

        #region Constructors
        public ConditionController(Guid conditionDefinitionId, Guid extensionId, string extensionAssemblyPath,
            Guid configurationExtensionId, string configurationExtensionAssemblyPath, string configuration)
        {
            this.ConditionControllerLoger = log4net.LogManager.GetLogger($"ConditionControllerLoger.{conditionDefinitionId}");

            var triggerConditionConfigurationController = new ExtensionConfigurationController(configurationExtensionId,
                configurationExtensionAssemblyPath);
            var conditionConfiguration = triggerConditionConfigurationController.Load(configuration);

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(extensionAssemblyPath));
            _conditionsContainer = new CompositionContainer(catalog);
            _conditionsContainer.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", conditionConfiguration);

            try
            {
                _conditionsContainer.ComposeParts(this);

                foreach (Lazy<IConfigurableExtension, IConfigurableExtensionMetaData> i in _extensions)
                {
                    if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                    {
                        this.Condition = (Condition)i.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ConditionControllerLoger.Error(ex);
            }
        }
        #endregion

        #region Public Methods
        public bool CheckCondition()
        {
            return this.Condition.CheckCondition();
        }
        #endregion
    }
}
