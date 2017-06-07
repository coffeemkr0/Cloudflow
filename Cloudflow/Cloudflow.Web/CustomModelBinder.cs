using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
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
                //TODO:This will need to work for every extension configuration so it needs to know the property names to use to ge the
                //configuration extension Id and the configuration extension assembly path
                var jobConfigurationExtensionId = Guid.Parse(bindingContext.ModelState["ConfigurationViewModel.ExtensionId"].Value.AttemptedValue.ToString());
                var extensionConfigurationAssemblyPath = bindingContext.ModelState["ConfigurationViewModel.ExtensionAssemblyPath"].Value.AttemptedValue.ToString();

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