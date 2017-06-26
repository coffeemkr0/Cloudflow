using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public interface IConfigurableExtensionMetaData : IExtensionMetaData
    {
        string ExtensionName { get; }

        string ExtensionDescription { get; }

        string ConfigurationExtensionId { get; }
    }
}
