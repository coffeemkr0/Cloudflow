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
        public JobDefinition JobDefinition { get; set; }

        public ExtensionConfigurationViewModel ConfigurationViewModel { get; set; }
        #endregion

        #region Constructors
        public EditViewModel()
        {
            this.ConfigurationViewModel = new ExtensionConfigurationViewModel();
        }
        #endregion

        #region Public Methods
        public static EditViewModel FromJobDefinition(JobDefinition jobDefinition)
        {
            var editViewModel = new EditViewModel();
            editViewModel.JobDefinition = jobDefinition;

            var extensionConfigurationController = new ExtensionConfigurationController(jobDefinition.JobConfigurationExtensionId,
               jobDefinition.JobConfigurationExtensionAssemblyPath);

            editViewModel.ConfigurationViewModel.ExtensionId = jobDefinition.JobConfigurationExtensionId;
            editViewModel.ConfigurationViewModel.ExtensionAssemblyPath = jobDefinition.JobConfigurationExtensionAssemblyPath;
            editViewModel.ConfigurationViewModel.Configuration = extensionConfigurationController.Load(jobDefinition.Configuration);

            return editViewModel;
        }
        #endregion
    }
}