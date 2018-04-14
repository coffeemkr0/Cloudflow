using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using Cloudflow.Core.Extensions;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class EditJobViewModel
    {
        #region Properties
        public Guid JobDefinitionId { get; set; }

        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        public List<TriggerViewModel> Triggers { get; set; }

        public List<StepViewModel> Steps { get; set; }

        public ExtensionBrowserViewModel TriggerBrowserViewModel { get; set; }

        public ExtensionBrowserViewModel StepBrowserViewModel { get; set; }

        public ExtensionBrowserViewModel ConditionBrowserViewModel { get; set; }
        #endregion

        #region Constructors
        public EditJobViewModel()
        {
            ExtensionConfiguration = new ExtensionConfigurationViewModel();
            Triggers = new List<TriggerViewModel>();
            Steps = new List<StepViewModel>();
        }
        #endregion

        #region Public Methods
        public static EditJobViewModel FromJobDefinition(JobDefinition jobDefinition, string extensionLibraryFolder)
        {
            var model = new EditJobViewModel();

            model.LoadExtensions(extensionLibraryFolder);

            model.JobDefinitionId = jobDefinition.JobDefinitionId;
            model.ExtensionConfiguration.ConfigurationExtensionId = jobDefinition.ConfigurationExtensionId;
            model.ExtensionConfiguration.ConfigurationExtensionAssemblyPath = jobDefinition.ConfigurationExtensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(jobDefinition.ConfigurationExtensionId,
               jobDefinition.ConfigurationExtensionAssemblyPath);
            model.ExtensionConfiguration.Configuration = extensionConfigurationController.Load(jobDefinition.Configuration);

            var index = 0;
            foreach (var triggerDefinition in jobDefinition.TriggerDefinitions.OrderBy(i => i.Index))
            {
                var triggerViewModel = TriggerViewModel.FromTriggerDefinition(triggerDefinition, index);
                triggerViewModel.Active = index == 0;
                model.Triggers.Add(triggerViewModel);

                index += 1;
            }

            index = 0;
            foreach (var stepDefinition in jobDefinition.StepDefinitions.OrderBy(i => i.Index))
            {
                var stepViewModel = StepViewModel.FromStepDefinition(stepDefinition, index);
                stepViewModel.Active = index == 0;
                model.Steps.Add(stepViewModel);

                index += 1;
            }

            return model;
        }

        public void Save(ServerDbContext serverDbContext)
        {
            var jobDefinition = serverDbContext.JobDefinitions.FirstOrDefault(i => i.JobDefinitionId == JobDefinitionId);

            jobDefinition.Version += 1;
            jobDefinition.Configuration = ExtensionConfiguration.Configuration.ToJson();

            SaveTriggers(serverDbContext, jobDefinition);
            SaveSteps(serverDbContext, jobDefinition);

            serverDbContext.SaveChanges();
        }

        private void SaveTriggers(ServerDbContext serverDbContext, JobDefinition jobDefinition)
        {
            var triggerIds = Triggers.Select(i => i.TriggerDefinitionId);
            var deletedTriggers = serverDbContext.TriggerDefinitions.Where(
                i => i.JobDefinitionId == JobDefinitionId && !triggerIds.Contains(i.TriggerDefinitionId));
            serverDbContext.TriggerDefinitions.RemoveRange(deletedTriggers);

            var index = 0;
            foreach (var trigger in Triggers)
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
        }

        private void SaveSteps(ServerDbContext serverDbContext, JobDefinition jobDefinition)
        {
            var stepIds = Steps.Select(i => i.StepDefinitionId);
            var deletedSteps = serverDbContext.StepDefinitions.Where(
                i => i.JobDefinitionId == JobDefinitionId && !stepIds.Contains(i.StepDefinitionId));
            serverDbContext.StepDefinitions.RemoveRange(deletedSteps);

            var index = 0;
            foreach (var step in Steps)
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
        }

        public void LoadExtensions(string extensionLibraryFolder)
        {
            TriggerBrowserViewModel = ExtensionBrowserViewModel.GetModel("TriggerExtensionBrowser", extensionLibraryFolder, ConfigurableExtensionTypes.Trigger);
            StepBrowserViewModel = ExtensionBrowserViewModel.GetModel("StepExtensionBrowser", extensionLibraryFolder, ConfigurableExtensionTypes.Step);
            ConditionBrowserViewModel = ExtensionBrowserViewModel.GetModel("ConditionExtensionBrowser", extensionLibraryFolder, ConfigurableExtensionTypes.Condition);
        }
        #endregion
    }
}