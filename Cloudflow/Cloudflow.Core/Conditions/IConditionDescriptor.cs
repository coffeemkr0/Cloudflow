using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Conditions
{
    public interface IConditionDescriptor
    {
        Guid ExtensionId { get; }

        Type ExtensionType { get; }

        string Name { get; }

        string Description { get; }

        Type ConfigurationType { get; }

        Image Icon { get; }
    }
}
