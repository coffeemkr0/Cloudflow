using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Cloudflow.Core.Agents;
using Cloudflow.Core.Data.Shared.Models;
using log4net;

namespace Cloudflow.Core.Extensions.Controllers
{
    public class StepController
    {
        public delegate void StepOutputEventHandler(Step step, OutputEventLevels level, string message);

        [ImportMany]
        private IEnumerable<Lazy<IConfigurableExtension, IConfigurableExtensionMetaData>> _extensions = null;

        private readonly CompositionContainer _stepsContainer;

        public StepController(StepDefinition stepDefinition)
        {
            StepDefinition = stepDefinition;
            StepControllerLogger = LogManager.GetLogger($"StepController.{stepDefinition.StepDefinitionId}");

            var stepConfigurationController = new ExtensionConfigurationController(
                stepDefinition.ConfigurationExtensionId,
                stepDefinition.ConfigurationExtensionAssemblyPath);
            StepConfiguration = stepConfigurationController.Load(stepDefinition.Configuration);

            ConditionControllers = new List<ConditionController>();
            foreach (var stepConditionDefinition in stepDefinition.StepConditionDefinitions)
            {
                var conditionController = new ConditionController(stepConditionDefinition.StepConditionDefinitionId,
                    stepConditionDefinition.ExtensionId, stepConditionDefinition.ExtensionAssemblyPath,
                    stepConditionDefinition.ConfigurationExtensionId,
                    stepConditionDefinition.ConfigurationExtensionAssemblyPath,
                    stepConditionDefinition.Configuration);

                ConditionControllers.Add(conditionController);
            }

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(stepDefinition.ExtensionAssemblyPath));
            _stepsContainer = new CompositionContainer(catalog);
            _stepsContainer.ComposeExportedValue("ExtensionConfiguration", StepConfiguration);

            try
            {
                _stepsContainer.ComposeParts(this);

                foreach (var i in _extensions)
                    if (Guid.Parse(i.Metadata.ExtensionId) == stepDefinition.ExtensionId)
                    {
                        var step = (Step) i.Value;
                        step.StepOutput += Value_StepOutput;
                        Step = step;
                    }
            }
            catch (Exception ex)
            {
                StepControllerLogger.Error(ex);
            }
        }

        public StepDefinition StepDefinition { get; }

        public ExtensionConfiguration StepConfiguration { get; }

        public Step Step { get; }

        public List<ConditionController> ConditionControllers { get; }

        public ILog StepControllerLogger { get; }

        private void Value_StepOutput(Step step, OutputEventLevels level, string message)
        {
            OnStepOutput(step, level, message);
        }

        public void Execute()
        {
            try
            {
                //Do not execute the step if any condition is not met
                foreach (var conditionController in ConditionControllers)
                    if (!conditionController.CheckCondition())
                        return;

                Step.Execute();
            }
            catch (Exception ex)
            {
                StepControllerLogger.Error(ex);
            }
        }

        public event StepOutputEventHandler StepOutput;

        protected virtual void OnStepOutput(Step step, OutputEventLevels level, string message)
        {
            var temp = StepOutput;
            if (temp != null) temp(step, level, message);
        }
    }
}