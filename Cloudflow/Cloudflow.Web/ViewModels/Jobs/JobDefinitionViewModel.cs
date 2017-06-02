using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class JobDefinitionViewModel
    {
        #region Properties
        public Guid Id { get; }

        public ExtensionConfiguration Configuration { get; set; }
        #endregion

        #region Constructors
        public JobDefinitionViewModel(JobDefinition jobDefinition)
        {
            this.Id = jobDefinition.JobDefinitionId;

            var extensionConfigurationController = new ExtensionConfigurationController(jobDefinition.JobConfigurationExtensionId,
                jobDefinition.JobConfigurationExtensionAssemblyPath);
            this.Configuration = extensionConfigurationController.Load(jobDefinition.Configuration);
        }
        #endregion
    }
}