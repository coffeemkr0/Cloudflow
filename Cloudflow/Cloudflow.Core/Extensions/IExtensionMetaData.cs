using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public interface IExtensionMetaData
    {
        string ExtensionId { get; }

        Type ExtensionType { get; }
    }
}
