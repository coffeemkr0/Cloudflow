using System;
using System.Drawing;

namespace Cloudflow.Core.Steps
{
    public interface IStepDescriptor
    {
        Guid ExtensionId { get; }

        Type ExtensionType { get; }

        string Name { get; }

        string Description { get; }

        Type ConfigurationType { get; }

        Image Icon { get; }
    }
}