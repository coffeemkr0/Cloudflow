using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ConditionViewModel
    {
        #region Properties
        public Guid ConditionDefinitionId { get; set; }

        public bool Active { get; set; }

        public int Index { get; set; }

        public string BasePropertyName { get; set; }

        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }
        #endregion

        #region Constructors
        public ConditionViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
        }
        #endregion

        #region Public Methods
        public static ConditionViewModel FromTriggerConditionDefinition(TriggerConditionDefinition conditionDefinition, int index)
        {
            var model = new ConditionViewModel
            {
                ConditionDefinitionId = conditionDefinition.TriggerConditionDefinitionId,
                Index = index,
                BasePropertyName = $"Triggers[{conditionDefinition.TriggerDefinition.Index}]"
            };

            model.ExtensionConfiguration = ExtensionConfigurationViewModel.FromConfigurableExtensionDefinition(conditionDefinition);

            return model;
        }
        #endregion
    }
}