using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggersViewModel
    {
        public List<ExtensionConfigurationViewModel> Triggers { get; set; }

        #region Constructors
        public TriggersViewModel()
        {
            this.Triggers = new List<ExtensionConfigurationViewModel>();
        }
        #endregion
    }
}