using System;

namespace TempProject.Interfaces
{
    public interface IStepExtensionService
    {
        IStep GetStep(Guid extensionId);
    }
}