using Cloudflow.Core.Data.Server;
using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class EditViewModel
    {
        #region Properties
        public ExtensionConfigurationViewModel JobConfigurationViewModel { get; set; }

        public List<TriggerViewModel> Triggers { get; set; }

        public StepsViewModel Steps { get; set; }
        #endregion

        #region Constructors
        public EditViewModel()
        {
            this.JobConfigurationViewModel = new ExtensionConfigurationViewModel();
            this.Triggers = new List<TriggerViewModel>();
            this.Steps = new StepsViewModel();
        }
        #endregion

        #region Public Methods
        public static EditViewModel FromJobDefinition(JobDefinition jobDefinition)
        {
            var editViewModel = new EditViewModel();

            editViewModel.JobConfigurationViewModel.Id = jobDefinition.JobDefinitionId;
            editViewModel.JobConfigurationViewModel.ConfigurationExtensionId = jobDefinition.ConfigurationExtensionId;
            editViewModel.JobConfigurationViewModel.ConfigurationExtensionAssemblyPath = jobDefinition.ConfigurationExtensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(jobDefinition.ConfigurationExtensionId,
               jobDefinition.ConfigurationExtensionAssemblyPath);
            editViewModel.JobConfigurationViewModel.Configuration = extensionConfigurationController.Load(jobDefinition.Configuration);

            int index = 0;
            foreach (var triggerDefinition in jobDefinition.TriggerDefinitions.OrderBy(i => i.Index))
            {
                var triggerViewModel = new TriggerViewModel();
                triggerViewModel.Id = triggerDefinition.TriggerDefinitionId;
                triggerViewModel.Index = index;
                if (index == 0) triggerViewModel.Active = true;
                triggerViewModel.ConfigurationExtensionId = triggerDefinition.ConfigurationExtensionId;
                triggerViewModel.ConfigurationExtensionAssemblyPath = triggerDefinition.ConfigurationExtensionAssemblyPath;

                extensionConfigurationController = new ExtensionConfigurationController(triggerDefinition.ConfigurationExtensionId,
                    triggerDefinition.ConfigurationExtensionAssemblyPath);
                triggerViewModel.Configuration = extensionConfigurationController.Load(triggerDefinition.Configuration);

                editViewModel.Triggers.Add(triggerViewModel);

                var conditionIndex = 0;
                foreach (var conditionDefinition in triggerDefinition.TriggerConditionDefinitions)
                {
                    var conditionConfigurationViewModel = new ConditionConfigurationViewModel();
                    conditionConfigurationViewModel.Id = conditionDefinition.TriggerConditionDefinitionId;
                    conditionConfigurationViewModel.Index = conditionIndex;
                    if (conditionIndex == 0) conditionConfigurationViewModel.Active = true;

                    conditionConfigurationViewModel.ConfigurationExtensionId = conditionDefinition.ConfigurationExtensionId;
                    conditionConfigurationViewModel.ConfigurationExtensionAssemblyPath = conditionDefinition.ConfigurationExtensionAssemblyPath;

                    extensionConfigurationController = new ExtensionConfigurationController(conditionDefinition.ConfigurationExtensionId,
                        conditionDefinition.ConfigurationExtensionAssemblyPath);
                    conditionConfigurationViewModel.Configuration = extensionConfigurationController.Load(conditionDefinition.Configuration);

                    conditionIndex += 1;
                }

                index += 1;
            }

            index = 0;
            foreach (var stepDefinition in jobDefinition.StepDefinitions.OrderBy(i => i.Index))
            {
                var stepConfigurationViewModel = new ExtensionConfigurationViewModel();
                stepConfigurationViewModel.Id = stepDefinition.StepDefinitionId;
                stepConfigurationViewModel.Index = index;
                if (index == 0) stepConfigurationViewModel.Active = true;
                stepConfigurationViewModel.ConfigurationExtensionId = stepDefinition.ConfigurationExtensionId;
                stepConfigurationViewModel.ConfigurationExtensionAssemblyPath = stepDefinition.ConfigurationExtensionAssemblyPath;

                extensionConfigurationController = new ExtensionConfigurationController(stepDefinition.ConfigurationExtensionId,
                    stepDefinition.ConfigurationExtensionAssemblyPath);
                stepConfigurationViewModel.Configuration = extensionConfigurationController.Load(stepDefinition.Configuration);

                editViewModel.Steps.Steps.Add(stepConfigurationViewModel);

                index += 1;
            }

            return editViewModel;
        }

        public void Save(ServerDbContext serverDbContext)
        {
            var jobDefinition = serverDbContext.JobDefinitions.FirstOrDefault(i => i.JobDefinitionId == this.JobConfigurationViewModel.Id);

            jobDefinition.Version += 1;
            jobDefinition.Configuration = this.JobConfigurationViewModel.Configuration.ToJson();

            var deletedTriggerIds = this.Triggers.Where(i => i.Deleted).Select(i => i.Id).ToList();
            serverDbContext.TriggerDefinitions.RemoveRange(serverDbContext.TriggerDefinitions.Where(i => deletedTriggerIds.Contains(i.TriggerDefinitionId)));

            int index = 0;
            foreach (var trigger in this.Triggers.OrderBy(i => i.Position))
            {
                var triggerDefinition = serverDbContext.TriggerDefinitions.FirstOrDefault(i => i.TriggerDefinitionId == trigger.Id);

                if (triggerDefinition != null)
                {
                    triggerDefinition.Index = index;
                    triggerDefinition.Configuration = trigger.Configuration.ToJson();
                }
                else
                {
                    triggerDefinition = new TriggerDefinition();
                    triggerDefinition.Index = index;
                    triggerDefinition.ExtensionId = trigger.ExtensionId;
                    triggerDefinition.ExtensionAssemblyPath = trigger.ExtensionAssemblyPath;
                    triggerDefinition.ConfigurationExtensionId = trigger.ConfigurationExtensionId;
                    triggerDefinition.ConfigurationExtensionAssemblyPath = trigger.ConfigurationExtensionAssemblyPath;
                    triggerDefinition.Configuration = trigger.Configuration.ToJson();

                    jobDefinition.TriggerDefinitions.Add(triggerDefinition);
                }

                var conditionIndex = 0;
                foreach (var condition in trigger.Conditions)
                {
                    var conditionDefinition = serverDbContext.TriggerConditionDefinitions.FirstOrDefault(i => i.TriggerConditionDefinitionId == condition.Id);

                    if (conditionDefinition != null)
                    {
                        conditionDefinition.Index = conditionIndex;
                        conditionDefinition.Configuration = trigger.Configuration.ToJson();
                    }
                    else
                    {
                        conditionDefinition = new TriggerConditionDefinition();
                        conditionDefinition.Index = conditionIndex;
                        conditionDefinition.ExtensionId = condition.ExtensionId;
                        conditionDefinition.ExtensionAssemblyPath = condition.ExtensionAssemblyPath;
                        conditionDefinition.ConfigurationExtensionId = condition.ConfigurationExtensionId;
                        conditionDefinition.ConfigurationExtensionAssemblyPath = condition.ConfigurationExtensionAssemblyPath;
                        conditionDefinition.Configuration = condition.Configuration.ToJson();

                        triggerDefinition.TriggerConditionDefinitions.Add(conditionDefinition);
                    }

                    conditionIndex += 1;
                }
                
                index += 1;
            }

            var deletedStepIds = this.Steps.Steps.Where(i => i.Deleted).Select(i => i.Id).ToList();
            serverDbContext.StepDefinitions.RemoveRange(serverDbContext.StepDefinitions.Where(i => deletedStepIds.Contains(i.StepDefinitionId)));

            index = 0;
            foreach (var step in this.Steps.Steps.OrderBy(i => i.Position))
            {
                var stepDefinition = serverDbContext.StepDefinitions.FirstOrDefault(i => i.StepDefinitionId == step.Id);
                if (stepDefinition != null)
                {
                    stepDefinition.Index = index;
                    stepDefinition.Configuration = step.Configuration.ToJson();
                }
                else
                {
                    stepDefinition = new StepDefinition();
                    stepDefinition.Index = index;
                    stepDefinition.ExtensionId = step.ExtensionId;
                    stepDefinition.ExtensionAssemblyPath = step.ExtensionAssemblyPath;
                    stepDefinition.ConfigurationExtensionId = step.ConfigurationExtensionId;
                    stepDefinition.ConfigurationExtensionAssemblyPath = step.ConfigurationExtensionAssemblyPath;
                    stepDefinition.Configuration = step.Configuration.ToJson();

                    jobDefinition.StepDefinitions.Add(stepDefinition);
                }
                index += 1;
            }

            serverDbContext.SaveChanges();
        }
        #endregion
    }
}