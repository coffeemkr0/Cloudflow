using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Extensions
{
    public interface IConfigurableExtensionMetaData : IExtensionMetaData
    {
        string ConfigurationId { get; }
    }
}
