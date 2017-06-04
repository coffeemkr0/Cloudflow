using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class EditViewModel
    {
        #region Properties
        public Guid JobDefinitionId { get; set; }

        public ExtensionConfiguration Configuration { get; set; }
        #endregion

        #region Constructors
        public EditViewModel()
        {

        }
        #endregion

        #region Public Methods
        public static EditViewModel FromJobDefinition(JobDefinition jobDefinition)
        {
            var editViewModel = new EditViewModel();
            editViewModel.JobDefinitionId = jobDefinition.JobDefinitionId;

            var extensionConfigurationController = new ExtensionConfigurationController(jobDefinition.JobConfigurationExtensionId,
               jobDefinition.JobConfigurationExtensionAssemblyPath);
            editViewModel.Configuration = extensionConfigurationController.Load(jobDefinition.Configuration);

            return editViewModel;
        }
        #endregion
    }
}