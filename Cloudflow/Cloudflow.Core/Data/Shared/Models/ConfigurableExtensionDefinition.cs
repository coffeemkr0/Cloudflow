using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class ConfigurableExtensionDefinition
    {
        public Guid ExtensionId { get; set; }

        public string ExtensionAssemblyPath { get; set; }

        public Guid ConfigurationExtensionId { get; set; }

        public string ConfigurationExtensionAssemblyPath { get; set; }
    }
}
