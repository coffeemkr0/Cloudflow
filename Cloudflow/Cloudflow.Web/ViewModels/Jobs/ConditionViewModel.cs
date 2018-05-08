using Cloudflow.Core.Data.Shared.Models;
using System;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ConditionViewModel
    {
        #region Properties
        public Guid ConditionDefinitionId { get; set; }

        public bool Active { get; set; }

        public int Index { get; set; }

        public string PropertyName { get; set; }

        public ConditionConfigurationViewModel ConfigurationViewModel { get; set; }
        #endregion

        #region Constructors
        public ConditionViewModel()
        {

        }
        #endregion

        #region Public Methods
        public static ConditionViewModel FromTriggerConditionDefinition(TriggerConditionDefinition conditionDefinition, int index)
        {
            var model = new ConditionViewModel
            {
                ConditionDefinitionId = conditionDefinition.TriggerConditionDefinitionId,
                Index = index,
                PropertyName = $"Triggers[{conditionDefinition.TriggerDefinition.Index}].Conditions"
            };

            model.ConfigurationViewModel = Jobs.ConditionConfigurationViewModel.FromTriggerConditionDefinition(conditionDefinition);

            return model;
        }

        public static ConditionViewModel FromStepConditionDefinition(StepConditionDefinition conditionDefinition, int index)
        {
            var model = new ConditionViewModel
            {
                ConditionDefinitionId = conditionDefinition.StepConditionDefinitionId,
                Index = index,
                PropertyName = $"Steps[{conditionDefinition.StepDefinition.Index}].Conditions"
            };

            model.ConfigurationViewModel = Jobs.ConditionConfigurationViewModel.FromStepConditionDefinition(conditionDefinition);

            return model;
        }
        #endregion
    }
}