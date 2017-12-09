using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggerNavigationItemViewModel
    {
        #region Properties
        public Guid TriggerDefinitionId { get; set; }

        public string Caption { get; set; }
        #endregion
    }
}