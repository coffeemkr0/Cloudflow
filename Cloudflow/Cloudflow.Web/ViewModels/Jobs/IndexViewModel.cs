using Cloudflow.Core.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class IndexViewModel
    {
        #region Properties
        public List<JobDefinitionViewModel> JobDefinitions { get; set; }
        #endregion

        #region Constructors
        public IndexViewModel()
        {
            this.JobDefinitions = new List<JobDefinitionViewModel>();
        }
        #endregion
    }
}