using System;

namespace Cloudflow.Core.Data.Shared.Models
{
    public class ConfigurableExtensionDefinition
    {
        public Guid ExtensionId { get; set; }

        public string ExtensionAssemblyPath { get; set; }

        public Guid ConfigurationExtensionId { get; set; }

        public string ConfigurationExtensionAssemblyPath { get; set; }

        public string Configuration { get; set; }
    }
}