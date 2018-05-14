using Cloudflow.Core.Data.Shared.Models;
using System;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class JobSummaryViewModel
    {
        #region Properties
        public JobDefinition JobDefinition { get; }

        #endregion

        #region Constructors
        public JobSummaryViewModel(JobDefinition jobDefinition)
        {
            JobDefinition = jobDefinition;
        }
        #endregion
    }
}