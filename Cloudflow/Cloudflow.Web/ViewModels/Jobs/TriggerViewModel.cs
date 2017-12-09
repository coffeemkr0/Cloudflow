using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggerViewModel
    {
        #region Properties
        public Guid TriggerDefinitionId { get; set; }

        public bool Active { get; set; }

        public int Index { get; set; }

        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        public List<ConditionViewModel> Conditions { get; set; }
        #endregion

        #region Constructors
        public TriggerViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
            this.Conditions = new List<ConditionViewModel>();
        }
        #endregion

        #region Public Methods
        public static TriggerViewModel FromTriggerDefinition(TriggerDefinition triggerDefinition, int index)
        {
            var model = new TriggerViewModel
            {
                TriggerDefinitionId = triggerDefinition.TriggerDefinitionId,
                Index = index
            };

            model.ExtensionConfiguration = ExtensionConfigurationViewModel.FromConfigurableExtensionDefinition(triggerDefinition);

            var conditionIndex = 0;
            foreach (var conditionDefinition in triggerDefinition.TriggerConditionDefinitions)
            {
                var conditionModel = ConditionViewModel.FromTriggerConditionDefinition(conditionDefinition, conditionIndex);
                conditionModel.Active = conditionIndex == 0;
                model.Conditions.Add(conditionModel);

                conditionIndex += 1;
            }

            return model;
        }
        #endregion
    }
}