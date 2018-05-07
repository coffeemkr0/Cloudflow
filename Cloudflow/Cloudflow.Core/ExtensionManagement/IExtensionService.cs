using System;
using System.Collections.Generic;
using Cloudflow.Core.Steps;
using Cloudflow.Core.Triggers;

namespace Cloudflow.Core.ExtensionManagement
{
    public interface IExtensionService
    {
        ITriggerConfiguration LoadTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId,
            string configuration);

        ITrigger LoadTrigger(ICatalogProvider catalogProvider, Guid extensionId, ITriggerConfiguration configuration);
        IStep LoadStep(ICatalogProvider catalogProvider, Guid extensionId, IStepConfiguration configuration);

        IStepConfiguration LoadStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId,
            string configuration);

        ITriggerConfiguration CreateNewTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId);
        IStepConfiguration CreateNewStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId);
        IEnumerable<ITriggerDescriptor> GetTriggerDescriptors(ICatalogProvider catalogProvider);
        IEnumerable<IStepDescriptor> GetStepDescriptors(ICatalogProvider catalogProvider);
    }
}