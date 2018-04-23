using System;

namespace TempProject.Interfaces
{
    public interface IStepConfigurationExtensionService
    {
        IStepConfiguration GetConfiguration(Guid extensionId);
    }
}