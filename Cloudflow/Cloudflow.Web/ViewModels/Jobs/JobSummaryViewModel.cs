using Cloudflow.Core.Data.Shared.Models;
using System;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class JobSummaryViewModel
    {
        #region Properties
        public Guid Id { get; }

        public JobDefinition JobDefinition { get; set; }
        #endregion

        #region Constructors
        public JobSummaryViewModel(JobDefinition jobDefinition)
        {
            Id = jobDefinition.JobDefinitionId;
        }
        #endregion
    }
}