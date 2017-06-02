using Cloudflow.Core.Data.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class EditViewModel
    {
        #region Properties
        public JobDefinitionViewModel JobDefinitionViewModel { get; set; }
        #endregion

        #region Constructors
        public EditViewModel(JobDefinition jobDefinition)
        {
            this.JobDefinitionViewModel = new JobDefinitionViewModel(jobDefinition);
        }
        #endregion
    }
}