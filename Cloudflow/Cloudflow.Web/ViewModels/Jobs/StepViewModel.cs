using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ObjectFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class StepViewModel
    {
        #region Properties
        public Guid StepDefinitionId { get; set; }

        public ExtensionConfigurationViewModel ExtensionConfiguration { get; set; }

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