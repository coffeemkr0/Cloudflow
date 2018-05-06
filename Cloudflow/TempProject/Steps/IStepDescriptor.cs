using System;

namespace TempProject.Steps
{
    public interface IStepDescriptor
    {
        Guid ExtensionId { get; }

        Type ExtensionType { get; }

        string Name { get; }

        string Description { get; }

        Type ConfigurationType { get; }

        byte[] Icon { get; }
    }
}
