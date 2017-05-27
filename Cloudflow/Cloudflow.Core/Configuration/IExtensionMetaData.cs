using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Configuration
{
    public interface IExtensionMetaData
    {
        string ExtensionId { get; }

        Type Type { get; }
    }
}
