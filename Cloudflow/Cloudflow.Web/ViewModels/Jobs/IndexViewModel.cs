using Cloudflow.Core.Data.Server.Models;
using System.Collections.Generic;

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
            AgentConfigurations = new List<AgentConfiguration>();
            JobSummaries = new List<JobSummaryViewModel>();
        }
        #endregion
    }
}