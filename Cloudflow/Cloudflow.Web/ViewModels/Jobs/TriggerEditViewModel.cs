using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggerEditViewModel
    {
        #region Properties
        public Guid TriggerDefinitionId { get; set; }

        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }
        #endregion

        #region Constructors
        public TriggerEditViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
        }
        #endregion

        #region Public Methods
        public static TriggerEditViewModel FromTriggerDefinition(TriggerDefinition triggerDefinition)
        {
            var model = new TriggerEditViewModel
            {
                TriggerDefinitionId = triggerDefinition.TriggerDefinitionId,
            };

            model.ExtensionConfiguration = ExtensionConfigurationViewModel.FromConfigurableExtensionDefinition(triggerDefinition);

            return model;
        }
        #endregion
    }
}