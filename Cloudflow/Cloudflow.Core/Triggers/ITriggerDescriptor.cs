using System;
using System.Drawing;

namespace Cloudflow.Core.Triggers
{
    public interface ITriggerDescriptor
    {
        Guid ExtensionId { get; }

        Type ExtensionType { get; }

        string Name { get; }

        string Description { get; }

        Type ConfigurationType { get; }

        Image Icon { get; }
    }
}