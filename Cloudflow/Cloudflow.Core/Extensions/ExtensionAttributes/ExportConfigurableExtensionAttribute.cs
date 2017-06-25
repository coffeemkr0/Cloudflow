using System;
using System.ComponentModel.Composition;

namespace Cloudflow.Core.Extensions.ExtensionAttributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true), MetadataAttribute]
    public class ExportConfigurableExtensionAttribute : ExportExtensionAttribute, IConfigurableExtensionMetaData
    {
        public string ConfigurationId { get; set; }

        public ExportConfigurableExtensionAttribute(string id, Type type, string configurationId) :
            base(id, type, typeof(IConfigurableExtension))
        {
            if (string.IsNullOrEmpty(configurationId))
                throw new ArgumentException("'configurationId' is required.", "configurationId");

            this.ConfigurationId = configurationId;
        }
    }
}
