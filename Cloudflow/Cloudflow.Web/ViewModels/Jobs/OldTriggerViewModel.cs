using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ObjectFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    [DisplayTextPropertyName("ExtensionConfiguration.Configuration.Name")]
    public class OldTriggerViewModel
    {
        #region Private Members
        private CategorizedItemCollection _conditionExtensions;
        #endregion

        #region Properties
        public Guid TriggerDefinitionId { get; set; }

        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        public List<ConditionViewModel> Conditions { get; set; }
        #endregion

        #region Constructors
        public OldTriggerViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
            this.Conditions = new List<ConditionViewModel>();
        }
        #endregion
    }
}