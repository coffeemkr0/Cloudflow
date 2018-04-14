using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using System;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class JobSummaryViewModel
    {
        #region Properties
        public Guid Id { get; }

        public ExtensionConfiguration Configuration { get; set; }
        #endregion

        #region Constructors
        public JobSummaryViewModel(JobDefinition jobDefinition)
        {
            Id = jobDefinition.JobDefinitionId;

            var extensionConfigurationController = new ExtensionConfigurationController(jobDefinition.ConfigurationExtensionId,
                jobDefinition.ConfigurationExtensionAssemblyPath);
            Configuration = extensionConfigurationController.Load(jobDefinition.Configuration);
        }
        #endregion
    }
}