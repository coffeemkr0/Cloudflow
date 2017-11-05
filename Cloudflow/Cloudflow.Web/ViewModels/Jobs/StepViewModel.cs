using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class StepViewModel : ExtensionConfigurationViewModel
    {
        #region Properties
        public List<ConditionConfigurationViewModel> Conditions { get; set; }
        #endregion

        #region Constructors
        public StepViewModel()
        {
            this.Conditions = new List<ConditionConfigurationViewModel>();
        }
        #endregion
    }
}