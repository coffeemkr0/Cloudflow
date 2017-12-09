using Cloudflow.Core.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ConditionCollectionViewModel : List<ConditionViewModel>
    {
        #region Properties
        public string PropertyName { get; set; }
        #endregion

        #region Public Methods
        public static ConditionCollectionViewModel FromTriggerDefinition(TriggerDefinition triggerDefinition, int triggerIndex)
        {
            var model = new ConditionCollectionViewModel
            {
                PropertyName = $"Triggers[{triggerIndex}].Conditions"
            };

            foreach (var conditionDefinition in triggerDefinition.TriggerConditionDefinitions.OrderBy(i => i.Index))
            {
                var conditionModel = ConditionViewModel.FromTriggerConditionDefinition(conditionDefinition, conditionDefinition.Index);
                conditionModel.Active = conditionDefinition.Index == 0;
                model.Add(conditionModel);
            }

            return model;
        }

        public static ConditionCollectionViewModel FromStepDefinition(StepDefinition stepDefinition, int stepIndex)
        {
            var model = new ConditionCollectionViewModel
            {
                PropertyName = $"Steps[{stepIndex}].Conditions"
            };

            foreach (var conditionDefinition in stepDefinition.StepConditionDefinitions.OrderBy(i => i.Index))
            {
                var conditionModel = ConditionViewModel.FromStepConditionDefinition(conditionDefinition, conditionDefinition.Index);
                conditionModel.Active = conditionDefinition.Index == 0;
                model.Add(conditionModel);
            }

            return model;
        }
        #endregion
    }
}