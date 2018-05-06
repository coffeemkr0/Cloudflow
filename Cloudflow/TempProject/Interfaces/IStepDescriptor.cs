using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempProject.Interfaces
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
