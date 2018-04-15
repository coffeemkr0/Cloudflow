using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using log4net;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class ConditionController
    {
        #region Constructors

        public ConditionController(Guid conditionDefinitionId, Guid extensionId, string extensionAssemblyPath,
            Guid configurationExtensionId, string configurationExtensionAssemblyPath, string configuration)
        {
            ConditionControllerLoger = LogManager.GetLogger($"ConditionControllerLoger.{conditionDefinitionId}");

            var triggerConditionConfigurationController = new ExtensionConfigurationController(configurationExtensionId,
                configurationExtensionAssemblyPath);
            var conditionConfiguration = triggerConditionConfigurationController.Load(configuration);

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(extensionAssemblyPath));
            _conditionsContainer = new CompositionContainer(catalog);
            _conditionsContainer.ComposeExportedValue("ExtensionConfiguration", conditionConfiguration);

            try
            {
                _conditionsContainer.ComposeParts(this);

                foreach (var i in _extensions)
                    if (Guid.Parse(i.Metadata.ExtensionId) == extensionId)
                        Condition = (Condition) i.Value;
            }
            catch (Exception ex)
            {
                ConditionControllerLoger.Error(ex);
            }
        }

        #endregion

        #region Public Methods

        public bool CheckCondition()
        {
            return Condition.CheckCondition();
        }

        #endregion

        #region Private Members

        private readonly CompositionContainer _conditionsContainer;

        [ImportMany]
        private readonly IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;

        #endregion

        #region Properties

        public ILog ConditionControllerLoger { get; }

        public Condition Condition { get; }

        #endregion
    }
}