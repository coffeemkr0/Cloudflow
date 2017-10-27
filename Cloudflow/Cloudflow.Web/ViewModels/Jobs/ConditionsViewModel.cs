using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ConditionsViewModel
    {
        #region Properties
        public List<ExtensionConfigurationViewModel> Conditions { get; set; }
        #endregion

        #region Constructors
        public ConditionsViewModel()
        {
            this.Conditions = new List<ExtensionConfigurationViewModel>();
        }
        #endregion
    }
}