using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ExtensionConfigurationViewModel
    {
        [Hidden]
        public Guid ExtensionId { get; set; }

        [Hidden]
        public string ExtensionAssemblyPath { get; set; }

        [Hidden]
        public Guid ConfigurationExtensionId { get; set; }

        [Hidden]
        public string ConfigurationExtensionAssemblyPath { get; set; }

        public ExtensionConfiguration Configuration { get; set; }

        #region Public Methods
        public static ExtensionConfigurationViewModel FromConfigurableExtensionDefinition(ConfigurableExtensionDefinition definition)
        {
            var model = new ExtensionConfigurationViewModel();

            model.ExtensionId = definition.ExtensionId;
            model.ExtensionAssemblyPath = definition.ExtensionAssemblyPath;
            model.ConfigurationExtensionId = definition.ConfigurationExtensionId;
            model.ConfigurationExtensionAssemblyPath = definition.ConfigurationExtensionAssemblyPath;

            var extensionConfigurationController = new ExtensionConfigurationController(definition.ConfigurationExtensionId,
                   definition.ConfigurationExtensionAssemblyPath);
            model.Configuration = extensionConfigurationController.Load(definition.Configuration);

            return model;
        }
        #endregion
    }
}