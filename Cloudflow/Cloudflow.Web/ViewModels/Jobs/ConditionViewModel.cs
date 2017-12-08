using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    [DisplayTextPropertyName("ExtensionConfiguration.Configuration.Name")]
    public class ConditionViewModel
    {
        #region Properties
        public Guid CategorizedItemSelectorId { get; set; }

        public Guid ConditionDefinitionId { get; set; }

        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }
        #endregion

        #region Constructors
        public ConditionViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
        }
        #endregion
    }
}