using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class StepsViewModel
    {
        #region Properties
        public List<ExtensionConfigurationViewModel> Steps { get; set; }
        #endregion

        #region Constructors
        public StepsViewModel()
        {
            this.Steps = new List<ExtensionConfigurationViewModel>();
        }
        #endregion
    }
}