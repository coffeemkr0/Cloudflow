using System;
using System.ComponentModel.Composition;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true), MetadataAttribute]
    public class ExportConfigurableExtensionAttribute : ExportExtensionAttribute, IConfigurableExtensionMetaData
    {
        public string ExtensionName { get; set; }

        public string ExtensionDescription { get; set; }

        public string ConfigurationExtensionId { get; set; }

        public ExportConfigurableExtensionAttribute(string extensionId, Type extensionType, string extensionName, 
            string configurationExtensionId, string extensionDescription = "") :
            base(extensionId, extensionType, typeof(IConfigurableExtension))
        {
            if (string.IsNullOrWhiteSpace(configurationExtensionId))
                throw new ArgumentException("'configurationExtensionId' is required.", "configurationExtensionId");
            if (string.IsNullOrWhiteSpace(configurationExtensionId))
                throw new ArgumentException("'extensionName' is required.", "extensionName");

            this.ExtensionName = extensionName;
            this.ExtensionDescription = extensionDescription;
            this.ConfigurationExtensionId = configurationExtensionId;
        }
    }
}
