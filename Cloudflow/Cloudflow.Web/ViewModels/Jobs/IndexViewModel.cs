using Cloudflow.Core.Data.Server.Models;
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
        public List<AgentConfiguration> AgentConfigurations { get; set; }

        public List<JobSummaryViewModel> JobSummaries { get; set; }
        #endregion

        #region Constructors
        public IndexViewModel()
        {
            this.AgentConfigurations = new List<AgentConfiguration>();
            this.JobSummaries = new List<JobSummaryViewModel>();
        }
        #endregion
    }
}