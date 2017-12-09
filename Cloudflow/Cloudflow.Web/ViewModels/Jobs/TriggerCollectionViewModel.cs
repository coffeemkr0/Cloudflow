using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggerCollectionViewModel
    {
        #region Properties
        public List<TriggerNavigationItemViewModel> TriggerNavigationItems { get; set; }

        public List<TriggerEditViewModel> TriggerEdits { get; set; }
        #endregion

        #region Constructors
        public TriggerCollectionViewModel()
        {
            this.TriggerNavigationItems = new List<TriggerNavigationItemViewModel>();
            this.TriggerEdits = new List<TriggerEditViewModel>();
        }
        #endregion
    }
}