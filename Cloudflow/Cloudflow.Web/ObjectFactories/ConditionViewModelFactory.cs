using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.Controllers;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using Cloudflow.Web.ViewModels.Jobs;
using System;
using System.ComponentModel.Composition;

namespace Cloudflow.Web.ObjectFactories
{
    [ExportExtension("E0F69CBB-4199-4F62-B344-97325B252B91", typeof(ConditionViewModelFactory))]
    public class ConditionViewModelFactory : ObjectFactory
    {
        [ImportingConstructor]
        public ConditionViewModelFactory([Import("factoryData")]string factoryData) : base(factoryData)
        {

        }

        public override object CreateObject(string instanceData)
        {
            var extensionLibraryPath = FactoryData;

            var configurableExtensionBrowser = new ConfigurableExtensionBrowser(extensionLibraryPath);
            var extension = configurableExtensionBrowser.GetConfigurableExtension(Guid.Parse(instanceData));

            var viewModel = new ConditionViewModel();
            viewModel.ConditionDefinitionId = Guid.NewGuid();
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