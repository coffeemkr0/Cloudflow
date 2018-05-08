using Cloudflow.Core.Extensions;
using Cloudflow.Web.ViewModels.Jobs;
using System;
using System.Web.Mvc;

namespace Cloudflow.Web
{
    public class CustomModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            //if (modelType.Equals(typeof(ExtensionConfiguration)))
            //{
            //    var jobConfigurationExtensionId = ((StepConfigurationViewModel)bindingContext.ModelMetadata.Container).ConfigurationExtensionId;
            //    var extensionConfigurationAssemblyPath = ((StepConfigurationViewModel)bindingContext.ModelMetadata.Container).ConfigurationExtensionAssemblyPath;

            //    var configurationController = new ExtensionConfigurationController(jobConfigurationExtensionId, extensionConfigurationAssemblyPath);
            //    var configuration = configurationController.CreateNewConfiguration();

            //    bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, configurationController.GetConfigurationType());
            //    bindingContext.ModelMetadata.Model = configuration;
            //    return configuration;
            //}

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}