using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using Cloudflow.Core.Conditions;
using Cloudflow.Core.ExtensionManagement;
using Cloudflow.Core.Serialization;
using Cloudflow.Core.Steps;
using Cloudflow.Core.Triggers;
using StepDefinition = Cloudflow.Core.Data.Shared.Models.StepDefinition;
using TriggerDefinition = Cloudflow.Core.Data.Shared.Models.TriggerDefinition;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class ConditionConfigurationViewModel
    {
        [Hidden]
        public Guid ExtensionId { get; set; }

        [Hidden]
        public string AssemblyPath { get; set; }

        public IConditionConfiguration Configuration { get; set; }

        public static ConditionConfigurationViewModel FromTriggerConditionDefinition(TriggerConditionDefinition definition)
        {
            var model = new ConditionConfigurationViewModel
            {
                ExtensionId = definition.ExtensionId,
                AssemblyPath = definition.AssemblyPath
            };

            var extensionService = new ExtensionService(new JsonConfigurationSerializer());
            var assemblyCatalogProvider = new AssemblyCatalogProvider(definition.AssemblyPath);
            //model.Configuration = extensionService.LoadTriggerConfiguration(assemblyCatalogProvider,
            //    definition.ExtensionId, definition.Configuration);

            return model;
        }

        public static ConditionConfigurationViewModel FromStepConditionDefinition(StepConditionDefinition definition)
        {
            var model = new ConditionConfigurationViewModel
            {
                ExtensionId = definition.ExtensionId,
                AssemblyPath = definition.AssemblyPath
            };

            var extensionService = new ExtensionService(new JsonConfigurationSerializer());
            var assemblyCatalogProvider = new AssemblyCatalogProvider(definition.AssemblyPath);
            //model.Configuration = extensionService.LoadTriggerConfiguration(assemblyCatalogProvider,
            //    definition.ExtensionId, definition.Configuration);

            return model;
        }
    }
}