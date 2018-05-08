using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using Cloudflow.Core.ExtensionManagement;
using Cloudflow.Core.Serialization;
using Cloudflow.Core.Steps;
using Cloudflow.Core.Triggers;
using StepDefinition = Cloudflow.Core.Data.Shared.Models.StepDefinition;
using TriggerDefinition = Cloudflow.Core.Data.Shared.Models.TriggerDefinition;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class TriggerConfigurationViewModel
    {
        [Hidden]
        public Guid ExtensionId { get; set; }

        [Hidden]
        public string AssemblyPath { get; set; }

        public ITriggerConfiguration Configuration { get; set; }

        public static TriggerConfigurationViewModel FromTriggerDefinition(TriggerDefinition definition)
        {
            var model = new TriggerConfigurationViewModel
            {
                ExtensionId = definition.ExtensionId,
                AssemblyPath = definition.AssemblyPath
            };

            var extensionService = new ExtensionService(new JsonConfigurationSerializer());
            var assemblyCatalogProvider = new AssemblyCatalogProvider(definition.AssemblyPath);
            model.Configuration = extensionService.LoadTriggerConfiguration(assemblyCatalogProvider,
                definition.ExtensionId, definition.Configuration);

            return model;
        }
    }
}