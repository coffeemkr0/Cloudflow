using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class EditJobViewModel
    {
        #region Properties
        [Hidden]
        public Guid JobDefinitionId { get; set; }

        [PropertyGroup("GeneralTabText")]
        [DisplayOrder(0)]
        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        [PropertyGroup("TriggersTabText")]
        [DisplayOrder(1)]
        [TriggerSelector("AddTriggerCaption", "AddTriggerCategoriesCaption")]
        public List<TriggerViewModel> Triggers { get; set; }

        [PropertyGroup("StepsTabText")]
        [DisplayOrder(2)]
        public List<StepViewModel> Steps { get; set; }
        #endregion

        #region Constructors
        public EditJobViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
            this.Triggers = new List<TriggerViewModel>();
            this.Steps = new List<StepViewModel>();
        }
        #endregion

        #region Public Methods
        public static EditJobViewModel FromJobDefinition(JobDefinition jobDefinition)
        {
            var model = new EditJobViewModel();

            model.JobDefinitionId = jobDefinition.JobDefinitionId;
            model.ExtensionConfiguration.ConfigurationExtensionId = jobDefinition.ConfigurationExtensionId;
            model.ExtensionConfiguration.ConfigurationExtensionAssemblyPath = jobDefinition.ConfigurationExtensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(jobDefinition.ConfigurationExtensionId,
               jobDefinition.ConfigurationExtensionAssemblyPath);
            model.ExtensionConfiguration.Configuration = extensionConfigurationController.Load(jobDefinition.Configuration);

            int index = 0;
            foreach (var triggerDefinition in jobDefinition.TriggerDefinitions.OrderBy(i => i.Index))
            {
                var triggerViewModel = new TriggerViewModel();
                triggerViewModel.TriggerDefinitionId = triggerDefinition.TriggerDefinitionId;
                if (index == 0) triggerViewModel.Active = true;
                triggerViewModel.ExtensionConfiguration.ConfigurationExtensionId = triggerDefinition.ConfigurationExtensionId;
                triggerViewModel.ExtensionConfiguration.ConfigurationExtensionAssemblyPath = triggerDefinition.ConfigurationExtensionAssemblyPath;

                extensionConfigurationController = new ExtensionConfigurationController(triggerDefinition.ConfigurationExtensionId,
                    triggerDefinition.ConfigurationExtensionAssemblyPath);
                triggerViewModel.ExtensionConfiguration.Configuration = extensionConfigurationController.Load(triggerDefinition.Configuration);

                model.Triggers.Add(triggerViewModel);

                var conditionIndex = 0;
                foreach (var conditionDefinition in triggerDefinition.TriggerConditionDefinitions.OrderBy(i => i.Index))
                {
                    var conditionConfigurationViewModel = new ConditionViewModel();
                    conditionConfigurationViewModel.ViewModelPropertyName = $"Triggers[{index}]";
                    conditionConfigurationViewModel.ConditionDefinitionId = conditionDefinition.TriggerConditionDefinitionId;
                    if (conditionIndex == 0) conditionConfigurationViewModel.Active = true;

                    conditionConfigurationViewModel.ExtensionConfiguration.ConfigurationExtensionId = conditionDefinition.ConfigurationExtensionId;
                    conditionConfigurationViewModel.ExtensionConfiguration.ConfigurationExtensionAssemblyPath = conditionDefinition.ConfigurationExtensionAssemblyPath;

                    extensionConfigurationController = new ExtensionConfigurationController(conditionDefinition.ConfigurationExtensionId,
                        conditionDefinition.ConfigurationExtensionAssemblyPath);
                    conditionConfigurationViewModel.ExtensionConfiguration.Configuration = extensionConfigurationController.Load(conditionDefinition.Configuration);

                    triggerViewModel.Conditions.Add(conditionConfigurationViewModel);

                    conditionIndex += 1;
                }

                index += 1;
            }

            index = 0;
            foreach (var stepDefinition in jobDefinition.StepDefinitions.OrderBy(i => i.Index))
            {
                var stepViewModel = new StepViewModel();
                stepViewModel.StepDefinitionId = stepDefinition.StepDefinitionId;
                if (index == 0) stepViewModel.Active = true;
                stepViewModel.ExtensionConfiguration.ConfigurationExtensionId = stepDefinition.ConfigurationExtensionId;
                stepViewModel.ExtensionConfiguration.ConfigurationExtensionAssemblyPath = stepDefinition.ConfigurationExtensionAssemblyPath;

                extensionConfigurationController = new ExtensionConfigurationController(stepDefinition.ConfigurationExtensionId,
                    stepDefinition.ConfigurationExtensionAssemblyPath);
                stepViewModel.ExtensionConfiguration.Configuration = extensionConfigurationController.Load(stepDefinition.Configuration);

                model.Steps.Add(stepViewModel);

                index += 1;
            }

            return model;
        }

        public void Save(ServerDbContext serverDbContext)
        {
            var jobDefinition = serverDbContext.JobDefinitions.FirstOrDefault(i => i.JobDefinitionId == this.JobDefinitionId);

            jobDefinition.Version += 1;
            jobDefinition.Configuration = this.ExtensionConfiguration.Configuration.ToJson();

            var triggerIds = this.Triggers.Select(i => i.TriggerDefinitionId);
            var deletedTriggers = serverDbContext.TriggerDefinitions.Where(
                i => i.JobDefinitionId == this.JobDefinitionId && !triggerIds.Contains(i.TriggerDefinitionId));
            serverDbContext.TriggerDefinitions.RemoveRange(deletedTriggers);

            int index = 0;
            foreach (var trigger in this.Triggers)
            {
                var triggerDefinition = serverDbContext.TriggerDefinitions.FirstOrDefault(i => i.TriggerDefinitionId == trigger.TriggerDefinitionId);

                if (triggerDefinition != null)
                {
                    triggerDefinition.Index = index;
                    triggerDefinition.Configuration = trigger.ExtensionConfiguration.Configuration.ToJson();
                }
                else
                {
                    triggerDefinition = new TriggerDefinition();
                    triggerDefinition.Index = index;
                    triggerDefinition.ExtensionId = trigger.ExtensionConfiguration.ExtensionId;
                    triggerDefinition.ExtensionAssemblyPath = trigger.ExtensionConfiguration.ExtensionAssemblyPath;
                    triggerDefinition.ConfigurationExtensionId = trigger.ExtensionConfiguration.ConfigurationExtensionId;
                    triggerDefinition.ConfigurationExtensionAssemblyPath = trigger.ExtensionConfiguration.ConfigurationExtensionAssemblyPath;
                    triggerDefinition.Configuration = trigger.ExtensionConfiguration.Configuration.ToJson();

                    jobDefinition.TriggerDefinitions.Add(triggerDefinition);
                }

                var triggerConditionIds = trigger.Conditions.Select(i => i.ConditionDefinitionId);
                var deletedTriggerConditions = serverDbContext.TriggerConditionDefinitions.Where(
                    i => i.TriggerDefinitionId == trigger.TriggerDefinitionId && !triggerConditionIds.Contains(i.TriggerConditionDefinitionId));
                serverDbContext.TriggerConditionDefinitions.RemoveRange(deletedTriggerConditions);

                var conditionIndex = 0;
                foreach (var condition in trigger.Conditions)
                {
                    var conditionDefinition = serverDbContext.TriggerConditionDefinitions.FirstOrDefault(i => i.TriggerConditionDefinitionId == condition.ConditionDefinitionId);

                    if (conditionDefinition != null)
                    {
                        conditionDefinition.Index = conditionIndex;
                        conditionDefinition.Configuration = condition.ExtensionConfiguration.Configuration.ToJson();
                    }
                    else
                    {
                        conditionDefinition = new TriggerConditionDefinition();
                        conditionDefinition.Index = conditionIndex;
                        conditionDefinition.ExtensionId = condition.ExtensionConfiguration.ExtensionId;
                        conditionDefinition.ExtensionAssemblyPath = condition.ExtensionConfiguration.ExtensionAssemblyPath;
                        conditionDefinition.ConfigurationExtensionId = condition.ExtensionConfiguration.ConfigurationExtensionId;
                        conditionDefinition.ConfigurationExtensionAssemblyPath = condition.ExtensionConfiguration.ConfigurationExtensionAssemblyPath;
                        conditionDefinition.Configuration = condition.ExtensionConfiguration.Configuration.ToJson();

                        triggerDefinition.TriggerConditionDefinitions.Add(conditionDefinition);
                    }

                    conditionIndex += 1;
                }

                index += 1;
            }

            var stepIds = this.Steps.Select(i => i.StepDefinitionId);
            var deletedSteps = serverDbContext.StepDefinitions.Where(
                i => i.JobDefinitionId == this.JobDefinitionId && !stepIds.Contains(i.StepDefinitionId));
            serverDbContext.StepDefinitions.RemoveRange(deletedSteps);

            index = 0;
            foreach (var step in this.Steps)
            {
                var stepDefinition = serverDbContext.StepDefinitions.FirstOrDefault(i => i.StepDefinitionId == step.StepDefinitionId);
                if (stepDefinition != null)
                {
                    stepDefinition.Index = index;
                    stepDefinition.Configuration = step.ExtensionConfiguration.Configuration.ToJson();
                }
                else
                {
                    stepDefinition = new StepDefinition();
                    stepDefinition.Index = index;
                    stepDefinition.ExtensionId = step.ExtensionConfiguration.ExtensionId;
                    stepDefinition.ExtensionAssemblyPath = step.ExtensionConfiguration.ExtensionAssemblyPath;
                    stepDefinition.ConfigurationExtensionId = step.ExtensionConfiguration.ConfigurationExtensionId;
                    stepDefinition.ConfigurationExtensionAssemblyPath = step.ExtensionConfiguration.ConfigurationExtensionAssemblyPath;
                    stepDefinition.Configuration = step.ExtensionConfiguration.Configuration.ToJson();

                    jobDefinition.StepDefinitions.Add(stepDefinition);
                }

                var stepConditionIds = step.Conditions.Select(i => i.ConditionDefinitionId);
                var deletedStepConditions = serverDbContext.StepConditionDefinitions.Where(
                    i => i.StepDefinitionId == step.StepDefinitionId && !stepConditionIds.Contains(i.StepConditionDefinitionId));
                serverDbContext.StepConditionDefinitions.RemoveRange(deletedStepConditions);

                var conditionIndex = 0;
                foreach (var condition in step.Conditions)
                {
                    var conditionDefinition = serverDbContext.StepConditionDefinitions.FirstOrDefault(i => i.StepConditionDefinitionId == condition.ConditionDefinitionId);

                    if (conditionDefinition != null)
                    {
                        conditionDefinition.Index = conditionIndex;
                        conditionDefinition.Configuration = condition.ExtensionConfiguration.Configuration.ToJson();
                    }
                    else
                    {
                        conditionDefinition = new StepConditionDefinition();
                        conditionDefinition.Index = conditionIndex;
                        conditionDefinition.ExtensionId = condition.ExtensionConfiguration.ExtensionId;
                        conditionDefinition.ExtensionAssemblyPath = condition.ExtensionConfiguration.ExtensionAssemblyPath;
                        conditionDefinition.ConfigurationExtensionId = condition.ExtensionConfiguration.ConfigurationExtensionId;
                        conditionDefinition.ConfigurationExtensionAssemblyPath = condition.ExtensionConfiguration.ConfigurationExtensionAssemblyPath;
                        conditionDefinition.Configuration = condition.ExtensionConfiguration.Configuration.ToJson();

                        stepDefinition.StepConditionDefinitions.Add(conditionDefinition);
                    }

                    conditionIndex += 1;
                }

                index += 1;
            }

            serverDbContext.SaveChanges();
        }
        #endregion
    }
}