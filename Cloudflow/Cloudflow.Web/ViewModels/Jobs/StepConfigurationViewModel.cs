using Cloudflow.Core.Data.Shared.Models;
using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System;
using Cloudflow.Core.ExtensionManagement;
using Cloudflow.Core.Serialization;
using Cloudflow.Core.Steps;
using StepDefinition = Cloudflow.Core.Data.Shared.Models.StepDefinition;

namespace Cloudflow.Web.ViewModels.Jobs
{
    public class StepConfigurationViewModel
    {
        [Hidden]
        public Guid ExtensionId { get; set; }

        [Hidden]
        public string AssemblyPath { get; set; }

        public IStepConfiguration Configuration { get; set; }

        #region Public Methods
        public static StepConfigurationViewModel FromStepDefinition(StepDefinition definition)
        {
            var model = new StepConfigurationViewModel
            {
                ExtensionId = definition.ExtensionId,
                AssemblyPath = definition.AssemblyPath
            };

            var extensionService = new ExtensionService(new JsonConfigurationSerializer());
            var assemblyCatalogProvider = new AssemblyCatalogProvider(definition.AssemblyPath);
            model.Configuration = extensionService.LoadStepConfiguration(assemblyCatalogProvider,
                definition.ExtensionId, definition.Configuration);

            return model;
        }
        #endregion
    }
}