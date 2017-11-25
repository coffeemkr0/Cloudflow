using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ObjectFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    [DisplayTextPropertyName("ExtensionConfiguration.Configuration.Name")]
    public class TriggerViewModel
    {
        #region Private Members
        private CategorizedItemCollection _conditionExtensions;
        #endregion

        #region Properties
        [Hidden]
        public Guid TriggerDefinitionId { get; set; }

        [PropertyGroupAttribute("GeneralTabText")]
        [DisplayOrder(0)]
        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        [PropertyGroupAttribute("ConditionsTabText")]
        [DisplayOrder(1)]
        [CategorizedItemSelector(ConfigurableExtensionFetcher.ConditionsExtensionCollectionId, ConfigurableExtensionFetcher.ConditionObjectFactoryExtensionId)]
        public List<ConditionViewModel> Conditions { get; set; }
        #endregion

        #region Constructors
        public TriggerViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
            this.Conditions = new List<ConditionViewModel>();
        }
        #endregion
    }
}