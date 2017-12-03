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
    public class TriggerController
    {
        #region Events
        public delegate void TriggerFiredEventHandler(Trigger trigger);
        public event TriggerFiredEventHandler TriggerFired;
        protected virtual void OnTriggerFired(Trigger trigger)
        {
            TriggerFiredEventHandler temp = TriggerFired;
            if (temp != null)
            {
                temp(trigger);
            }
        }
        #endregion

        #region Private Members
        private CompositionContainer _triggersContainer;
        [ImportMany]
        IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;
        #endregion

        #region Properties
        public TriggerDefinition TriggerDefinition { get; }

        public ExtensionConfiguration TriggerConfiguration { get; }

        public List<ConditionController> ConditionControllers { get; }

        public log4net.ILog TriggerControllerLoger { get; }

        public Trigger Trigger { get; }
        #endregion

        #region Constructors
        public TriggerController(TriggerDefinition triggerDefinition)
        {
            this.TriggerDefinition = triggerDefinition;
            this.TriggerControllerLoger = log4net.LogManager.GetLogger($"TriggerController.{triggerDefinition.TriggerDefinitionId}");

            var triggerConfigurationController = new ExtensionConfigurationController(triggerDefinition.ConfigurationExtensionId,
                triggerDefinition.ConfigurationExtensionAssemblyPath);
            this.TriggerConfiguration = triggerConfigurationController.Load(triggerDefinition.Configuration);

            this.ConditionControllers = new List<ConditionController>();
            foreach (var triggerConditionDefinition in triggerDefinition.TriggerConditionDefinitions)
            {
                var conditionController = new ConditionController(triggerConditionDefinition.TriggerConditionDefinitionId,
                    triggerConditionDefinition.ExtensionId, triggerConditionDefinition.ExtensionAssemblyPath,
                    triggerConditionDefinition.ConfigurationExtensionId, triggerConditionDefinition.ConfigurationExtensionAssemblyPath,
                    triggerConditionDefinition.Configuration);

                this.ConditionControllers.Add(conditionController);
            }

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(triggerDefinition.ExtensionAssemblyPath));
            _triggersContainer = new CompositionContainer(catalog);
            _triggersContainer.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", this.TriggerConfiguration);

            try
            {
                _triggersContainer.ComposeParts(this);

                foreach (Lazy<IConfigurableExtension, IConfigurableExtensionMetaData> i in _extensions)
                {
                    if (Guid.Parse(i.Metadata.ExtensionId) == triggerDefinition.ExtensionId)
                    {
                        this.Trigger = (Trigger)i.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.TriggerControllerLoger.Error(ex);
            }
        }
        #endregion

        #region Private Methods
        private void Trigger_Fired(Trigger trigger)
        {
            //Do not raise the TriggerFired event if any condition is not met
            foreach (var conditionController in this.ConditionControllers)
            {
                if (!conditionController.CheckCondition()) return;
            }

            OnTriggerFired(trigger);
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            this.Trigger.Fired += Trigger_Fired;
            this.Trigger.Start();
        }

        public void Stop()
        {
            this.Trigger.Fired -= Trigger_Fired;
            this.Trigger.Stop();
        }
        #endregion
    }
}
