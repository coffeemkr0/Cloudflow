﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggersViewModel
    {
        #region Properties
        public List<TriggerViewModel> Triggers { get; set; }
        #endregion

        #region Constructors
        public TriggersViewModel()
        {
            this.Triggers = new List<TriggerViewModel>();
        }
        #endregion
    }
}