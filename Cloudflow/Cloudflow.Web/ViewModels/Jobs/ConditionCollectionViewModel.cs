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

            var conditionIndex = 0;
            foreach (var conditionDefinition in triggerDefinition.TriggerConditionDefinitions)
            {
                var conditionModel = ConditionViewModel.FromTriggerConditionDefinition(conditionDefinition, conditionIndex);
                conditionModel.Active = conditionIndex == 0;
                model.Add(conditionModel);

                conditionIndex += 1;
            }

            return model;
        }
        #endregion
    }
}