using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    [DisplayTextPropertyName("ExtensionConfiguration.Configuration.Name")]
    public class StepViewModel
    {
        #region Properties
        [Hidden]
        public Guid StepDefinitionId { get; set; }

        [Hidden]
        public bool Deleted { get; set; }

        [Hidden]
        public bool Active { get; set; }

        [PropertyGroupAttribute("GeneralTabText")]
        [DisplayOrder(0)]
        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

        [PropertyGroupAttribute("ConditionsTabText")]
        [DisplayOrder(1)]
        public List<ConditionViewModel> Conditions { get; set; }
        #endregion

        #region Constructors
        public StepViewModel()
        {
            this.ExtensionConfiguration = new ExtensionConfigurationViewModel();
            this.Conditions = new List<ConditionViewModel>();
        }
        #endregion
    }
}