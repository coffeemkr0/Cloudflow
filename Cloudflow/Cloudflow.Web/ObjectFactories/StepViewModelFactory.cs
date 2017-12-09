using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ViewModels.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Cloudflow.Web.ObjectFactories
{
    [ExportExtension("8CAC8ED3-DA70-48B9-B720-361735114FAC", typeof(StepViewModelFactory))]
    public class StepViewModelFactory : ObjectFactory
    {
        [ImportingConstructor]
        public StepViewModelFactory([Import("factoryData")]string factoryData) : base(factoryData)
        {

        }

        public override object CreateObject(string instanceData)
        {
            var extensionLibraryPath = this.FactoryData;

            var configurableExtensionBrowser = new ConfigurableExtensionBrowser(extensionLibraryPath);
            var extension = configurableExtensionBrowser.GetConfigurableExtension(Guid.Parse(instanceData));

            var viewModel = new StepViewModel();
            viewModel.StepDefinitionId = Guid.NewGuid();
            viewModel.ExtensionConfiguration.ExtensionId = Guid.Parse(extension.ExtensionId);
            viewModel.ExtensionConfiguration.ExtensionAssemblyPath = extensionLibraryPath;
            viewModel.ExtensionConfiguration.ConfigurationExtensionId = Guid.Parse(extension.ConfigurationExtensionId);
            viewModel.ExtensionConfiguration.ConfigurationExtensionAssemblyPath = extensionLibraryPath;

            var extensionConfigurationController = new ExtensionConfigurationController(Guid.Parse(extension.ConfigurationExtensionId),
                extensionLibraryPath);

            viewModel.ExtensionConfiguration.Configuration = extensionConfigurationController.CreateNewConfiguration();
            viewModel.ExtensionConfiguration.Configuration.Name = extension.ExtensionName;

            return viewModel;
        }
    }
}