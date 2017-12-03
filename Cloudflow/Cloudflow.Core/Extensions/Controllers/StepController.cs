using Cloudflow.Core.Configuration;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class StepController
    {
        #region Events
        public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);
        public event StepOutputEventHandler StepOutput;
        protected virtual void OnStepOutput(Step step, OutputEventLevels level, string message)
        {
            StepOutputEventHandler temp = StepOutput;
            if (temp != null)
            {
                temp(step, level, message);
            }
        }
        #endregion

        #region Private Members
        private CompositionContainer _stepsContainer;
        [ImportMany]
        IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;
        #endregion

        #region Properties
        public StepDefinition StepDefinition { get; }

        public ExtensionConfiguration StepConfiguration { get; }

        public Step Step { get; }

        public List<ConditionController> ConditionControllers { get; }

        public log4net.ILog StepControllerLogger { get; }
        #endregion

        #region Constructors
        public StepController(StepDefinition stepDefinition)
        {
            this.StepDefinition = stepDefinition;
            this.StepControllerLogger = log4net.LogManager.GetLogger($"StepController.{stepDefinition.StepDefinitionId}");

            var stepConfigurationController = new ExtensionConfigurationController(stepDefinition.ConfigurationExtensionId,
                    stepDefinition.ConfigurationExtensionAssemblyPath);
            this.StepConfiguration = stepConfigurationController.Load(stepDefinition.Configuration);

            this.ConditionControllers = new List<ConditionController>();
            foreach (var stepConditionDefinition in stepDefinition.StepConditionDefinitions)
            {
                var conditionController = new ConditionController(stepConditionDefinition.StepConditionDefinitionId,
                    stepConditionDefinition.ExtensionId, stepConditionDefinition.ExtensionAssemblyPath,
                    stepConditionDefinition.ConfigurationExtensionId, stepConditionDefinition.ConfigurationExtensionAssemblyPath,
                    stepConditionDefinition.Configuration);

                this.ConditionControllers.Add(conditionController);
            }

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(stepDefinition.ExtensionAssemblyPath));
            _stepsContainer = new CompositionContainer(catalog);
            _stepsContainer.ComposeExportedValue<ExtensionConfiguration>("ExtensionConfiguration", this.StepConfiguration);

            try
            {
                _stepsContainer.ComposeParts(this);

                foreach (Lazy<IConfigurableExtension, IConfigurableExtensionMetaData> i in _extensions)
                {
                    if (Guid.Parse(i.Metadata.ExtensionId) == stepDefinition.ExtensionId)
                    {
                        var step = (Step)i.Value;
                        step.StepOutput += Value_StepOutput;
                        this.Step = step;
                    }
                }
            }
            catch (Exception ex)
            {
                this.StepControllerLogger.Error(ex);
            }
        }
        #endregion

        #region Private Methods
        private void Value_StepOutput(Step step, OutputEventLevels level, string message)
        {
            OnStepOutput(step, level, message);
        }
        #endregion

        #region Public Methods
        public void Execute()
        {
            try
            {
                //Do not execute the step if any condition is not met
                foreach (var conditionController in this.ConditionControllers)
                {
                    if (!conditionController.CheckCondition()) return;
                }

                this.Step.Execute();
            }
            catch (Exception ex)
            {
                this.StepControllerLogger.Error(ex);
            }
        }
        #endregion
    }
}
