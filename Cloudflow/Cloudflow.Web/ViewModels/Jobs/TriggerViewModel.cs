using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggerViewModel : ExtensionConfigurationViewModel
    {
        #region Properties
        public ConditionsViewModel ConditionsViewModel { get; set; }
        #endregion

        #region Constructors
        public TriggerViewModel()
        {
            this.ConditionsViewModel = new ConditionsViewModel();
        }
        #endregion
    }
}