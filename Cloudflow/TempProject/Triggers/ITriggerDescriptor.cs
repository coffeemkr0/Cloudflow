using System;

namespace TempProject.Triggers
{
    public interface ITriggerDescriptor
    {
        Guid ExtensionId { get; }

        Type ExtensionType { get; }

        string Name { get; }

        string Description { get; }

        Type ConfigurationType { get; }

        byte[] Icon { get; }
    }
}