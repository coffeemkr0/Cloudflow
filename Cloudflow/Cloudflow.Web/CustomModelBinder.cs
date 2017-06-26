using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Web.ViewModels.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloudflow.Web
{
    public class CustomModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType.Equals(typeof(ExtensionConfiguration)))
            {
                var jobConfigurationExtensionId = ((ExtensionConfigurationViewModel)bindingContext.ModelMetadata.Container).ConfigurationExtensionId;
                var extensionConfigurationAssemblyPath = ((ExtensionConfigurationViewModel)bindingContext.ModelMetadata.Container).ConfigurationExtensionAssemblyPath;

                var configurationController = new ExtensionConfigurationController(jobConfigurationExtensionId, extensionConfigurationAssemblyPath);
                var configuration = configurationController.CreateNewConfiguration();

                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, configurationController.GetConfigurationType());
                bindingContext.ModelMetadata.Model = configuration;
                return configuration;
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}