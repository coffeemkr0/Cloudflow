using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
{
    public interface IExtensionService
    {
        ITriggerConfiguration LoadTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId, string configuration);
        ITrigger LoadTrigger(ICatalogProvider catalogProvider, Guid extensionId, ITriggerConfiguration configuration);
        IStep LoadStep(ICatalogProvider catalogProvider, Guid extensionId, IStepConfiguration configuration);
        IStepConfiguration LoadStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId, string configuration);
        ITriggerConfiguration CreateNewTriggerConfiguration(ICatalogProvider catalogProvider, Guid extensionId);
        IStepConfiguration CreateNewStepConfiguration(ICatalogProvider catalogProvider, Guid extensionId);
    }
}
